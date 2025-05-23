using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MelonLoader.Utils;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using System;
using System.Reflection;
using UnityEngine.Rendering;
using MoreGunsMono.Guns;
using ScheduleOne.ItemFramework;
using HarmonyLib;
using System.Text.RegularExpressions;
using ScheduleOne;
using ScheduleOne.Persistence;
using MoreGunsMono.Patches;
using VLB;
using ModManagerPhoneApp;
using Steamworks;
using MoreGunsMono.Sync;
using MoreGunsMono.Gui;

namespace MoreGunsMono
{
    public class MoreGunsMod : MelonMod
    {
        public static Transform map;
        public static Transform container;
        public static Transform midcanal;
        public static Transform stanNPC;
        public static AssetBundle assetBundle;
        public static bool isInitialized;
        public static HarmonyLib.Harmony harmony;

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("MoreGuns Is Initializing");
            MelonLogger.Msg("Thank you for using More Guns! Discord: discord.gg/XB7ruKtJje");

            var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MoreGunsMono.voidanesguns");
            if (stream == null)
            {
                MelonLogger.Error("Could not find manifest resource stream. MoreGuns will not run.");
                isInitialized = false;
                return;
            }

            harmony = new HarmonyLib.Harmony("com.voidane.moreguns");
            harmony.PatchAll();
            MelonLogger.Msg("All harmony patches patched.");

            assetBundle = AssetBundle.LoadFromStream(stream);
            stream.Close();

            if (assetBundle != null)
            {
                isInitialized = true;
                MelonLogger.Msg("Assetbundle loaded in.");

                Config.Initialize();

                new AK47
                (
                    "ak47",
                    new Shopping() { purchasePrice = 15000F, displayName = "AK47", available = true, nonAvailableReason = "" },
                    new Shopping() { purchasePrice = 1000F, displayName = "AK47 Magazine", available = true, nonAvailableReason = "" }
                );

                TryLoadingDependencies();
            }
            else
            {
                MelonLogger.Error("Assetbundle was not loaded in. MoreGuns will not run.");
                MelonLogger.Warning("All patches were unpatched.");
                StopProcess();
            }
        }

        public override void OnApplicationQuit()
        {
            harmony.UnpatchSelf();
        }

        private static void TryLoadingDependencies()
        {
            try
            {
                MelonLogger.Msg("Subscribing to all guns configuration events to phone manager");
                foreach (var weapon in WeaponBase.allWeapons)
                {
                    weapon.config.OnSettingChanged += weapon.ApplySettingsFromConfig;
                    ModSettingsEvents.OnPreferencesSaved += weapon.config.HandleSettingsUpdate;
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"{ex.Message}");
            }
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName == "Main")
            {
                NetworkController.SyncConfiguration();
                Reticle.Initialize();
            }
            else
            {
                ItemRegistryPatch.isWeaponsRegistered = false;
            }
        }

        public static void StopProcess()
        {
            harmony.UnpatchSelf();
            isInitialized = false;
        }
    }
}
