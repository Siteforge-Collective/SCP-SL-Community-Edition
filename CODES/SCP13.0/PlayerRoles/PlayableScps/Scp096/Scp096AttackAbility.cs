using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameObjectPools;
using Mirror;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096AttackAbility : KeySubroutine<Scp096Role>, IPoolResettable
	{
		public const float DefaultAttackCooldown = 0.5f;

		private const float HumanDamage = 60f;

		private const float DoorDamage = 250f;

		private const int WindowDamage = 500;

		private const float BacktrackingDisSqr = 9f;

		private const byte LeftAttackSyncCode = 64;

		[SerializeField]
		private float _sphereHitboxRadius;

		[SerializeField]
		private float _sphereHitboxOffset;

		private static readonly List<FpcBacktracker> BacktrackedPlayers;

		private static readonly List<ReferenceHub> PlayersToSend;

		private readonly AbilityCooldown _clientAttackCooldown;

		private readonly TolerantAbilityCooldown _serverAttackCooldown;

		private Scp096HitHandler _leftHitHandler;

		private Scp096HitHandler _rightHitHandler;

		private Scp096AudioPlayer _audioPlayer;

		private Scp096HitResult _hitResult;

		private bool AttackPossible => false;

		protected override ActionName TargetKey => default(ActionName);

		public bool LeftAttack { get; private set; }

		public event Action<Scp096HitResult> OnHitReceived
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

		public event Action OnAttackTriggered
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

		private void ServerAttack()
		{
		}

		public override void ResetObject()
		{
		}

		public override void SpawnObject()
		{
		}

		protected override void Update()
		{
		}

		protected override void OnKeyDown()
		{
		}

		protected override void Awake()
		{
		}
	}
}
