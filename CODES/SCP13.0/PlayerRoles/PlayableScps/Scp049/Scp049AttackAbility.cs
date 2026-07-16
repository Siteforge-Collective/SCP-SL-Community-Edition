using System;
using System.Runtime.CompilerServices;
using Mirror;
using PlayerRoles.PlayableScps.HUDs;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp049
{
	public class Scp049AttackAbility : KeySubroutine<Scp049Role>
	{
		private const float CooldownTime = 1.5f;

		private const float LagBacktrackingCompensation = 0.4f;

		private static int _attackLayerMask;

		private const float AttackDistance = 1.728f;

		[SerializeField]
		private float _statusEffectDuration;

		[SerializeField]
		private AudioClip _attackSound;

		[SerializeField]
		private AudioClip _secondAttackSound;

		[SerializeField]
		private AudioClip _afflictedAttackSound;

		[SerializeField]
		private CooldownAudio _cooldownAudio;

		private bool _isInstaKillAttack;

		private bool _isTarget;

		private ReferenceHub _target;

		private Scp049ResurrectAbility _resurrect;

		private Scp049SenseAbility _sense;

		public readonly AbilityCooldown Cooldown;

		public AbilityHud AttackAbilityHUD;

		internal static LayerMask AttackMask => default(LayerMask);

		protected override ActionName TargetKey => default(ActionName);

		public event Action<ReferenceHub> OnServerHit
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

		public override void ServerProcessCmd(NetworkReader reader)
		{
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

		public override void ResetObject()
		{
		}

		protected override void Awake()
		{
		}

		protected override void OnKeyDown()
		{
		}

		private bool IsTargetValid(ReferenceHub target)
		{
			return false;
		}

		private bool CanFindTarget(Transform camera, out ReferenceHub target)
		{
			target = null;
			return false;
		}
	}
}
