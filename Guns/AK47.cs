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
        private static AK47 instance;
        public static AK47 Instance
        {
            get => instance;
        }

        public AK47(string ID, Shopping gunShop, Shopping magShop)
        {
            Init(ID, gunShop, magShop);
            instance = this;
        }
    }
}
