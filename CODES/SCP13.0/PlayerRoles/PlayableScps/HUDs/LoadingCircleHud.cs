using System;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerRoles.PlayableScps.HUDs
{
	[Serializable]
	public class LoadingCircleHud
	{
		[SerializeField]
		private GameObject _parent;

		[SerializeField]
		private Image _loadingBar;

		[SerializeField]
		private Gradient _colorGradient;

		[SerializeField]
		private bool _inverseFill;

		[SerializeField]
		private bool _hideWhenFull;

		[SerializeField]
		private bool _hideWhenEmpty;

		public void Apply(float percent, bool forceHide = false)
		{
		}
	}
}
