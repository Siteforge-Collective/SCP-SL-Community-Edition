namespace PlayerRoles.PlayableScps.HUDs
{
	[global::System.Serializable]
	public class LoadingCircleHud
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _parent;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Image _loadingBar;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Gradient _colorGradient;

		[global::UnityEngine.SerializeField]
		private bool _inverseFill;

		[global::UnityEngine.SerializeField]
		private bool _hideWhenFull;

		[global::UnityEngine.SerializeField]
		private bool _hideWhenEmpty;

		public void Apply(float percent, bool forceHide = false)
		{
			percent = global::UnityEngine.Mathf.Clamp01(percent);
			bool flag = forceHide || (percent == 1f && _hideWhenFull) || (percent == 0f && _hideWhenEmpty);
			_parent.SetActive(!flag);
			if (!flag)
			{
				_loadingBar.fillAmount = (_inverseFill ? (1f - percent) : percent);
				_loadingBar.color = _colorGradient.Evaluate(percent);
			}
		}
	}
}
