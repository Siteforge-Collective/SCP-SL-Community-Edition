using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Mirror;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.Subroutines;
using PlayerStatsSystem;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Subroutines
{
	public abstract class ScpAttackAbilityBase<T> : KeySubroutine<T> where T : PlayerRoleBase, IFpcRole
	{
		[SerializeField]
		private float _detectionRadius;

		[SerializeField]
		private float _detectionOffset;

		[SerializeField]
		private AudioClip _killSound;

		[SerializeField]
		private AudioClip[] _hitClipsHuman;

		[SerializeField]
		private AudioClip[] _hitClipsObjects;

		private AttackResult _syncAttack;

		private readonly Stopwatch _delaySw;

		private readonly TolerantAbilityCooldown _clientCooldown;

		private readonly TolerantAbilityCooldown _serverCooldown;

		private static readonly HashSet<FpcBacktracker> BacktrackedPlayers;

		private static readonly IDestructible[] DestDetectionsNonAlloc;

		private static readonly Collider[] DetectionsNonAlloc;

		private static readonly CachedLayerMask DetectionMask;

		private const int DetectionsNumber = 128;

		protected readonly HashSet<ReferenceHub> DetectedPlayers;

		public TolerantAbilityCooldown Cooldown => null;

		public bool AttackTriggered { get; private set; }

		public abstract float DamageAmount { get; }

		protected virtual float SoundRange => 0f;

		protected virtual float AttackDelay => 0f;

		protected virtual float BaseCooldown => 0f;

		protected virtual bool SelfRepeating => false;

		protected virtual bool CanTriggerAbility => false;

		protected override ActionName TargetKey => default(ActionName);

		protected override bool KeyPressable => false;

		private Transform PlyCam => null;

		private Vector3 OverlapSphereOrigin => default(Vector3);

		public event Action<AttackResult> OnAttacked
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

		public event Action OnTriggered
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

		protected abstract DamageHandlerBase DamageHandler(float damage);

		public static ArraySegment<IDestructible> DetectDestructibles(ReferenceHub detector, float offset, float radius, bool losTest = true)
		{
			return default(ArraySegment<IDestructible>);
		}

		private void ServerPerformAttack()
		{
		}

		protected virtual void DamagePlayers()
		{
		}

		protected virtual void DamagePlayer(ReferenceHub hub, float damage)
		{
		}

		protected virtual void DamageDestructible(IDestructible dest)
		{
		}

		protected bool HasAttackResultFlag(AttackResult flag)
		{
			return false;
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

		public override void ResetObject()
		{
		}

		protected override void Update()
		{
		}

		protected virtual void OnClientUpdate()
		{
		}

		protected override void OnKeyDown()
		{
		}

		protected virtual void ClientPerformAttack(bool attackTriggered = true)
		{
		}

		private void OnDrawGizmosSelected()
		{
		}
	}
}
