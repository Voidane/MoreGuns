using HarmonyLib;
using MelonLoader;
using ScheduleOne.AvatarFramework;
using ScheduleOne.AvatarFramework.Equipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreGunsMono.Patches
{
    [HarmonyPatch]
    public static class SetEquippablePatch
    {
        [HarmonyPatch(typeof(Avatar), nameof(Avatar.SetEquippable))]
        [HarmonyPrefix]
        public static bool Prefix(ref AvatarEquippable __result, string assetPath, Avatar __instance)
        {
            // Check if the original Resources.Load can handle it
            GameObject gameObject = Resources.Load(assetPath) as GameObject;
            if (gameObject != null)
            {
                return true;
            }

            // Handle empty path case
            if (string.IsNullOrEmpty(assetPath))
            {
                return true;
            }

            // IMPORTANT: Unequip current equippable
            if (__instance.CurrentEquippable != null)
            {
                __instance.CurrentEquippable.Unequip();
            }

            // CRITICAL: Set the CurrentEquippable property on the instance
            GameObject asset = Resource.TryGetAsset(assetPath) as GameObject;
            MelonLogger.Msg("Attempting to load custom asset");

            if (asset == null)
            {
                MelonLogger.Error("asset couldntbe found");
            }

            GameObject equippable = UnityEngine.Object.Instantiate(asset, null);
            AvatarEquippable avatarEquippable = equippable.GetComponent<AvatarEquippable>();

            if (avatarEquippable == null)
            {
                MelonLogger.Msg("avatar equip was null");
            }

            MelonLogger.Msg("Successfully instantiated custom equippable");
            AccessTools.Property(typeof(Avatar), "CurrentEquippable").SetValue(__instance, avatarEquippable);

            avatarEquippable.Equip(__instance);
            __result = avatarEquippable;
            return false; 
        }
    }
}
