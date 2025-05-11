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
                    weapon.config.OnSettingChanged += weapon.UpdateSettingsFromConfig;
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
                MelonLogger.Msg("Main scene loaded!");

                MelonCoroutines.Start(GetTransformFromScene(null, "Map", 20.0F, (_MAP) => 
                {
                    map = _MAP;
                    MelonCoroutines.Start(GetTransformFromScene(null, "Container", 5.0F, (_container) =>
                    {
                    }));
                }));
            }
            else
            {
                ItemRegistryPatch.isWeaponsRegistered = false;
            }
        }

        private IEnumerator GetTransformFromScene(Transform parent, string name, float timeoutLimit, Action<Transform> onComplete)
        {
            Transform target = null;
            float timeOutCounter = 0F;
            int attempt = 0;

            while (target == null && timeOutCounter < timeoutLimit)
            {
                target = (parent == null) ? GameObject.Find(name).transform : parent.Find(name);
                if (target == null)
                {
                    timeOutCounter += 0.5F;
                    yield return new WaitForSeconds(0.5F);
                }
            }

            if (target != null)
            {
                onComplete?.Invoke(target);
            }
            else
            {
                MelonLogger.Error("Failed to find target object within timeout period!");
                onComplete?.Invoke(null);
            }

            yield return target;
        }

        public static void StopProcess()
        {
            harmony.UnpatchSelf();
            isInitialized = false;
        }
    }
}
