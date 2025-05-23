using FishNet.Object;
using MelonLoader;
using MoreGunsMono.Sync;
using ScheduleOne.Dialogue;
using ScheduleOne.Equipping;
using ScheduleOne.ItemFramework;
using ScheduleOne.Trash;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreGunsMono.Guns
{
    public class WeaponBase
    {
        public string ID;

        public GameObject gunEquippable;
        public Equippable_RangedWeapon gunRangedWeapon;

        public GameObject gunHandgun;
        public GameObject magAvatarEquippable;

        public IntegerItemDefinition gunIntItemDef;
        public IntegerItemDefinition magIntItemDef;

        public DialogueController_ArmsDealer.WeaponOption rangedGun;
        public DialogueController_ArmsDealer.WeaponOption ammoGun;

        public GameObject gunMagTrash;
        public TrashItem gunMagTrashItem;
        public Dictionary<string, AnimationClip> animations = new Dictionary<string, AnimationClip>();

        public GunConfiguration config;

        public Shopping gunShop;
        public Shopping magShop;

        public bool IsConfigurationFinished { get; private set; }

        public static List<WeaponBase> allWeapons = new List<WeaponBase>();
        public static Dictionary<string, WeaponBase> weaponsByName = new Dictionary<string, WeaponBase>();

        public void Init(string ID, Shopping gunShop, Shopping magShop)
        {
            this.ID = ID;
            this.gunShop = gunShop;
            this.magShop = magShop;

            MelonLogger.Msg($"Initializing {ID}");
            MelonCoroutines.Start(LoadGun());
        }

        private void SetCustomItemUI()
        {
            var defintion = Resources.Load("Weapons/M1911/M1911") as ItemDefinition;

            if (defintion == null)
            {
                MelonLogger.Error("m1911 couldnt be loaded? Unable to register UI to custom guns!");
            }
            else
            {
                gunIntItemDef.CustomItemUI = defintion.CustomItemUI;
                magIntItemDef.CustomItemUI = defintion.CustomItemUI;
            }
        }

        private void CreateDialogueControllerOptions()
        {
            rangedGun = new DialogueController_ArmsDealer.WeaponOption
            {
                Name = config.ItemName.Value,
                Price = config.PurchasePrice.Value,
                IsAvailable = config.Available.Value,
                NotAvailableReason = config.AvailableReason.Value,
                Item = gunIntItemDef
            };

            ammoGun = new DialogueController_ArmsDealer.WeaponOption
            {
                Name = config.MagItemName.Value,
                Price = config.MagPurchasePrice.Value,
                IsAvailable = config.MagAvailable.Value,
                NotAvailableReason = config.MagAvailableReason.Value,
                Item = magIntItemDef
            };

            IsConfigurationFinished = true;
        }

        public void CreateConfig()
        {
            config = new GunConfiguration(this);
            MelonLogger.Msg("Created new config");
        }

        public static bool CheckAssetLoaded(UnityEngine.Object asset, string assetName, string weaponName)
        {
            if (asset == null)
            {
                MelonLogger.Error($"Could not load asset: {assetName}");
                MoreGunsMod.StopProcess();
                return false;
            }
            else
            {
                MelonLogger.Msg($"Loaded asset for {weaponName} : {assetName}");
                return true;
            }
        }

        private IEnumerator LoadGun()
        {
            AssetBundleRequest rqGunEquippable = MoreGunsMod.assetBundle.LoadAssetAsync<GameObject>($"{ID}_Equippable");
            yield return rqGunEquippable;
            GameObject _GunEquippable = rqGunEquippable.asset as GameObject;
            if (!CheckAssetLoaded(_GunEquippable,$"{ID}_Equippable", $"{ID}"))
            {
                yield break;
            }

            AssetBundleRequest rqGunMagIntItemDef = MoreGunsMod.assetBundle.LoadAssetAsync<IntegerItemDefinition>($"{ID}_Magazine");
            yield return rqGunMagIntItemDef;
            IntegerItemDefinition _GunMagIntItemDef = rqGunMagIntItemDef.asset as IntegerItemDefinition;
            if (!CheckAssetLoaded(_GunMagIntItemDef, $"{ID}_Magazine", $"{ID}"))
            {
                yield break;
            }

            AssetBundleRequest rqGunIntItemDef = MoreGunsMod.assetBundle.LoadAssetAsync<IntegerItemDefinition>($"{ID}");
            yield return rqGunIntItemDef;
            IntegerItemDefinition _GunIntItemDef = rqGunIntItemDef.asset as IntegerItemDefinition;
            if (!CheckAssetLoaded(_GunIntItemDef, $"{ID}", $"{ID}"))
            {
                yield break;
            }

            AssetBundleRequest rqGunMagTrash = MoreGunsMod.assetBundle.LoadAssetAsync<GameObject>($"assets/resources/weapons/{ID}/magazine/{ID}_Magazine_Trash.prefab");
            yield return rqGunMagTrash;
            GameObject _GunMagTrash = rqGunMagTrash.asset as GameObject;
            if (!CheckAssetLoaded(_GunMagTrash, $"assets/resources/weapons/{ID}/magazine/{ID}_Magazine_Trash.prefab", $"{ID}"))
            {
                yield break;
            }

            AssetBundleRequest rqGunPrefab = MoreGunsMod.assetBundle.LoadAssetAsync<GameObject>($"assets/resources/avatar/equippables/{ID}.prefab");
            yield return rqGunPrefab;
            GameObject _GunHandgun = rqGunPrefab.asset as GameObject;
            if (!CheckAssetLoaded(_GunHandgun, $"assets/resources/avatar/equippables/{ID}.prefab", $"{ID}"))
            {
                yield break;
            }

            AssetBundleRequest rqGunMagAvatarEquippable = MoreGunsMod.assetBundle.LoadAssetAsync<GameObject>($"assets/resources/weapons/{ID}/magazine/{ID}_magazine_avatarequippable.prefab");
            yield return rqGunMagAvatarEquippable;
            GameObject _GunMagAvatarEquippable = rqGunMagAvatarEquippable.asset as GameObject;
            if (!CheckAssetLoaded(_GunMagAvatarEquippable, $"assets/resources/weapons/{ID}/magazine/{ID}_magazine_avatarequippable.prefab", $"{ID}"))
            {
                yield break;
            }

            GunSettings settings = _GunEquippable.AddComponent<GunSettings>();
            settings.isAutomatic = true;

            gunEquippable = _GunEquippable;
            magIntItemDef = _GunMagIntItemDef;
            gunIntItemDef = _GunIntItemDef;
            gunMagTrash = _GunMagTrash;
            gunHandgun = _GunHandgun;
            magAvatarEquippable = _GunMagAvatarEquippable;

            gunRangedWeapon = gunEquippable.GetComponent<Equippable_RangedWeapon>();
            gunMagTrashItem = gunMagTrash.GetComponent<TrashItem>();

            CreateConfig();
            SetCustomItemUI();
            LoadAnimations();
            ApplySettingsFromConfig();

            Resource.RegisterAsset($"Avatar/Equippables/{ID.ToUpper()}", gunHandgun);
            Resource.RegisterAsset($"Weapons/ak47/Magazine/{ID.ToUpper()}_Magazine_AvatarEquippable", magAvatarEquippable);

            allWeapons.Add(this);
            weaponsByName.Add($"{ID}", this);

            MelonLogger.Msg($"Finished Initializing {ID}");
        }

        public void ApplySettingsFromConfig()
        {
            gunRangedWeapon.Damage = config.Damage.Value;
            gunRangedWeapon.ImpactForce = config.ImpactForce.Value;
            gunRangedWeapon.AimFOVReduction = config.AimFOVReduction.Value;
            gunRangedWeapon.AccuracyChangeDuration = config.AccuracyChangeDuration.Value;
            gunRangedWeapon.MagazineSize = config.MagazineSize.Value;

            gunIntItemDef.Name = config.DisplayItemName.Value;
            gunIntItemDef.Description = config.DisplayDescription.Value;
            gunIntItemDef.LabelDisplayColor = config.LabelDisplayColor.Value;
            gunIntItemDef.legalStatus = config.LegalStatus.Value;
            gunIntItemDef.RequiredRank = config.RequiredRank.Value;

            magIntItemDef.Name = config.MagDisplayItemName.Value;
            magIntItemDef.Description = config.MagDisplayDescription.Value;
            magIntItemDef.LabelDisplayColor = config.MagLabelDisplayColor.Value;
            magIntItemDef.legalStatus = config.MagLegalStatus.Value;
            magIntItemDef.RequiredRank = config.MagRequiredRank.Value;

            CreateDialogueControllerOptions();
        }

        private void LoadAnimations()
        {
            Equippable_RangedWeapon equipWeapon = (Equippable_RangedWeapon) gunIntItemDef.Equippable;
            RuntimeAnimatorController animatorController = equipWeapon.AnimatorController;
            
            foreach (AnimationClip anim in animatorController.animationClips)
            {
                MelonLogger.Msg($"Animation: {anim.name}");
                if (anim.name.Contains("Idle"))
                {
                    animations.Add("BothHands_Grip_Lowered", anim);
                }
                if (anim.name.Contains("Aiming"))
                {
                    animations.Add("BothHands_Grip_Raised", anim);
                }
                if (anim.name.Contains("Fire"))
                {
                    if (!animations.ContainsKey("BothHands_Grip_Recoil"))
                    {
                        animations.Add("BothHands_Grip_Recoil", anim);
                    }
                }
            }
        }
    }
}