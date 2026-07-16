namespace PlayerRoles.PlayableScps.Scp106
{
	public class Scp106Minimap : global::UnityEngine.MonoBehaviour, global::CursorManagement.ICursorOverride
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Transform _rotorTransform;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Transform _offsetTransform;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Transform _rangeTransform;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp106.Scp106MinimapElement _template;

		[global::UnityEngine.SerializeField]
		private float _gridScale;

		[global::UnityEngine.SerializeField]
		private float _radiusScale;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _errorSurface;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _errorElevator;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.RectTransform _cursor;

		private bool _setUp;

		private float _lastElevatorRide;

		private int _poolSize;

		private int _usedRooms;

		private float _scaleCache;

		private global::UnityEngine.Transform _tr;

		private static global::PlayerRoles.PlayableScps.Scp106.Scp106MinimapElement[] _pool;

		private const float SurfaceHeightThreshold = 800f;

		private const float HeightRange = 350f;

		private const float ElevatorCooldown = 0.2f;

		private const float FadeSpeed = 19f;

		private float CurTime => global::UnityEngine.Time.timeSinceLevelLoad;

		private bool InElevator => CurTime - _lastElevatorRide < 0.2f;

		private float Scale
		{
			get
			{
				return _scaleCache;
			}
			set
			{
				_scaleCache = global::UnityEngine.Mathf.Clamp01(value);
				_tr.localScale = global::UnityEngine.Vector3.one * _scaleCache;
			}
		}

		public global::CursorManagement.CursorOverrideMode CursorOverride
		{
			get
			{
				if (!IsVisible)
				{
					return global::CursorManagement.CursorOverrideMode.NoOverride;
				}
				return global::CursorManagement.CursorOverrideMode.Free;
			}
		}

		public bool LockMovement => false;

		public bool IsVisible { get; set; }

		public global::UnityEngine.Vector3 LastWorldPos { get; set; }

		public static global::PlayerRoles.PlayableScps.Scp106.Scp106Minimap Singleton { get; private set; }

		private void Setup()
		{
			_poolSize = global::MapGeneration.RoomIdentifier.AllRoomIdentifiers.Count;
			if (_pool == null || _poolSize > _pool.Length)
			{
				_pool = new global::PlayerRoles.PlayableScps.Scp106.Scp106MinimapElement[_poolSize];
			}
			for (int i = 0; i < _poolSize; i++)
			{
				global::PlayerRoles.PlayableScps.Scp106.Scp106MinimapElement scp106MinimapElement = global::UnityEngine.Object.Instantiate(_template, _offsetTransform);
				scp106MinimapElement.Rt.localScale = global::UnityEngine.Vector3.one;
				_pool[i] = scp106MinimapElement;
			}
			_setUp = true;
		}

		private void OnElevatorMoved(global::UnityEngine.Bounds elevatorBounds, global::Interactables.Interobjects.ElevatorChamber chamber, global::UnityEngine.Vector3 deltaPos, global::UnityEngine.Quaternion deltaRot)
		{
			if (elevatorBounds.Contains(MainCameraController.CurrentCamera.position))
			{
				_lastElevatorRide = CurTime;
			}
		}

		private void Awake()
		{
			global::CursorManagement.CursorManager.Register(this);
			global::Interactables.Interobjects.ElevatorChamber.OnElevatorMoved += OnElevatorMoved;
			Singleton = this;
			_tr = base.transform;
		}

		private void OnDestroy()
		{
			global::CursorManagement.CursorManager.Unregister(this);
			global::Interactables.Interobjects.ElevatorChamber.OnElevatorMoved -= OnElevatorMoved;
		}

		private void Update()
		{
			if (!global::MapGeneration.SeedSynchronizer.MapGenerated)
			{
				return;
			}
			if (!_setUp)
			{
				Setup();
			}
			if (global::PlayerRoles.PlayableScps.Scp106.Scp106MinimapElement.AnyHighlighted)
			{
				global::UnityEngine.RectTransform rt = global::PlayerRoles.PlayableScps.Scp106.Scp106MinimapElement.LastHighlighted.Rt;
				_cursor.SetParent(rt);
				_cursor.position = global::UnityEngine.Input.mousePosition;
				_cursor.gameObject.SetActive(value: true);
			}
			else
			{
				_cursor.gameObject.SetActive(value: false);
			}
			Scale = global::UnityEngine.Mathf.MoveTowards(Scale, IsVisible ? 1 : 0, 19f * global::UnityEngine.Time.deltaTime);
			if (!(Scale <= 0f))
			{
				global::UnityEngine.Transform currentCamera = MainCameraController.CurrentCamera;
				global::UnityEngine.Vector3 position = currentCamera.position;
				int usedRooms = _usedRooms;
				_usedRooms = 0;
				if (InElevator)
				{
					_errorSurface.SetActive(value: false);
					_errorElevator.SetActive(value: true);
				}
				else if (position.y > 800f)
				{
					_errorSurface.SetActive(value: true);
					_errorElevator.SetActive(value: false);
				}
				else
				{
					_errorSurface.SetActive(value: false);
					_errorElevator.SetActive(value: false);
					SetupRooms(currentCamera, position);
					RefreshRange();
				}
				for (int i = _usedRooms; i < usedRooms; i++)
				{
					_pool[i].Img.enabled = false;
				}
				global::UnityEngine.Vector3 vector = _template.Rt.InverseTransformPoint(global::UnityEngine.Input.mousePosition) / _gridScale;
				LastWorldPos = new global::UnityEngine.Vector3(0f - vector.x, position.y, 0f - vector.y);
			}
		}

		private void RefreshRange()
		{
			if (ReferenceHub.TryGetLocalHub(out var hub) && hub.roleManager.CurrentRole is global::PlayerRoles.PlayableScps.Scp106.Scp106Role scp106Role && scp106Role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp106.Scp106Vigor>(out var subroutine))
			{
				float num = subroutine.DisplayedVigor / 0.019f;
				_rangeTransform.localScale = global::UnityEngine.Vector3.one / (_radiusScale * num);
			}
		}

		private void SetupRooms(global::UnityEngine.Transform camTr, global::UnityEngine.Vector3 camPos)
		{
			foreach (global::MapGeneration.RoomIdentifier allRoomIdentifier in global::MapGeneration.RoomIdentifier.AllRoomIdentifiers)
			{
				global::UnityEngine.Transform transform = allRoomIdentifier.transform;
				global::UnityEngine.Vector3 position = transform.position;
				if (allRoomIdentifier.Zone != global::MapGeneration.FacilityZone.None && !(global::UnityEngine.Mathf.Abs(position.y - camPos.y) > 350f))
				{
					global::PlayerRoles.PlayableScps.Scp106.Scp106MinimapElement obj = _pool[_usedRooms];
					obj.Room = allRoomIdentifier;
					obj.Img.sprite = allRoomIdentifier.Icon;
					obj.Img.enabled = true;
					obj.Rt.localEulerAngles = global::UnityEngine.Vector3.back * transform.eulerAngles.y;
					obj.Rt.localPosition = new global::UnityEngine.Vector3(position.x, position.z, 0f) * _gridScale;
					_usedRooms++;
				}
			}
			global::UnityEngine.Vector3 vector = camPos * _gridScale;
			_offsetTransform.localPosition = -new global::UnityEngine.Vector3(vector.x, vector.z, 0f);
			_rotorTransform.localEulerAngles = global::UnityEngine.Vector3.forward * camTr.eulerAngles.y;
		}
	}
}
