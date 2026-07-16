namespace InventorySystem.Searching
{
	public class ItemSearchCompletor : global::InventorySystem.Searching.SearchCompletor
	{
		private readonly ItemCategory _category;

		private sbyte CategoryCount
		{
			get
			{
				sbyte b = 0;
				foreach (global::InventorySystem.Items.ItemBase value in Hub.inventory.UserInventory.Items.Values)
				{
					if (value.Category == _category)
					{
						b++;
					}
				}
				return b;
			}
		}

		public ItemSearchCompletor(ReferenceHub hub, global::InventorySystem.Items.Pickups.ItemPickupBase targetPickup, global::InventorySystem.Items.ItemBase targetItem, double maxDistanceSquared)
			: base(hub, targetPickup, targetItem, maxDistanceSquared)
		{
			_category = targetItem.Category;
		}

		protected override bool ValidateAny()
		{
			if (!base.ValidateAny())
			{
				return false;
			}
			if (Hub.inventory.UserInventory.Items.Count >= 8)
			{
				Hub.hints.Show(new global::Hints.TranslationHint(global::Hints.HintTranslations.MaxItemsAlreadyReached, new global::Hints.HintParameter[1]
				{
					new global::Hints.ByteHintParameter(8)
				}, new global::Hints.HintEffect[1] { global::Hints.HintEffectPresets.TrailingPulseAlpha(0.5f, 1f, 0.5f, 2f, 0f, 3) }, 2f));
				return false;
			}
			if (_category != ItemCategory.None)
			{
				int num = global::UnityEngine.Mathf.Abs(global::InventorySystem.Configs.InventoryLimits.GetCategoryLimit(_category, Hub));
				if (CategoryCount >= num)
				{
					Hub.hints.Show(new global::Hints.TranslationHint(global::Hints.HintTranslations.MaxItemCategoryAlreadyReached, new global::Hints.HintParameter[2]
					{
						new global::Hints.ItemCategoryHintParameter(_category),
						new global::Hints.ByteHintParameter((byte)num)
					}, new global::Hints.HintEffect[1] { global::Hints.HintEffectPresets.TrailingPulseAlpha(0.5f, 1f, 0.5f, 2f, 0f, 2) }, 2f));
					return false;
				}
			}
			return true;
		}

		public override bool ValidateStart()
		{
			if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerSearchPickup, Hub, TargetPickup))
			{
				return false;
			}
			if (!base.ValidateStart())
			{
				return false;
			}
			if (TargetItem.ItemTypeId == ItemType.None)
			{
				throw new global::System.InvalidOperationException("Item has an invalid ItemType.");
			}
			if (TargetItem.Category == ItemCategory.Ammo)
			{
				throw new global::System.InvalidOperationException("Item is not equippable (can be held in inventory).");
			}
			return true;
		}

		public override void Complete()
		{
			if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerSearchedPickup, Hub, TargetPickup))
			{
				Hub.inventory.ServerAddItem(TargetPickup.Info.ItemId, TargetPickup.Info.Serial, TargetPickup);
				TargetPickup.DestroySelf();
				CheckCategoryLimitHint();
			}
		}

		protected void CheckCategoryLimitHint()
		{
			sbyte categoryLimit = global::InventorySystem.Configs.InventoryLimits.GetCategoryLimit(_category, Hub);
			if (_category != ItemCategory.None && categoryLimit >= 0 && CategoryCount >= categoryLimit)
			{
				global::Hints.HintEffect[] effects = global::Hints.HintEffectPresets.FadeInAndOut(0.25f);
				global::Hints.HintParameter[] parameters = new global::Hints.HintParameter[2]
				{
					new global::Hints.ItemCategoryHintParameter(_category),
					new global::Hints.ByteHintParameter((byte)categoryLimit)
				};
				Hub.hints.Show(new global::Hints.TranslationHint(global::Hints.HintTranslations.MaxItemCategoryReached, parameters, effects, 1.5f));
			}
		}
	}
}
