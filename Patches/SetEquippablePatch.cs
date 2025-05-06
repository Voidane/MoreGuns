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
        [HarmonyPatch(typeof(Avatar), nameof(Avatar.SetEquippable))]
        [HarmonyPrefix]
        public static bool Prefix(ref AvatarEquippable __result, string assetPath, Avatar __instance)
        {
            // Paths may be null or empty, if so return to original method
            if (string.IsNullOrEmpty(assetPath.Trim()))
            {
                return true;
            }

            UnityEngine.Object il2cppResourceAsset = Resources.Load(assetPath);
            if (il2cppResourceAsset != null)
            {
                MelonLogger.Msg($"{assetPath} asset was loaded.");
                return true;
            }
            else
            {
                MelonLogger.Msg($"{assetPath} Custom Asset Selected.");
            }

            if (__instance.CurrentEquippable != null)
            {
                __instance.CurrentEquippable.Unequip();
            }

            UnityEngine.Object UEObjectAsset = MoreGunsMod.TryGetAsset(assetPath);
            if (UEObjectAsset == null)
            {
                MelonLogger.Warning("Could not find custom asset.");
                return true;
            }
            
            GameObject GOAsset = UEObjectAsset.Cast<GameObject>();
            if (GOAsset == null)
            {
                MelonLogger.Warning("GOAsset was null");
                return true;
            }
            
            GameObject equippable = UnityEngine.Object.Instantiate(GOAsset);
            if (equippable == null)
            {
                MelonLogger.Warning("equippable was null");
                return true;
            }

            AvatarEquippable avatarEquippable = equippable.GetComponent<AvatarEquippable>();
            if (equippable == null)
            {
                MelonLogger.Warning("avatar equippable was null");
                return true;
            }

            __instance.CurrentEquippable = avatarEquippable;
            avatarEquippable.Equip(__instance);
            __result = avatarEquippable;

            MelonLogger.Msg($"Finished loading asset on equip.");
            return false;
        }
    }
}
