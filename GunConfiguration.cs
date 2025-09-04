using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Levelling;
using MelonLoader;
using MoreGuns.Guns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreGuns
{
    public class GunConfiguration
    {
        public string ID { get; private set; }

        public MelonPreferences_Category Category { get; private set; }

        public MelonPreferences_Entry<float> Damage { get; private set; }
        public MelonPreferences_Entry<float> ImpactForce { get; private set; }
        public MelonPreferences_Entry<float> AimFOVReduction { get; private set; }
        public MelonPreferences_Entry<float> AccuracyChangeDuration { get; private set; }
        public MelonPreferences_Entry<int> MagazineSize { get; private set; }

        public MelonPreferences_Entry<string> DisplayItemName { get; private set; }
        public MelonPreferences_Entry<string> DisplayDescription { get; private set; }
        public MelonPreferences_Entry<Color> LabelDisplayColor { get; private set; }
        public MelonPreferences_Entry<ELegalStatus> LegalStatus { get; private set; }
        public MelonPreferences_Entry<FullRank> RequiredRank { get; private set; }

        public MelonPreferences_Entry<string> MagDisplayItemName { get; private set; }
        public MelonPreferences_Entry<string> MagDisplayDescription { get; private set; }
        public MelonPreferences_Entry<Color> MagLabelDisplayColor { get; private set; }
        public MelonPreferences_Entry<ELegalStatus> MagLegalStatus { get; private set; }
        public MelonPreferences_Entry<FullRank> MagRequiredRank { get; private set; }

        public MelonPreferences_Entry<float> PurchasePrice { get; private set; }
        public MelonPreferences_Entry<float> MagPurchasePrice { get; private set; }
        public MelonPreferences_Entry<string> ItemName { get; private set; }
        public MelonPreferences_Entry<string> MagItemName { get; private set; }
        public MelonPreferences_Entry<bool> Available { get; private set; }
        public MelonPreferences_Entry<bool> MagAvailable { get; private set; }
        public MelonPreferences_Entry<string> AvailableReason { get; private set; }
        public MelonPreferences_Entry<string> MagAvailableReason { get; private set; }

        public event Action OnSettingChanged;
        public static string folderPath = "UserData/MoreGuns.cfg";

        public GunConfiguration(WeaponBase weapon)
        {
            string ID = weapon.ID;
            string categoryName = $"MoreGuns-{ID} Settings";
            string categoryDesc = $"{ID} Settings";

            Category = MelonPreferences.CreateCategory(categoryName, categoryDesc);

            InitializePreferences(ID, weapon);

            Category.SetFilePath(folderPath);
            Category.SaveToFile();

            MelonLogger.Msg($"Loaded configuration for {ID} from: {folderPath}");
        }

        private void InitializePreferences(string ID, WeaponBase weapon)
        {
            var gunEquippable = weapon.gunRangedWeapon;
            Damage = Category.CreateEntry($"{ID} Damage", gunEquippable.Damage);
            ImpactForce = Category.CreateEntry($"{ID} Impact Force", gunEquippable.ImpactForce);
            AimFOVReduction = Category.CreateEntry($"{ID} Aim FOV Reduction", gunEquippable.AimFOVReduction);
            AccuracyChangeDuration = Category.CreateEntry($"{ID} Accuracy Change Duration", gunEquippable.AccuracyChangeDuration);
            MagazineSize = Category.CreateEntry($"{ID} Magazine Size", gunEquippable.MagazineSize);

            var gunIntItemDef = weapon.gunIntItemDef;
            DisplayItemName = Category.CreateEntry($"{ID} Display Name", gunIntItemDef.Name);
            DisplayDescription = Category.CreateEntry($"{ID} Display Description", gunIntItemDef.Description);
            LabelDisplayColor = Category.CreateEntry($"{ID} Label Color", gunIntItemDef.LabelDisplayColor);
            LegalStatus = Category.CreateEntry($"{ID} Legal Status", gunIntItemDef.legalStatus);
            RequiredRank = Category.CreateEntry($"{ID} Required Rank", gunIntItemDef.RequiredRank);

            var magIntItemDef = weapon.magIntItemDef;
            MagDisplayItemName = Category.CreateEntry($"{ID} Mag Display Name", magIntItemDef.Name);
            MagDisplayDescription = Category.CreateEntry($"{ID} Mag Display Description", magIntItemDef.Description);
            MagLabelDisplayColor = Category.CreateEntry($"{ID} Mag Label Color", magIntItemDef.LabelDisplayColor);
            MagLegalStatus = Category.CreateEntry($"{ID} Mag Legal Status", ELegalStatus.Legal);
            MagRequiredRank = Category.CreateEntry($"{ID} Mag Required Rank", new FullRank(ERank.Underlord, 3));

            var gunShop = weapon.gunShop;
            PurchasePrice = Category.CreateEntry($"{ID} Price", gunIntItemDef.BasePurchasePrice);
            ItemName = Category.CreateEntry($"{ID} Name", gunIntItemDef.Name);
            Available = Category.CreateEntry($"{ID} Shop Availability", true);
            AvailableReason = Category.CreateEntry($"{ID} Non-Available Reason", "");

            var magShop = weapon.magShop;
            MagPurchasePrice = Category.CreateEntry($"{ID} Magazine Price", magIntItemDef.BasePurchasePrice);
            MagItemName = Category.CreateEntry($"{ID} Magazine Name", magIntItemDef.Name);
            MagAvailable = Category.CreateEntry($"{ID} Magazine Shop Availability", true);
            MagAvailableReason = Category.CreateEntry($"{ID} Magazine Non-Available Reason", "");
        }

        public void HandleSettingsUpdate()
        {
            MelonLogger.Msg("Updating More Guns Settings");
            Category.LoadFromFile();
            OnSettingChanged?.Invoke();
        }
    }
}
