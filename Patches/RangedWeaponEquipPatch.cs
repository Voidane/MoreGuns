using HarmonyLib;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Equipping;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppVLB;
using MoreGuns.Gui;
using MoreGuns.Guns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreGuns.Patches
{
    [HarmonyPatch]
    public static class RangedWeaponEquipPatch
    {
        [HarmonyPatch(typeof(Equippable_RangedWeapon), nameof(Equippable_RangedWeapon.Equip))]
        [HarmonyPostfix]
        public static void PostfixEquip(Equippable_RangedWeapon __instance)
        {
            Reticle.reticle.SetActive(Config.EnableCrosshairForGuns.Value);

            GunSettings settings = __instance.GetComponent<GunSettings>();
            if (settings != null)
            {
                if (settings.requireWindup)
                {
                    WindupIndicator.Show(true);
                }
                PlayerSingleton<PlayerMovement>.Instance.MoveSpeedMultiplier = settings.speedMultiplier;
            }
        }

        [HarmonyPatch(typeof(Equippable_RangedWeapon), nameof(Equippable_RangedWeapon.Unequip))]
        [HarmonyPostfix]
        public static void PostfixUnequip(Equippable_RangedWeapon __instance)
        {
            Reticle.reticle.SetActive(false);
            WindupIndicator.Show(false);
            PlayerSingleton<PlayerMovement>.Instance.MoveSpeedMultiplier = 1.0F;
        }
    }
}
