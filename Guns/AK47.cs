using ScheduleOne.Equipping;
using ScheduleOne.ItemFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MelonLoader;
using ScheduleOne.Storage;
using MelonLoader;
using UnityEngine.Events;
using ScheduleOne.Dialogue;

namespace MoreGunsMono.Guns
{
    public static class AK47 
    {
        public static GameObject _AK47_Equippable;
        public static IntegerItemDefinition _AK47_Magazine_IntegerItemDefiniition;
        public static IntegerItemDefinition _AK47_IntegerItemDefiniition;
        public static DialogueController_ArmsDealer.WeaponOption rangedAK47;
        public static DialogueController_ArmsDealer.WeaponOption ammoAK47;

        public static void InitializeAK47(GameObject ak47, IntegerItemDefinition ak47MagazineIntegerItemDefiniition, IntegerItemDefinition ak47IntegerItemDefinition)
        {
            GunSettings settings = ak47.AddComponent<GunSettings>();
            settings.isAutomatic = true;

            _AK47_Equippable = ak47;
            _AK47_Magazine_IntegerItemDefiniition = ak47MagazineIntegerItemDefiniition;
            _AK47_IntegerItemDefiniition = ak47IntegerItemDefinition;

            SetCustomItemUI();
            CreateDialogueControllgerOptions();
        }

        private static void SetCustomItemUI()
        {
            var defintion = Resources.Load("Weapons/M1911/M1911") as ItemDefinition;
            
            if (defintion == null)
            {
                MelonLogger.Error("m1911 couldnt be loaded? Unable to register UI to custom guns!");
            }
            else
            {
                _AK47_IntegerItemDefiniition.CustomItemUI = defintion.CustomItemUI;
                _AK47_Magazine_IntegerItemDefiniition.CustomItemUI = defintion.CustomItemUI;
            }
        }

        private static void CreateDialogueControllgerOptions()
        {
            rangedAK47 = new DialogueController_ArmsDealer.WeaponOption
            {
                Name = "AK47",
                Price = 15000F,
                IsAvailable = true,
                NotAvailableReason = "",
                Item = _AK47_IntegerItemDefiniition
            };

            ammoAK47 = new DialogueController_ArmsDealer.WeaponOption
            {
                Name = "AK47 Magazine (Medium)",
                Price = 1000F,
                IsAvailable = true,
                NotAvailableReason = "",
                Item = _AK47_Magazine_IntegerItemDefiniition
            };
        }
    }
}
