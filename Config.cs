using MelonLoader;
using ScheduleOne.ItemFramework;
using ScheduleOne.Levelling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreGunsMono
{
    public static class Config
    {
        public static MelonPreferences_Category ak47;
        public static MelonPreferences_Entry<float> ak47Damage;
        public static MelonPreferences_Entry<float> ak47ImpactForce;
        public static MelonPreferences_Entry<float> ak47aimFOVReduction;
        public static MelonPreferences_Entry<float> ak47accuracyChangeDuration;
        public static MelonPreferences_Entry<int> ak47magazineSize;

        public static MelonPreferences_Entry<string> ak47DisplayItemName;
        public static MelonPreferences_Entry<string> ak47DisplayDescription;
        public static MelonPreferences_Entry<Color> ak47LabelDisplayColor;
        public static MelonPreferences_Entry<ELegalStatus> ak47LegalStatus;
        public static MelonPreferences_Entry<FullRank> ak47requiredRank;

        public static MelonPreferences_Entry<string> ak47MagDisplayItemName;
        public static MelonPreferences_Entry<string> ak47MagDisplayDescription;
        public static MelonPreferences_Entry<Color> ak47MagLabelDisplayColor;
        public static MelonPreferences_Entry<ELegalStatus> ak47MagLegalStatus;
        public static MelonPreferences_Entry<FullRank> ak47MagrequiredRank;

        public static MelonPreferences_Entry<float> ak47PurchasePrice;
        public static MelonPreferences_Entry<float> ak47magPurchasePrice;
        public static MelonPreferences_Entry<string> ak47ItemName;
        public static MelonPreferences_Entry<string> ak47MagItemName;
        public static MelonPreferences_Entry<bool> ak47available;
        public static MelonPreferences_Entry<bool> ak47Magavailable;
        public static MelonPreferences_Entry<string> ak47availableReason;
        public static MelonPreferences_Entry<string> ak47MagAvailableReason;

        public static string folderPath = "UserData/MoreGuns.cfg";
        public static event Action OnSettingChanged;

        public static void Init()
        {
            ak47 = MelonPreferences.CreateCategory("MoreGuns-AK47 Settings", "AK47 Settings");
            ak47Damage = ak47.CreateEntry("AK47 Damage", 70F);
            ak47ImpactForce = ak47.CreateEntry("AK47 Impact Force", 200F);
            ak47aimFOVReduction = ak47.CreateEntry("AK47 Aim FOV Reduction", 10F);
            ak47accuracyChangeDuration = ak47.CreateEntry("Accuracy Change Duration", 0.5F);
            ak47magazineSize = ak47.CreateEntry("AK47 Magazine Size", 30);

            ak47DisplayItemName = ak47.CreateEntry("AK47 Display Name", "AK47");
            ak47DisplayDescription = ak47.CreateEntry("AK47 Display Description", "AK47 assault rifle A true American classic.");
            ak47LabelDisplayColor = ak47.CreateEntry("AK47 Label Color", Color.white);
            ak47LegalStatus = ak47.CreateEntry("AK47 Legal Status", ELegalStatus.Legal);
            ak47requiredRank = ak47.CreateEntry("AK47 Required Rank", new FullRank(ERank.Underlord, 3));

            ak47MagDisplayItemName = ak47.CreateEntry("AK47 Mag Display Name", "AK47 Magazine");
            ak47MagDisplayDescription = ak47.CreateEntry("AK47 Mag Display Description", "30-round magazine for the ak47 assault rifle.");
            ak47MagLabelDisplayColor = ak47.CreateEntry("AK47 Mag Label Color", Color.white);
            ak47MagLegalStatus = ak47.CreateEntry("AK47 Mag Legal Status", ELegalStatus.Legal);
            ak47MagrequiredRank = ak47.CreateEntry("AK47 Mag Required Rank", new FullRank(ERank.Underlord, 3));

            ak47PurchasePrice = ak47.CreateEntry("AK47 Price", 15000F, "AK47 Price");
            ak47magPurchasePrice = ak47.CreateEntry("AK47 Magazine Price", 1000F, "AK47 Price");
            ak47ItemName = ak47.CreateEntry("AK47 Name", "AK47");
            ak47MagItemName = ak47.CreateEntry("AK47 Magazine Name", "AK47 Magazine");
            ak47available = ak47.CreateEntry("AK47 Shop Availability", true);
            ak47Magavailable = ak47.CreateEntry("AK47 Magazine Shop Availability", true);
            ak47availableReason = ak47.CreateEntry("AK47 Non-Available Reason", "");
            ak47MagAvailableReason = ak47.CreateEntry("AK47 Magazine Non-Available Reason", "");

            ak47.SetFilePath(folderPath);
            ak47.SaveToFile();
            MelonLogger.Msg($"Melon Preferences saved and loaded from: {folderPath}");
        }
    }
}
