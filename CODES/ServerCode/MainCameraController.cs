public class MainCameraController : global::UnityEngine.MonoBehaviour
{
	private static global::UnityEngine.Transform _currentCamera;

	private static MainCameraController _singleton;

	private static global::UnityEngine.Vector3 _defaultPos;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.Transform _defaultCamera;

	public static global::UnityEngine.Transform CurrentCamera
	{
		get
		{
			return _currentCamera;
		}
		set
		{
			global::UnityEngine.Transform transform = value;
			if (transform == null)
			{
				transform = _singleton._defaultCamera;
			}
			if (_currentCamera != null)
			{
				_currentCamera.gameObject.SetActive(value: false);
			}
			_currentCamera = transform;
			transform.gameObject.SetActive(value: true);
		}
	}

	public static bool InstanceActive { get; private set; }

	public static event global::System.Action OnUpdated;

	public static bool TryGetCurrentRoom(out global::MapGeneration.RoomIdentifier rid)
	{
		if (CurrentCamera == null)
		{
			rid = null;
			return false;
		}
		global::UnityEngine.Vector3Int key = global::MapGeneration.RoomIdUtils.PositionToCoords(CurrentCamera.position);
		return global::MapGeneration.RoomIdentifier.RoomsByCoordinates.TryGetValue(key, out rid);
	}

	private void Awake()
	{
		InstanceActive = true;
		_singleton = this;
		_defaultPos = _defaultCamera.position;
		CurrentCamera = _defaultCamera;
	}

	private void OnDestroy()
	{
		InstanceActive = false;
	}

	private void LateUpdate()
	{
		ForceUpdatePosition();
	}

	private static void GetPositionAndRotation(out global::UnityEngine.Vector3 pos, out global::UnityEngine.Quaternion rot)
	{
		if (ReferenceHub.TryGetLocalHub(out var hub) && hub.roleManager.CurrentRole is global::PlayerRoles.ICameraController cameraController)
		{
			pos = cameraController.CameraPosition;
			float z = ((cameraController is global::PlayerRoles.IAdvancedCameraController advancedCameraController) ? advancedCameraController.RollRotation : 0f);
			rot = global::UnityEngine.Quaternion.Euler(cameraController.VerticalRotation, cameraController.HorizontalRotation, z);
		}
		else
		{
			pos = _defaultPos;
			rot = global::UnityEngine.Quaternion.identity;
		}
	}

	public static void ForceUpdatePosition()
	{
		if (InstanceActive)
		{
			GetPositionAndRotation(out var pos, out var rot);
			CurrentCamera.SetPositionAndRotation(pos, rot);
			MainCameraController.OnUpdated?.Invoke();
		}
	}
}
