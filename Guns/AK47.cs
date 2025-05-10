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
using System.Collections;
using ScheduleOne.Doors;

namespace MoreGunsMono.Guns
{
    public class AK47 : WeaponBase
    {
        public static GameObject AK47Equippable;
        public static Equippable_RangedWeapon AK47RangedWeapon;

        public static GameObject AK47Handgun;
        public static GameObject AK47MagAvatarEquippable;

        public static IntegerItemDefinition AK47IntItemDef;
        public static IntegerItemDefinition AK47MagazineIntItemDef;
        
        public static DialogueController_ArmsDealer.WeaponOption rangedAK47;
        public static DialogueController_ArmsDealer.WeaponOption ammoAK47;

        public static GameObject AK47MagTrash;
        public static TrashItem AK47MagTrashItem;

        public static void Init()
        {
            MelonLogger.Msg("Initializing AK47");
            MelonCoroutines.Start(LoadGun());
            MelonLogger.Msg("Finished Initializing AK47");
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

        private static IEnumerator LoadGun()
        {
            AssetBundleRequest rqAK47Equippable = MoreGunsMod.assetBundle.LoadAssetAsync<GameObject>("AK47_Equippable");
            yield return rqAK47Equippable;
            GameObject _AK47Equippable = rqAK47Equippable.asset as GameObject;
            if (!CheckAssetLoaded(_AK47Equippable, "AK47_Equippable", "AK47"))
            {
                yield break;
            }

            AssetBundleRequest request_AK47_Magazine_IntegerItemDefinition = MoreGunsMod.assetBundle.LoadAssetAsync<IntegerItemDefinition>("AK47_Magazine");
            yield return request_AK47_Magazine_IntegerItemDefinition;
            IntegerItemDefinition _AK47MagIntItemDef = request_AK47_Magazine_IntegerItemDefinition.asset as IntegerItemDefinition;
            if (!CheckAssetLoaded(_AK47MagIntItemDef, "AK47_Magazine", "AK47"))
            {
                yield break;
            }

            AssetBundleRequest request_AK47_IntegerItemDefinition = MoreGunsMod.assetBundle.LoadAssetAsync<IntegerItemDefinition>("AK47");
            yield return request_AK47_IntegerItemDefinition;
            IntegerItemDefinition _AK47IntItemDef = request_AK47_IntegerItemDefinition.asset as IntegerItemDefinition;
            if (!CheckAssetLoaded(_AK47IntItemDef, "AK47", "AK47"))
            {
                yield break;
            }

            AssetBundleRequest request_AK47_Magazine_Trash = MoreGunsMod.assetBundle.LoadAssetAsync<GameObject>("assets/resources/weapons/ak47/magazine/AK47_Magazine_Trash.prefab");
            yield return request_AK47_Magazine_Trash;
            GameObject _AK47MagTrash = request_AK47_Magazine_Trash.asset as GameObject;
            if (!CheckAssetLoaded(_AK47MagTrash, "assets/resources/weapons/ak47/magazine/AK47_Magazine_Trash.prefab", "AK47"))
            {
                yield break;
            }

            AssetBundleRequest request_AK47_Prefab = MoreGunsMod.assetBundle.LoadAssetAsync<GameObject>("assets/resources/avatar/equippables/ak47.prefab");
            yield return request_AK47_Prefab;
            GameObject _AK47Handgun = request_AK47_Prefab.asset as GameObject;
            if (!CheckAssetLoaded(_AK47Handgun, "assets/resources/avatar/equippables/ak47.prefab", "AK47"))
            {
                yield break;
            }

            AssetBundleRequest requestAk47MagazineAvatarEquippable = MoreGunsMod.assetBundle.LoadAssetAsync<GameObject>("assets/resources/weapons/ak47/magazine/ak47_magazine_avatarequippable.prefab");
            yield return requestAk47MagazineAvatarEquippable;
            GameObject _AK47MagAvatarEquippable = requestAk47MagazineAvatarEquippable.asset as GameObject;
            if (!CheckAssetLoaded(_AK47MagAvatarEquippable, "assets/resources/weapons/ak47/magazine/ak47_magazine_avatarequippable.prefab", "AK47"))
            {
                yield break;
            }

            GunSettings settings = _AK47Equippable.AddComponent<GunSettings>();
            settings.isAutomatic = true;

            AK47Equippable = _AK47Equippable;
            AK47MagazineIntItemDef = _AK47MagIntItemDef;
            AK47IntItemDef = _AK47IntItemDef;
            AK47MagTrash = _AK47MagTrash;
            AK47Handgun = _AK47Handgun;
            AK47MagAvatarEquippable = _AK47MagAvatarEquippable;

            AK47RangedWeapon = AK47Equippable.GetComponent<Equippable_RangedWeapon>();
            AK47MagTrashItem = AK47MagTrash.GetComponent<TrashItem>();

            SetCustomItemUI();
            UpdateSettingsFromConfig();
            CreateDialogueControllerOptions();

            Resource.RegisterAsset("Avatar/Equippables/AK47", AK47Handgun);
            Resource.RegisterAsset("Weapons/ak47/Magazine/AK47_Magazine_AvatarEquippable", AK47MagAvatarEquippable);
        }
    }
}
