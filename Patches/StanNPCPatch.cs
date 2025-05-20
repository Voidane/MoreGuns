using HarmonyLib;
using ScheduleOne.NPCs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using System.Collections;
using UnityEngine;
using ScheduleOne.Dialogue;
using MoreGunsMono.Guns;

namespace MoreGunsMono.Patches
{
    [HarmonyPatch]
    public static class StanNPCPatch
    {
        public static object coroutine;

        [HarmonyPatch(typeof(Stan), "Loaded")]
        public static void Postfix(Stan __instance)
        {
            MelonLogger.Msg("Stan Loaded in");
            MelonCoroutines.Start(InitializeStore(__instance));
        }

        public static IEnumerator InitializeStore(Stan __instance)
        {
            while (!ItemRegistryPatch.isWeaponsRegistered)
            {
                MelonLogger.Msg("Attempting to setup stan store, waiting on registries!");
                yield return new WaitForSeconds(0.5F);
            }
            
            if (MoreGunsMod.stanNPC != null)
                yield return false;

            MoreGunsMod.stanNPC = GameObject.Find("Stan/Dialogue").transform;
            if (MoreGunsMod.stanNPC == null)
            {
                MelonLogger.Msg("stan dialogue was not loaded in");
            }

            DialogueController_ArmsDealer dialogueController_ArmsDealer = MoreGunsMod.stanNPC.GetComponent<DialogueController_ArmsDealer>();
            if (dialogueController_ArmsDealer == null)
            {
                MelonLogger.Error("Dialogue comp not found");
            }

            var allWeaponsFields = typeof(DialogueController_ArmsDealer).GetField("allWeapons", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (allWeaponsFields != null)
            {
                var allWeapons = allWeaponsFields.GetValue(dialogueController_ArmsDealer) as List<DialogueController_ArmsDealer.WeaponOption>;
                if (allWeapons != null)
                {
                    foreach (WeaponBase weapon in WeaponBase.allWeapons)
                    {
                        if (!allWeapons.Contains(weapon.rangedGun))
                        {
                            allWeapons.Add(weapon.rangedGun);
                        }

                        if (!allWeapons.Contains(weapon.ammoGun))
                        {
                            allWeapons.Add(weapon.ammoGun);
                        }
                        MelonLogger.Msg($"[MoreGuns] Loaded {weapon.ID} to the store!");
                    }
                    MelonLogger.Msg("[MoreGuns] Finished adding new guns to store!");
                }
                else
                {
                    MelonLogger.Error("[MoreGuns] All weapons was null");
                }
            }
            else
            {
                MelonLogger.Error("[MoreGuns] Weapon field was null");
            }

            foreach (WeaponBase weapon in WeaponBase.allWeapons)
            {
                if (!dialogueController_ArmsDealer.RangedWeapons.Contains(weapon.rangedGun))
                {
                    dialogueController_ArmsDealer.RangedWeapons.Add(weapon.rangedGun);
                }
                if (!dialogueController_ArmsDealer.Ammo.Contains(weapon.ammoGun))
                {
                    dialogueController_ArmsDealer.Ammo.Add(weapon.ammoGun);
                }
            }
        }
    }
}
