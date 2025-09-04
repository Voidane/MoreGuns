using HarmonyLib;
using MelonLoader;
using MoreGunsMono.Guns;
using ScheduleOne.UI.Shop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreGunsMono.Patches
{
    [HarmonyPatch(typeof(ShopInterface), "Awake")]
    public static class ArmsDealerInterfacePatch
    {

        private static bool _isInititalized = false;
        private static string _shopName = "ArmsDealerInterface";

        public static void Prefix(ShopInterface __instance)
        {
            if (__instance.gameObject.name != _shopName) return;
            if (_isInititalized) return;

            foreach (WeaponBase weapon in WeaponBase.allWeapons)
            {
                __instance.Listings.Add(weapon.gunShop);
                __instance.Listings.Add(weapon.magShop);
                MelonLogger.Msg($"Added {weapon.ID} to the store");
            }

            _isInititalized = true;
        }
    }
}
