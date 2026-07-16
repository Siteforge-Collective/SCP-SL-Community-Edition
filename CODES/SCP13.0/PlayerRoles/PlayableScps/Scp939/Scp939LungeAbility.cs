using System;
using System.Runtime.CompilerServices;
using Mirror;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.Subroutines;
using RelativePositioning;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939LungeAbility : KeySubroutine<Scp939Role>
	{
		[SerializeField]
		private float _harshLandingHeight;

		[SerializeField]
		private float _lungeAngleLimit;

		[SerializeField]
		private float _overallTolerance;

		[SerializeField]
		private float _bottomTolerance;

		[SerializeField]
		private float _secondaryRangeSqr;

		[SerializeField]
		private Scp939LungeAudio _audio;

		[SerializeField]
		private AnimationCurve _jumpSpeedOverPitch;

		[SerializeField]
		private AnimationCurve _forwardSpeedOverPitch;

		private Scp939FocusAbility _focus;

		private Scp939MovementModule _movementModule;

		private FpcStandardRoleBase _playerToHit;

		private Scp939LungeState _state;

		private float _lungePitch;

		private const float MainHitmarkerSize = 1f;

		private const float SecondaryHitmarkerSize = 0.6f;

		public const float LungeDamage = 120f;

		public const float SecondaryDamage = 30f;

		public bool IsReady => false;

		public bool LungeRequested { get; private set; }

		public RelativePosition TriggerPos { get; private set; }

		public float LungeForwardSpeed => 0f;

		public float LungeJumpSpeed => 0f;

		[field: SerializeField]
		public RagdollAnimationTemplate LungeDeathAnim { get; private set; }

		public Scp939LungeState State
		{
			get
			{
				return default(Scp939LungeState);
			}
			private set
			{
			}
		}

		protected override ActionName TargetKey => default(ActionName);

		private bool HasAuthority => false;

		private RelativePosition CurPos => default(RelativePosition);

		public event Action<Scp939LungeState> OnStateChanged
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

		private void OnGrounded()
		{
		}

		private void OnFocusStateChanged()
		{
		}

		protected override void OnKeyDown()
		{
		}

		protected override void Awake()
		{
		}

		protected override void Update()
		{
		}

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		public void TriggerLunge()
		{
		}

		public void ClientSendHit(FpcStandardRoleBase targetRole)
		{
		}

		public override void ClientWriteCmd(NetworkWriter writer)
		{
		}

		public override void ServerProcessCmd(NetworkReader reader)
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}
	}
}
