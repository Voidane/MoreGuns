using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.PlayerScripts;
using MelonLoader;
using MoreGuns;
using MoreGuns.Guns;
using System.Collections;
using UnityEngine;

[assembly: MelonInfo(typeof(MoreGunsMod), "MoreGuns", "1.0.0", "Voidane")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace MoreGuns
{
    public class MoreGunsMod : MelonMod
    {

        public static Il2CppAssetBundle assetBundle;
        public static Transform map;
        public static Transform container;
        public static Transform midcanal;
        public static Transform stanNPC;

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("Initialized");

            new HarmonyLib.Harmony("com.voidane.moregunsil2cpp").PatchAll();
            assetBundle = Il2CppAssetBundleManager.LoadFromMemory(Assets.VoidanesGuns);

            if (assetBundle != null)
            {
                MelonLogger.Msg("Voidanes Guns loaded from resx.");
                MelonCoroutines.Start(LoadAssetBundleCoroutine());
            }
            else
            {
                MelonLogger.Error("Voidanes Guns could not be loaded from resx.");
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
                    MelonLogger.Msg("Found Map");
                    MelonCoroutines.Start(GetTransformFromScene(null, "Container", 5.0F, (_container) =>
                    {
                        MelonLogger.Msg("Found container");
                    }));
                }));
            }
            else
            {
                // RegisterItemsBeforeLoad.isWeaponsRegistered = false;
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

        public static IEnumerator LoadAssetBundleCoroutine()
        {
            GameObject AK47Equippable = TryLoadAsset<GameObject>("AK47_Equippable");
            yield return AK47Equippable;

            IntegerItemDefinition AK47MagazineIntIDef = TryLoadAsset<IntegerItemDefinition>("AK47_Magazine");
            yield return AK47MagazineIntIDef;

            IntegerItemDefinition AK47IntIDef = TryLoadAsset<IntegerItemDefinition>("assets/resources/weapons/ak47/ak47.asset");
            yield return AK47IntIDef;

            GameObject AK47HandGun = TryLoadAsset<GameObject>("assets/resources/avatar/equippables/ak47.prefab");
            yield return AK47HandGun;

            GameObject AK47MagazineEquippable = TryLoadAsset<GameObject>("assets/resources/weapons/ak47/magazine/ak47_magazine_avatarequippable.prefab");
            yield return AK47MagazineEquippable;

            AK47.Initialize(AK47Equippable, AK47IntIDef, AK47MagazineIntIDef);

        }

        public static T TryLoadAsset<T>(string assetName) where T : UnityEngine.Object
        {
            T asset = assetBundle.LoadAsset<T>(assetName);
            
            if (asset != null)
            {
                MelonLogger.Msg($"Loaded {assetName} ({typeof(T).Name})");
            }
            else
            {
                MelonLogger.Error($"Could not load {assetName} ({typeof(T).Name})");
            }

            return asset;
        }

    }
}
