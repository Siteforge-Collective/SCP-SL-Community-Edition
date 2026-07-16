using InventorySystem.Items;
using InventorySystem.Items.Pickups;
using InventorySystem.Items.Usables.Scp244;

namespace InventorySystem.Searching
{
    public class Scp244SearchCompletor : ItemSearchCompletor
    {
        public Scp244SearchCompletor(ReferenceHub hub, ItemPickupBase targetPickup, ItemBase targetItem, double maxDistanceSquared)
            : base(hub, targetPickup, targetItem, maxDistanceSquared)
        {

        }

        protected override bool ValidateAny()
        {
            if (!base.ValidateAny())
                return false;

            if (TargetPickup is Scp244DeployablePickup scp244Pickup)
            {
                return !scp244Pickup.ModelDestroyed;
            }

            return false;
        }

        public override void Complete()
        {
            if (TargetPickup == null)
                return;

            ItemBase itemBase = Hub.inventory.ServerAddItem(TargetPickup.Info.ItemId, TargetPickup.Info.Serial, TargetPickup);

            if (itemBase != null)
            {
                TargetPickup.DestroySelf();
                base.CheckCategoryLimitHint();
            }
        }
    }
}