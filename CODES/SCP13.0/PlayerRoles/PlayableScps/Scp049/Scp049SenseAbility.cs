using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Mirror;
using PlayerRoles.PlayableScps.HUDs;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp049
{
	public class Scp049SenseAbility : KeySubroutine<Scp049Role>
	{
		private const float BaseCooldown = 40f;

		private const float ReducedCooldown = 20f;

		private const float AttemptFailCooldown = 2.5f;

		private const float EffectDuration = 20f;

		private const float HeightDiffIgnoreY = 0.1f;

		private const float NearbyDistanceSqr = 4.5f;

		public readonly AbilityCooldown Cooldown;

		public readonly AbilityCooldown Duration;

		public readonly HashSet<ReferenceHub> DeadTargets;

		public readonly HashSet<ReferenceHub> SpecialZombies;

		public AbilityHud SenseAbilityHUD;

		[SerializeField]
		private GameObject _effectPrefab;

		[SerializeField]
		private float _dotThreshold;

		[SerializeField]
		private float _distanceThreshold;

		[SerializeField]
		private AudioClip _goodsenseClip;

		[SerializeField]
		private CooldownAudio _cooldownAudio;

		private AudioSource _goodsenseSource;

		private Scp049AttackAbility _attackAbility;

		private Transform _pulseEffect;

		private bool _hasPulseUnsafe;

		public ReferenceHub Target { get; private set; }

		public bool HasTarget { get; private set; }

		public float DistanceFromTarget { get; private set; }

		protected override ActionName TargetKey => default(ActionName);

		private bool CanSeeIndicator => false;

		public event Action OnFailed
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

		public event Action OnSuccess
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

		public void ServerLoseTarget()
		{
		}

		public void ServerProcessKilledPlayer(ReferenceHub hub)
		{
		}

		public bool IsTarget(ReferenceHub hub)
		{
			return false;
		}

		protected override void Update()
		{
		}

		protected override void OnKeyDown()
		{
		}

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		private void OnServerHit(ReferenceHub hub)
		{
		}

		private void OnSpectatorTargetChanged()
		{
		}

		private void OnRoleChanged(ReferenceHub userHub, PlayerRoleBase prevRole, PlayerRoleBase newRole)
		{
		}

		public override void ServerProcessCmd(NetworkReader reader)
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientWriteCmd(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		private bool CanFindTarget(out ReferenceHub bestTarget)
		{
			bestTarget = null;
			return false;
		}
	}
}
