using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Levelling;
using Il2CppScheduleOne.Networking;
using Il2CppSteamworks;
using MelonLoader;
using MoreGuns.Guns;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Il2Cpp.Interop;

namespace MoreGuns.Sync
{
    public static class NetworkController
    {
        private const string IDENTIFICATION_PREFIX = "moreguns_settings";
        private static readonly string version = typeof(MoreGunsMod).Assembly.GetName().Version.ToString();
        public static bool IsSynced { get; private set; } = false;
        public static StringBuilder payload;

        public static bool forceHost = false;
        public static bool forceClient = false;

        public static void SyncConfiguration()
        {
            bool isHost = Lobby.Instance?.IsHost == true;
            bool isClient = Lobby.Instance?.IsHost == false && Lobby.Instance?.IsInLobby == true;

            MelonLogger.Msg($"Player joined with ID: {Lobby.Instance.LocalPlayerID}. Syncing configuration file.");
            payload = new StringBuilder();
            payload.Append($"{IDENTIFICATION_PREFIX}_{version}|");

            if (isHost || forceHost)
            {
                MelonLogger.Msg($"Host player");
                MelonCoroutines.Start(SyncHostToLobbyPayload());
                Lobby.Instance.SetLobbyData("MoreGunsConfig", payload.ToString());
            }
            else if (isClient || forceClient)
            {
                MelonLogger.Msg("Client loaded!");
                MelonCoroutines.Start(WaitOnLobbyPayload());
            }
            else
            {
                MelonLogger.Msg("Other side loaded.");
                foreach (var weapon in WeaponBase.allWeapons)
                {
                    weapon.ApplySettingsFromConfig();
                }
                MelonCoroutines.Start(SyncHostToLobbyPayload());
            }
        }

        public static IEnumerator WaitOnLobbyPayload()
        {
            while (true)
            {
                string data = SteamMatchmaking.GetLobbyData(Lobby.Instance.LobbySteamID, "MoreGunsConfig");
                if (!string.IsNullOrEmpty(data))
                {
                    HostToClientConfigurationSync(data);
                    yield break;
                }
                MelonLogger.Msg("Waiting for payload.");
                yield return new WaitForSeconds(1F);
            }
        }

        private static IEnumerator SyncHostToLobbyPayload()
        {
            foreach (WeaponBase weapon in WeaponBase.allWeapons)
            {

                weapon.ApplySettingsFromConfig();
                while (!weapon.IsConfigurationFinished)
                {
                    MelonLogger.Msg("config not ready");
                    yield return new WaitForSeconds(0.05F);
                }

                MelonLogger.Msg("Adding new gun");
                payload.Append($"@{weapon.ID}" +
                $":" +
                $"{weapon.gunRangedWeapon.Damage}:" +
                $"{weapon.gunRangedWeapon.ImpactForce}:" +
                $"{weapon.gunRangedWeapon.AimFOVReduction}:" +
                $"{weapon.gunRangedWeapon.AccuracyChangeDuration}:" +
                $"{weapon.gunRangedWeapon.MagazineSize}" +
                $":" +
                $"{weapon.gunIntItemDef.Name}:" +
                $"{weapon.gunIntItemDef.Description}:" +
                $"{weapon.gunIntItemDef.LabelDisplayColor}:" +
                $"{weapon.gunIntItemDef.legalStatus}:" +
                $"{weapon.gunIntItemDef.RequiredRank}" +
                $":" +
                $"{weapon.magIntItemDef.Name}:" +
                $"{weapon.magIntItemDef.Description}:" +
                $"{weapon.magIntItemDef.LabelDisplayColor}:" +
                $"{weapon.magIntItemDef.legalStatus}:" +
                $"{weapon.magIntItemDef.RequiredRank}" +
                $":" +
                $"{weapon.rangedGun.Name}:" +
                $"{weapon.rangedGun.Price}:" +
                $"{weapon.rangedGun.IsAvailable}:" +
                $"{weapon.rangedGun.NotAvailableReason}" +
                $":" +
                $"{weapon.ammoGun.Name}:" +
                $"{weapon.ammoGun.Price}:" +
                $"{weapon.ammoGun.IsAvailable}:" +
                $"{weapon.ammoGun.NotAvailableReason}");
                MelonLogger.Msg($"init {weapon.ID}");
            }
        }

