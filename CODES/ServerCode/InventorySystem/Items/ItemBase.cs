namespace InventorySystem.Items
{
	public abstract class ItemBase : global::UnityEngine.MonoBehaviour
	{
		public ItemType ItemTypeId;

		public ItemCategory Category;

		public ItemTierFlags TierFlags;

		public global::InventorySystem.Items.Thirdperson.ThirdpersonItemBase ThirdpersonModel;

		public global::InventorySystem.Items.ItemThrowSettings ThrowSettings;

		public global::InventorySystem.Items.Pickups.ItemPickupBase PickupDropModel;

		public virtual global::InventorySystem.Items.ItemDescriptionType DescriptionType { get; protected set; }

		public ReferenceHub Owner { get; internal set; }

		public ushort ItemSerial { get; internal set; }

		public bool IsEquipped { get; internal set; }

		internal global::InventorySystem.Inventory OwnerInventory => Owner.inventory;

		public abstract float Weight { get; }

		internal bool IsLocalPlayer => Owner.isLocalPlayer;

		public virtual void OnEquipped()
		{
		}

		public virtual void EquipUpdate()
		{
		}

		public virtual void AlwaysUpdate()
		{
		}

		public virtual void OnHolstered()
		{
		}

		public virtual void OnAdded(global::InventorySystem.Items.Pickups.ItemPickupBase pickup)
		{
		}

		public virtual void OnRemoved(global::InventorySystem.Items.Pickups.ItemPickupBase pickup)
		{
		}

		internal virtual void OnTemplateReloaded(bool wasEverLoaded)
		{
		}

		public virtual global::InventorySystem.Items.Pickups.ItemPickupBase ServerDropItem()
		{
			if (!global::Mirror.NetworkServer.active)
			{
				throw new global::System.InvalidOperationException("Method ServerDropItem can only be executed on the server.");
			}
			if (PickupDropModel == null)
			{
				global::UnityEngine.Debug.LogError("No pickup drop model set. Could not drop the item.");
				return null;
			}
			global::InventorySystem.Items.Pickups.PickupSyncInfo psi = new global::InventorySystem.Items.Pickups.PickupSyncInfo(ItemTypeId, Owner.transform.position, global::UnityEngine.Quaternion.identity, Weight, ItemSerial);
			global::InventorySystem.Items.Pickups.ItemPickupBase itemPickupBase = OwnerInventory.ServerCreatePickup(this, psi);
			OwnerInventory.ServerRemoveItem(psi.Serial, itemPickupBase);
			itemPickupBase.PreviousOwner = new global::Footprinting.Footprint(Owner);
			return itemPickupBase;
		}
	}
}
