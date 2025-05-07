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
    public static class RegistryPatch
    {
        public static bool isWeaponsRegistered = false;
        private static readonly object _mulock = new object();

        public static void Prefix(Registry __instance, string ID)
        {
            if (isWeaponsRegistered)
                return;

            lock (_mulock)
            {
                if (isWeaponsRegistered)
                    return;

                __instance.AddToRegistry(AK47.AK47MagazineIntItemDef);
                MelonLogger.Msg("Registered ak47mag");
                __instance.AddToRegistry(AK47.AK47IntItemDef);
                MelonLogger.Msg("Registered ak47");

                isWeaponsRegistered = true;
            }
        }
    }
}
