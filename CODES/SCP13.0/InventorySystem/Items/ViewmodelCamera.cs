using UnityEngine;
using PlayerRoles.Spectating;
using PlayerRoles;

namespace InventorySystem.Items
{
    public class ViewmodelCamera : MonoBehaviour
    {
        public bool _resetting;

        public static Camera _viewModelCamera;

        public static bool _camSet;

        public void ResetCam()
        {
            _resetting = true;
        }

        public void Awake()
        {
            _camSet = true;
            _viewModelCamera = GetComponent<Camera>();
            SpectatorTargetTracker.OnTargetChanged += ResetCam;
        }

        public void OnDestroy()
        {
            _camSet = false;
            SpectatorTargetTracker.OnTargetChanged -= ResetCam;
        }

        public void LateUpdate()
        {
            if (TryGetViewmodelFov(out var fov) && !_resetting)
            {
                _viewModelCamera.fieldOfView = fov;
            }
            else
            {
                _resetting = false;
            }
        }

        public bool TryGetViewmodelFov(out float fov)
        {
            fov = _viewModelCamera.fieldOfView;
            if (!ReferenceHub.TryGetLocalHub(out var hub))
            {
                return false;
            }

            if (!(hub.roleManager.CurrentRole is IViewmodelRole viewmodelRole))
            {
                return false;
            }

            if (!viewmodelRole.TryGetViewmodelFov(out var fov2))
            {
                return false;
            }

            fov = fov2;
            return true;
        }

        public static bool TryGetViewportPoint(Vector3 worldPos, out Vector3 viewport)
        {
            if (!_camSet)
            {
                viewport = Vector3.zero;
                return false;
            }

            viewport = _viewModelCamera.WorldToViewportPoint(worldPos);
            return true;
        }
    }
}