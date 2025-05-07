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
    public class AK47
    {
        public static GameObject AK47Equippable;
        public static IntegerItemDefinition AK47IntItemDef;
        public static IntegerItemDefinition AK47MagazineIntItemDef;
        public static DialogueController_ArmsDealer.WeaponOption rangedAK47;
        public static DialogueController_ArmsDealer.WeaponOption ammoAK47;

        public static void Initialize(GameObject _AK47Equippable, IntegerItemDefinition _AK47IntItemDef, IntegerItemDefinition _AK47MagazineIntItemDef)
        {
            AK47Equippable = _AK47Equippable;

            if (!AK47Equippable.TryGetComponent<GunSettings>(out GunSettings set))
            {
                GunSettings settings = _AK47Equippable.AddComponent<GunSettings>();
                if (settings != null)
                {
                    settings.isAutomatic.Value = true;
                    MelonLogger.Msg("GunSettings added successfully");
                }
                else
                {
                    MelonLogger.Error("settings was null and could not be added.");
                }
            }

            if (_AK47Equippable == null)
            {
                MelonLogger.Error("_AK47Equippable null");
            }

            AK47IntItemDef = _AK47IntItemDef;

            if (_AK47IntItemDef == null)
            {
                MelonLogger.Error("_AK47IntItemDef null");
            }

            AK47MagazineIntItemDef = _AK47MagazineIntItemDef;

            if (_AK47MagazineIntItemDef == null)
            {
                MelonLogger.Error("_AK47MagazineIntItemDef null");
            }

            SetCustomItemUI();
            CreateDialogueControllgerOptions();
        }

        private static void SetCustomItemUI()
        {
            UnityEngine.Object definition = Resources.Load("Weapons/M1911/M1911");

            if (definition == null)
            {
                MelonLogger.Error("Cast to ItemDefinition failed - type mismatch in IL2CPP");
                return;
            }

            var il2cppDefinition = definition.Cast<ItemDefinition>();

            if (il2cppDefinition != null)
            {
                AK47IntItemDef.CustomItemUI = il2cppDefinition.CustomItemUI;

                if (AK47IntItemDef.CustomItemUI != null)
                {
                    MelonLogger.Msg("il2cpp conversion worked!");
                }
                else
                {
                    MelonLogger.Error("Did not convert");
                }

                    AK47MagazineIntItemDef.CustomItemUI = il2cppDefinition.CustomItemUI;
                MelonLogger.Msg("Successfully set CustomItemUI using IL2CPP conversion");
            }
            else
            {
                MelonLogger.Error("IL2CPP conversion failed");
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
                Item = AK47IntItemDef
            };

            ammoAK47 = new DialogueController_ArmsDealer.WeaponOption
            {
                Name = "AK47 Magazine (Medium)",
                Price = 1000F,
                IsAvailable = true,
                NotAvailableReason = "",
                Item = AK47MagazineIntItemDef
            };
        }
    }
}
