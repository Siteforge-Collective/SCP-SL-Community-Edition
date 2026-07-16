using System;
using Footprinting;
using InventorySystem.Items.Pickups;
using InventorySystem.Items.Thirdperson;
using Mirror;
using UnityEngine;

namespace InventorySystem.Items
{
	public abstract class ItemBase : MonoBehaviour
	{
		public ItemType ItemTypeId;

		public ItemCategory Category;

		public ItemTierFlags TierFlags;

		public ThirdpersonItemBase ThirdpersonModel;

		public ItemViewmodelBase ViewModel;

		public Texture Icon;

		public ItemThrowSettings ThrowSettings;

		public ItemPickupBase PickupDropModel;

		public virtual ItemDescriptionType DescriptionType { get; protected set; }

		public ReferenceHub Owner { get; internal set; }

		public ushort ItemSerial { get; internal set; }

		public bool IsEquipped { get; internal set; }
		
        internal Inventory OwnerInventory => Owner.inventory;

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

		public virtual void OnAdded(ItemPickupBase pickup)
		{
		}

		public virtual void OnRemoved(ItemPickupBase pickup)
		{
		}

		internal virtual void OnTemplateReloaded(bool wasEverLoaded)
		{
		}

        public virtual ItemPickupBase ServerDropItem()
        {
            if (!NetworkServer.active)
            {
                throw new InvalidOperationException("Method ServerDropItem can only be executed on the server.");
            }
            if (PickupDropModel == null)
            {
                Debug.LogError("No pickup drop model set. Could not drop the item.");
                return null;
            }
            ItemPickupBase itemPickupBase = InventoryExtensions.ServerCreatePickup(psi: new PickupSyncInfo(ItemTypeId, Weight, ItemSerial), inv: OwnerInventory, item: this);
            OwnerInventory.ServerRemoveItem(ItemSerial, itemPickupBase);
            itemPickupBase.PreviousOwner = new Footprint(Owner);
            return itemPickupBase;
        }
    }
}
