using System;
using PlayerRoles.PlayableScps.Scp079.Map;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079ScannerGui : Scp079GuiElementBase
	{
		private struct ToggleInstance
		{
			public CanvasGroup Fader;

			public Image ToggleImage;

			public Graphic Outline;

			public bool IsSet;
		}

		[Serializable]
		private struct TeamFilter
		{
			public Toggle Checkbox;

			public Team Team;
		}

		[SerializeField]
		private float _lerpSpeed;

		[SerializeField]
		private RectTransform _fillRect;

		[SerializeField]
		private TMP_Text _selectionText;

		[SerializeField]
		private RectOffset _padding;

		[SerializeField]
		private CanvasGroup _uiFader;

		[SerializeField]
		private GameObject _centerCursor;

		[SerializeField]
		private GameObject _toggleInstanceTemplate;

		[SerializeField]
		private Sprite _onIcon;

		[SerializeField]
		private Sprite _offIcon;

		[SerializeField]
		private Color _onOutlineColor;

		[SerializeField]
		private Color _offOutlineColor;

		[SerializeField]
		private float _zoneBorderWidth;

		[SerializeField]
		private TMP_Text _nextScanText;

		[SerializeField]
		private TMP_Text _targetCounterText;

		[SerializeField]
		private TMP_Text _filtersWarningText;

		[SerializeField]
		private Scp079DetectedPlayerIndicator _indicator;

		[SerializeField]
		private TeamFilter[] _teamFilters;

		[SerializeField]
		private AudioClip[] _zoneSelectClips;

		[SerializeField]
		private AudioClip _toggleFliterSound;

		[SerializeField]
		private AudioClip _detectedHumanSound;

		private bool _wasOpen;

		private int _prevZoneCnt;

		private int _prevTeamsCnt;

		private Scp079ScannerTracker _tracker;

		private Scp079ScannerZoneSelector _zoneSelector;

		private Scp079ScannerTeamFilterSelector _teamSelector;

		private CachedValue<Bounds> _combinedBounds;

		private CachedValue<IZoneMap[]> _zoneMaps;

		private CachedValue<ToggleInstance[]> _toggleInstances;

		private static CachedValue<Vector3> _pos;

		public static float MapZoom => 0f;

		public static Vector2 MapPos => default(Vector2);

		public static float AnimInterpolant { get; private set; }

		private Bounds GenerateCombinedBounds()
		{
			return default(Bounds);
		}

		private ToggleInstance[] GenerateToggleInstances()
		{
			return null;
		}

		private void ProcessButtonEvent(int instanceId)
		{
		}

		private void RefreshZoneToggles()
		{
		}

		private void RefreshFilters()
		{
		}

		private void OnHumanDetected(ReferenceHub hub)
		{
		}

		internal override void Init(Scp079Role role, ReferenceHub owner)
		{
		}

		private void OnDestroy()
		{
		}

		private void Update()
		{
		}

		private void UpdateOpen()
		{
		}

		private void UpdateLayout()
		{
		}
	}
}
