using Harmony;
using MelonLoader;
using MoreGunsMono.Guns;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.Equipping;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Trash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreGunsMono.Dialogue
{
    public static class StanDialogue
    {
        public const string allWeaponCategoryOptions = "925ea1d9-9b7e-47a0-b016-6a3b787290c3";
        public static List<ItemInstance> allGuns = new List<ItemInstance>();
        public static Dictionary<string, ItemInstance> allMags = new Dictionary<string, ItemInstance>();

        public static void StartSpecialGunReloads()
        {
            try
            {
                allGuns.Clear();
                allMags.Clear();

                foreach (ItemSlot item in PlayerSingleton<PlayerInventory>.Instance.hotbarSlots)
                {
                    if (item?.ItemInstance?.Definition == null)
                    {
                        continue;
                    }

                    string ID = item.ItemInstance.Definition.ID;
                    if (WeaponBase.weaponsByName.ContainsKey(ID))
                    {
                        allGuns.Add(item.ItemInstance);
                    }

                    if (ID.EndsWith("mag"))
                    {
                        allMags.Add(ID, item.ItemInstance);
                    }
                }

                foreach (ItemInstance gun in allGuns)
                {
                    if (allMags.TryGetValue($"{gun.Definition.ID}mag", out ItemInstance mag))
                    {
                        Equippable_RangedWeapon weapon = (Equippable_RangedWeapon) gun.Definition.Equippable;
                        IntegerItemInstance gunInt = (IntegerItemInstance)gun;
                        IntegerItemInstance magInt = (IntegerItemInstance)mag;

                        int capacity = weapon.MagazineSize;
                        int ammoNeeded = capacity - gunInt.Value;
                        int remainder = magInt.Value - ammoNeeded;

                        if (remainder <= 0)
                        {
                            int newGunAmmo = gunInt.Value + magInt.Value;
                            gunInt.SetValue(newGunAmmo);
                            magInt.SetValue(0);
                            mag.ChangeQuantity(-1);

                            Vector3 position = PlayerSingleton<PlayerCamera>.Instance.transform.position - PlayerSingleton<PlayerCamera>.Instance.transform.up * 0.4f;
                            NetworkSingleton<TrashManager>.Instance.CreateTrashItem(weapon.ReloadTrash.ID, position, UnityEngine.Random.rotation, default(Vector3), "", false);
                        }
                        else
                        {
                            gunInt.SetValue(capacity);
                            magInt.SetValue(remainder);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"Exception in StartSpecialGunReloads: {ex.Message}");
                MelonLogger.Error($"Stack trace: {ex.StackTrace}");
            }
        }
    }
}
