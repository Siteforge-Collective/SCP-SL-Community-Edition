using System.Collections.Generic;
using PlayerRoles.FirstPersonControl;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939Motor : FpcMotor
	{
		private const float MinDot = 0.4f;

		private const float LungeRadius = 0.6f;

		private const float FocusToleranceOffset = 0.13f;

		private const float FocusTimeTolerance = 3.5f;

		private const int MaxDetections = 32;

		private const float MinDistanceSqr = 0.4f;

		private readonly Scp939Role _role;

		private readonly Scp939FocusAbility _focus;

		private readonly Scp939LungeAbility _lunge;

		private readonly Scp939AmnesticCloudAbility _cloud;

		private static readonly Collider[] Detections;

		private static readonly CachedLayerMask Mask;

		private static readonly HashSet<FpcStandardRoleBase> DetectedTargets;

		private bool IsLocalPlayer => false;

		private bool WantsToLunge => false;

		private bool IsLunging => false;

		protected override float Speed => 0f;

		protected override Vector3 DesiredMove => default(Vector3);

		public override Vector3 Velocity => default(Vector3);

		public bool MovingForwards => false;

		private void ProcessHitboxCollision(HitboxIdentity hid)
		{
		}

		private void ProcessWindowCollision(BreakableWindow window)
		{
		}

		private void OverlapCapsule(Vector3 point1, Vector3 point2)
		{
		}

		protected override void UpdateFloating()
		{
		}

		protected override void UpdateGrounded(ref bool sendJump, float jumpSpeed)
		{
		}

		protected override Vector3 GetFrameMove()
		{
			return default(Vector3);
		}

		public Scp939Motor(ReferenceHub hub, Scp939Role role)
			: base(null, null, enableFallDamage: false)
		{
		}
	}
}
