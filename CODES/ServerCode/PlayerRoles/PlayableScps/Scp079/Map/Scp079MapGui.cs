namespace PlayerRoles.PlayableScps.Scp079.Map
{
	public class Scp079MapGui : global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079GuiElementBase
	{
		[global::System.Serializable]
		private struct MapAnimation
		{
			public global::UnityEngine.AnimationCurve Background;

			public global::UnityEngine.AnimationCurve Horizontal;

			public global::UnityEngine.AnimationCurve Vertical;

			public global::UnityEngine.AnimationCurve Compressor;
		}

		[global::UnityEngine.SerializeField]
		private float _animDuration;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.CanvasGroup _background;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.CanvasGroup _compressor;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.RectTransform _scalable;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp079.Map.Scp079MapGui.MapAnimation _closeAnim;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp079.Map.Scp079MapGui.MapAnimation _openAnim;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.MonoBehaviour[] _zoneMaps;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.RectTransform _mapMover;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.RectTransform _mapScaler;

		private float _animValue;

		private float _prevAnimVal;

		private float _zoom;

		private bool _prevOpen;

		private global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync _curCamSync;

		private global::UnityEngine.Vector3 _prevOffset;

		private const string AxisX = "Mouse X";

		private const string AxisY = "Mouse Y";

		private const string AxisScroll = "Mouse ScrollWheel";

		private const float MouseSensitivity = 30f;

		private const float SpectatorLerp = 15f;

		public static global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera HighlightedCamera { get; private set; }

		public static global::UnityEngine.Vector3 SyncVars { get; internal set; }

		private void Update()
		{
			bool mapState = global::PlayerRoles.PlayableScps.Scp079.Map.Scp079ToggleMapAbility.MapState;
			float num = global::UnityEngine.Time.deltaTime * (float)(mapState ? 1 : (-1));
			_animValue = global::UnityEngine.Mathf.Clamp(_animValue + num, 0f, _animDuration);
			if (mapState)
			{
				if (!_prevOpen)
				{
					OnOpened();
				}
				UpdateOpen();
			}
			else if (_prevOpen)
			{
				HighlightedCamera = null;
			}
			_prevOpen = mapState;
			if (_prevAnimVal != _animValue)
			{
				EvaluateAll(mapState, _animValue);
				_prevAnimVal = _animValue;
			}
		}

		private void OnOpened()
		{
			if (!global::PlayerRoles.PlayableScps.Scp079.Scp079Role.LocalInstanceActive)
			{
				_mapMover.localPosition = (global::UnityEngine.Vector2)SyncVars;
				_mapScaler.localScale = global::UnityEngine.Vector3.one * SyncVars.z;
				return;
			}
			_zoom = 1f;
			_mapScaler.localScale = global::UnityEngine.Vector3.one;
			global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera currentCamera = _curCamSync.CurrentCamera;
			global::UnityEngine.MonoBehaviour[] zoneMaps = _zoneMaps;
			for (int i = 0; i < zoneMaps.Length; i++)
			{
				if (((global::PlayerRoles.PlayableScps.Scp079.Map.IZoneMap)zoneMaps[i]).TryGetCenterTransform(currentCamera, out var center))
				{
					_prevOffset = center;
					break;
				}
			}
			_mapMover.anchoredPosition = _prevOffset;
		}

		private void UpdateOpen()
		{
			HighlightedCamera = null;
			bool flag = !global::PlayerRoles.PlayableScps.Scp079.Scp079Role.LocalInstanceActive;
			bool flag2 = !global::UnityEngine.Cursor.visible && !flag;
			if (flag2)
			{
				float num = (SensitivitySettings.RawInput ? global::UnityEngine.Input.GetAxisRaw("Mouse ScrollWheel") : global::UnityEngine.Input.GetAxis("Mouse ScrollWheel"));
				_zoom = global::UnityEngine.Mathf.Clamp(_zoom + num, 0.3f, 2.1f);
				_mapScaler.localScale = global::UnityEngine.Vector3.one * _zoom;
				global::UnityEngine.Vector2 vector = (SensitivitySettings.RawInput ? new global::UnityEngine.Vector2(global::UnityEngine.Input.GetAxisRaw("Mouse X"), global::UnityEngine.Input.GetAxisRaw("Mouse Y")) : new global::UnityEngine.Vector2(global::UnityEngine.Input.GetAxis("Mouse X"), global::UnityEngine.Input.GetAxis("Mouse Y")));
				global::UnityEngine.Vector3 localPosition = _mapMover.localPosition;
				localPosition -= 30f * SensitivitySettings.SensMultiplier * (global::UnityEngine.Vector3)vector / _zoom;
				_mapMover.localPosition = localPosition;
				SyncVars = new global::UnityEngine.Vector3(localPosition.x, localPosition.y, _zoom);
			}
			if (flag)
			{
				_mapScaler.localScale = global::UnityEngine.Vector3.one * SyncVars.z;
				_mapMover.localPosition = global::UnityEngine.Vector3.Lerp(_mapMover.localPosition, SyncVars, global::UnityEngine.Time.deltaTime * 15f);
			}
			global::UnityEngine.MonoBehaviour[] zoneMaps = _zoneMaps;
			for (int i = 0; i < zoneMaps.Length; i++)
			{
				global::PlayerRoles.PlayableScps.Scp079.Map.IZoneMap zoneMap = (global::PlayerRoles.PlayableScps.Scp079.Map.IZoneMap)zoneMaps[i];
				if (!flag2 && !flag)
				{
					break;
				}
				if (zoneMap.TryGetCamera(out var target))
				{
					HighlightedCamera = target;
				}
			}
			global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera currentCamera = _curCamSync.CurrentCamera;
			zoneMaps = _zoneMaps;
			for (int i = 0; i < zoneMaps.Length; i++)
			{
				((global::PlayerRoles.PlayableScps.Scp079.Map.IZoneMap)zoneMaps[i]).UpdateOpened(currentCamera);
			}
		}

		private void EvaluateAll(bool open, float val)
		{
			global::PlayerRoles.PlayableScps.Scp079.Map.Scp079MapGui.MapAnimation mapAnimation = (open ? _openAnim : _closeAnim);
			_background.alpha = mapAnimation.Background.Evaluate(val);
			_compressor.alpha = mapAnimation.Compressor.Evaluate(val);
			_scalable.localScale = new global::UnityEngine.Vector3(mapAnimation.Horizontal.Evaluate(val), mapAnimation.Vertical.Evaluate(val), 1f);
		}

		internal override void Init(global::PlayerRoles.PlayableScps.Scp079.Scp079Role role, ReferenceHub owner)
		{
			base.Init(role, owner);
			role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync>(out _curCamSync);
			global::UnityEngine.MonoBehaviour[] zoneMaps = _zoneMaps;
			for (int i = 0; i < zoneMaps.Length; i++)
			{
				((global::PlayerRoles.PlayableScps.Scp079.Map.IZoneMap)zoneMaps[i]).Generate();
			}
		}
	}
}
