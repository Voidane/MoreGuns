using HarmonyLib;
using Il2CppScheduleOne.Equipping;
using MoreGuns.Gui;
using MoreGuns.Guns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreGuns.Patches
{
    [HarmonyPatch]
    public static class ReloadingPatch
    {
        [HarmonyPatch(typeof(Equippable_RangedWeapon), nameof(Equippable_RangedWeapon.Reload))]
        [HarmonyPrefix]
        public static bool Prefix(Equippable_RangedWeapon __instance)
        {
            GunSettings settings = __instance.gameObject.GetComponent<GunSettings>();
            if (settings != null)
            {
                if (!settings.canManualyReload)
                {
                    ReloadMessage.Show(true);
                    return false;
                }
            }
            return true;
        }
    }
}
