using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreGunsMono.Guns
{
    public abstract class WeaponBase
    {
        public static GameObject GunEquippable;

        public static bool CheckAssetLoaded(UnityEngine.Object asset, string assetName, string weaponName)
        {
            if (asset == null)
            {
                MelonLogger.Error($"Could not load asset: {assetName}");
                MoreGunsMod.StopProcess();
                return false;
            }
            else
            {
                MelonLogger.Msg($"Loaded asset for {weaponName} : {assetName}");
                return true;
            }
        }
    }
}
