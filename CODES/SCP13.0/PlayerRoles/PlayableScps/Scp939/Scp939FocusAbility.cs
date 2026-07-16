using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using CameraShaking;
using CustomPlayerEffects;
using Mirror;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939FocusAbility : StandardSubroutine<Scp939Role>, IShakeEffect
	{
		private SoundtrackMute _muteEffect;

		private Scp939FocusKeySync _keySync;

		private Scp939ClawAbility _clawAbility;

		private Scp939LungeAbility _lungeAbility;

		private Transform _ownerTransform;

		private float _state;

		private bool _targetState;

		private float _offsetMultiplier;

		private float _relativeFreezeRot;

		private byte _relativeWaypoint;

		[SerializeField]
		private float _transitionSpeed;

		[SerializeField]
		private AudioSource _focusInSource;

		[SerializeField]
		private AnimationCurve _cameraHeightOffset;

		[SerializeField]
		private AnimationCurve _cameraForwardOffset;

		[SerializeField]
		private float _cameraFov;

		private bool _hitAnimationPlaying;

		private const int RotationSpeed = 480;

		private const float RotationRestartState = 0.9f;

		private const float OffsetLerpSpeed = 15f;

		private const float CamMinRadius = 0.16f;

		private const float SourceDecaySpeed = 10f;

		private static readonly CachedLayerMask VisibilityMask;

		private readonly AbilityCooldown _serverSendCooldown;

		private readonly Stopwatch _frozenSw;

		private const int AngleSyncAccuracy = 64;

		private const float ResendCooldown = 1.5f;

		public float State
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public bool TargetState
		{
			get
			{
				return false;
			}
			private set
			{
			}
		}

		public float FrozenTime => 0f;

		public float FrozenRotation => 0f;

		public float AngularDeviation => 0f;

		private byte CurrentWaypointId => 0;

		private bool IsAvailable => false;

		public event Action OnStateChanged
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		private void Update()
		{
		}

		private void UpdateRelativeRotation()
		{
		}

		protected override void Awake()
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		public bool GetEffect(ReferenceHub ply, out ShakeEffectValues values)
		{
            values = default(ShakeEffectValues);
            return false;
		}
	}
}
