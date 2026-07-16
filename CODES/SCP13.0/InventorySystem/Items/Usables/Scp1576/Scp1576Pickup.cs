using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InventorySystem.Items.Pickups;
using Mirror;
using UnityEngine;

namespace InventorySystem.Items.Usables.Scp1576
{
	public class Scp1576Pickup : CollisionDetectionPickup
	{
		private byte _prevSyncHorn;

		[SyncVar]
		private byte _syncHorn;

		[SerializeField]
		private Transform _horn;

		[SerializeField]
		private Vector3 _posZero;

		[SerializeField]
		private Vector3 _posOne;

		public float HornPos
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public static event Action<ushort, float> OnHornPositionUpdated;

		private void Update()
		{
		}
	}
}
