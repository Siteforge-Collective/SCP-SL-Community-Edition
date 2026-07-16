using System;
using PlayerRoles.PlayableScps.Scp079.Cameras;
using PlayerRoles.PlayableScps.Scp079.GUI;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.Map
{
	public class Scp079MapGui : Scp079GuiElementBase
	{
		[Serializable]
		private struct MapAnimation
		{
			public AnimationCurve Background;

			public AnimationCurve Horizontal;

			public AnimationCurve Vertical;

			public AnimationCurve Compressor;
		}

		[SerializeField]
		private float _animDuration;

		[SerializeField]
		private CanvasGroup _background;

		[SerializeField]
		private CanvasGroup _compressor;

		[SerializeField]
		private RectTransform _scalable;

		[SerializeField]
		private MapAnimation _closeAnim;

		[SerializeField]
		private MapAnimation _openAnim;

		[SerializeField]
		private MonoBehaviour[] _zoneMaps;

		[SerializeField]
		private RectTransform _mapMover;

		[SerializeField]
		private RectTransform _mapScaler;

		private float _animValue;

		private float _prevAnimVal;

		private float _zoom;

		private bool _prevOpen;

		private Scp079CurrentCameraSync _curCamSync;

		private Vector3 _prevOffset;

		private Vector3 _moverPosition;

		private const string AxisX = "Mouse X";

		private const string AxisY = "Mouse Y";

		private const string AxisScroll = "Mouse ScrollWheel";

		private const float MouseSensitivity = 30f;

		private const float SpectatorLerp = 15f;

		public static Scp079Camera HighlightedCamera { get; private set; }

		public static Vector3 SyncVars { get; internal set; }

		private void Update()
		{
		}

		private void OnOpened()
		{
		}

		private void UpdateOpen()
		{
		}

		private void EvaluateAll(bool open, float val)
		{
		}

		internal override void Init(Scp079Role role, ReferenceHub owner)
		{
		}
	}
}
