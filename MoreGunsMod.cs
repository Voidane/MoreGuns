using Il2CppInterop.Runtime.Injection;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.PlayerScripts;
using MelonLoader;
using MoreGuns;
using MoreGuns.Gui;
using MoreGuns.Guns;
using MoreGuns.Patches;
using MoreGuns.Sync;
using System.Collections;
using UnityEngine;

[assembly: MelonInfo(typeof(MoreGunsMod), "MoreGuns", "1.4.0", "Voidane")]
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

                Config.Initialize();

                new AK47();
                new MiniGun();
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

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName == "Main")
            {
                NetworkController.SyncConfiguration();
                Reticle.Initialize();

                Transform HUD = GameObject.Find("UI/HUD").transform;
                ReloadMessage.Initialize(HUD);
                WindupIndicator.Initialize(HUD);
            }
            else
            {
                ItemRegistryPatch.isWeaponsRegistered = false;
            }
        }

        public static void RegisterAsset(string path, UnityEngine.Object asset)
        {
            Resources[path] = asset;
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
