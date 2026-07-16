namespace PlayerRoles.PlayableScps.Scp939.Ripples
{
	public class VoiceChatRippleTrigger : global::PlayerRoles.PlayableScps.Scp939.Ripples.RippleTriggerBase
	{
		private readonly global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown _radioCooldown = new global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown();

		private readonly global::System.Collections.Generic.Dictionary<global::PlayerRoles.HumanRole, global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown> _cooldowns = new global::System.Collections.Generic.Dictionary<global::PlayerRoles.HumanRole, global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown>();

		private readonly global::System.Collections.Generic.Dictionary<global::PlayerRoles.HumanRole, float> _prevLoudness = new global::System.Collections.Generic.Dictionary<global::PlayerRoles.HumanRole, float>();

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _cooldownPerLoudness;

		[global::UnityEngine.SerializeField]
		private float _minLoudness;

		[global::UnityEngine.SerializeField]
		private float _radioCooldownDuration;

		[global::UnityEngine.SerializeField]
		private float _loudnessDecayRate;

		public override void SpawnObject()
		{
			base.SpawnObject();
			_cooldowns.Clear();
			_prevLoudness.Clear();
			_radioCooldown.Clear();
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged += OnRoleChanged;
		}

		public override void ResetObject()
		{
			base.ResetObject();
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged -= OnRoleChanged;
		}

		private void Update()
		{
			if (!base.IsLocalOrSpectated)
			{
				return;
			}
			global::PlayerRoles.PlayerRolesUtils.ForEachRole<global::PlayerRoles.HumanRole>(UpdateHuman);
			if (!_radioCooldown.IsReady)
			{
				return;
			}
			_radioCooldown.Trigger(_radioCooldownDuration);
			foreach (global::VoiceChat.Playbacks.SpatializedRadioPlaybackBase allInstance in global::VoiceChat.Playbacks.SpatializedRadioPlaybackBase.AllInstances)
			{
				if (!allInstance.NoiseSource.mute)
				{
					PlayInRange(allInstance.LastPosition, allInstance.Source.maxDistance, global::UnityEngine.Color.red);
				}
			}
		}

		private void OnRoleChanged(ReferenceHub hub, global::PlayerRoles.PlayerRoleBase prevRole, global::PlayerRoles.PlayerRoleBase newRole)
		{
			if (newRole is global::PlayerRoles.HumanRole key)
			{
				_prevLoudness[key] = 0f;
			}
		}

		private void UpdateHuman(global::PlayerRoles.HumanRole human)
		{
			global::PlayerRoles.Voice.HumanVoiceModule humanVoiceModule = human.VoiceModule as global::PlayerRoles.Voice.HumanVoiceModule;
			float a = (_prevLoudness.TryGetValue(human, out var value) ? value : 0f);
			a = global::UnityEngine.Mathf.Max(a, humanVoiceModule.ProximityPlayback.Loudness);
			if (a > _minLoudness)
			{
				global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown orAdd = _cooldowns.GetOrAdd(human, () => new global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown());
				float maxDistance = humanVoiceModule.ProximityPlayback.Source.maxDistance;
				global::UnityEngine.Vector3 position = human.FpcModule.Position;
				if (orAdd.IsReady && (base.ScpRole.FpcModule.Position - position).sqrMagnitude < maxDistance * maxDistance)
				{
					base.Player.Play(human);
					orAdd.Trigger(_cooldownPerLoudness.Evaluate(a));
				}
			}
			a -= global::UnityEngine.Time.deltaTime * _loudnessDecayRate;
			_prevLoudness[human] = global::UnityEngine.Mathf.Max(0f, a);
		}
	}
}
