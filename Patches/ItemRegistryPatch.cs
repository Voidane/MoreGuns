using HarmonyLib;
using Il2CppScheduleOne;
using MelonLoader;
using MoreGuns.Guns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreGuns.Patches
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
                    __instance.AddToRegistry(weapon.gunIntItemDef);
                    MelonLogger.Msg($"Registered {weapon.ID} item definition and magazine.");
                }
            }
            isWeaponsRegistered = true;
        }
    }
}
