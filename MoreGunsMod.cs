using Il2CppInterop.Runtime.Injection;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.PlayerScripts;
using MelonLoader;
using ModManagerPhoneApp;
using MoreGuns;
using MoreGuns.Guns;
using MoreGuns.Patches;
using System.Collections;
using UnityEngine;

[assembly: MelonInfo(typeof(MoreGunsMod), "MoreGuns", "1.0.0", "Voidane")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace MoreGuns
{
    public class MoreGunsMod : MelonMod
    {
        public static Transform map;
        public static Transform container;
        public static Transform midcanal;
        public static Transform stanNPC;
        public static Il2CppAssetBundle assetBundle;
        public static Dictionary<string, UnityEngine.Object> Resources = new Dictionary<string, UnityEngine.Object>();
        public static bool isInitialized;
        public static HarmonyLib.Harmony harmony;

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("Thank you for using More Guns! Discord: discord.gg/XB7ruKtJje");

            MelonLogger.Msg("Registering types to IL2CPP");
            ClassInjector.RegisterTypeInIl2Cpp<GunSettings>();

            harmony = new HarmonyLib.Harmony("com.voidane.moregunsil2cpp");
            harmony.PatchAll();
            MelonLogger.Msg("All harmony patches patched.");

            assetBundle = Il2CppAssetBundleManager.LoadFromMemory(Assets.VoidanesGuns);

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

        public static void RegisterAsset(string path, UnityEngine.Object asset)
        {
            Resources[path] = asset;
            MelonLogger.Msg($"Registered custom asset at path: {path}");
        }

        public static UnityEngine.Object TryGetAsset(string path)
        {
            if (Resources.TryGetValue(path, out UnityEngine.Object asset))
            {
                return asset;
            }
            return null;
        }

        public static void StopProcess()
        {
            harmony.UnpatchSelf();
            isInitialized = false;
        }
    }
}
