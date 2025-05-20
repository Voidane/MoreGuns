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
        [HarmonyPatch(typeof(ScheduleOne.AvatarFramework.Avatar), nameof(ScheduleOne.AvatarFramework.Avatar.SetEquippable))]
        [HarmonyPrefix]
        public static bool Prefix(ref AvatarEquippable __result, string assetPath, ScheduleOne.AvatarFramework.Avatar __instance)
        {
            GameObject gameObject = Resources.Load(assetPath) as GameObject;
            if (gameObject != null)
            {
                return true;
            }

            if (string.IsNullOrEmpty(assetPath))
            {
                return true;
            }

            if (__instance.CurrentEquippable != null)
            {
                __instance.CurrentEquippable.Unequip();
            }

            GameObject asset = Resource.TryGetAsset(assetPath) as GameObject;
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

            AccessTools.Property(typeof(ScheduleOne.AvatarFramework.Avatar), "CurrentEquippable").SetValue(__instance, avatarEquippable);

            avatarEquippable.Equip(__instance);
            __result = avatarEquippable;
            return false; 
        }
    }
}
