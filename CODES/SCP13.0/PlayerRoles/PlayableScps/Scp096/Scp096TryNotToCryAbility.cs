using System.Diagnostics;
using CursorManagement;
using Mirror;
using PlayerRoles.PlayableScps.HumeShield;
using PlayerRoles.Subroutines;
using RelativePositioning;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096TryNotToCryAbility : KeySubroutine<Scp096Role>, ICursorOverride
	{
		private const float HumeRegenerationMultiplier = 2f;

		[SerializeField]
		private float _clientDotTolerance;

		[SerializeField]
		private float _serverDotTolerance;

		[SerializeField]
		private float _clientDisTolerance;

		[SerializeField]
		private float _serverDisTolerance;

		[SerializeField]
		private float _maxVerticalAngle;

		[SerializeField]
		private float _maxDistance;

		[SerializeField]
		private float _minWidth;

		[SerializeField]
		private float _sideOffset;

		[SerializeField]
		private float _groundLevelMaxDiff;

		private DynamicHumeShieldController _dhs;

		private RelativePosition _syncPoint;

		private Quaternion _syncRot;

		private float _cachedBaseRegen;

		private bool _canceled;

		private readonly Stopwatch _freezeSw;

		private const float AbsFreezeDuration = 0.1f;

		private const float RadiusTolerance = 0.9f;

		private static readonly Quaternion[] RotationAngles;

		private static readonly ActionName[] CancelKeys;

		private static readonly CachedLayerMask Mask;

		private static readonly float[] Heights;

		private static readonly Vector3[] Offsets;

		private static readonly Vector3[] GroundPoints;

		public CursorOverrideMode CursorOverride => default(CursorOverrideMode);

		public bool LockMovement => false;

		protected override ActionName TargetKey => default(ActionName);

		private bool IsActive
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		protected override void Update()
		{
		}

		protected override void OnKeyDown()
		{
		}

		public override void ClientWriteCmd(NetworkWriter writer)
		{
		}

		public override void ServerProcessCmd(NetworkReader reader)
		{
		}

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		private void UpdateClient()
		{
		}

		private bool ServerValidate()
		{
			return false;
		}

		private bool ValidatePoint()
		{
			return false;
		}

		private bool ValidateGround()
		{
			return false;
		}

		private bool ValidateWall()
		{
			return false;
		}
	}
}
