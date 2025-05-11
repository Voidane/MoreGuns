namespace MoreGunsMono.Guns
{
    public class AK47 : WeaponBase
    {
        private static AK47 instance;
        public static AK47 Instance
        {
            get => instance;
        }

        public AK47(string ID, Shopping gunShop, Shopping magShop)
        {
            if (instance == null)
            {
                Init(ID, gunShop, magShop);
                instance = this;
            }
        }
    }
}
