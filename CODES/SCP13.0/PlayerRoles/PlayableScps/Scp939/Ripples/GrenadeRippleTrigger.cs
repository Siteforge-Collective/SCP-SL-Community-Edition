using System.Collections.Generic;
using InventorySystem.Items.Pickups;
using InventorySystem.Items.ThrowableProjectiles;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939.Ripples
{
	public class GrenadeRippleTrigger : RippleTriggerBase
	{
		private class ThrownGrenadeHandler
		{
			private readonly GrenadeRippleTrigger _tr;

			private readonly float _startTime;

			private int _nextTime;

			public ThrownGrenadeHandler(GrenadeRippleTrigger trigger)
			{
			}

			public bool UpdateSound()
			{
				return false;
			}
		}

		[SerializeField]
		private float[] _rippleTimes;

		[SerializeField]
		private float _audibleRangeSqr;

		private readonly Dictionary<ThrownProjectile, ThrownGrenadeHandler> _trackedGrenades;

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		private void OnProjectileSpawned(ThrownProjectile tp)
		{
		}

		private void OnPickupDestroyed(ItemPickupBase ipb)
		{
		}

		private void Update()
		{
		}
	}
}
