using HarmonyLib;
using Il2CppScheduleOne;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Equipping;
using Il2CppScheduleOne.UI;
using MelonLoader;
using MoreGuns.Guns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreGuns.Patches
{
    [HarmonyPatch]
    public static class Equippalbe_RangedWeaponPatch
    {
        private static float timeSinceLastAutoFire = 0F;

        [HarmonyPatch(typeof(Equippable_RangedWeapon), "UpdateInput")]
        [HarmonyPostfix]
        public static void Postfix(Equippable_RangedWeapon __instance)
        {
            if (Time.timeScale == 0f || Singleton<PauseMenu>.Instance.IsPaused)
                return;

            if (__instance.gameObject.TryGetComponent<GunSettings>(out GunSettings settings))
            {
                if (settings.isAutomatic.Value)
                {
                    timeSinceLastAutoFire += Time.deltaTime;

                    if (GameInput.GetButton(GameInput.ButtonCode.PrimaryClick))
                    {
                        if (timeSinceLastAutoFire >= __instance.FireCooldown)
                        {
                            timeSinceLastAutoFire = 0F;

                            if (__instance.CanFire(false))
                            {
                                if (__instance.Ammo > 0)
                                {
                                    if (!__instance.MustBeCocked || __instance.IsCocked)
                                    {
                                        __instance.Fire();
                                    }
                                    else
                                    {
                                        __instance.Cock();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        timeSinceLastAutoFire = __instance.FireCooldown;
                    }
                }
            }
        }
    }
}
