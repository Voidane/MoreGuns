using ScheduleOne.UI.Shop;

namespace MoreGunsMono.Guns
{
    public class AK47 : WeaponBase
    {
        private static AK47 instance;
        public static AK47 Instance
        {
            get => instance;
        }

        public AK47()
        {
            if (instance == null)
            {
                string _ID = "ak47";

                GunSettings settings = new GunSettings()
                {
                    isAutomatic = true,
                    cameraJolt = true,
                    speedMultiplier = 1.0F,
                    requiredWindup = false,
                    windupTime = 0F,
                    canManuallyReload = true
                };

                Init(_ID, settings);
                instance = this;
            }
        }
    }
}
