using System.Collections.Generic;
using MapGeneration;
using PlayerRoles.PlayableScps.Scp079.Cameras;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerRoles.PlayableScps.Scp079.Map
{
	public class SurfaceMap : MonoBehaviour, IZoneMap
	{
		private const float ActivationMagSqr = 3500f;

		private static readonly Vector3 BottomLeftWorldspace;

		private static readonly Vector3 TopRightWorldspace;

		[SerializeField]
		private RectTransform _parent;

		[SerializeField]
		private Image _template;

		[SerializeField]
		private LczMap _lczMap;

		[SerializeField]
		private Vector2 _spacing;

		[SerializeField]
		private TextMeshProUGUI _zoneLabel;

		[SerializeField]
		private RectTransform _scalerRoot;

		[SerializeField]
		private RectTransform _indicatorsRoot;

		private Image[] _icons;

		private Image _parentImage;

		private Transform _nonExactTransform;

		private int _targetCam;

		private List<Scp079Camera> _surfaceCameras;

		public bool Ready { get; private set; }

		public Bounds RectBounds { get; private set; }

		public FacilityZone Zone => default(FacilityZone);

		private Vector2 WorldspaceToAnchored(Vector3 pos)
		{
			return default(Vector2);
		}

		public void Generate()
		{
		}

		public bool TryGetCamera(out Scp079Camera target)
		{
			target = null;
			return false;
		}

		public bool TryGetCenterTransform(Scp079Camera curCam, out Vector3 center)
		{
			center = default(Vector3);
			return false;
		}

		public bool TrySetPlayerIndicator(ReferenceHub ply, RectTransform indicator, bool exact)
		{
			return false;
		}

		public void UpdateOpened(Scp079Camera curCam)
		{
		}
	}
}
