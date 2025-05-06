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

        public static void Initialize(GameObject _AK47Equippable, IntegerItemDefinition _AK47IntItemDef, IntegerItemDefinition _AK47MagazineIntItemDef)
        {
            /**
            if (_AK47Equippable == null)
            {
                MelonLogger.Error("Cannot add GunSettings - _AK47Equippable is null!");
                return;
            }

            GunSettings settings = _AK47Equippable.AddComponent<GunSettings>();
            if (settings != null)
            {
                settings.IsAutomatic = true;
                MelonLogger.Msg("GunSettings added successfully");
            }
            else
            {
                MelonLogger.Error("settings was null and could not be added.");
            }
            **/

            AK47Equippable = _AK47Equippable;
            AK47IntItemDef = _AK47IntItemDef;
            AK47MagazineIntItemDef = _AK47MagazineIntItemDef;

            SetCustomItemUI();
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
    }
}
