using InventorySystem.Items.Pickups;
using InventorySystem.Searching;
using UnityEngine;

namespace InventorySystem.Items.Firearms.Ammo
{
	public class AmmoItem : ItemBase, IItemNametag, ICustomSearchCompletorItem
	{
		public int UnitPrice;

		[SerializeField]
		private string _caliber;

		public override float Weight => 0f;

		public string Name => null;

		public override ItemDescriptionType DescriptionType => default(ItemDescriptionType);

		public SearchCompletor GetCustomSearchCompletor(ReferenceHub hub, ItemPickupBase ipb, ItemBase ib, double disSqrt)
		{
			return null;
		}

		public override void OnAdded(ItemPickupBase pickup)
		{
		}
	}
}
