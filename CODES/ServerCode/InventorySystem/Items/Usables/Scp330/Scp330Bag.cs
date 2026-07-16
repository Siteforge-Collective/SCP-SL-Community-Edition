namespace InventorySystem.Items.Usables.Scp330
{
	public class Scp330Bag : global::InventorySystem.Items.Usables.UsableItem, global::InventorySystem.Items.ICustomSearchCompletorItem, global::InventorySystem.Items.IAcquisitionConfirmationTrigger
	{
		public int SelectedCandyId;

		public global::System.Collections.Generic.List<global::InventorySystem.Items.Usables.Scp330.CandyKindID> Candies = new global::System.Collections.Generic.List<global::InventorySystem.Items.Usables.Scp330.CandyKindID>();

		public const int MaxCandies = 6;

		public override bool CanStartUsing => false;

		public bool AcquisitionAlreadyReceived { get; set; }

		public override global::InventorySystem.Items.ItemDescriptionType DescriptionType => global::InventorySystem.Items.ItemDescriptionType.Scp330Bag;

		public bool IsCandySelected
		{
			get
			{
				if (SelectedCandyId >= 0)
				{
					return SelectedCandyId < Candies.Count;
				}
				return false;
			}
		}

		public global::InventorySystem.Searching.SearchCompletor GetCustomSearchCompletor(ReferenceHub hub, global::InventorySystem.Items.Pickups.ItemPickupBase ipb, global::InventorySystem.Items.ItemBase ib, double disSqrt)
		{
			return new global::InventorySystem.Searching.Scp330SearchCompletor(hub, ipb, ib, disSqrt);
		}

		public override void OnAdded(global::InventorySystem.Items.Pickups.ItemPickupBase pickup)
		{
			base.OnAdded(pickup);
			if (global::Mirror.NetworkServer.active && ServerProcessPickup(base.Owner, pickup as global::InventorySystem.Items.Usables.Scp330.Scp330Pickup, out var bag) && !(bag == null) && !(bag == this))
			{
				ServerRemoveSelf();
			}
		}

		public override void OnRemoved(global::InventorySystem.Items.Pickups.ItemPickupBase pickup)
		{
			base.OnRemoved(pickup);
			if (global::Mirror.NetworkServer.active && pickup is global::InventorySystem.Items.Usables.Scp330.Scp330Pickup scp330Pickup && scp330Pickup != null)
			{
				scp330Pickup.StoredCandies = Candies;
			}
		}

		public override void OnEquipped()
		{
			SelectedCandyId = -1;
		}

		public override void OnHolstered()
		{
			IsUsing = false;
		}

		public void ServerConfirmAcqusition()
		{
			ServerRefreshBag();
		}

		public override void ServerOnUsingCompleted()
		{
			if (IsCandySelected && global::InventorySystem.Items.Usables.Scp330.Scp330Candies.CandiesById.TryGetValue(Candies[SelectedCandyId], out var value))
			{
				IsUsing = false;
				value.ServerApplyEffects(base.Owner);
				Candies.RemoveAt(SelectedCandyId);
				base.OwnerInventory.ServerSelectItem(0);
				ServerRefreshBag();
			}
		}

		public void DropCandy(int index)
		{
			SendClientMessage(index, drop: true);
		}

		public void SelectCandy(int index)
		{
			SelectedCandyId = index;
			SendClientMessage(index, drop: false);
		}

		public bool TryAddSpecific(global::InventorySystem.Items.Usables.Scp330.CandyKindID kind)
		{
			if (Candies.Count >= 6)
			{
				return false;
			}
			Candies.Add(kind);
			return true;
		}

		public global::InventorySystem.Items.Usables.Scp330.CandyKindID TryRemove(int index)
		{
			if (index < 0 || index > Candies.Count)
			{
				return global::InventorySystem.Items.Usables.Scp330.CandyKindID.None;
			}
			global::InventorySystem.Items.Usables.Scp330.CandyKindID result = Candies[index];
			Candies.RemoveAt(index);
			ServerRefreshBag();
			return result;
		}

		public static bool ServerProcessPickup(ReferenceHub ply, global::InventorySystem.Items.Usables.Scp330.Scp330Pickup pickup, out global::InventorySystem.Items.Usables.Scp330.Scp330Bag bag)
		{
			if (!TryGetBag(ply, out bag))
			{
				int num = ((!(pickup == null)) ? pickup.Info.Serial : 0);
				return ply.inventory.ServerAddItem(ItemType.SCP330, (ushort)num, pickup) != null;
			}
			bool result = false;
			if (pickup == null)
			{
				result = bag.TryAddSpecific(global::InventorySystem.Items.Usables.Scp330.Scp330Candies.GetRandom());
			}
			else
			{
				while (pickup.StoredCandies.Count > 0 && bag.TryAddSpecific(pickup.StoredCandies[0]))
				{
					result = true;
					pickup.StoredCandies.RemoveAt(0);
				}
			}
			if (bag.AcquisitionAlreadyReceived)
			{
				bag.ServerRefreshBag();
			}
			return result;
		}

		public static bool TryGetBag(ReferenceHub hub, out global::InventorySystem.Items.Usables.Scp330.Scp330Bag bag)
		{
			bag = null;
			bool result = false;
			foreach (global::System.Collections.Generic.KeyValuePair<ushort, global::InventorySystem.Items.ItemBase> item in hub.inventory.UserInventory.Items)
			{
				if (item.Value is global::InventorySystem.Items.Usables.Scp330.Scp330Bag scp330Bag)
				{
					bag = scp330Bag;
					result = true;
					if (scp330Bag.Candies.Count > 0)
					{
						return true;
					}
				}
			}
			return result;
		}

		public static void AddSimpleRegeneration(ReferenceHub hub, float rate, float duration)
		{
			global::UnityEngine.AnimationCurve regenCurve = global::UnityEngine.AnimationCurve.Constant(0f, duration, rate);
			global::InventorySystem.Items.Usables.UsableItemsController.GetHandler(hub).ActiveRegenerations.Add(new global::InventorySystem.Items.Usables.RegenerationProcess(regenCurve, 1f, 1f));
		}

		private void SendClientMessage(int candyIdex, bool drop)
		{
			global::Mirror.NetworkClient.Send(new global::InventorySystem.Items.Usables.Scp330.SelectScp330Message
			{
				Serial = base.ItemSerial,
				CandyID = (byte)candyIdex,
				Drop = drop
			});
		}

		public void ServerRefreshBag()
		{
			if (Candies.Count > 0)
			{
				base.OwnerInventory.connectionToClient.Send(new global::InventorySystem.Items.Usables.Scp330.SyncScp330Message
				{
					Serial = base.ItemSerial,
					Candies = Candies
				});
			}
			else
			{
				ServerRemoveSelf();
			}
		}
	}
}
