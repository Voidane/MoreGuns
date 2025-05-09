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
            if (!isWeaponsRegistered && AK47.AK47MagazineIntItemDef != null)
            {
                __instance.AddToRegistry(AK47.AK47MagazineIntItemDef);
                MelonLogger.Msg("Registered ak47mag");
                __instance.AddToRegistry(AK47.AK47IntItemDef);
                MelonLogger.Msg("Registered ak47");
                isWeaponsRegistered = true;
            }
        }
    }
}
