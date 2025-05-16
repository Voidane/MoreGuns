using Il2CppScheduleOne.Dialogue;
using Il2CppScheduleOne.Equipping;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Trash;
using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreGuns.Guns
{
    public class WeaponBase
    {
        public string ID;

        public GameObject gunEquippable;
        public Equippable_RangedWeapon gunRangedWeapon;

        public GameObject gunHandgun;
        public GameObject magAvatarEquippable;

        public IntegerItemDefinition gunIntItemDef;
        public IntegerItemDefinition magIntItemDef;

        public DialogueController_ArmsDealer.WeaponOption rangedGun;
        public DialogueController_ArmsDealer.WeaponOption ammoGun;

        public GameObject gunMagTrash;
        public TrashItem gunMagTrashItem;

        public GunConfiguration config;

        public Shopping gunShop;
        public Shopping magShop;

        public static List<WeaponBase> allWeapons = new List<WeaponBase>();

        public void Init(string ID, Shopping gunShop, Shopping magShop)
        {
            this.ID = ID;
            this.gunShop = gunShop;
            this.magShop = magShop;

            MelonLogger.Msg($"Initializing {ID}");
            MelonCoroutines.Start(LoadGun());
        }

        private IEnumerator LoadGun()
        {
            Il2CppAssetBundleRequest Il2CppGunEquippableRq = MoreGunsMod.assetBundle.LoadAssetAsync($"assets/resources/weapons/{ID}/{ID}_equippable.prefab");
            yield return Il2CppGunEquippableRq;
            UnityEngine.Object UEOGunEquippable = Il2CppGunEquippableRq.asset;
            GameObject _GunEquippable = UEOGunEquippable.Cast<GameObject>();
            if (!CheckAssetLoaded(_GunEquippable, $"assets/resources/weapons/{ID}/{ID}_equippable.prefab", $"{ID}"))
            {
                yield break;
            }

            Il2CppAssetBundleRequest Il2CppGunMagIntItemDef = MoreGunsMod.assetBundle.LoadAssetAsync($"{ID}_Magazine");
            yield return Il2CppGunMagIntItemDef;
            UnityEngine.Object UEOGunMagIntItemDef = Il2CppGunMagIntItemDef.asset;
            IntegerItemDefinition _MagIntItemDef = UEOGunMagIntItemDef.Cast<IntegerItemDefinition>();
            if (!CheckAssetLoaded(_MagIntItemDef, $"{ID}_Magazine", $"{ID}"))
            {
                yield break;
            }

            Il2CppAssetBundleRequest Il2CppGunIntItemDef = MoreGunsMod.assetBundle.LoadAssetAsync($"assets/resources/weapons/{ID}/{ID}.asset");
            yield return Il2CppGunIntItemDef;
            UnityEngine.Object UEOGunIntItemDef = Il2CppGunIntItemDef.asset;
            IntegerItemDefinition _GunIntItemDef = UEOGunIntItemDef.Cast<IntegerItemDefinition>();
            if (!CheckAssetLoaded(_GunIntItemDef, $"assets/resources/weapons/{ID}/{ID}.asset", $"{ID}"))
            {
                yield break;
            }

            Il2CppAssetBundleRequest Il2CppGunMagTrash = MoreGunsMod.assetBundle.LoadAssetAsync($"assets/resources/weapons/{ID}/magazine/{ID}_Magazine_Trash.prefab");
            yield return Il2CppGunMagTrash;
            UnityEngine.Object UEOGunMagTrash = Il2CppGunMagTrash.asset;
            GameObject _GunMagTrash = UEOGunMagTrash.Cast<GameObject>();
            if (!CheckAssetLoaded(_GunMagTrash, $"assets/resources/weapons/{ID}/magazine/{ID}_Magazine_Trash.prefab", $"{ID}"))
            {
                yield break;
            }

            Il2CppAssetBundleRequest Il2CppGunHandGun = MoreGunsMod.assetBundle.LoadAssetAsync($"assets/resources/avatar/equippables/{ID}.prefab");
            yield return Il2CppGunHandGun;
            UnityEngine.Object UEOGunHandGun = Il2CppGunHandGun.asset;
            GameObject _GunHandGun = UEOGunHandGun.Cast<GameObject>();
            if (!CheckAssetLoaded(_GunHandGun, $"assets/resources/avatar/equippables/{ID}.prefab", $"{ID}"))
            {
                yield break;
            }

            Il2CppAssetBundleRequest Il2CppGunMagAvatarEquippable = MoreGunsMod.assetBundle.LoadAssetAsync($"assets/resources/weapons/{ID}/magazine/{ID}_magazine_avatarequippable.prefab");
            yield return Il2CppGunMagAvatarEquippable;
            UnityEngine.Object UEOGunMagAvatarEquippable = Il2CppGunMagAvatarEquippable.asset;
            GameObject _GunMagAvatarEquippable = UEOGunMagAvatarEquippable.Cast<GameObject>();
            if (!CheckAssetLoaded(_GunMagAvatarEquippable, $"assets/resources/weapons/{ID}/magazine/{ID}_magazine_avatarequippable.prefab", $"{ID}"))
            {
                yield break;
            }

            GunSettings settings = _GunEquippable.AddComponent<GunSettings>();
            settings.isAutomatic.Value = true;

            gunEquippable = _GunEquippable;
            magIntItemDef = _MagIntItemDef;
            gunIntItemDef = _GunIntItemDef;
            gunMagTrash = _GunMagTrash;
            magAvatarEquippable = _GunMagAvatarEquippable;
            gunHandgun = _GunHandGun;

            gunRangedWeapon = gunEquippable.GetComponent<Equippable_RangedWeapon>();
            gunMagTrashItem = gunMagTrash.GetComponent<TrashItem>();

            CreateConfig();
            SetCustomItemUI();
            UpdateSettingsFromConfig();

            MoreGunsMod.RegisterAsset($"Avatar/Equippables/{ID.ToUpper()}", gunHandgun);
            MoreGunsMod.RegisterAsset($"Weapons/ak47/Magazine/{ID.ToUpper()}_Magazine_AvatarEquippable", magAvatarEquippable);

            allWeapons.Add(this);
            MelonLogger.Msg($"Finished Initializing {ID}");
        }

        public static bool CheckAssetLoaded(UnityEngine.Object asset, string assetName, string weaponName)
        {
            if (asset == null)
            {
                MelonLogger.Error($"Could not load asset: {assetName}");
                MoreGunsMod.StopProcess();
                return false;
            }
            else
            {
                MelonLogger.Msg($"Loaded asset for {weaponName} : {assetName}");
                return true;
            }
        }

        public void CreateConfig()
        {
            config = new GunConfiguration(this);
            MelonLogger.Msg("Created new config");
        }

        private void SetCustomItemUI()
        {
            UnityEngine.Object definition = Resources.Load("Weapons/M1911/M1911");

            if (definition == null)
            {
                MelonLogger.Error("Cast to ItemDefinition failed - type mismatch in IL2CPP");
                return;
            }

            var il2cppDefinition = definition.Cast<ItemDefinition>();

            if (il2cppDefinition != null)
            {
                gunIntItemDef.CustomItemUI = il2cppDefinition.CustomItemUI;
                magIntItemDef.CustomItemUI = il2cppDefinition.CustomItemUI;
                MelonLogger.Msg("Successfully set CustomItemUI using IL2CPP conversion");
            }
            else
            {
                MelonLogger.Error("IL2CPP conversion failed");
            }
        }

        public void UpdateSettingsFromConfig()
        {
            gunRangedWeapon.Damage = config.Damage.Value;
            gunRangedWeapon.ImpactForce = config.ImpactForce.Value;
            gunRangedWeapon.AimFOVReduction = config.AimFOVReduction.Value;
            gunRangedWeapon.AccuracyChangeDuration = config.AccuracyChangeDuration.Value;
            gunRangedWeapon.MagazineSize = config.MagazineSize.Value;

            gunIntItemDef.Name = config.DisplayItemName.Value;
            gunIntItemDef.Description = config.DisplayDescription.Value;
            gunIntItemDef.LabelDisplayColor = config.LabelDisplayColor.Value;
            gunIntItemDef.legalStatus = config.LegalStatus.Value;
            gunIntItemDef.RequiredRank = config.RequiredRank.Value;

            magIntItemDef.Name = config.MagDisplayItemName.Value;
            magIntItemDef.Description = config.MagDisplayDescription.Value;
            magIntItemDef.LabelDisplayColor = config.MagLabelDisplayColor.Value;
            magIntItemDef.legalStatus = config.MagLegalStatus.Value;
            magIntItemDef.RequiredRank = config.MagRequiredRank.Value;

            CreateDialogueControllerOptions();
        }

        private void CreateDialogueControllerOptions()
        {
            rangedGun = new DialogueController_ArmsDealer.WeaponOption
            {
                Name = config.ItemName.Value,
                Price = config.PurchasePrice.Value,
                IsAvailable = config.Available.Value,
                NotAvailableReason = config.AvailableReason.Value,
                Item = gunIntItemDef
            };

            ammoGun = new DialogueController_ArmsDealer.WeaponOption
            {
                Name = config.MagItemName.Value,
                Price = config.MagPurchasePrice.Value,
                IsAvailable = config.MagAvailable.Value,
                NotAvailableReason = config.MagAvailableReason.Value,
                Item = magIntItemDef
            };
        }
    }
}
