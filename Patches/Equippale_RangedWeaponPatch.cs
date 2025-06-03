using HarmonyLib;
using MelonLoader;
using MoreGunsMono.Gui;
using MoreGunsMono.Guns;
using ScheduleOne;
using ScheduleOne.Audio;
using ScheduleOne.AvatarFramework;
using ScheduleOne.DevUtilities;
using ScheduleOne.Equipping;
using ScheduleOne.Tools;
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
        private static float timeSinceLastAutoFire = 0f;
        private static float timeSinceWindingUp = 0f;

        [HarmonyPatch(typeof(Equippable_RangedWeapon), "UpdateInput")]
        [HarmonyPostfix]
        public static void Postfix(Equippable_RangedWeapon __instance)
        {
            if (Time.timeScale == 0f || Singleton<PauseMenu>.Instance.IsPaused)
                return;

            if (__instance.gameObject.TryGetComponent<GunSettings>(out GunSettings settings))
            {
                bool isAttemptingToShoot = GameInput.GetButton(GameInput.ButtonCode.PrimaryClick);
                bool isWindingUp = GameInput.GetButton(GameInput.ButtonCode.SecondaryClick);

                if (settings.requiredWindup)
                {
                    PlayAnimation anim = __instance.transform.GetChild(0).GetComponent<PlayAnimation>();
                    AudioSourceController windupSound = anim.transform.Find("Windup Sound").GetComponent<AudioSourceController>();
                    AudioSourceController shutdownSound = anim.transform.Find("Shutdown Sound").GetComponent<AudioSourceController>();

                    timeSinceWindingUp += Time.deltaTime;
                    WindupIndicator.SetValueByTime(timeSinceWindingUp, settings.windupTime);

                    if (isWindingUp)
                    {
                        if (timeSinceWindingUp <= settings.windupTime || !isAttemptingToShoot)
                        {
                            // TODO : Hardcoded windup
                            anim.Play("MiniGun Windup");
                            
                            if (!windupSound.isPlaying)
                                windupSound.Play();
                        }
                    }
                    else
                    {
                        WindupIndicator.SetValue(0);
                        timeSinceWindingUp = 0F;
                        windupSound.Stop();
                    }
                }

                if (settings.isAutomatic && (settings.requiredWindup && timeSinceWindingUp > settings.windupTime) || settings.isAutomatic && !settings.requiredWindup)
                {
                    timeSinceLastAutoFire += Time.deltaTime;
                    if (isAttemptingToShoot)
                    {
                        if (timeSinceLastAutoFire >= __instance.FireCooldown)
                        {
                            timeSinceLastAutoFire = 0F;
                            if ((bool)AccessTools.Method(typeof(Equippable_RangedWeapon), "CanFire").Invoke(__instance, new object[] { false }))
                            {
                                if (__instance.Ammo > 0)
                                {
                                    if (!__instance.MustBeCocked || __instance.IsCocked)
                                    {
                                        AccessTools.Method(typeof(Equippable_RangedWeapon), "Fire").Invoke(__instance, null);
                                    }
                                    else
                                    {
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

        [HarmonyPatch(typeof(Equippable_RangedWeapon), "Fire")]
        [HarmonyPrefix]
        public static bool Prefix(Equippable_RangedWeapon __instance)
        {
            if (__instance.gameObject.TryGetComponent<GunSettings>(out GunSettings settings))
            {
                if (settings.requiredWindup && (timeSinceWindingUp < settings.windupTime))
                {
                    return false;
                }
            }
            else
            {
            }
            return true;
        }
    }
}