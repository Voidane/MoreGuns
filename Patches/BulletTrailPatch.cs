using HarmonyLib;
using Il2CppScheduleOne.FX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreGuns.Patches
{
    [HarmonyPatch]
    public static class BulletTrailPatch
    {
        [HarmonyPatch(typeof(FXManager), "CreateBulletTrail")]
        [HarmonyPrefix]
        public static bool Prefix(FXManager __instance, Vector3 start, Vector3 dir, float speed, float range, LayerMask mask)
        {
            return false;
        }
    }
}
