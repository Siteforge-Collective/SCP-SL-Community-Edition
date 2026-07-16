using CursorManagement;
using InventorySystem.Items;
using Mirror;
using RelativePositioning;
using UnityEngine;
using UserSettings.ControlsSettings;

namespace PlayerRoles.FirstPersonControl
{
    public class FpcMouseLook
    {
        private static FpcMouseLook _localInstance;

        public const float MinimumVer = -88f;

        public const float MaximumVer = 88f;

        public const float OverallMultiplier = 2f;

        private const float FullAngle = 360f;

        private const float SmoothTime = 10f;

        private const float ThirdpersonSmooth = 22f;

        private readonly ReferenceHub _hub;

        private readonly FirstPersonMovementModule _fpmm;

        private float _curHorizontal;

        private float _curVertical;

        private float _syncHorizontal;

        private float _syncVertical;

        private float _inputHorizontal;

        private float _inputVertical;

        private ushort _prevSyncH;

        private ushort _prevSyncV;

        protected virtual float BiaxialSensitivity
        {
            get
            {
                float sens = UserSettings.ControlsSettings.SensitivitySettings.SensMultiplier;
                if (_hub.inventory.CurInstance is IZoomModifyingItem zoomItem)
                {
                    sens *= zoomItem.SensitivityScale;
                }
                return sens;
            }
        }

        public float CurrentHorizontal
        {
            get => _curHorizontal;
            set
            {
                _curHorizontal = ClampHorizontal(value);
                _inputHorizontal = _curHorizontal;
            }
        }

        public float CurrentVertical
        {
            get => _curVertical;
            set
            {
                _curVertical = ClampVertical(value);
                _inputVertical = _curVertical;
            }
        }

        private Quaternion TargetHubRotation => Quaternion.Euler(0f, _curHorizontal, 0f);

        private Quaternion TargetCamRotation => Quaternion.Euler(-_curVertical, 0f, 0f);

        public FpcMouseLook(ReferenceHub hub, FirstPersonMovementModule fpmm)
        {
            _hub = hub;
            _fpmm = fpmm;
            float initialHorizontal = hub.transform.eulerAngles.y;
            CurrentHorizontal = ClampHorizontal(initialHorizontal);
            CurrentVertical = ClampVertical(0f);
            if (hub.isLocalPlayer)
            {
                _localInstance = this;
            }
        }

        public static bool TryGetLocalMouseLook(out FpcMouseLook ml)
        {
            ml = _localInstance;
            return ml != null;
        }

        public void UpdateRotation()
        {
            Quaternion hubRotation;
            Quaternion camRotation;

            if (_hub.isLocalPlayer)
            {
                float mouseX = Input.GetAxisRaw("Mouse X");
                float mouseY = Input.GetAxisRaw("Mouse Y");
                float biaxialSens = BiaxialSensitivity;

                mouseX = ProcessHorizontalInput(mouseX * biaxialSens);
                mouseY = ProcessVerticalInput(mouseY * biaxialSens);

                if (Cursor.visible || CursorManager.MovementLocked)
                {
                    mouseX = 0f;
                    mouseY = 0f;
                }

                _inputHorizontal += mouseX;
                _inputVertical += mouseY;

                _inputVertical = ClampVertical(_inputVertical);
                _inputHorizontal = ClampHorizontal(_inputHorizontal);

                float lerpT = Time.deltaTime * SmoothTime;

                _curVertical = Mathf.LerpAngle(_curVertical, _inputVertical, lerpT);
                _curHorizontal = Mathf.LerpAngle(_curHorizontal, _inputHorizontal, lerpT);

                hubRotation = TargetHubRotation;
                camRotation = TargetCamRotation;
            }
            else
            {
                _fpmm.Motor.RotationDetected = (_prevSyncH != 0 || _prevSyncV != 0);

                CurrentHorizontal = WaypointBase.GetWorldRotation(_fpmm.Motor.ReceivedPosition.WaypointId, Quaternion.Euler(0f, _syncHorizontal, 0f)).eulerAngles.y;
                CurrentVertical = _syncVertical;

                float lerpT = NetworkServer.active ? 1f : (Time.deltaTime * ThirdpersonSmooth);

                hubRotation = Quaternion.Lerp(_hub.transform.rotation, TargetHubRotation, lerpT);
                camRotation = Quaternion.Lerp(_hub.PlayerCameraReference.localRotation, TargetCamRotation, lerpT);
            }

            _hub.transform.rotation = hubRotation;
            _hub.PlayerCameraReference.localRotation = camRotation;
        }

        public void GetMouseInput(out float hRot, out float vRot)
        {
            hRot = Input.GetAxisRaw("Mouse X") * BiaxialSensitivity;
            vRot = Input.GetAxisRaw("Mouse Y") * BiaxialSensitivity;
        }

        public void ApplySyncValues(ushort horizontal, ushort vertical)
        {
            if (_prevSyncH == horizontal && _prevSyncV == vertical)
            {
                _fpmm.Motor.RotationDetected = false;
                return;
            }

            _prevSyncH = horizontal;
            _prevSyncV = vertical;

            _syncHorizontal = Mathf.Lerp(0f, 360f, (float)horizontal / 65535f);
            _syncVertical = Mathf.Lerp(-88f, 88f, (float)vertical / 65535f);

            if (_hub.isLocalPlayer)
            {
                CurrentHorizontal = _syncHorizontal;
                CurrentVertical = _syncVertical;
            }

            _fpmm.Motor.RotationDetected = true;
        }

        public void GetSyncValues(byte waypointId, out ushort syncH, out ushort syncV)
        {
            Quaternion hubRot = Quaternion.Euler(0f, CurrentHorizontal, 0f);
            Quaternion relativeRot = WaypointBase.GetRelativeRotation(waypointId, hubRot);
            float horizontalAngle = relativeRot.eulerAngles.y;

            syncH = (ushort)Mathf.RoundToInt(Mathf.InverseLerp(0f, 360f, horizontalAngle) * 65535f);
            syncV = (ushort)Mathf.RoundToInt(Mathf.InverseLerp(-88f, 88f, CurrentVertical) * 65535f);
        }

        protected virtual float ClampHorizontal(float f)
        {
            while (f < 0f)
            {
                f += 360f;
            }
            while (f > 360f)
            {
                f -= 360f;
            }
            return f;
        }

        protected virtual float ClampVertical(float f)
        {
            return Mathf.Clamp(f, -88f, 88f);
        }

        protected virtual float ProcessVerticalInput(float f)
        {
            if (SensitivitySettings.Invert)
            {
                f = -f;
            }
            return f;
        }

        protected virtual float ProcessHorizontalInput(float f)
        {
            return f;
        }
    }
}