namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079ZoomGui : global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079GuiElementBase
	{
		private const float RoundingAccuracy = 10f;

		private global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync _curCamSync;

		private string _format;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Vector2 _minPos;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Vector2 _maxPos;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.RectTransform _slider;

		[global::UnityEngine.SerializeField]
		private global::TMPro.TextMeshProUGUI _text;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _root;

		internal override void Init(global::PlayerRoles.PlayableScps.Scp079.Scp079Role role, ReferenceHub owner)
		{
			base.Init(role, owner);
			_format = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.Zoom);
			role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync>(out _curCamSync);
		}

		private void Update()
		{
			global::PlayerRoles.PlayableScps.Scp079.Cameras.CameraZoomAxis zoomAxis = _curCamSync.CurrentCamera.ZoomAxis;
			float num = global::UnityEngine.Mathf.InverseLerp(zoomAxis.MinValue, zoomAxis.MaxValue, zoomAxis.CurValue);
			float num2 = global::UnityEngine.Mathf.Round(10f * zoomAxis.CurrentZoom) / 10f;
			if (num <= 0f)
			{
				_root.SetActive(value: false);
				return;
			}
			_slider.localPosition = global::UnityEngine.Vector2.Lerp(_minPos, _maxPos, num);
			_text.text = string.Format(_format, num2.ToString("0.0"));
			_root.SetActive(value: true);
		}
	}
}
