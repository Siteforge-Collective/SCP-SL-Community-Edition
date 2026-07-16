using InventorySystem.Items.Pickups;
using InventorySystem.Items.Usables.Scp244;
using UnityEngine;

namespace Scp914.Processors
{
    public class Scp244ItemProcessor : UsableItemProcessor
    {
        public override ItemPickupBase OnPickupUpgraded(Scp914KnobSetting setting, ItemPickupBase ipb, Vector3 newPosition)
        {
            if (!(ipb is Scp244DeployablePickup { ModelDestroyed: false }))
            {
                return null;
            }
            return base.OnPickupUpgraded(setting, ipb, newPosition);
        }

        protected override void HandleOldPickup(ItemPickupBase ipb, Vector3 newPos)
        {
            if (ipb is Scp244DeployablePickup { State: Scp244State.Active } scp244DeployablePickup)
            {
                scp244DeployablePickup.State = Scp244State.PickedUp;
            }
            else
            {
                base.HandleOldPickup(ipb, newPos);
            }
        }

        protected override void HandleNone(ItemPickupBase ipb, Vector3 newPos)
        {
            if (ipb is Scp244DeployablePickup scp244DeployablePickup)
            {
                if (!scp244DeployablePickup.ModelDestroyed)
                {
                    scp244DeployablePickup.State = Scp244State.Destroyed;
                }
            }
            else
            {
                base.HandleNone(ipb, newPos);
            }
        }
    }
}
