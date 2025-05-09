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

namespace MoreGunsMono
{
    public class MoreGunsMod : MelonMod
    {
        public static Transform map;
        public static Transform container;
        public static Transform midcanal;
        public static Transform stanNPC;
        public static AssetBundle assetBundle;

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("Initialized");

            var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MoreGunsMono.voidanesguns");
            if (stream == null)
            {
                return;
            }

            new HarmonyLib.Harmony("com.voidane.moreguns").PatchAll();
            assetBundle = AssetBundle.LoadFromStream(stream);
            stream.Close();
            Config.Init();

            if (assetBundle != null)
            {
                MelonLogger.Msg("Assetbundle loaded in.");
                MelonCoroutines.Start(LoadAssetBundleCoroutine());
            }
            else
            {
                MelonLogger.Error("Assetbundle was null");
            }
        }

        public override void OnApplicationQuit()
        {
            
        }

        private IEnumerator LoadAssetBundleCoroutine()
        {
            string[] assetNames = assetBundle.GetAllAssetNames();
            foreach (string name in assetNames)
            {
                MelonLogger.Msg($"Asset in bundle: {name}");
            }

            AssetBundleRequest request_AK47_Equippable = assetBundle.LoadAssetAsync<GameObject>("AK47_Equippable");
            yield return request_AK47_Equippable;

            AssetBundleRequest request_AK47_Magazine_IntegerItemDefinition = assetBundle.LoadAssetAsync<IntegerItemDefinition>("AK47_Magazine");
            yield return request_AK47_Magazine_IntegerItemDefinition;

            AssetBundleRequest request_AK47_IntegerItemDefinition = assetBundle.LoadAssetAsync<IntegerItemDefinition>("AK47");
            yield return request_AK47_IntegerItemDefinition;

            AssetBundleRequest request_AK47_Magazine_Trash = assetBundle.LoadAssetAsync<GameObject>("assets/resources/weapons/ak47/magazine/AK47_Magazine_Trash.prefab");
            yield return request_AK47_Magazine_Trash;
            GameObject ak47MagTrash = request_AK47_Magazine_Trash.asset as GameObject;

            AssetBundleRequest request_AK47_Prefab = assetBundle.LoadAssetAsync<GameObject>("assets/resources/avatar/equippables/ak47.prefab");
            yield return request_AK47_Prefab;
            GameObject ak47Prefab = request_AK47_Prefab.asset as GameObject;

            AssetBundleRequest requestAk47MagazineAvatarEquippable = assetBundle.LoadAssetAsync<GameObject>("assets/resources/weapons/ak47/magazine/ak47_magazine_avatarequippable.prefab");
            yield return requestAk47MagazineAvatarEquippable;
            GameObject ak47magazineAvatarEquippable = requestAk47MagazineAvatarEquippable.asset as GameObject;

            IntegerItemDefinition ak47MagazineIntegerItemDefinition = request_AK47_Magazine_IntegerItemDefinition.asset as IntegerItemDefinition;
            IntegerItemDefinition ak47IntegerItemDefinition = request_AK47_IntegerItemDefinition.asset as IntegerItemDefinition;

            AK47.InitializeAK47(request_AK47_Equippable.asset as GameObject, ak47MagazineIntegerItemDefinition, ak47IntegerItemDefinition, ak47MagTrash);
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName == "Main")
            {
                MelonLogger.Msg("Main scene loaded!");

                MelonCoroutines.Start(GetTransformFromScene(null, "Map", 20.0F, (_MAP) => 
                {
                    map = _MAP;
                    MelonLogger.Msg("Found Map");
                    MelonCoroutines.Start(GetTransformFromScene(null, "Container", 5.0F, (_container) =>
                    {
                        MelonLogger.Msg("Found container");
                        MelonCoroutines.Start(SpawnGunWhenPlayerReady());
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

        private IEnumerator SpawnGunWhenPlayerReady()
        {
            while (Player.Local == null)
            {
                MelonLogger.Msg("Waiting for player to initialize...");
                yield return new WaitForSeconds(0.5f);
            }

            yield return null;

            MelonLogger.Msg("Setting up AK47");

            FixShader(AK47.AK47_Equippable);

            MelonLogger.Msg($"Added Comp");
        }

        public static void FixShader(GameObject target)
        {
            foreach (MeshRenderer render in target.GetComponentsInChildren<MeshRenderer>(true))
            {
                foreach (Material mat in render.sharedMaterials)
                {
                    mat.shader = Shader.Find(mat.shader.name);
                }
            }
        }
    }
}
