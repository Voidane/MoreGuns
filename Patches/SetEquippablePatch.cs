using HarmonyLib;
using Il2CppScheduleOne.AvatarFramework.Equipping;
using Il2CppScheduleOne.AvatarFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using UnityEngine;
using Il2CppScheduleOne.ItemFramework;

namespace MoreGuns.Patches
{
    [HarmonyPatch]
    public static class SetEquippablePatch
    {
        [HarmonyPatch(typeof(Il2CppScheduleOne.AvatarFramework.Avatar), nameof(Il2CppScheduleOne.AvatarFramework.Avatar.SetEquippable))]
        [HarmonyPrefix]
        public static bool Prefix(ref AvatarEquippable __result, string assetPath, Il2CppScheduleOne.AvatarFramework.Avatar __instance)
        {
            // Paths may be null or empty, if so return to original method
            if (string.IsNullOrEmpty(assetPath.Trim()))
            {
                return true;
            }

            UnityEngine.Object il2cppResourceAsset = Resources.Load(assetPath);
            if (il2cppResourceAsset != null)
            {
                return true;
            }

            if (__instance.CurrentEquippable != null)
            {
                __instance.CurrentEquippable.Unequip();
            }

            UnityEngine.Object UEObjectAsset = MoreGunsMod.TryGetAsset(assetPath);
            if (UEObjectAsset == null)
            {
                return true;
            }
            
            GameObject GOAsset = UEObjectAsset.Cast<GameObject>();
            if (GOAsset == null)
            {
                return true;
            }
            
            GameObject equippable = UnityEngine.Object.Instantiate(GOAsset);
            if (equippable == null)
            {
                return true;
            }

            AvatarEquippable avatarEquippable = equippable.GetComponent<AvatarEquippable>();
            if (equippable == null)
            {
                return true;
            }

            __instance.CurrentEquippable = avatarEquippable;
            avatarEquippable.Equip(__instance);
            __result = avatarEquippable;

            return false;
        }
    }
}
