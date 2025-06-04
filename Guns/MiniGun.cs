using Il2CppSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreGuns.Guns
{
    public class MiniGun : WeaponBase
    {
        public GunSettings settings;

        private static MiniGun instance;
        public static MiniGun Instance
        {
            get => instance;
        }

        public MiniGun()
        {
            if (instance == null)
            {
                string name = "MiniGun";
                string _ID = "minigun";
                Shopping gunShop = new Shopping() { purchasePrice = 75000F, displayName = "MiniGun", available = true, nonAvailableReason = "" };
                Shopping magShop = new Shopping() { purchasePrice = 10000F, displayName = "MiniGun Magazine", available = true, nonAvailableReason = "" };

                GunSettings settings = new GunSettings();
                settings.isAutomatic.Value = true;
                settings.cameraJolt.Value = false;
                settings.speedMultiplier.Value = 0.75F;
                settings.requireWindup.Value = true;
                settings.windupTime.Value = 2.0F;
                settings.canManualyReload.Value = false;

                Init(name, _ID, gunShop, magShop, settings);
                instance = this;
            };
        }
    }
}
