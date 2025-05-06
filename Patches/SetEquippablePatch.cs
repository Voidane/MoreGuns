using HarmonyLib;
using Il2CppScheduleOne.AvatarFramework.Equipping;
using Il2CppScheduleOne.AvatarFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;

namespace MoreGuns.Patches
{
    [HarmonyPatch]
    public static class SetEquippablePatch
    {
        [HarmonyPatch(typeof(Avatar), nameof(Avatar.SetEquippable))]
        [HarmonyPrefix]
        public static bool Prefix(ref AvatarEquippable __result, string assetPath, Avatar __instance)
        {
            MelonLogger.Msg("Asset Path: " + assetPath);
            return true;
        }
    }
}
