using HarmonyLib;
using Il2CppScheduleOne.Trash;
using MelonLoader;
using MoreGuns.Guns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreGuns.Patches
{
    [HarmonyPatch]
    public static class TrashRegistryPatch
    {
        [HarmonyPatch(typeof(TrashManager), "Start")]
        [HarmonyPrefix]
        public static void Prefix(TrashManager __instance)
        {
            List<TrashItem> allTrashItems = __instance.TrashPrefabs.ToList();
            foreach (WeaponBase weapon in WeaponBase.allWeapons)
            {
                if (!allTrashItems.Contains(weapon.gunMagTrashItem))
                {
                    MelonLogger.Msg($"Added {weapon.ID} gun trash to TrashManager");
                    allTrashItems.Add(weapon.gunMagTrashItem);
                }
            }
            __instance.TrashPrefabs = allTrashItems.ToArray();
        }
    }
}
