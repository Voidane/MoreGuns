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
        public static bool hasPatchBeenRan = false;

        public static void Prefix(Registry __instance, string ID)
        {
            MelonLogger.Msg("Registry is patched now!");

            if (!isWeaponsRegistered)
            {
                if (AK47.AK47MagazineIntItemDef == null)
                {
                    MelonLogger.Msg("re implementing guns");
                    MelonCoroutines.Start(MoreGunsMod.LoadAssetBundleCoroutine());
                }
            }

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
