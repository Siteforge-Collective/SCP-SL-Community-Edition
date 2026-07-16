using System.Collections.Generic;
using System.Diagnostics;
using Interactables.Interobjects;
using PlayerRoles.FirstPersonControl;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096MovementModule : FirstPersonMovementModule
	{
		[SerializeField]
		private float _jumpSpeedRage;

		private const float SlowedSpeed = 2.55f;

		private const float NormalSpeed = 3.9f;

		private const float RageSpeed = 8f;

		private const float ChargeSpeed = 18.5f;

		private const float PryGateDuration = 2f;

		private const float AngleAdjustSpeed = 120f;

		private Scp096StateController _stateController;

		private float _gateLookAngle;

		private float _normalJumpSpeed;

		private readonly Stopwatch _gatePrySw;

		private readonly AnimationCurve _gatePryX;

		private readonly AnimationCurve _gatePryZ;

		private readonly List<Transform> _pryablePoints;

		private static readonly Keyframe[] TemplateKeyframes;

		private float MovementSpeed
		{
			set
			{
			}
		}

		public override bool LockMovement => false;

		protected override FpcMotor NewMotor => null;

		protected override void UpdateMovement()
		{
		}

		private void UpdateSpeedAndOverrides()
		{
		}

		private void UpdateGatePrying()
		{
		}

		private void SetGatePryCurves(int index, Vector3 pos)
		{
		}

		private void Awake()
		{
		}

		public override void SpawnObject()
		{
		}

		public void SetTargetGate(PryableDoor door)
		{
		}
	}
}
