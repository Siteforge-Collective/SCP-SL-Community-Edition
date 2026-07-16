using CursorManagement;
using Interactables.Interobjects;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp106
{
	public class Scp106Minimap : MonoBehaviour, ICursorOverride
	{
		[SerializeField]
		private Transform _rotorTransform;

		[SerializeField]
		private Transform _offsetTransform;

		[SerializeField]
		private Transform _rangeTransform;

		[SerializeField]
		private Scp106MinimapElement _template;

		[SerializeField]
		private float _gridScale;

		[SerializeField]
		private float _radiusScale;

		[SerializeField]
		private GameObject _errorSurface;

		[SerializeField]
		private GameObject _errorElevator;

		[SerializeField]
		private RectTransform _cursor;

		private bool _setUp;

		private float _lastElevatorRide;

		private int _poolSize;

		private int _usedRooms;

		private float _scaleCache;

		private Transform _tr;

		private static Scp106MinimapElement[] _pool;

		private const float SurfaceHeightThreshold = 800f;

		private const float HeightRange = 350f;

		private const float ElevatorCooldown = 0.2f;

		private const float FadeSpeed = 19f;

		private float CurTime => 0f;

		private bool InElevator => false;

		private float Scale
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public CursorOverrideMode CursorOverride => default(CursorOverrideMode);

		public bool LockMovement => false;

		public bool IsVisible { get; set; }

		public Vector3 LastWorldPos { get; set; }

		public static Scp106Minimap Singleton { get; private set; }

		private void Setup()
		{
		}

		private void OnElevatorMoved(Bounds elevatorBounds, ElevatorChamber chamber, Vector3 deltaPos, Quaternion deltaRot)
		{
		}

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void Update()
		{
		}

		private void RefreshRange()
		{
		}

		private void SetupRooms(Transform camTr, Vector3 camPos)
		{
		}
	}
}
