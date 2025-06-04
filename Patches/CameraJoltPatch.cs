using HarmonyLib;
using Il2CppScheduleOne.Equipping;
using Il2CppScheduleOne.PlayerScripts;
using MoreGuns.Guns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreGuns.Patches
{
    [HarmonyPatch]
    public static class CameraJoltPatch
    {
        public static string ID = "";

        [HarmonyPatch(typeof(PlayerCamera), "JoltCamera")]
        [HarmonyPrefix]
        public static bool Prefix(PlayerCamera __instance)
        {
            if (WeaponBase.weaponsByName.TryGetValue(ID, out var gun))
            {
                if (!gun.settings.cameraJolt)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }

        [HarmonyPatch(typeof(Equippable_RangedWeapon), "Fire")]
        [HarmonyPrefix]
        public static void Prefix(Equippable_RangedWeapon __instance)
        {
            ID = __instance.gameObject.name.Replace("_Equippable(Clone)", "").ToLower();
        }
    }
}
