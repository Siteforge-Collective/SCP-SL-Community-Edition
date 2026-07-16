using System.Collections.Generic;
using InventorySystem.Items.Pickups;
using Mirror;
using RelativePositioning;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939.Ripples
{
	public class PickupRippleTrigger : RippleTriggerBase
	{
		private const float MinVelSqr = 8.5f;

		private const float SoundRangeMin = 4f;

		private const float SoundRangeKg = 0.75f;

		private RelativePosition _syncPos;

		private readonly RateLimiter _rateLimiter;

		private static bool _anyInstances;

		private static readonly HashSet<PickupRippleTrigger> ActiveInstances;

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		private void OnDestroy()
		{
		}

		private void RemoveSelf()
		{
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void OnPickupAdded(ItemPickupBase ipb)
		{
		}

		private static void OnCollided(CollisionDetectionPickup cdp, Collision collision)
		{
		}
	}
}
