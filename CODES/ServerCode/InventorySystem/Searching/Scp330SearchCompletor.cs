namespace InventorySystem.Searching
{
	public class Scp330SearchCompletor : global::InventorySystem.Searching.SearchCompletor
	{
		private readonly global::InventorySystem.Items.Usables.Scp330.Scp330Bag _playerBag;

		public Scp330SearchCompletor(ReferenceHub hub, global::InventorySystem.Items.Pickups.ItemPickupBase targetPickup, global::InventorySystem.Items.ItemBase targetItem, double maxDistanceSquared)
			: base(hub, targetPickup, targetItem, maxDistanceSquared)
		{
			global::InventorySystem.Items.Usables.Scp330.Scp330Bag.TryGetBag(hub, out _playerBag);
		}

		protected override bool ValidateAny()
		{
			if (!base.ValidateAny())
			{
				return false;
			}
			bool flag = _playerBag != null;
			int count = Hub.inventory.UserInventory.Items.Count;
			if ((!flag && count < 8) || (flag && _playerBag.Candies.Count < 6))
			{
				return true;
			}
			ShowOverloadHint(Hub, flag);
			return false;
		}

		public static void ShowOverloadHint(ReferenceHub ply, bool hasBag)
		{
			if (hasBag)
			{
				ply.hints.Show(new global::Hints.TranslationHint(global::Hints.HintTranslations.MaxItemCategoryAlreadyReached, new global::Hints.HintParameter[2]
				{
					new global::Hints.Scp330HintParameter(global::InventorySystem.Items.Usables.Scp330.Scp330Translations.Entry.Candies),
					new global::Hints.ByteHintParameter(6)
				}, new global::Hints.HintEffect[1] { global::Hints.HintEffectPresets.TrailingPulseAlpha(0.5f, 1f, 0.5f, 2f, 0f, 2) }, 2f));
			}
			else
			{
				ply.hints.Show(new global::Hints.TranslationHint(global::Hints.HintTranslations.MaxItemsAlreadyReached, new global::Hints.HintParameter[1]
				{
					new global::Hints.ByteHintParameter(8)
				}, new global::Hints.HintEffect[1] { global::Hints.HintEffectPresets.TrailingPulseAlpha(0.5f, 1f, 0.5f, 2f, 0f, 3) }, 2f));
			}
		}

		public override void Complete()
		{
			if (TargetPickup is global::InventorySystem.Items.Usables.Scp330.Scp330Pickup scp330Pickup && global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerPickupScp330, Hub, scp330Pickup))
			{
				global::InventorySystem.Items.Usables.Scp330.Scp330Bag.ServerProcessPickup(Hub, scp330Pickup, out var _);
				if (scp330Pickup.StoredCandies.Count == 0)
				{
					scp330Pickup.DestroySelf();
					return;
				}
				global::InventorySystem.Items.Pickups.PickupSyncInfo info = TargetPickup.Info;
				info.InUse = false;
				TargetPickup.NetworkInfo = info;
			}
		}
	}
}
