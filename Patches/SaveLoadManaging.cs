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

        }
    }

}
