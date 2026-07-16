using System.Diagnostics;
using PlayerRoles.FirstPersonControl;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp173
{
	public class Scp173MovementModule : FirstPersonMovementModule
	{
		private float _normalSpeed;

		private float _fastSpeed;

		private float _observerSpeed;

		private float _jumpSpeed;

		private Scp173Role _role;

		private Scp173BreakneckSpeedsAbility _breakneckSpeeds;

		private Scp173ObserversTracker _observersTracker;

		private static int _snapMask;

		private readonly Stopwatch _lookStopwatch;

		private const float ObserverSpeedMultiplier = 2f;

		private const float ServerStopTime = 0.4f;

		private const int GlassLayerMask = 16384;

		private const float GlassRaycastDis = 0.3f;

		private const float RaycastFloorHeight = 3.6f;

		private const float RaycastCeilHeight = 7.2f;

		private const float RaycastPilotRadius = 0.025f;

		private const float RaycastFloorDot = 0.15f;

		private const float RaycastCcRadiusMultiplier = 1.2f;

		private const float RaycastStabilityRadiusRatio = 0.5f;

		private const float RaycastStabilityDistance = 0.6f;

		private float MovementSpeed
		{
			set
			{
			}
		}

		private float TargetSpeed => 0f;

		private float ServerSpeed => 0f;

		private static int TpMask => 0;

		private void Awake()
		{
		}

		protected override void UpdateMovement()
		{
		}

		private void UpdateGlassBreaking()
		{
		}

		public bool TryGetTeleportPos(float maxDis, out Vector3 pos, out float usedDistance)
		{
			pos = default(Vector3);
			usedDistance = default(float);
			return false;
		}

		private bool CheckTeleportPosition(RaycastHit hit, out Vector3 groundPoint)
		{
			groundPoint = default(Vector3);
			return false;
		}

		public void ServerTeleportTo(Vector3 pos)
		{
		}
	}
}
