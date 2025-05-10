using HarmonyLib;
using MelonLoader;
using MoreGunsMono.Guns;
using ScheduleOne.Trash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreGunsMono.Patches
{
    [HarmonyPatch]
    public static class TrashRegistryPatch
    {
        [HarmonyPatch(typeof(TrashManager), "Start")]
        [HarmonyPrefix]
        public static void Prefix(TrashManager __instance)
        {
            MelonLogger.Msg("Trash manager started");
            List<TrashItem> i = __instance.TrashPrefabs.ToList();
            i.Add(AK47.AK47MagTrashItem);
            __instance.TrashPrefabs = i.ToArray();

            foreach (TrashItem item in __instance.TrashPrefabs)
            {
                MelonLogger.Msg("Trash:" + item.name);
            }
        }
    }
}
