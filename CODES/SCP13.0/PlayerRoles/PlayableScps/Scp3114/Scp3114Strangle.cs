using System;
using System.Runtime.CompilerServices;
using CursorManagement;
using Mirror;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.Subroutines;
using PlayerStatsSystem;
using RelativePositioning;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp3114
{
	public class Scp3114Strangle : KeySubroutine<Scp3114Role>, ICursorOverride
	{
		private enum RpcType
		{
			TargetResync = 0,
			TargetKilled = 1,
			AttackInterrupted = 2,
			OutOfRange = 3
		}

		public readonly struct StrangleTarget
		{
			public readonly ReferenceHub Target;

			public readonly RelativePosition TargetPosition;

			public readonly RelativePosition AttackerPosition;

			public void WriteSelf(NetworkWriter writer)
			{
			}

			public StrangleTarget(ReferenceHub target, Vector3 targetPosition, Vector3 attackerPosition)
			{
				Target = null;
				TargetPosition = default(RelativePosition);
				AttackerPosition = default(RelativePosition);
			}

			public StrangleTarget(ReferenceHub target, RelativePosition targetPosition, RelativePosition attackerPosition)
			{
				Target = null;
				TargetPosition = default(RelativePosition);
				AttackerPosition = default(RelativePosition);
			}

			public StrangleTarget(ReferenceHub target, NetworkReader reader)
			{
				Target = null;
				TargetPosition = default(RelativePosition);
				AttackerPosition = default(RelativePosition);
			}
		}

		private static readonly CachedLayerMask BlockerMask;

		[SerializeField]
		private float _targetAcquisitionMinDot;

		[SerializeField]
		private float _targetAcquisitionMaxDistance;

		[SerializeField]
		private float _stranglePositionOffset;

		[SerializeField]
		private float _strangleSqrCutoffHorizontal;

		[SerializeField]
		private float _strangleAbsCutoffVertical;

		[SerializeField]
		private float _onReleaseCooldown;

		[SerializeField]
		private float _onInterruptedCooldown;

		[SerializeField]
		private float _onKillCooldown;

		[SerializeField]
		private float _maxKeyHoldTime;

		[SerializeField]
		private float _attackerDamageImmunityTime;

		private Scp3114Slap _attackAbility;

		private ReferenceHub _clientDesiredTargetHub;

		private FpcStandardRoleBase _clientDesiredTargetRole;

		private Vector3 _validatedPosition;

		private bool _clientTargetting;

		private float _keyHoldingTime;

		private bool _warningHintAlreadyDisplayed;

		private RpcType _rpcType;

		public readonly AbilityCooldown ClientCooldown;

		public StrangleTarget? SyncTarget { get; private set; }

		public CursorOverrideMode CursorOverride => default(CursorOverrideMode);

		public bool LockMovement => false;

		protected override ActionName TargetKey => default(ActionName);

		public event Action OnAttemptedWhileDisguised
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

		public event Action ServerOnKill
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

		public event Action ServerOnBegin
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

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		protected override void OnKeyUp()
		{
		}

		protected override void Awake()
		{
		}

		protected override void Update()
		{
		}

		private void OnThisPlayerDamaged(DamageHandlerBase dhb)
		{
		}

		private void ServerUpdateTarget()
		{
		}

		private void OnAnyPlayerDied(ReferenceHub deadPly, DamageHandlerBase handler)
		{
		}

		private void ClientUpdateTarget()
		{
		}

		private void OnRoleChanged(ReferenceHub userHub, PlayerRoleBase prevRole, PlayerRoleBase newRole)
		{
		}

		private void OnPlayerRemoved(ReferenceHub hub)
		{
		}

		private void ClientAttack()
		{
		}

		private StrangleTarget? ProcessAttackRequest(NetworkReader reader)
		{
			return null;
		}

		private Vector3 GetStranglePosition(IFpcRole targetFpc)
		{
			return default(Vector3);
		}

		private bool ValidateTarget(ReferenceHub player)
		{
			return false;
		}
	}
}
