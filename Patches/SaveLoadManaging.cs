using HarmonyLib;
using ScheduleOne.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using ScheduleOne;
using MoreGunsMono.Guns;

namespace MoreGunsMono.Patches
{
    public class SaveLoadManaging
    {
        
    }

    [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.Save), new Type[] { typeof(string) })]
    public static class SaveManager_Save_Patch
    {
        public static void Postfix(SaveManager __instance, string saveFolderPath)
        {
            
        }
    }

    [HarmonyPatch(typeof(LoadManager), nameof(LoadManager.StartGame))]
    public static class LoadStartPatch
    {
        public static void Postfix(LoadManager __instance, SaveInfo info, bool allowLoadStacking)
        {
            MelonLogger.Msg("Loadmanager start game");
        }
    }

    [HarmonyPatch(typeof(Registry), nameof(Registry._GetItem))]
    public static class RegisterItemsBeforeLoad
    {
        public static bool isWeaponsRegistered = false;

        public static void Prefix(Registry __instance, string ID)
        {
            if (!isWeaponsRegistered && AK47._AK47_Magazine_IntegerItemDefiniition != null)
            {
                __instance.AddToRegistry(AK47._AK47_Magazine_IntegerItemDefiniition);
                MelonLogger.Msg("Registered ak47mag");
                __instance.AddToRegistry(AK47._AK47_IntegerItemDefiniition);
                MelonLogger.Msg("Registered ak47");
                isWeaponsRegistered = true;
            }
        }
    }

}
