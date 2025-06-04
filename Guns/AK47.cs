using Il2CppScheduleOne.Dialogue;
using Il2CppScheduleOne.ItemFramework;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreGuns.Guns
{
    public class AK47 : WeaponBase
    {
        public GunSettings settings;

        private static AK47 instance;
        public static AK47 Instance
        {
            get => instance;
        }

        public AK47()
        {
            string name = "AK47";
            string _ID = "ak47";
            Shopping gunShop = new Shopping() { purchasePrice = 15000F, displayName = "AK47", available = true, nonAvailableReason = "" };
            Shopping magShop = new Shopping() { purchasePrice = 1000F, displayName = "AK47 Magazine", available = true, nonAvailableReason = "" };

            GunSettings settings = new GunSettings();
            settings.isAutomatic.Value = true;
            settings.cameraJolt.Value = true;
            settings.speedMultiplier.Value = 1.0F;
            settings.requireWindup.Value = false;
            settings.windupTime.Value = 0.0F;
            settings.canManualyReload.Value = true;

            Init(name, _ID, gunShop, magShop, settings);
            instance = this;
        }
    }
}
