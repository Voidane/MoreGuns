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

        public static void StopProcess()
        {
            harmony.UnpatchSelf();
            isInitialized = false;
        }
    }
}
