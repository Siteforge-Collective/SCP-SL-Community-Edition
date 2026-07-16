namespace PlayerRoles.PlayableScps.Scp079.Map
{
	public class Scp079TargetCounter : global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079GuiElementBase
	{
		[global::System.Serializable]
		private struct CounterSet
		{
			[global::UnityEngine.SerializeField]
			private global::PlayerRoles.PlayableScps.Scp079.Map.Scp079TargetCounter.TargetCounter[] _allCounters;

			public string Text
			{
				get
				{
					global::System.Text.StringBuilder stringBuilder = global::NorthwoodLib.Pools.StringBuilderPool.Shared.Rent();
					global::PlayerRoles.PlayableScps.Scp079.Map.Scp079TargetCounter.TargetCounter[] allCounters = _allCounters;
					for (int i = 0; i < allCounters.Length; i++)
					{
						global::PlayerRoles.PlayableScps.Scp079.Map.Scp079TargetCounter.TargetCounter targetCounter = allCounters[i];
						stringBuilder.Append(targetCounter.Header);
						stringBuilder.Append(": ");
						stringBuilder.Append(global::Utils.NonAllocLINQ.HashsetExtensions.Count(ReferenceHub.AllHubs, ((global::PlayerRoles.PlayableScps.Scp079.Map.Scp079TargetCounter.TargetCounter)targetCounter).Check));
						stringBuilder.Append("\n");
					}
					return global::NorthwoodLib.Pools.StringBuilderPool.Shared.ToStringReturn(stringBuilder);
				}
			}
		}

		[global::System.Serializable]
		private struct TargetCounter
		{
			[global::UnityEngine.SerializeField]
			private string _defaultValue;

			[global::UnityEngine.SerializeField]
			private string _translationKey;

			[global::UnityEngine.SerializeField]
			private int _translationIndex;

			[global::UnityEngine.SerializeField]
			private global::PlayerRoles.Team[] _teams;

			public string Header => TranslationReader.Get(_translationKey, _translationIndex, _defaultValue);

			public bool Check(ReferenceHub hub)
			{
				return _teams.Contains(hub.GetTeam());
			}
		}

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp079.Map.Scp079TargetCounter.CounterSet[] _countersForTier;

		[global::UnityEngine.SerializeField]
		private global::TMPro.TextMeshProUGUI _counterTxt;

		private bool _isDirty;

		private global::PlayerRoles.PlayableScps.Scp079.Scp079TierManager _tier;

		internal override void Init(global::PlayerRoles.PlayableScps.Scp079.Scp079Role role, ReferenceHub owner)
		{
			base.Init(role, owner);
			_isDirty = true;
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged += OnRoleChanged;
			role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Scp079TierManager>(out _tier);
		}

		private void OnDestroy()
		{
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged -= OnRoleChanged;
		}

		private void Update()
		{
			if (global::PlayerRoles.PlayableScps.Scp079.Map.Scp079ToggleMapAbility.MapState && _isDirty)
			{
				int num = global::UnityEngine.Mathf.Clamp(_tier.AccessTierIndex, 0, _countersForTier.Length - 1);
				_counterTxt.text = _countersForTier[num].Text;
			}
		}

		private void OnRoleChanged(ReferenceHub hub, global::PlayerRoles.PlayerRoleBase oldRole, global::PlayerRoles.PlayerRoleBase newRole)
		{
			_isDirty = true;
		}
	}
}
