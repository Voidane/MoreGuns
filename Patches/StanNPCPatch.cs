using HarmonyLib;
using Il2CppScheduleOne.NPCs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using System.Collections;
using UnityEngine;
using Il2CppScheduleOne.Dialogue;
using MoreGuns.Guns;

namespace MoreGuns.Patches
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

            MelonLogger.Msg("Registries finished. Loading stan dialogue");

            if (MoreGunsMod.stanNPC != null)
                yield return null;

            MoreGunsMod.stanNPC = GameObject.Find("Stan/Dialogue").transform;

            if (MoreGunsMod.stanNPC != null)
            {
                MelonLogger.Msg("stan dialogue finished loading.");

            }
            else
            {
                MelonLogger.Msg("stan dialogue was not loaded in");
                yield return null;
            }

            DialogueController_ArmsDealer dialogueController_ArmsDealer = MoreGunsMod.stanNPC.GetComponent<DialogueController_ArmsDealer>();

            if (dialogueController_ArmsDealer != null)
            {
                MelonLogger.Msg("Dialogue comp found");
            }
            else
            {
                MelonLogger.Error("Dialogue comp not found");
            }

            var allWeapons = dialogueController_ArmsDealer.allWeapons;
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
