using Il2CppInterop.Runtime.Injection;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.PlayerScripts;
using MelonLoader;
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

        public static Il2CppAssetBundle assetBundle;
        public static Transform map;
        public static Transform container;
        public static Transform midcanal;
        public static Transform stanNPC;
        public static Dictionary<string, UnityEngine.Object> Resources = new Dictionary<string, UnityEngine.Object>();

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("Thank you for using More Guns! Discord: ");
            

            ClassInjector.RegisterTypeInIl2Cpp<GunSettings>();
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
            MelonLogger.Msg("Initialized");
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
                        // Must reset, statics in non mod asm are reset
                        MelonCoroutines.Start(LoadAssetBundleCoroutine());
                    }));
                }));
            }
            else
            {
                RegistryPatch.isWeaponsRegistered = false;
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
            Il2CppAssetBundleRequest Il2CppAK47EquippableRq = assetBundle.LoadAssetAsync("assets/resources/weapons/ak47/ak47_equippable.prefab");
            yield return Il2CppAK47EquippableRq;
            UnityEngine.Object UEOAK47Equippable = Il2CppAK47EquippableRq.asset;
            GameObject AK47Equippable = UEOAK47Equippable.Cast<GameObject>();

            if (AK47Equippable == null)
            {
                MelonLogger.Error("[Loaded Assets Failure] AK47Equippable prefab is null");
            }
            else
            {
                MelonLogger.Msg("[Loaded Assets] AK47Equippable (assets/resources/weapons/ak47/ak47_equippable.prefab)");
            }

            Il2CppAssetBundleRequest Il2CppAK47MagazineIntIDef = assetBundle.LoadAssetAsync("AK47_Magazine");
            yield return Il2CppAK47MagazineIntIDef;
            UnityEngine.Object UEOAK47MagazineIntIDef = Il2CppAK47MagazineIntIDef.asset;
            IntegerItemDefinition AK47MagazineIntIDef = UEOAK47MagazineIntIDef.Cast<IntegerItemDefinition>();

            if (AK47MagazineIntIDef == null)
            {
                MelonLogger.Error("[Loaded Assets Failure] AK47MagazineIntIDef prefab is null");
            }
            else
            {
                MelonLogger.Msg("[Loaded Assets] AK47MagazineIntIDef (AK47_Magazine)");
            }

            Il2CppAssetBundleRequest Il2CppAK47IntIDef = assetBundle.LoadAssetAsync("assets/resources/weapons/ak47/ak47.asset");
            yield return Il2CppAK47IntIDef;
            UnityEngine.Object UEOAk47IntIDef = Il2CppAK47IntIDef.asset;
            IntegerItemDefinition AK47IntIDef = UEOAk47IntIDef.Cast<IntegerItemDefinition>();

            if (AK47IntIDef == null)
            {
                MelonLogger.Error("[Loaded Assets Failure] AK47IntIDef prefab is null");
            }
            else
            {
                MelonLogger.Msg("[Loaded Assets] AK47IntIDef (assets/resources/weapons/ak47/ak47.asset)");
            }

            Il2CppAssetBundleRequest Il2CppAk47HandGun = assetBundle.LoadAssetAsync("assets/resources/avatar/equippables/ak47.prefab");
            yield return Il2CppAk47HandGun;
            UnityEngine.Object UEOAk47HandGun = Il2CppAk47HandGun.asset;
            GameObject AK47HandGun = UEOAk47HandGun.Cast<GameObject>();

            if (AK47IntIDef == null)
            {
                MelonLogger.Error("[Loaded Assets Failure] AK47HandGun prefab is null");
            }
            else
            {
                RegisterAsset("Avatar/Equippables/AK47", AK47HandGun);
                MelonLogger.Msg("[Loaded Assets] AK47HandGun (assets/resources/avatar/equippables/ak47.prefab)");
            }

            Il2CppAssetBundleRequest Il2CppAK47MagazineEquippable = assetBundle.LoadAssetAsync("assets/resources/weapons/ak47/magazine/ak47_magazine_avatarequippable.prefab");
            yield return Il2CppAK47MagazineEquippable;
            UnityEngine.Object UEOAK47MagazineEquippable = Il2CppAK47MagazineEquippable.asset;
            GameObject AK47MagazineEquippable = UEOAK47MagazineEquippable.Cast<GameObject>();

            if (AK47IntIDef == null)
            {
                MelonLogger.Error("[Loaded Assets Failure] AK47HandGun prefab is null");
            }
            else
            {
                RegisterAsset("Weapons/ak47/Magazine/AK47_Magazine_AvatarEquippable", AK47MagazineEquippable);
                MelonLogger.Msg("[Loaded Assets] AK47HandGun (assets/resources/avatar/equippables/ak47.prefab)");
            }

            AK47.Initialize(AK47Equippable, AK47IntIDef, AK47MagazineIntIDef);

            yield return null;
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

            // Add this check to ensure the log matches the actual state
            if (asset == null)
            {
                MelonLogger.Error($"Asset {assetName} is null despite successful load report!");
            }

            return asset;
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
    }
}
