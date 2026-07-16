using UnityEngine;
using InventorySystem.Items;
using InventorySystem.Items.Pickups;
using InventorySystem.Disarming;
using Interactables;
using InventorySystem.Items.Firearms.Ammo;

namespace InventorySystem.Searching
{
    public abstract class SearchCompletor
    {
        public readonly ReferenceHub Hub;
        public readonly ItemPickupBase TargetPickup;
        public readonly ItemBase TargetItem;
        private readonly double _maxDistanceSquared;

        public virtual bool AllowPickupUponEscape => true;

        protected SearchCompletor(ReferenceHub hub, ItemPickupBase targetPickup, ItemBase targetItem, double maxDistanceSquared)
        {
            this.Hub = hub;
            this.TargetPickup = targetPickup;
            this.TargetItem = targetItem;
            this._maxDistanceSquared = maxDistanceSquared;
        }

        public bool ValidateDistance()
        {
            if (TargetPickup == null || Hub == null) return false;

            float distSq = (Hub.transform.position - TargetPickup.transform.position).sqrMagnitude;
            return distSq <= _maxDistanceSquared;
        }

        protected virtual bool ValidateAny()
        {
            if (Hub == null || Hub.roleManager.CurrentRole == null) return false;
            if (TargetPickup == null || TargetPickup.Info.Locked) return false;

            if (DisarmedPlayers.IsDisarmed(Hub.inventory)) return false;

            if (Hub.interCoordinator.AnyBlocker(BlockedInteraction.GrabItems)) return false;

            return true;
        }

        public virtual bool ValidateStart()
        {
            return ValidateAny() && ValidateDistance();
        }

        public virtual bool ValidateUpdate()
        {
            if (TargetPickup == null) return false;
            return ValidateAny() && ValidateDistance();
        }

        public abstract void Complete();

        public static SearchCompletor FromPickup(SearchCoordinator coordinator, ItemPickupBase targetPickup, double maxDistanceSquared)
        {
            ReferenceHub hub = coordinator.Hub;

            if (targetPickup is AmmoPickup ammoPickup)
            {
                return new AmmoSearchCompletor(hub, targetPickup, null, maxDistanceSquared);
            }

            if (InventoryItemLoader.AvailableItems.TryGetValue(targetPickup.Info.ItemId, out ItemBase template))
            {
                return new ItemSearchCompletor(hub, targetPickup, template, maxDistanceSquared);
            }

            return null;
        }
    }
}