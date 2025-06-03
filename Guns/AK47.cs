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
                Shopping gunShop = new Shopping() { purchasePrice = 15000F, displayName = "AK47", available = true, nonAvailableReason = "" };
                Shopping magShop = new Shopping() { purchasePrice = 1000F, displayName = "AK47 Magazine", available = true, nonAvailableReason = "" };
                
                GunSettings settings = new GunSettings()
                {
                    isAutomatic = true,
                    cameraJolt = true,
                    speedMultiplier = 1.0F,
                    requiredWindup = false,
                    windupTime = 0F,
                    canManuallyReload = true
                };

                Init(_ID, gunShop, magShop, settings);
                instance = this;
            }
        }
    }
}
