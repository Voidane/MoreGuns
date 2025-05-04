using HarmonyLib;
using MelonLoader;
using MoreGunsMono.Guns;
using ScheduleOne;
using ScheduleOne.AvatarFramework;
using ScheduleOne.DevUtilities;
using ScheduleOne.Equipping;
using ScheduleOne.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreGunsMono.Patches
{
    [HarmonyPatch]
    public static class Equippale_RangedWeaponPatch
    {
        private static int counter;
        private static float timeSinceLastAutoFire = 0f;

        [HarmonyPatch(typeof(Equippable_RangedWeapon), "UpdateInput")]
        [HarmonyPostfix]
        public static void Postfix(Equippable_RangedWeapon __instance)
        {
            if (Time.timeScale == 0f || Singleton<PauseMenu>.Instance.IsPaused)
                return;

            // MelonLogger.Msg("UpdateInputPostfix patch running");

            if (__instance.gameObject.TryGetComponent<GunSettings>(out GunSettings settings))
            {
                // MelonLogger.Msg("Gun has settings");
                if (settings.isAutomatic)
                {
                    // MelonLogger.Msg("Gun is automatic");
                    timeSinceLastAutoFire += Time.deltaTime;

                    if (GameInput.GetButton(GameInput.ButtonCode.PrimaryClick))
                    {
                        if (timeSinceLastAutoFire >= __instance.FireCooldown)
                        {
                            timeSinceLastAutoFire = 0F;
                            // MelonLogger.Msg("Canfire check method?");

                            if ((bool)AccessTools.Method(typeof(Equippable_RangedWeapon), "CanFire").Invoke(__instance, new object[] { false }))
                            {
                                // MelonLogger.Msg("Yes");
                                if (__instance.Ammo > 0)
                                {
                                    // MelonLogger.Msg("Ammo above 0, Gun shot " + counter++);
                                    // Check if it needs to be cocked
                                    if (!__instance.MustBeCocked || __instance.IsCocked)
                                    {
                                        // Access Fire method through reflection since it's public
                                        AccessTools.Method(typeof(Equippable_RangedWeapon), "Fire").Invoke(__instance, null);
                                    }
                                    else
                                    {
                                        // Access Cock method through reflection since it's private
                                        AccessTools.Method(typeof(Equippable_RangedWeapon), "Cock").Invoke(__instance, null);
                                    }
                                }
                            }
                            else
                            {
                                // MelonLogger.Msg("No");
                            }
                        }
                    }
                    else
                    {
                        timeSinceLastAutoFire = __instance.FireCooldown;
                    }
                }
                else
                {
                    // MelonLogger.Msg("Gun is not automatic");
                }
            }
        }
    }
}
