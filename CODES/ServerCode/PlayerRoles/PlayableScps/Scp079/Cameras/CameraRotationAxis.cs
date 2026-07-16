namespace PlayerRoles.PlayableScps.Scp079.Cameras
{
	[global::System.Serializable]
	public class CameraRotationAxis : global::PlayerRoles.PlayableScps.Scp079.Cameras.CameraAxisBase
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Transform _pivot;

		[global::UnityEngine.SerializeField]
		private bool _isVertical;

		private const string HorizontalAxis = "Mouse X";

		private const string VerticalAxis = "Mouse Y";

		private const float OverallSensMultiplier = 2f;

		private const float MoveSpeed = 150f;

		private const float BeginMovePercent = 0.93f;

		private const float FullSpeedPercent = 0.98f;

		private const int EdgeThreshold = 2;

		public global::UnityEngine.Transform Pivot => _pivot;

		private float MouseInput => (SensitivitySettings.RawInput ? global::UnityEngine.Input.GetAxisRaw(_isVertical ? "Mouse Y" : "Mouse X") : global::UnityEngine.Input.GetAxis(_isVertical ? "Mouse Y" : "Mouse X")) * 2f * SensitivitySettings.SensMultiplier;

		protected override float SpectatorLerpMultiplier => 7.5f;

		internal override void Update(global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera cam)
		{
			base.Update(cam);
			if (!cam.IsActive || !cam.IsUsedByLocalPlayer || global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079CursorManager.LockCameras)
			{
				return;
			}
			if (!global::UnityEngine.Cursor.visible)
			{
				float num = MouseInput;
				if (_isVertical && !SensitivitySettings.Invert)
				{
					num *= -1f;
				}
				num /= global::UnityEngine.Mathf.LerpUnclamped(1f, cam.ZoomAxis.CurrentZoom, SensitivitySettings.AdsReductionMultiplier);
				base.TargetValue += num;
				return;
			}
			float num2;
			float num3;
			if (_isVertical)
			{
				num2 = global::UnityEngine.Screen.height;
				num3 = num2 - global::UnityEngine.Input.mousePosition.y;
			}
			else
			{
				num3 = global::UnityEngine.Input.mousePosition.x;
				num2 = global::UnityEngine.Screen.width;
			}
			float num4 = num2 / 2f;
			float num5 = ((num3 < num4) ? (-1f) : 1f);
			float value = global::UnityEngine.Mathf.Abs((num3 - num4) / num4);
			float num6 = num4 - global::UnityEngine.Mathf.Abs(num3 - num4);
			float num7 = 150f * global::UnityEngine.Time.deltaTime;
			num7 *= global::UnityEngine.Mathf.InverseLerp(0.93f, 0.98f, value);
			if (num6 <= 2f)
			{
				float num8 = MouseInput * num5;
				if (_isVertical)
				{
					num8 *= -1f;
				}
				num7 += global::UnityEngine.Mathf.Max(num8, 0f);
			}
			base.TargetValue += num7 * num5;
		}

		protected override void OnValueChanged(float newValue, global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera cam)
		{
			float num = (global::PlayerRoles.PlayableScps.Scp079.Scp079Role.LocalInstanceActive ? base.TargetValue : newValue);
			_pivot.localRotation = (_isVertical ? global::UnityEngine.Quaternion.Euler(num, 0f, 0f) : global::UnityEngine.Quaternion.Euler(0f, num, 0f));
		}

		internal override void Awake(global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera cam)
		{
			base.Awake(cam);
			global::UnityEngine.Vector3 eulerAngles = _pivot.localRotation.eulerAngles;
			float num;
			for (num = (_isVertical ? eulerAngles.x : eulerAngles.y); num < base.MinValue; num += 360f)
			{
			}
			while (num > base.MaxValue)
			{
				num -= 360f;
			}
			base.TargetValue = num;
		}
	}
}
