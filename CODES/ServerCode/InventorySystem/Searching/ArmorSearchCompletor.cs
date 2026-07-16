namespace InventorySystem.Searching
{
	public class ArmorSearchCompletor : global::InventorySystem.Searching.SearchCompletor
	{
		private readonly ItemType _armorType;

		private global::InventorySystem.Items.Armor.BodyArmor _currentArmor;

		private ushort _currentArmorSerial;

		public ArmorSearchCompletor(ReferenceHub hub, global::InventorySystem.Items.Pickups.ItemPickupBase targetPickup, global::InventorySystem.Items.ItemBase targetItem, double maxDistanceSquared)
			: base(hub, targetPickup, targetItem, maxDistanceSquared)
		{
			_armorType = targetItem.ItemTypeId;
		}

		protected override bool ValidateAny()
		{
			if (!base.ValidateAny())
			{
				return false;
			}
			bool num = 8 > Hub.inventory.UserInventory.Items.Count;
			bool flag = global::InventorySystem.Items.Armor.BodyArmorUtils.TryGetBodyArmorAndItsSerial(Hub.inventory, out _currentArmor, out _currentArmorSerial);
			if (!num && !flag)
			{
				Hub.hints.Show(new global::Hints.TranslationHint(global::Hints.HintTranslations.MaxItemsAlreadyReached, new global::Hints.HintParameter[1]
				{
					new global::Hints.ByteHintParameter(8)
				}, new global::Hints.HintEffect[1] { global::Hints.HintEffectPresets.TrailingPulseAlpha(0.5f, 1f, 0.5f, 2f, 0f, 3) }, 2f));
				return false;
			}
			return true;
		}

		public override bool ValidateStart()
		{
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
			if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerPickupArmor, Hub, TargetPickup))
			{
				if (_currentArmor != null)
				{
					_currentArmor.DontRemoveExcessOnDrop = true;
					Hub.inventory.ServerDropItem(_currentArmorSerial);
				}
				global::InventorySystem.Items.Armor.BodyArmor armor = Hub.inventory.ServerAddItem(TargetPickup.Info.ItemId, TargetPickup.Info.Serial, TargetPickup) as global::InventorySystem.Items.Armor.BodyArmor;
				global::InventorySystem.Items.Armor.BodyArmorUtils.RemoveEverythingExceedingLimits(Hub.inventory, armor);
				TargetPickup.DestroySelf();
			}
		}
	}
}
