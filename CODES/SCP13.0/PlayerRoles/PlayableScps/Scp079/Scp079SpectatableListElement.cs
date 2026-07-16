using System.Text;
using PlayerRoles.Spectating;
using TMPro;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079SpectatableListElement : FullSizeSpectatableListElement
	{
		[SerializeField]
		private TextMeshProUGUI _info;

		private bool _isSet;

		private string _formatTier;

		private Scp079TierManager _tierMng;

		private readonly StringBuilder _sb;

		private void Awake()
		{
		}

		protected override void OnTargetChanged(SpectatableModuleBase prevTarget, SpectatableModuleBase newTarget)
		{
		}

		protected override void Update()
		{
		}
	}
}
