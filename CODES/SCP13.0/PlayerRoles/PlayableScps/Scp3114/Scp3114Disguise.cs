using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using PlayerRoles.PlayableScps.Scp049;
using PlayerRoles.Ragdolls;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp3114
{
	public class Scp3114Disguise : RagdollAbilityBase<Scp3114Role>
	{
		public readonly AbilityCooldown Cooldown;

		private readonly Dictionary<BasicRagdoll, byte> _prevUnitIds;

		private Scp3114Identity _identity;

		[SerializeField]
		private AudioSource _equipSkinSound;

		protected override float RangeSqr => 0f;

		protected override float Duration => 0f;

		public event Action<Scp3114HudTranslation> OnClientError
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

		public override void SpawnObject()
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

		protected override void OnKeyUp()
		{
		}

		protected override void OnProgressSet()
		{
		}

		protected override void ServerComplete()
		{
		}

		protected override byte ServerValidateBegin(BasicRagdoll ragdoll)
		{
			return 0;
		}

		protected override bool ClientValidateBegin(BasicRagdoll raycastedRagdoll)
		{
			return false;
		}

		private bool AnyValidateBegin(BasicRagdoll rg, out Scp3114HudTranslation error)
		{
			error = default(Scp3114HudTranslation);
			return false;
		}

		private void ServerOnRagdollCreated(ReferenceHub owner, BasicRagdoll ragdoll)
		{
		}
	}
}
