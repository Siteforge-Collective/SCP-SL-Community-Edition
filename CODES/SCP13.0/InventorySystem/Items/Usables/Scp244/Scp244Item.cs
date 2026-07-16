using InventorySystem.Items.Pickups;
using InventorySystem.Searching;

namespace InventorySystem.Items.Usables.Scp244
{
	public class Scp244Item : UsableItem, ICustomSearchCompletorItem
	{
		private bool _primed;

		private const float DropHeightOffset = 0.72f;

		public override void ServerOnUsingCompleted()
		{
		}

		public override void OnUsingCancelled()
		{
		}

		public override ItemPickupBase ServerDropItem()
		{
			return null;
		}

		public SearchCompletor GetCustomSearchCompletor(ReferenceHub hub, ItemPickupBase ipb, ItemBase ib, double disSqrt)
		{
			return null;
		}
	}
}
