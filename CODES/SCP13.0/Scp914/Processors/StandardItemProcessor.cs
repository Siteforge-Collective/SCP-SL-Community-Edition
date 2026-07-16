using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Pickups;
using UnityEngine;

namespace Scp914.Processors
{
    public class StandardItemProcessor : Scp914ItemProcessor
    {
        [SerializeField]
        private ItemType[] _roughOutputs;

        [SerializeField]
        private ItemType[] _coarseOutputs;

        [SerializeField]
        private ItemType[] _oneToOneOutputs;

        [SerializeField]
        private ItemType[] _fineOutputs;

        [SerializeField]
        private ItemType[] _veryFineOutputs;

        [SerializeField]
        private bool _fireUpgradeTrigger;

        public override ItemBase OnInventoryItemUpgraded(Scp914KnobSetting setting, ReferenceHub hub, ushort serial)
        {
            if (!hub.inventory.UserInventory.Items.TryGetValue(serial, out var value))
            {
                return null;
            }
            ItemType itemType = RandomOutput(setting, value.ItemTypeId);
            if (itemType == value.ItemTypeId)
            {
                if (_fireUpgradeTrigger && value is IUpgradeTrigger upgradeTrigger)
                {
                    upgradeTrigger.ServerOnUpgraded(setting);
                }
                return null;
            }
            hub.inventory.ServerRemoveItem(serial, null);
            if (!InventoryItemLoader.AvailableItems.ContainsKey(itemType))
            {
                return null;
            }
            ItemBase itemBase = hub.inventory.ServerAddItem(itemType, 0);
            if (itemBase is Firearm firearm)
            {
                firearm.Status = GetStatusForFirearm(itemType);
            }
            return itemBase;
        }

        public override ItemPickupBase OnPickupUpgraded(Scp914KnobSetting setting, ItemPickupBase ipb, Vector3 newPosition)
        {
            ItemType newType = RandomOutput(setting, ipb.Info.ItemId);
            if (newType == ItemType.None)
            {
                HandleNone(ipb, newPosition);
                return null;
            }
            if (newType == ipb.Info.ItemId || !InventoryItemLoader.AvailableItems.TryGetValue(newType, out var value))
            {
                ipb.transform.position = newPosition;
                if (_fireUpgradeTrigger && ipb is IUpgradeTrigger upgradeTrigger)
                {
                    upgradeTrigger.ServerOnUpgraded(setting);
                }
                return ipb;
            }
            PickupSyncInfo psi = new PickupSyncInfo
            {
                ItemId = newType,
                Serial = ItemSerialGenerator.GenerateNext(),
                WeightKg = value.Weight
            };
            ItemPickupBase result = InventoryExtensions.ServerCreatePickup(value, psi, newPosition, spawn: true, delegate (ItemPickupBase itemPickupBase)
            {
                if (itemPickupBase is FirearmPickup firearmPickup)
                {
                    firearmPickup.Status = GetStatusForFirearm(newType);
                }
            });
            HandleOldPickup(ipb, newPosition);
            return result;
        }

        protected virtual void HandleNone(ItemPickupBase ipb, Vector3 newPosition)
        {
            ipb.DestroySelf();
        }

        protected virtual void HandleOldPickup(ItemPickupBase ipb, Vector3 newPosition)
        {
            ipb.DestroySelf();
        }

        private ItemType RandomOutput(Scp914KnobSetting setting, ItemType id)
        {
            ItemType[] array;
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
                    return id;
            }
            return array[Random.Range(0, array.Length)];
        }
    }
}
