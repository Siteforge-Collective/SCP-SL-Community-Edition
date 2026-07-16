using InventorySystem.Items;
using InventorySystem.Items.Armor;
using InventorySystem.Items.Pickups;
using InventorySystem.Configs;
using Hints;
using UnityEngine;
using System;

namespace InventorySystem.Searching
{
    public class ArmorSearchCompletor : SearchCompletor
    {
        private readonly ItemType _armorType;
        private BodyArmor _currentArmor;
        private ushort _currentArmorSerial;

        public override bool AllowPickupUponEscape => false;

        public ArmorSearchCompletor(ReferenceHub hub, ItemPickupBase targetPickup, ItemBase targetItem, double maxDistanceSquared)
            : base(hub, targetPickup, targetItem, maxDistanceSquared)
        {
            _armorType = targetItem.ItemTypeId;
        }

        protected override bool ValidateAny()
        {
            if (!base.ValidateAny())
                return false;

            bool hasArmor = Hub.inventory.TryGetBodyArmorAndItsSerial(out _currentArmor, out _currentArmorSerial);

            if (InventoryLimits.GetCategoryLimit(ItemCategory.Armor, Hub) > 0)
                return true;

            if (!hasArmor)
            {
                Hub.hints.Show(new TranslationHint(
                    HintTranslations.MaxItemsAlreadyReached,
                    new HintParameter[] { new ByteHintParameter(8) },
                    new HintEffect[]
                    { 
                        HintEffectPresets.TrailingPulseAlpha(0.5f, 1f, 0.5f, 1f, 0f, 1)
                    },
                    0f
                ));
                return false;
            }

            return true;
        }

        public override bool ValidateStart()
        {
            if (!ValidateAny())
                return false;

            if (!base.ValidateDistance())
                return false;

            if (TargetItem.ItemTypeId == (ItemType)(-1))
                throw new InvalidOperationException("Item has an invalid ItemType.");

            if (TargetItem.Category != ItemCategory.Armor)
                throw new InvalidOperationException("Item is not equippable (can be held in inventory).");

            return true;
        }

        public override void Complete()
        {
            if (_currentArmor != null)
            {
                _currentArmor.DontRemoveExcessOnDrop = true;
                Hub.inventory.ServerDropItem(_currentArmorSerial);
            }

            ItemBase itemBase = Hub.inventory.ServerAddItem(TargetPickup.Info.ItemId, TargetPickup.Info.Serial, TargetPickup);

            if (itemBase is BodyArmor armor)
            {
                BodyArmorUtils.RemoveEverythingExceedingLimits(Hub.inventory, armor);
            }

            TargetPickup.DestroySelf();
        }
    }
}