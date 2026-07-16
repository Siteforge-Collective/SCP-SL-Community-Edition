namespace PlayerRoles.PlayableScps.Scp079.Map
{
	public class SurfaceMap : global::UnityEngine.MonoBehaviour, global::PlayerRoles.PlayableScps.Scp079.Map.IZoneMap
	{
		private const float ActivationMagSqr = 3500f;

		private static readonly global::UnityEngine.Vector3 BottomLeftWorldspace = new global::UnityEngine.Vector3(-25.168f, 0f, -82.351f);

		private static readonly global::UnityEngine.Vector3 TopRightWorldspace = new global::UnityEngine.Vector3(149.977f, 0f, 33.144f);

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.RectTransform _parent;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Image _template;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp079.Map.LczMap _lczMap;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Vector2 _spacing;

		[global::UnityEngine.SerializeField]
		private global::TMPro.TextMeshProUGUI _zoneLabel;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.RectTransform _scalerRoot;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.RectTransform _offsetRoot;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.RectTransform _indicatorsRoot;

		private global::UnityEngine.UI.Image[] _icons;

		private global::UnityEngine.UI.Image _parentImage;

		private global::UnityEngine.Transform _nonExactTransform;

		private int _targetCam;

		private global::System.Collections.Generic.List<global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera> _surfaceCameras;

		private global::UnityEngine.Vector2 WorldspaceToAnchored(global::UnityEngine.Vector3 pos)
		{
			global::UnityEngine.Vector2 sizeDelta = _parent.sizeDelta;
			return new global::UnityEngine.Vector2(global::UnityEngine.Mathf.InverseLerp(BottomLeftWorldspace.x, TopRightWorldspace.x, pos.x) * sizeDelta.x, global::UnityEngine.Mathf.InverseLerp(BottomLeftWorldspace.z, TopRightWorldspace.z, pos.z) * sizeDelta.y);
		}

		public void Generate()
		{
			_parentImage = _parent.GetComponent<global::UnityEngine.UI.Image>();
			_nonExactTransform = GetComponentInChildren<global::UnityEngine.UI.LayoutGroup>(includeInactive: true).transform;
			_zoneLabel.text = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Map.ProceduralZoneMap.ZoneTranslations[global::MapGeneration.FacilityZone.Surface]);
			_surfaceCameras = new global::System.Collections.Generic.List<global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera>();
			foreach (global::PlayerRoles.PlayableScps.Scp079.Scp079InteractableBase allInstance in global::PlayerRoles.PlayableScps.Scp079.Scp079InteractableBase.AllInstances)
			{
				if (allInstance is global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera item && allInstance.Room.Zone == global::MapGeneration.FacilityZone.Surface)
				{
					_surfaceCameras.Add(item);
				}
			}
			int count = _surfaceCameras.Count;
			_icons = new global::UnityEngine.UI.Image[count];
			for (int i = 0; i < count; i++)
			{
				_icons[i] = global::UnityEngine.Object.Instantiate(_template, _parent);
				_icons[i].rectTransform.anchoredPosition = WorldspaceToAnchored(_surfaceCameras[i].Position);
			}
			global::UnityEngine.Object.Destroy(_template);
		}

		public bool TryGetCamera(out global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera target)
		{
			if (_targetCam < 0)
			{
				target = null;
				return false;
			}
			target = _surfaceCameras[_targetCam];
			return true;
		}

		public bool TryGetCenterTransform(global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera curCam, out global::UnityEngine.Vector3 center)
		{
			center = global::UnityEngine.Vector3.zero;
			if (curCam.Room.Zone != global::MapGeneration.FacilityZone.Surface)
			{
				return false;
			}
			for (int i = 0; i < _icons.Length; i++)
			{
				if (!(_surfaceCameras[i] != curCam))
				{
					center = -(global::UnityEngine.Vector2)_icons[i].rectTransform.localPosition - _offsetRoot.anchoredPosition;
					return true;
				}
			}
			return false;
		}

		public bool TrySetTeammateIndicator(ReferenceHub ply, global::UnityEngine.RectTransform indicator, bool exact)
		{
			if (!(ply.roleManager.CurrentRole is global::PlayerRoles.PlayableScps.FpcStandardScp fpcStandardScp))
			{
				return false;
			}
			global::UnityEngine.Vector3 position = fpcStandardScp.FpcModule.Position;
			global::MapGeneration.RoomIdentifier roomIdentifier = global::MapGeneration.RoomIdUtils.RoomAtPositionRaycasts(position);
			if (roomIdentifier == null || roomIdentifier.Zone != global::MapGeneration.FacilityZone.Surface)
			{
				return false;
			}
			indicator.SetParent(exact ? _indicatorsRoot : _nonExactTransform);
			indicator.localPosition = global::UnityEngine.Vector3.zero;
			indicator.localScale = global::UnityEngine.Vector3.one;
			if (exact)
			{
				indicator.anchoredPosition = WorldspaceToAnchored(position);
				indicator.localEulerAngles = global::UnityEngine.Vector3.back * fpcStandardScp.FpcModule.MouseLook.CurrentHorizontal;
			}
			else
			{
				indicator.localEulerAngles = global::UnityEngine.Vector3.zero;
			}
			return true;
		}

		public void UpdateOpened(global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera curCam)
		{
			global::UnityEngine.Bounds rectBounds = _lczMap.RectBounds;
			global::UnityEngine.Vector2 vector = (global::UnityEngine.Vector2)rectBounds.center - _spacing - _parent.sizeDelta / 2f;
			_offsetRoot.localPosition = new global::UnityEngine.Vector3(vector.x - rectBounds.extents.x, vector.y + rectBounds.extents.y, 0f);
			_parentImage.color = ((curCam.Room.Zone == global::MapGeneration.FacilityZone.Surface) ? global::PlayerRoles.PlayableScps.Scp079.Map.ProceduralZoneMap.CurrentZoneColor : global::PlayerRoles.PlayableScps.Scp079.Map.ProceduralZoneMap.OtherZoneColor);
			_targetCam = -1;
			for (int i = 0; i < _icons.Length; i++)
			{
				if (_surfaceCameras[i] == curCam)
				{
					_icons[i].color = global::PlayerRoles.PlayableScps.Scp079.Map.ProceduralZoneMap.CurrentRoomColor;
				}
				else if (_scalerRoot.InverseTransformPoint(_icons[i].rectTransform.position).sqrMagnitude < 3500f)
				{
					_targetCam = i;
					_icons[i].color = global::PlayerRoles.PlayableScps.Scp079.Map.ProceduralZoneMap.HighlightedColor;
				}
				else
				{
					_icons[i].color = global::PlayerRoles.PlayableScps.Scp079.Map.ProceduralZoneMap.CurrentZoneColor;
				}
			}
		}
	}
}
