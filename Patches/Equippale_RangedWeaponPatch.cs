using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreGuns.Patches
{
    [HarmonyPatch]
    public static class Equippale_RangedWeaponPatch
    {
        private static float timeSinceLastAutoFure = 0F;
    }
}
