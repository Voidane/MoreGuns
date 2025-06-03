using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreGunsMono.Guns
{
    public class MiniGun : WeaponBase
    {
        private static MiniGun instance;
        public static MiniGun Instance
        {
            get => instance;
        }

        public MiniGun()
        {
            if (instance == null)
            {
                string _ID = "minigun";
                Shopping gunShop = new Shopping() { purchasePrice = 75000F, displayName = "MiniGun", available = true, nonAvailableReason = ""  };
                Shopping magShop = new Shopping() { purchasePrice = 10000F, displayName = "MiniGun Magazine", available = true, nonAvailableReason = ""  };

                GunSettings settings = new GunSettings() 
                { 
                    isAutomatic = true,
                    speedMultiplier = 0.75F,
                    cameraJolt = false,
                    requiredWindup = true,
                    windupTime = 2.0F,
                    canManuallyReload = false
                };

                Init(_ID, gunShop, magShop, settings);
                instance = this;
            }
        }
    }
}
