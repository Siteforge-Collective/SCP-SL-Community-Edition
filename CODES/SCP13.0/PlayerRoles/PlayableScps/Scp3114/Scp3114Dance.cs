using CameraShaking;
using Mirror;
using PlayerRoles.Subroutines;
using RelativePositioning;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp3114
{
	public class Scp3114Dance : StandardSubroutine<Scp3114Role>, IShakeEffect
	{
		[SerializeField]
		private int _danceVariants;

		[SerializeField]
		private string _secretCode;

		[SerializeField]
		private float _cameraAdjustSpeed;

		[SerializeField]
		private float _cameraMaxDistance;

		[SerializeField]
		private ActionName[] _cancelKeys;

		private float _curCameraDis;

		private int _nextMatchIndex;

		private int _codeLength;

		private bool _shakeActive;

		private Transform _tr;

		private Vector3 _lastFwd;

		private double _lastRpcTime;

		private RelativePosition _serverStartPos;

		private const float MaxPositionDiffSqr = 2.5f;

		private const float CameraRadius = 0.16f;

		private const float MinDuration = 0.5f;

		public bool IsDancing { get; private set; }

		public int DanceVariant { get; private set; }

		public bool ThirdpersonMode => false;

		private void Update()
		{
		}

		private void UpdateServer()
		{
		}

		private void SetModelVisibility(bool b)
		{
		}

		private void UpdateCamera()
		{
		}

		private bool TryStartDancing()
		{
			return false;
		}

		private bool TryEndDancing()
		{
			return false;
		}

		protected override void Awake()
		{
		}

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		public override void ClientWriteCmd(NetworkWriter writer)
		{
		}

		public override void ServerProcessCmd(NetworkReader reader)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public bool GetEffect(ReferenceHub ply, out ShakeEffectValues values)
		{
            values = default;
            return false;
		}
	}
}
