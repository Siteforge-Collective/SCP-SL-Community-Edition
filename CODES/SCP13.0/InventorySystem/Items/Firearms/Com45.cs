using InventorySystem.Items.Pickups;
using UnityEngine;

namespace InventorySystem.Items.Firearms
{
	public class Com45 : AutomaticFirearm
	{
		[SerializeField]
		private Vector3[] _offsets;

		private const int MagCount = 3;

		public override FirearmStatus Status
		{
			get
			{
				return default(FirearmStatus);
			}
			set
			{
			}
		}

		public override void OnAdded(ItemPickupBase pickup)
		{
		}
	}
}
