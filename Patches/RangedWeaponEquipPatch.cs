using HarmonyLib;
using MelonLoader;
using MoreGunsMono.Gui;
using MoreGunsMono.Guns;
using ScheduleOne.DevUtilities;
using ScheduleOne.Equipping;
using ScheduleOne.PlayerScripts;
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
        public static void PostfixEquip(Equippable_RangedWeapon __instance)
        {
            Reticle.reticle.SetActive(Config.EnableCrosshairForGuns.Value);

            GunSettings settings = __instance.GetComponent<GunSettings>();
            if (settings != null)
            {
                if (settings.requiredWindup)
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