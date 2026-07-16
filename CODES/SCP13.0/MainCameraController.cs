using System;
using MapGeneration;
using PlayerRoles;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    private static Transform _currentCamera;

    private static MainCameraController _singleton;

    private static Vector3 _defaultPos;

    [SerializeField]
    private Transform _defaultCamera;

    public static Transform CurrentCamera
    {
        get
        {
            return _currentCamera;
        }
        set
        {
            if (!(_currentCamera == value))
            {
                Transform transform = value;
                if (transform == null)
                {
                    transform = _singleton._defaultCamera;
                }
                if (_currentCamera != null)
                {
                    _currentCamera.gameObject.SetActive(value: false);
                }
                _currentCamera = transform;
                if (transform != null)
                {
                    transform.gameObject.SetActive(value: true);
                }
            }
        }
    }

    public static bool InstanceActive { get; private set; }

    public static event Action OnUpdated;

    public static bool TryGetCurrentRoom(out RoomIdentifier rid)
    {
        if (CurrentCamera == null)
        {
            rid = null;
            return false;
        }
        Vector3Int key = RoomIdUtils.PositionToCoords(CurrentCamera.position);
        return RoomIdentifier.RoomsByCoordinates.TryGetValue(key, out rid);
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

    private static void GetPositionAndRotation(out Vector3 pos, out Quaternion rot)
    {
        if (ReferenceHub.TryGetLocalHub(out var hub) && hub.roleManager.CurrentRole is ICameraController cameraController)
        {
            pos = cameraController.CameraPosition;
            float z = ((cameraController is IAdvancedCameraController advancedCameraController) ? advancedCameraController.RollRotation : 0f);
            rot = Quaternion.Euler(cameraController.VerticalRotation, cameraController.HorizontalRotation, z);
        }
        else
        {
            pos = _defaultPos;
            rot = Quaternion.identity;
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
