using HarmonyLib;
using MelonLoader;
using MoreGunsMono.Guns;
using ScheduleOne.Equipping;
using ScheduleOne.PlayerScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreGunsMono.Patches
{
    [HarmonyPatch]
    public static class CameraJoltPatch
    {

        public static string ID = "";

        [HarmonyPatch(typeof(PlayerCamera), "JoltCamera")]
        [HarmonyPrefix]
        public static bool Prefix(PlayerCamera __instance)
        {
            MelonLogger.Msg($"JOLTED! From Gun: {ID}");
            
            if (WeaponBase.weaponsByName.TryGetValue(ID, out WeaponBase gun))
            {
                if (!gun.settings.cameraJolt)
                {
                    return false;
                }
                return true;
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
