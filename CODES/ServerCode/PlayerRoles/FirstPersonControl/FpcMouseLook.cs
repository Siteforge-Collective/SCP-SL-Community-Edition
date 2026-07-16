namespace PlayerRoles.FirstPersonControl
{
	public class FpcMouseLook
	{
		private static global::PlayerRoles.FirstPersonControl.FpcMouseLook _localInstance;

		public const float MinimumVer = -88f;

		public const float MaximumVer = 88f;

		public const float OverallMultiplier = 2f;

		private const float FullAngle = 360f;

		private const float SmoothTime = 10f;

		private const float ThirdpersonSmooth = 22f;

		private readonly ReferenceHub _hub;

		private readonly global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule _fpmm;

		private float _curHorizontal;

		private float _curVertical;

		private float _syncHorizontal;

		private float _syncVertical;

		private float _inputHorizontal;

		private float _inputVertical;

		private ushort _prevSyncH;

		private ushort _prevSyncV;

		public float CurrentHorizontal
		{
			get
			{
				return _curHorizontal;
			}
			set
			{
				_inputHorizontal = (_curHorizontal = ClampHorizontal(value));
			}
		}

		public float CurrentVertical
		{
			get
			{
				return _curVertical;
			}
			set
			{
				_inputVertical = (_curVertical = ClampVertical(value));
			}
		}

		private global::UnityEngine.Quaternion TargetHubRotation => global::UnityEngine.Quaternion.Euler(global::UnityEngine.Vector3.up * _curHorizontal);

		private global::UnityEngine.Quaternion TargetCamRotation => global::UnityEngine.Quaternion.Euler(global::UnityEngine.Vector3.left * _curVertical);

		public FpcMouseLook(ReferenceHub hub, global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule fpmm)
		{
			_hub = hub;
			_fpmm = fpmm;
			CurrentHorizontal = hub.transform.eulerAngles.y;
			CurrentVertical = 0f;
			if (hub.isLocalPlayer)
			{
				_localInstance = this;
			}
		}

		public static bool TryGetLocalMouseLook(out global::PlayerRoles.FirstPersonControl.FpcMouseLook ml)
		{
			ml = _localInstance;
			return ml != null;
		}

		public void UpdateRotation()
		{
			global::UnityEngine.Quaternion rotation;
			global::UnityEngine.Quaternion localRotation;
			if (_hub.isLocalPlayer)
			{
				GetMouseInput(out var hRot, out var vRot);
				if (global::UnityEngine.Cursor.visible || global::CursorManagement.CursorManager.MovementLocked)
				{
					hRot = 0f;
					vRot = 0f;
				}
				_inputVertical = ClampVertical(_inputVertical + vRot);
				_inputHorizontal = ClampHorizontal(_inputHorizontal + hRot);
				float t = (SensitivitySettings.SmoothInput ? (10f * global::UnityEngine.Time.deltaTime) : 1f);
				_curVertical = ClampVertical(global::UnityEngine.Mathf.LerpAngle(_curVertical, _inputVertical, t));
				_curHorizontal = ClampHorizontal(global::UnityEngine.Mathf.LerpAngle(_curHorizontal, _inputHorizontal, t));
				rotation = TargetHubRotation;
				localRotation = TargetCamRotation;
			}
			else
			{
				CurrentHorizontal = global::RelativePositioning.WaypointBase.GetWorldRotation(_fpmm.Motor.ReceivedPosition.WaypointId, global::UnityEngine.Quaternion.Euler(global::UnityEngine.Vector3.up * _syncHorizontal)).eulerAngles.y;
				CurrentVertical = _syncVertical;
				float t2 = (global::Mirror.NetworkServer.active ? 1f : (22f * global::UnityEngine.Time.deltaTime));
				rotation = global::UnityEngine.Quaternion.Lerp(_hub.transform.rotation, TargetHubRotation, t2);
				localRotation = global::UnityEngine.Quaternion.Lerp(_hub.PlayerCameraReference.localRotation, TargetCamRotation, t2);
			}
			_hub.transform.rotation = rotation;
			_hub.PlayerCameraReference.localRotation = localRotation;
		}

		public void GetMouseInput(out float hRot, out float vRot)
		{
			if (SensitivitySettings.RawInput)
			{
				hRot = global::UnityEngine.Input.GetAxisRaw("Mouse X");
				vRot = global::UnityEngine.Input.GetAxisRaw("Mouse Y");
			}
			else
			{
				hRot = global::UnityEngine.Input.GetAxis("Mouse X");
				vRot = global::UnityEngine.Input.GetAxis("Mouse Y");
			}
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
			_syncHorizontal = global::UnityEngine.Mathf.Lerp(0f, 360f, (float)(int)horizontal / 65535f);
			_syncVertical = global::UnityEngine.Mathf.Lerp(-88f, 88f, (float)(int)vertical / 65535f);
			if (_hub.isLocalPlayer)
			{
				CurrentHorizontal = _syncHorizontal;
				CurrentVertical = _syncVertical;
			}
			_fpmm.Motor.RotationDetected = true;
		}

		public void GetSyncValues(byte waypointId, out ushort syncH, out ushort syncV)
		{
			syncH = (ushort)global::UnityEngine.Mathf.RoundToInt(global::UnityEngine.Mathf.InverseLerp(0f, 360f, global::RelativePositioning.WaypointBase.GetRelativeRotation(waypointId, global::UnityEngine.Quaternion.Euler(global::UnityEngine.Vector3.up * CurrentHorizontal)).eulerAngles.y) * 65535f);
			syncV = (ushort)global::UnityEngine.Mathf.RoundToInt(global::UnityEngine.Mathf.InverseLerp(-88f, 88f, CurrentVertical) * 65535f);
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
			return global::UnityEngine.Mathf.Clamp(f, -88f, 88f);
		}
	}
}
