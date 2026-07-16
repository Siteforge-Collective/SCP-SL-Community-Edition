namespace PlayerRoles.PlayableScps.Scp079.Map
{
	public class ProceduralZoneMap : global::UnityEngine.MonoBehaviour, global::PlayerRoles.PlayableScps.Scp079.Map.IZoneMap
	{
		[global::System.Serializable]
		private struct RoomIcon
		{
			public global::MapGeneration.RoomName Room;

			public global::MapGeneration.RoomShape Shape;

			public string Name;

			public global::UnityEngine.Vector3 TextOffset;

			public global::UnityEngine.Bounds IndicatorLimits;
		}

		public static readonly global::System.Collections.Generic.Dictionary<global::MapGeneration.FacilityZone, global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation> ZoneTranslations = new global::System.Collections.Generic.Dictionary<global::MapGeneration.FacilityZone, global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation>
		{
			[global::MapGeneration.FacilityZone.LightContainment] = global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.LightContZone,
			[global::MapGeneration.FacilityZone.HeavyContainment] = global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.HeavyContZone,
			[global::MapGeneration.FacilityZone.Entrance] = global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.EntranceZone,
			[global::MapGeneration.FacilityZone.Surface] = global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.SurfaceZone
		};

		public static readonly global::UnityEngine.Color CurrentZoneColor = new global::UnityEngine.Color(1f, 1f, 1f, 0.055f);

		public static readonly global::UnityEngine.Color OtherZoneColor = new global::UnityEngine.Color(0.5f, 0.5f, 0.5f, 0.042f);

		public static readonly global::UnityEngine.Color CurrentRoomColor = new global::UnityEngine.Color(0f, 1f, 0.4f, 0.15f);

		public static readonly global::UnityEngine.Color HighlightedColor = new global::UnityEngine.Color(1f, 1f, 1f, 0.27f);

		public readonly global::System.Collections.Generic.Dictionary<global::MapGeneration.RoomIdentifier, global::UnityEngine.UI.Image> NodesByRoom = new global::System.Collections.Generic.Dictionary<global::MapGeneration.RoomIdentifier, global::UnityEngine.UI.Image>();

		public readonly global::System.Collections.Generic.Dictionary<global::UnityEngine.UI.Image, global::MapGeneration.RoomIdentifier> RoomsByNode = new global::System.Collections.Generic.Dictionary<global::UnityEngine.UI.Image, global::MapGeneration.RoomIdentifier>();

		[global::UnityEngine.SerializeField]
		protected global::TMPro.TextMeshProUGUI ZoneLabel;

		[global::UnityEngine.SerializeField]
		private global::MapGeneration.FacilityZone _targetZone;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Image _template;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp079.Map.ProceduralZoneMap.RoomIcon[] _roomIcons;

		[global::UnityEngine.SerializeField]
		private float _positionScale;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.RectTransform _rectParent;

		private const float NonExactCooldown = 1.5f;

		private global::UnityEngine.Vector2 _nodeSize;

		private global::PlayerRoles.PlayableScps.Scp079.Scp079InteractableBase _highlightedCamera;

		private readonly global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown _nonExactCooldown = new global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown();

		private readonly global::System.Collections.Generic.HashSet<global::TMPro.TextMeshProUGUI> _spawnedTexts = new global::System.Collections.Generic.HashSet<global::TMPro.TextMeshProUGUI>();

		private readonly global::System.Collections.Generic.Queue<global::PlayerRoles.PlayableScps.Scp079.Map.ProceduralZoneMap> _queuedPostProcessing = new global::System.Collections.Generic.Queue<global::PlayerRoles.PlayableScps.Scp079.Map.ProceduralZoneMap>();

		public global::UnityEngine.Bounds RectBounds { get; protected set; }

		private void Update()
		{
			if (CollectionExtensions.TryDequeue(_queuedPostProcessing, out var element))
			{
				element.PostProcessRooms();
			}
		}

		private bool TryGetIcon(global::MapGeneration.RoomIdentifier room, out global::PlayerRoles.PlayableScps.Scp079.Map.ProceduralZoneMap.RoomIcon result)
		{
			global::PlayerRoles.PlayableScps.Scp079.Map.ProceduralZoneMap.RoomIcon[] roomIcons = _roomIcons;
			for (int i = 0; i < roomIcons.Length; i++)
			{
				global::PlayerRoles.PlayableScps.Scp079.Map.ProceduralZoneMap.RoomIcon roomIcon = roomIcons[i];
				if (roomIcon.Room == room.Name && roomIcon.Shape == room.Shape)
				{
					result = roomIcon;
					return true;
				}
			}
			result = default(global::PlayerRoles.PlayableScps.Scp079.Map.ProceduralZoneMap.RoomIcon);
			return false;
		}

		private void SpawnRoomNode(global::MapGeneration.RoomIdentifier room)
		{
			global::UnityEngine.UI.Image image = global::UnityEngine.Object.Instantiate(_template, _template.rectTransform.parent);
			if (TryGetIcon(room, out var result))
			{
				global::TMPro.TextMeshProUGUI componentInChildren = image.GetComponentInChildren<global::TMPro.TextMeshProUGUI>();
				componentInChildren.rectTransform.localPosition = result.TextOffset;
				componentInChildren.text = result.Name;
				_spawnedTexts.Add(componentInChildren);
				image.rectTransform.localEulerAngles = global::UnityEngine.Vector3.back * room.transform.eulerAngles.y;
			}
			global::UnityEngine.Vector3 position = room.transform.position;
			image.rectTransform.localPosition = new global::UnityEngine.Vector3(position.x, position.z, 0f);
			NodesByRoom.Add(room, image);
			RoomsByNode.Add(image, room);
			image.sprite = room.Icon;
		}

		private bool TryGetCamOfRoom(global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera curCam, global::MapGeneration.RoomIdentifier room, out global::PlayerRoles.PlayableScps.Scp079.Scp079InteractableBase result)
		{
			return global::Utils.NonAllocLINQ.HashsetExtensions.TryGetFirst(global::PlayerRoles.PlayableScps.Scp079.Scp079InteractableBase.AllInstances, (global::PlayerRoles.PlayableScps.Scp079.Scp079InteractableBase x) => x is global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera scp079Camera && scp079Camera.IsMain && scp079Camera.Room == room && scp079Camera != curCam, out result);
		}

		protected virtual void PlaceRooms()
		{
			foreach (global::MapGeneration.RoomIdentifier allRoomIdentifier in global::MapGeneration.RoomIdentifier.AllRoomIdentifiers)
			{
				if (allRoomIdentifier.Zone == _targetZone)
				{
					SpawnRoomNode(allRoomIdentifier);
				}
			}
			foreach (global::UnityEngine.UI.Image value in NodesByRoom.Values)
			{
				value.rectTransform.localPosition *= _positionScale;
			}
		}

		protected virtual void PostProcessRooms()
		{
			foreach (global::TMPro.TextMeshProUGUI spawnedText in _spawnedTexts)
			{
				global::UnityEngine.RectTransform rectTransform = spawnedText.rectTransform;
				rectTransform.rotation = _rectParent.rotation;
				if (!string.IsNullOrEmpty(spawnedText.text))
				{
					rectTransform.GetComponentInChildren<global::UnityEngine.UI.LayoutGroup>(includeInactive: true).transform.localPosition = global::UnityEngine.Vector3.down * rectTransform.sizeDelta.y / 2f;
				}
			}
			_nodeSize = _template.rectTransform.sizeDelta / 2f;
			global::UnityEngine.Object.Destroy(_template.gameObject);
			global::UnityEngine.Bounds rectBounds = default(global::UnityEngine.Bounds);
			bool flag = true;
			foreach (global::System.Collections.Generic.KeyValuePair<global::UnityEngine.UI.Image, global::MapGeneration.RoomIdentifier> item in RoomsByNode)
			{
				global::UnityEngine.RectTransform rectTransform2 = item.Key.rectTransform;
				global::UnityEngine.Bounds bounds = new global::UnityEngine.Bounds(rectTransform2.anchoredPosition, rectTransform2.sizeDelta);
				if (flag)
				{
					rectBounds = bounds;
				}
				else
				{
					rectBounds.Encapsulate(bounds);
				}
				flag = false;
			}
			RectBounds = rectBounds;
			global::UnityEngine.RectTransform rectTransform3 = ZoneLabel.rectTransform;
			rectTransform3.anchoredPosition = (global::UnityEngine.Vector2)rectBounds.center + global::UnityEngine.Vector2.up * (rectBounds.extents.y + rectTransform3.sizeDelta.y);
			ZoneLabel.text = Translations.Get(ZoneTranslations[_targetZone]);
		}

		public void Generate()
		{
			PlaceRooms();
			_queuedPostProcessing.Enqueue(this);
		}

		public virtual void UpdateOpened(global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera curCam)
		{
			_highlightedCamera = null;
			global::UnityEngine.Vector2 anchoredPosition = _rectParent.anchoredPosition;
			bool flag = curCam.Room.Zone == _targetZone;
			foreach (global::System.Collections.Generic.KeyValuePair<global::UnityEngine.UI.Image, global::MapGeneration.RoomIdentifier> item in RoomsByNode)
			{
				global::UnityEngine.Vector2 vector = item.Key.rectTransform.anchoredPosition + anchoredPosition;
				if (global::UnityEngine.Mathf.Abs(vector.x) < _nodeSize.x && global::UnityEngine.Mathf.Abs(vector.y) < _nodeSize.y)
				{
					if (TryGetCamOfRoom(curCam, item.Value, out var result))
					{
						item.Key.color = HighlightedColor;
						_highlightedCamera = result as global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera;
					}
				}
				else
				{
					item.Key.color = (flag ? CurrentZoneColor : OtherZoneColor);
				}
			}
			if (NodesByRoom.TryGetValue(curCam.Room, out var value))
			{
				value.color = CurrentRoomColor;
			}
		}

		public bool TryGetCamera(out global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera target)
		{
			target = _highlightedCamera as global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera;
			return target != null;
		}

		public bool TryGetCenterTransform(global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera curCam, out global::UnityEngine.Vector3 center)
		{
			if (NodesByRoom.TryGetValue(curCam.Room, out var value))
			{
				center = -value.rectTransform.anchoredPosition;
				return true;
			}
			center = global::UnityEngine.Vector3.zero;
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
			if (roomIdentifier == null || !NodesByRoom.TryGetValue(roomIdentifier, out var value))
			{
				return false;
			}
			if (exact)
			{
				global::UnityEngine.RectTransform rectTransform = value.rectTransform;
				global::UnityEngine.Transform transform = roomIdentifier.transform;
				global::UnityEngine.Vector3 vector = transform.InverseTransformPoint(position);
				global::UnityEngine.Vector2 vector2 = new global::UnityEngine.Vector3(vector.x, vector.z) * _positionScale;
				global::PlayerRoles.PlayableScps.Scp079.Map.ProceduralZoneMap.RoomIcon result;
				global::UnityEngine.Bounds bounds = (TryGetIcon(roomIdentifier, out result) ? result.IndicatorLimits : new global::UnityEngine.Bounds(global::UnityEngine.Vector3.zero, rectTransform.sizeDelta));
				indicator.SetParent(rectTransform);
				vector2.x = global::UnityEngine.Mathf.Clamp(vector2.x, bounds.min.x, bounds.max.x);
				vector2.y = global::UnityEngine.Mathf.Clamp(vector2.y, bounds.min.y, bounds.max.y);
				indicator.localPosition = vector2;
				indicator.localScale = global::UnityEngine.Vector3.one;
				indicator.rotation = _rectParent.rotation;
				indicator.Rotate(global::UnityEngine.Vector3.back * (fpcStandardScp.FpcModule.MouseLook.CurrentHorizontal - transform.eulerAngles.y - rectTransform.localEulerAngles.z), global::UnityEngine.Space.Self);
			}
			else
			{
				if (!_nonExactCooldown.IsReady)
				{
					return true;
				}
				global::UnityEngine.UI.LayoutGroup componentInChildren = value.GetComponentInChildren<global::UnityEngine.UI.LayoutGroup>();
				indicator.SetParent(componentInChildren.transform);
				indicator.localScale = global::UnityEngine.Vector3.one;
				indicator.localPosition = global::UnityEngine.Vector3.one;
				indicator.localEulerAngles = global::UnityEngine.Vector3.zero;
				_nonExactCooldown.Trigger(1.5f);
			}
			return true;
		}
	}
}
