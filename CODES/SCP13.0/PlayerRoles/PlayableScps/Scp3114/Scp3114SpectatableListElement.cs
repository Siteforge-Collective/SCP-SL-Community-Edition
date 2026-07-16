using PlayerRoles.Spectating;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerRoles.PlayableScps.Scp3114
{
	public class Scp3114SpectatableListElement : StandardSpectatableListElement
	{
		[SerializeField]
		private RawImage _disguiseIcon;

		[SerializeField]
		private TMP_Text _disguiseText;

		[SerializeField]
		private TMP_Text _overwatchNickname;

		protected override void Update()
		{
		}

		private bool TryGetDisguise(Scp3114Role scp3114, out HumanRole role)
		{
			role = null;
			return false;
		}
	}
}
