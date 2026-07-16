using System.Collections.Generic;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939.Ripples
{
	public class VoiceChatRippleTrigger : RippleTriggerBase
	{
		private readonly AbilityCooldown _radioCooldown;

		private readonly Dictionary<HumanRole, AbilityCooldown> _cooldowns;

		private readonly Dictionary<HumanRole, float> _prevLoudness;

		[SerializeField]
		private AnimationCurve _cooldownPerLoudness;

		[SerializeField]
		private float _minLoudness;

		[SerializeField]
		private float _radioCooldownDuration;

		[SerializeField]
		private float _loudnessDecayRate;

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		private void Update()
		{
		}

		private void OnRoleChanged(ReferenceHub hub, PlayerRoleBase prevRole, PlayerRoleBase newRole)
		{
		}

		private void UpdateHuman(HumanRole human)
		{
		}
	}
}
