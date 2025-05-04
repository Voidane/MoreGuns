using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreGunsMono
{
    public static class Resource
    {
        private static Dictionary<string, UnityEngine.Object> _customResources = new Dictionary<string, UnityEngine.Object>(StringComparer.OrdinalIgnoreCase);

        public static void RegisterAsset(string path, UnityEngine.Object asset)
        {
            _customResources[path] = asset;
            MelonLogger.Msg($"Registered custom asset at path: {path}");
        }

        public static UnityEngine.Object TryGetAsset(string path)
        {
            if (_customResources.TryGetValue(path, out UnityEngine.Object asset))
            {
                return asset;
            }
            return null;
        }
    }
}
