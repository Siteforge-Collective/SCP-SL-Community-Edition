using System.Collections.Generic;
using Mirror;
using RelativePositioning;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939.Ripples
{
	public class SurfaceRippleTrigger : RippleTriggerBase
	{
		private struct LastRippleInformation
		{
			public bool IsNatural;

			private double _time;

			public static LastRippleInformation Default => default(LastRippleInformation);

			public static LastRippleInformation SurfaceDefault => default(LastRippleInformation);

			public float Elapsed => 0f;
		}

		private const float TimeBetweenSurfaceRipples = 10f;

		private const float NaturalRippleCooldown = 20f;

		private readonly Dictionary<uint, LastRippleInformation> _lastRipples;

		private ReferenceHub _syncPlayer;

		private RelativePosition _syncPos;

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

		public override void ClientWriteCmd(NetworkWriter writer)
		{
		}

		public override void ServerProcessCmd(NetworkReader reader)
		{
		}

		public void ProcessRipple(ReferenceHub hub)
		{
		}

		private void LateUpdate()
		{
		}

		private static bool IsOnSurface(Vector3 position)
		{
			return false;
		}

		private void OnPlayerPlayedRipple(ReferenceHub player)
		{
		}
	}
}
