using System.Runtime.InteropServices;
using InventorySystem.Items.Pickups;
using Mirror;
using UnityEngine;

namespace InventorySystem.Items.MicroHID
{
	public class MicroHIDPickup : CollisionDetectionPickup
	{
		[SyncVar]
		public float Energy;

		[SerializeField]
		private Transform _needle;

		private float _prevEnergy;

		public float NetworkEnergy
		{
			get
			{
				return 0f;
			}
			[param: In]
			set
			{
			}
		}

		private void Update()
		{
		}
	}
}
