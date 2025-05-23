using HarmonyLib;
using MoreGunsMono.Gui;
using ScheduleOne.DevUtilities;
using ScheduleOne.Equipping;
using ScheduleOne.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreGunsMono.Patches
{
    [HarmonyPatch]
    public static class RangedWeaponEquipPatch
    {

        [HarmonyPatch(typeof(Equippable_RangedWeapon), nameof(Equippable_RangedWeapon.Equip))]
        [HarmonyPostfix]
        public static void EnableCrosshairGuns()
        {
            Reticle.reticle.SetActive(Config.EnableCrosshairForGuns.Value);
        }

        [HarmonyPatch(typeof(Equippable_RangedWeapon), nameof(Equippable_RangedWeapon.Unequip))]
        [HarmonyPostfix]
        public static void DisableCrosshairGuns()
        {
            Reticle.reticle.SetActive(false);
        }
    }
}