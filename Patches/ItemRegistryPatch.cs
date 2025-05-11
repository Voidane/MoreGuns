using HarmonyLib;
using MelonLoader;
using MoreGunsMono.Guns;
using ScheduleOne;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreGunsMono.Patches
{
    [HarmonyPatch(typeof(Registry), nameof(Registry._GetItem))]
    public static class ItemRegistryPatch
    {
        public static bool isWeaponsRegistered = false;

        public static void Prefix(Registry __instance, string ID)
        {
            if (!isWeaponsRegistered)
            {
                foreach (WeaponBase weapon in WeaponBase.allWeapons)
                {
                    __instance.AddToRegistry(weapon.magIntItemDef);
                    MelonLogger.Msg($"Registered {weapon.ID} magazine item def");
                    __instance.AddToRegistry(weapon.gunIntItemDef);
                    MelonLogger.Msg($"Registered {weapon.ID} item def");
                }
            }
            isWeaponsRegistered = true;
        }
    }
}