        private static void HostToClientConfigurationSync(string data)
        {
            MelonLogger.Msg($"{data}");
            string[] dataVersion = data.Split('|').Where(item => !string.IsNullOrEmpty(item)).ToArray();
            if (!IsModValidForSync(dataVersion[0]))
            {
                MelonLogger.Warning($"MoreGuns is outdated with the host or server.");
                MelonLogger.Warning($"Your Version: {IDENTIFICATION_PREFIX}_{version}, Host Version: {dataVersion[0]}");
            }

            string[] weapons = dataVersion[1].Split('@').Where(item => !string.IsNullOrEmpty(item)).ToArray();
            foreach (string weapon in weapons)
            {
                int i = 0;
                string[] fields = weapon.Split(':');

                if (WeaponBase.weaponsByName.TryGetValue(fields[0], out WeaponBase weap))
                {
                    if (!float.TryParse(fields[1], out float gunRangedDamage)) continue;
                    if (!float.TryParse(fields[2], out float gunRangedImpactForce)) continue;
                    if (!float.TryParse(fields[3], out float gunRangedAimFOVReduction)) continue;
                    if (!float.TryParse(fields[4], out float gunRangedAccuracyChangeDuration)) continue;
                    if (!int.TryParse(fields[5], out int gunRangedMagazineSize)) continue;

                    string gunIIDName = fields[6];
                    string gunIIDDescription = fields[7];
                    Color gunIIDLabelDisplayColor = Tools.Color.StringRGBAToColor(fields[8]);
                    ELegalStatus gunIIDELegalStatus = Tools.LegalStatus.StringConvertToELegalStatus(fields[9]);
                    FullRank gunIIDRequiredRank = Tools.Rank.StringConvertToFullRank(fields[10]);

                    string magIIDName = fields[11];
                    string magIIDDescription = fields[12];
                    Color magIIDLabelDisplayColor = Tools.Color.StringRGBAToColor(fields[13]);
                    ELegalStatus magIIDELegalStatus = Tools.LegalStatus.StringConvertToELegalStatus(fields[14]);
                    FullRank magIIDRequiredRank = Tools.Rank.StringConvertToFullRank(fields[15]);

                    string rangedGunName = fields[16];
                    if (!float.TryParse(fields[17], out float rangedGunPrice)) continue;
                    if (!bool.TryParse(fields[18], out bool rangedGunAvailable)) continue;
                    string rangedGunNonAvailableReason = fields[19];

                    string ammoGunName = fields[20];
                    if (!float.TryParse(fields[21], out float ammoGunPrice)) continue;
                    if (!bool.TryParse(fields[22], out bool ammoGunAvailable)) continue;
                    string ammoGunNonAvailableReason = fields[23];

                    weap.gunRangedWeapon.Damage = gunRangedDamage;
                    weap.gunRangedWeapon.ImpactForce = gunRangedImpactForce;
                    weap.gunRangedWeapon.AimFOVReduction = gunRangedAimFOVReduction;
                    weap.gunRangedWeapon.AccuracyChangeDuration = gunRangedAccuracyChangeDuration;
                    weap.gunRangedWeapon.MagazineSize = gunRangedMagazineSize;

                    weap.gunIntItemDef.Name = gunIIDName;
                    weap.gunIntItemDef.Description = gunIIDDescription;
                    weap.gunIntItemDef.LabelDisplayColor = gunIIDLabelDisplayColor;
                    weap.gunIntItemDef.legalStatus = gunIIDELegalStatus;
                    weap.gunIntItemDef.RequiredRank = gunIIDRequiredRank;

                    weap.magIntItemDef.Name = magIIDName;
                    weap.magIntItemDef.Description = magIIDDescription;
                    weap.magIntItemDef.LabelDisplayColor = magIIDLabelDisplayColor;
                    weap.magIntItemDef.legalStatus = magIIDELegalStatus;
                    weap.magIntItemDef.RequiredRank = magIIDRequiredRank;

                    weap.gunIntItemDef.Name = rangedGunName;
                    weap.gunIntItemDef.BasePurchasePrice = rangedGunPrice;
                    weap.rangedGun.IsAvailable = rangedGunAvailable;
                    weap.rangedGun.NotAvailableReason = rangedGunNonAvailableReason;

                    weap.magIntItemDef.Name = ammoGunName;
                    weap.magIntItemDef.BasePurchasePrice = ammoGunPrice;
                    weap.ammoGun.IsAvailable = ammoGunAvailable;
                    weap.ammoGun.NotAvailableReason = ammoGunNonAvailableReason;
                }
            }
        }

        private static bool IsModValidForSync(string pIdentify)
        {
            string identify = $"{IDENTIFICATION_PREFIX}_{version}";
            return (pIdentify == identify);
        }
    }
}
