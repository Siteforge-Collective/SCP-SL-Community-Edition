using System;
using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Attachments;
using InventorySystem.Items.Pickups;
using UnityEngine;

namespace Scp914.Processors
{
    public class FirearmItemProcessor : Scp914ItemProcessor
    {
        [Serializable]
        private struct FirearmOutput
        {
            [Range(0f, 1f)]
            public float Chance;

            public ItemType[] TargetItems;
        }

        [SerializeField]
        private FirearmOutput[] _roughOutputs;

        [SerializeField]
        private FirearmOutput[] _coarseOutputs;

        [SerializeField]
        private FirearmOutput[] _oneToOneOutputs;

        [SerializeField]
        private FirearmOutput[] _fineOutputs;

        [SerializeField]
        private FirearmOutput[] _veryFineOutputs;

        [SerializeField]
        private bool _refillAmmo;

        private static readonly ItemType[] None = new ItemType[1] { ItemType.None };

        public override ItemBase OnInventoryItemUpgraded(Scp914KnobSetting setting, ReferenceHub hub, ushort serial)
        {
            if (!hub.inventory.UserInventory.Items.TryGetValue(serial, out var value))
            {
                return null;
            }
            ItemBase itemBase = null;
            ItemType[] items = GetItems(setting, value.ItemTypeId);
            foreach (ItemType newType in items)
            {
                if (itemBase == null)
                {
                    itemBase = UpgradePlayer(newType, value, hub, serial);
                }
                else
                {
                    UpgradePlayer(newType, value, hub, serial);
                }
            }
            return itemBase;
        }

        public override ItemPickupBase OnPickupUpgraded(Scp914KnobSetting setting, ItemPickupBase ipb, Vector3 newPos)
        {
            ItemPickupBase itemPickupBase = null;
            ItemType[] items = GetItems(setting, ipb.Info.ItemId);
            foreach (ItemType newType in items)
            {
                if (itemPickupBase == null)
                {
                    itemPickupBase = UpgradePickup(newType, ipb, newPos);
                }
                else
                {
                    UpgradePickup(newType, ipb, newPos);
                }
            }
            return itemPickupBase;
        }

        private ItemBase UpgradePlayer(ItemType newType, ItemBase item, ReferenceHub hub, ushort serial)
        {
            if (!(item is Firearm firearm))
            {
                throw new InvalidOperationException("FirearmItemProcessor can't be used for non-firearm items, such as " + item.ItemTypeId);
            }
            byte b = 0;
            if (InventoryItemLoader.AvailableItems.TryGetValue(newType, out var value) && value is Firearm firearm2)
            {
                AmmoItemProcessor.ExchangeAmmo(firearm.AmmoType, firearm2.AmmoType, firearm.Status.Ammo, out var exchangedAmmo, out var change);
                hub.inventory.ServerAddAmmo(firearm.AmmoType, change);
                hub.inventory.ServerAddAmmo(firearm2.AmmoType, exchangedAmmo);
                if (firearm2.AmmoManagerModule != null)
                {
                    b = firearm2.AmmoManagerModule.MaxAmmo;
                }
            }
            if (newType == item.ItemTypeId)
            {
                FirearmStatusFlags flags = ((b > 0) ? FirearmStatusFlags.MagazineInserted : FirearmStatusFlags.None);
                uint randomAttachmentsCode = AttachmentsUtils.GetRandomAttachmentsCode(newType);
                firearm.Status = new FirearmStatus(b, flags, randomAttachmentsCode);
                firearm.ApplyAttachmentsCode(randomAttachmentsCode, reValidate: false);
            }
            else
            {
                hub.inventory.ServerRemoveItem(serial, null);
                if (InventoryItemLoader.AvailableItems.ContainsKey(newType))
                {
                    return hub.inventory.ServerAddItem(newType, 0);
                }
            }
            return item;
        }

        private ItemPickupBase UpgradePickup(ItemType newType, ItemPickupBase ipb, Vector3 newPos)
        {
            if (!InventoryItemLoader.AvailableItems.TryGetValue(newType, out var value))
            {
                if (newType == ItemType.None)
                {
                    ipb.DestroySelf();
                    return null;
                }
                ipb.transform.position = newPos;
                return ipb;
            }
            if (!(ipb is FirearmPickup firearmPickup))
            {
                throw new InvalidOperationException("FirearmItemProcessor can't be used for non-firearm items, such as " + value.ItemTypeId);
            }
            uint attachments = 0u;
            byte b = 0;
            if (value is Firearm firearm && InventoryItemLoader.AvailableItems.TryGetValue(firearmPickup.Info.ItemId, out var value2) && value2 is Firearm firearm2)
            {
                AmmoItemProcessor.ExchangeAmmo(firearm2.AmmoType, firearm.AmmoType, firearmPickup.Status.Ammo, out var exchangedAmmo, out var change);
                AmmoItemProcessor.CreateAmmoPickup(firearm2.AmmoType, change, newPos);
                AmmoItemProcessor.CreateAmmoPickup(firearm.AmmoType, exchangedAmmo, newPos);
                attachments = firearm.ValidateAttachmentsCode(0u);
                if (_refillAmmo)
                {
                    if (firearm.AmmoManagerModule != null)
                    {
                        b = firearm.AmmoManagerModule.MaxAmmo;
                    }
                    else if (firearm.ItemTypeId == ItemType.ParticleDisruptor)
                    {
                        b = 5;
                    }
                }
            }
            if (newType == ipb.Info.ItemId)
            {
                FirearmStatusFlags flags = ((b > 0) ? FirearmStatusFlags.MagazineInserted : FirearmStatusFlags.None);
                firearmPickup.Status = new FirearmStatus(b, flags, AttachmentsUtils.GetRandomAttachmentsCode(newType));
                firearmPickup.transform.position = newPos;
                return firearmPickup;
            }
            PickupSyncInfo psi = new PickupSyncInfo
            {
                ItemId = newType,
                Serial = ItemSerialGenerator.GenerateNext(),
                WeightKg = value.Weight
            };
            ItemPickupBase itemPickupBase = InventoryExtensions.ServerCreatePickup(value, psi, newPos, ipb.transform.rotation);
            if (itemPickupBase is FirearmPickup firearmPickup2)
            {
                firearmPickup2.Status = new FirearmStatus(0, FirearmStatusFlags.None, attachments);
            }
            ipb.DestroySelf();
            return itemPickupBase;
        }

        private ItemType[] GetItems(Scp914KnobSetting setting, ItemType input)
        {
            FirearmOutput[] array;
            switch (setting)
            {
                case Scp914KnobSetting.Rough:
                    array = _roughOutputs;
                    break;
                case Scp914KnobSetting.Coarse:
                    array = _coarseOutputs;
                    break;
                case Scp914KnobSetting.OneToOne:
                    array = _oneToOneOutputs;
                    break;
                case Scp914KnobSetting.Fine:
                    array = _fineOutputs;
                    break;
                case Scp914KnobSetting.VeryFine:
                    array = _veryFineOutputs;
                    break;
                default:
                    return None;
            }
            if (array.Length == 0)
            {
                return new ItemType[1] { input };
            }
            float value = UnityEngine.Random.value;
            float num = 0f;
            FirearmOutput[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                FirearmOutput firearmOutput = array2[i];
                num += firearmOutput.Chance;
                if (num >= value)
                {
                    return firearmOutput.TargetItems;
                }
            }
            return None;
        }
    }
}
