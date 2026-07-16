namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.RectTransform))]
	public class Scp079CanvasScaler : global::UnityEngine.UI.CanvasScaler
	{
		private global::UnityEngine.RectTransform _rt;

		private float _prevRatio;

		private float _defaultRatio;

		private global::UnityEngine.Vector2 _defaultSize;

		protected override void Awake()
		{
			base.Awake();
			_rt = GetComponent<global::UnityEngine.RectTransform>();
			_defaultSize = _rt.sizeDelta;
			_defaultRatio = _defaultSize.x / _defaultSize.y;
		}

		protected override void Update()
		{
			base.Update();
			float num = (float)global::UnityEngine.Screen.width / (float)global::UnityEngine.Screen.height;
			if (_prevRatio != num)
			{
				_prevRatio = num;
				float num2 = num / _defaultRatio;
				_rt.sizeDelta = new global::UnityEngine.Vector2(_defaultSize.x * num2, _defaultSize.y);
			}
		}
	}
}
