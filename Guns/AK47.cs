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
using ScheduleOne.DevUtilities;
using ScheduleOne.Trash;

namespace MoreGunsMono.Guns
{
    public static class AK47 
    {
        public static GameObject AK47_Equippable;
        public static Equippable_RangedWeapon AK47RangedWeapon;

        public static IntegerItemDefinition AK47IntItemDef;
        public static IntegerItemDefinition AK47MagazineIntItemDef;
        
        public static DialogueController_ArmsDealer.WeaponOption rangedAK47;
        public static DialogueController_ArmsDealer.WeaponOption ammoAK47;

        public static GameObject AK47MagTrash;
        public static TrashItem AK47MagTrashItem;

        public static void InitializeAK47(GameObject ak47, IntegerItemDefinition ak47MagazineIntegerItemDefiniition, IntegerItemDefinition ak47IntegerItemDefinition, GameObject ak47MagTrash)
        {
            GunSettings settings = ak47.AddComponent<GunSettings>();
            settings.isAutomatic = true;

            AK47_Equippable = ak47;
            AK47RangedWeapon = AK47_Equippable.GetComponent<Equippable_RangedWeapon>();

            AK47MagazineIntItemDef = ak47MagazineIntegerItemDefiniition;
            AK47IntItemDef = ak47IntegerItemDefinition;
            
            AK47MagTrash = ak47MagTrash;
            AK47MagTrashItem = AK47MagTrash.GetComponent<TrashItem>();

            SetCustomItemUI();
            UpdateSettingsFromConfig();
            CreateDialogueControllerOptions();
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
                AK47IntItemDef.CustomItemUI = defintion.CustomItemUI;
                AK47MagazineIntItemDef.CustomItemUI = defintion.CustomItemUI;
            }
        }

        private static void CreateDialogueControllerOptions()
        {
            rangedAK47 = new DialogueController_ArmsDealer.WeaponOption
            {
                Name = Config.ak47ItemName.Value,
                Price = Config.ak47PurchasePrice.Value,
                IsAvailable = true,
                NotAvailableReason = "",
                Item = AK47IntItemDef
            };

            ammoAK47 = new DialogueController_ArmsDealer.WeaponOption
            {
                Name = Config.ak47MagItemName.Value,
                Price = Config.ak47magPurchasePrice.Value,
                IsAvailable = true,
                NotAvailableReason = "",
                Item = AK47MagazineIntItemDef
            };
        }

        private static void UpdateSettingsFromConfig()
        {
            AK47RangedWeapon.Damage = Config.ak47Damage.Value;
            AK47RangedWeapon.ImpactForce = Config.ak47ImpactForce.Value;
            AK47RangedWeapon.AimFOVReduction = Config.ak47aimFOVReduction.Value;
            AK47RangedWeapon.AccuracyChangeDuration = Config.ak47accuracyChangeDuration.Value;
            AK47RangedWeapon.MagazineSize = Config.ak47magazineSize.Value;

            AK47IntItemDef.Name = Config.ak47DisplayItemName.Value;
            AK47IntItemDef.Description = Config.ak47DisplayDescription.Value;
            AK47IntItemDef.LabelDisplayColor = Config.ak47LabelDisplayColor.Value;
            AK47IntItemDef.legalStatus = Config.ak47LegalStatus.Value;
            AK47IntItemDef.RequiredRank = Config.ak47requiredRank.Value;

            AK47MagazineIntItemDef.Name = Config.ak47MagDisplayItemName.Value;
            AK47MagazineIntItemDef.Description = Config.ak47MagDisplayDescription.Value;
            AK47MagazineIntItemDef.LabelDisplayColor = Config.ak47LabelDisplayColor.Value;
            AK47MagazineIntItemDef.legalStatus = Config.ak47MagLegalStatus.Value;
            AK47MagazineIntItemDef.RequiredRank = Config.ak47MagrequiredRank.Value;
        }
    }
}
