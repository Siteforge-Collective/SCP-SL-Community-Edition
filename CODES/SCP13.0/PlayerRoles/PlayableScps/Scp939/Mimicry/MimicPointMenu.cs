using TMPro;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	public class MimicPointMenu : MimicryMenuBase
	{
		[SerializeField]
		private GameObject _onRoot;

		[SerializeField]
		private GameObject _offRoot;

		[SerializeField]
		private TMP_Text _distanceText;

		private MimicPointController _mimicController;

		private const string DistanceTextFormat = "{0}m / {1}m";

		private void Update()
		{
		}

		protected override void Setup(Scp939Role role)
		{
		}

		public void RequestToggle()
		{
		}
	}
}
