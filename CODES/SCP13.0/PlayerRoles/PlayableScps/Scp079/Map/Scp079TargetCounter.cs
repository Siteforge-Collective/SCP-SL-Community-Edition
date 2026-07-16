using System;
using PlayerRoles.PlayableScps.Scp079.GUI;
using TMPro;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.Map
{
	public class Scp079TargetCounter : Scp079GuiElementBase
	{
		[Serializable]
		private struct CounterSet
		{
			[SerializeField]
			private TargetCounter[] _allCounters;

			public string Text => null;
		}

		[Serializable]
		private struct TargetCounter
		{
			[SerializeField]
			private string _defaultValue;

			[SerializeField]
			private string _translationKey;

			[SerializeField]
			private int _translationIndex;

			[SerializeField]
			private Team[] _teams;

			public string Header => null;

			public bool Check(ReferenceHub hub)
			{
				return false;
			}
		}

		[SerializeField]
		private CounterSet[] _countersForTier;

		[SerializeField]
		private TextMeshProUGUI _counterTxt;

		private bool _isDirty;

		private Scp079TierManager _tier;

		internal override void Init(Scp079Role role, ReferenceHub owner)
		{
		}

		private void OnDestroy()
		{
		}

		private void Update()
		{
		}

		private void OnRoleChanged(ReferenceHub hub, PlayerRoleBase oldRole, PlayerRoleBase newRole)
		{
		}
	}
}
