using Il2CppInterop.Runtime;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace MoreGuns
{
    [RegisterTypeInIl2Cpp]
    public class AssetManager : MonoBehaviour
    {
        private static AssetManager _instance;
        public static AssetManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Create a new GameObject with AssetManager attached
                    GameObject go = new GameObject("MoreGuns_AssetManager");
                    _instance = go.AddComponent<AssetManager>();
                    DontDestroyOnLoad(go);
                    MelonLogger.Msg("AssetManager created");
                }
                return _instance;
            }
        }

        // Key data storage 
        private Il2CppAssetBundle _mainBundle;
        private Dictionary<string, UnityEngine.Object> _loadedAssets = new Dictionary<string, UnityEngine.Object>();
        private bool _assetsInitialized = false;

        public void Awake()
        {
            // Ensure singleton behavior
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(this.gameObject);

            // Instead of using the event, we'll monitor scene changes through MelonMod
            MelonLogger.Msg("AssetManager initialized");
        }

        // This method is called from MoreGunsMod when scenes load
        public void OnSceneLoaded(string sceneName)
        {
            MelonLogger.Msg($"Scene loaded: {sceneName}. Re-ensuring assets are protected.");
            // Make sure all critical assets are still marked as DontDestroyOnLoad
            EnsureAssetsPersistence();
        }

        public void SetMainBundle(Il2CppAssetBundle bundle)
        {
            _mainBundle = bundle;
            MelonLogger.Msg("Main asset bundle registered with AssetManager");
        }

        public Il2CppAssetBundle GetMainBundle()
        {
            return _mainBundle;
        }

        public void RegisterAsset(string key, UnityEngine.Object asset)
        {
            if (asset == null)
            {
                MelonLogger.Warning($"Tried to register null asset for key: {key}");
                return;
            }

            _loadedAssets[key] = asset;

            // If it's a GameObject, mark it as persistent
            if (asset is GameObject go)
            {
                // Create a child GameObject in our manager to hold this asset
                GameObject holder = new GameObject(key);
                holder.transform.SetParent(this.transform);

                // Parent the asset to our persistent holder
                go.transform.SetParent(holder.transform);

                MelonLogger.Msg($"Asset {key} registered and parented to AssetManager");
            }
            else
            {
                MelonLogger.Msg($"Asset {key} registered with AssetManager");
            }
        }

        public T GetAsset<T>(string key) where T : UnityEngine.Object
        {
            if (_loadedAssets.TryGetValue(key, out UnityEngine.Object asset) && asset != null)
            {
                return asset as T;
            }

            MelonLogger.Warning($"Asset {key} not found or null");
            return null;
        }

        private void EnsureAssetsPersistence()
        {
            foreach (var kvp in _loadedAssets)
            {
                if (kvp.Value is GameObject go && go != null)
                {
                    // Check if the GameObject is still alive and has a parent
                    Transform parent = go.transform.parent;
                    if (parent == null || parent.gameObject == null)
                    {
                        // Reparent to our manager to ensure persistence
                        GameObject holder = new GameObject(kvp.Key);
                        holder.transform.SetParent(this.transform);
                        go.transform.SetParent(holder.transform);
                        MelonLogger.Msg($"Re-parented orphaned asset: {kvp.Key}");
                    }
                }
            }
        }

        // The key method: Load assets in a way that mimics async loading behavior
        public T LoadAssetSafely<T>(string assetPath) where T : UnityEngine.Object
        {
            string cacheKey = $"{typeof(T).Name}_{assetPath}";

            // Check if already loaded
            T existing = GetAsset<T>(cacheKey);
            if (existing != null)
            {
                return existing;
            }

            // Not found in cache, try to load it
            if (_mainBundle == null)
            {
                MelonLogger.Error("Asset bundle is null when trying to load asset");
                return null;
            }

            try
            {
                // First try direct loading
                var asset = _mainBundle.LoadAsset(assetPath, Il2CppType.Of<T>());
                if (asset != null)
                {
                    // For GameObjects, we need to instantiate and parent to our manager
                    if (typeof(T) == typeof(GameObject))
                    {
                        GameObject prefab = asset.Cast<GameObject>();
                        GameObject instance = Instantiate(prefab);
                        instance.name = $"{System.IO.Path.GetFileNameWithoutExtension(assetPath)}_instance";

                        // Parent to a child of our manager to ensure persistence
                        GameObject holder = new GameObject(cacheKey);
                        holder.transform.SetParent(this.transform);
                        instance.transform.SetParent(holder.transform);

                        // Register the instantiated object
                        RegisterAsset(cacheKey, instance);

                        return instance.Cast<T>();
                    }
                    else
                    {
                        // For non-GameObject assets, just register them
                        RegisterAsset(cacheKey, asset);
                        return asset.Cast<T>();
                    }
                }

                // Try fuzzy matching if direct loading failed
                string[] assetNames = _mainBundle.GetAllAssetNames();
                foreach (var name in assetNames)
                {
                    if (name.EndsWith(assetPath, StringComparison.OrdinalIgnoreCase))
                    {
                        var fuzzyAsset = _mainBundle.LoadAsset(name);
                        if (fuzzyAsset != null)
                        {
                            MelonLogger.Msg($"Found asset with fuzzy match: {name}");

                            // Handle the same way as direct loading
                            if (typeof(T) == typeof(GameObject))
                            {
                                GameObject prefab = fuzzyAsset.Cast<GameObject>();
                                GameObject instance = Instantiate(prefab);
                                instance.name = $"{System.IO.Path.GetFileNameWithoutExtension(name)}_instance";

                                GameObject holder = new GameObject(cacheKey);
                                holder.transform.SetParent(this.transform);
                                instance.transform.SetParent(holder.transform);

                                RegisterAsset(cacheKey, instance);
                                return instance.Cast<T>();
                            }
                            else
                            {
                                RegisterAsset(cacheKey, fuzzyAsset);
                                return fuzzyAsset.Cast<T>();
                            }
                        }
                    }
                }

                MelonLogger.Error($"Failed to load asset: {assetPath}");
                return null;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"Exception loading asset {assetPath}: {ex.Message}");
                return null;
            }
        }
    }
}
