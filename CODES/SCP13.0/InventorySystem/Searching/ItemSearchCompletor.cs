using InventorySystem.Items;
using InventorySystem.Items.Pickups;
using InventorySystem.Configs;
using Hints;
using UnityEngine;
using System;
using System.Linq;

namespace InventorySystem.Searching
{
    public class ItemSearchCompletor : SearchCompletor
    {
        private readonly ItemCategory _category;
        private sbyte CategoryCount
        {
            get
            {
                sbyte count = 0;
                foreach (var item in Hub.inventory.UserInventory.Items.Values)
                {
                    if (item.Category == _category)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public ItemSearchCompletor(ReferenceHub hub, ItemPickupBase targetPickup, ItemBase targetItem, double maxDistanceSquared)
            : base(hub, targetPickup, targetItem, maxDistanceSquared)
        {
            _category = targetItem.Category;
        }

        protected override bool ValidateAny()
        {
            if (!base.ValidateAny())
                return false;

            int totalItems = Hub.inventory.UserInventory.Items.Count;
            if (totalItems >= 8)
            {
                Hub.hints.Show(new TranslationHint(
                    HintTranslations.MaxItemsAlreadyReached,
                    new HintParameter[] { new ByteHintParameter(8) },
                    new HintEffect[] { HintEffectPresets.TrailingPulseAlpha(0.5f, 1f, 0.7f) }
                ));
                return false;
            }

            if (_category != ItemCategory.None) 
            {
                sbyte categoryLimit = InventoryLimits.GetCategoryLimit(_category, Hub);
                if (categoryLimit >= 0 && CategoryCount >= categoryLimit)
                {
                    Hub.hints.Show(new TranslationHint(
                        HintTranslations.MaxItemCategoryAlreadyReached,
                        new HintParameter[]
                        {
                            new ItemCategoryHintParameter(_category),
                            new ByteHintParameter((byte)categoryLimit)
                        },
                        new HintEffect[] { HintEffectPresets.TrailingPulseAlpha(0.5f, 1f, 0.7f) }
                    ));
                    return false;
                }
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

            if (TargetItem.Category == ItemCategory.Armor)
                throw new InvalidOperationException("Item is not equippable (can be held in inventory).");

            return true;
        }

        public override void Complete()
        {
            Hub.inventory.ServerAddItem(TargetPickup.Info.ItemId, TargetPickup.Info.Serial, TargetPickup);
            TargetPickup.DestroySelf();
            CheckCategoryLimitHint();
        }

        protected void CheckCategoryLimitHint()
        {
            sbyte categoryLimit = InventoryLimits.GetCategoryLimit(_category, Hub);

            if (_category != ItemCategory.Armor && categoryLimit >= 0 && CategoryCount >= categoryLimit)
            {
                Hub.hints.Show(new TranslationHint(
                    HintTranslations.MaxItemCategoryReached,
                    new HintParameter[]
                    {
                        new ItemCategoryHintParameter(_category),
                        new ByteHintParameter((byte)categoryLimit)
                    },
                    HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)
                ));
            }
        }
    }
}