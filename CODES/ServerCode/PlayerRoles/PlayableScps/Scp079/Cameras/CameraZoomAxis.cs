namespace PlayerRoles.PlayableScps.Scp079.Cameras
{
	[global::System.Serializable]
	public class CameraZoomAxis : global::PlayerRoles.PlayableScps.Scp079.Cameras.CameraAxisBase
	{
		private const string ScrollAxis = "Mouse ScrollWheel";

		private readonly global::System.Diagnostics.Stopwatch _cooldownStopwatch = global::System.Diagnostics.Stopwatch.StartNew();

		private float _lastSoundZoom;

		private Offset _unzoomedOffset;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Transform _zoomBone;

		[global::UnityEngine.SerializeField]
		private Offset _zoomedOffset;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _magnificationCurve;

		[global::UnityEngine.SerializeField]
		private float _stepSize;

		[global::UnityEngine.SerializeField]
		private float _cooldown;

		public float CurrentZoom => _magnificationCurve.Evaluate(base.CurValue);

		internal override void Update(global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera cam)
		{
			if (global::PlayerRoles.PlayableScps.Scp079.Scp079Role.LocalInstanceActive && cam != null && cam.IsUsedByLocalPlayer && cam.IsActive)
			{
				UpdateInputs();
			}
			base.Update(cam);
		}

		internal override void Awake(global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera cam)
		{
			base.Awake(cam);
			_unzoomedOffset = new Offset
			{
				position = _zoomBone.localPosition,
				rotation = _zoomBone.localEulerAngles,
				scale = _zoomBone.localScale
			};
			base.TargetValue = 0f;
		}

		protected override void OnValueChanged(float newValue, global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera cam)
		{
			float t = global::UnityEngine.Mathf.InverseLerp(base.MinValue, base.MaxValue, newValue);
			_zoomBone.localPosition = global::UnityEngine.Vector3.Lerp(_unzoomedOffset.position, _zoomedOffset.position, t);
			_zoomBone.localRotation = global::UnityEngine.Quaternion.Lerp(global::UnityEngine.Quaternion.Euler(_unzoomedOffset.rotation), global::UnityEngine.Quaternion.Euler(_zoomedOffset.rotation), t);
			_zoomBone.localScale = global::UnityEngine.Vector3.Lerp(_unzoomedOffset.scale, _zoomedOffset.scale, t);
			if (!SoundEffectSource.loop && _lastSoundZoom != base.TargetValue)
			{
				SoundEffectSource.Play();
				_lastSoundZoom = base.TargetValue;
			}
		}

		private void UpdateInputs()
		{
			if (!global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079CursorManager.LockCameras)
			{
				float num = (SensitivitySettings.RawInput ? global::UnityEngine.Input.GetAxisRaw("Mouse ScrollWheel") : global::UnityEngine.Input.GetAxis("Mouse ScrollWheel"));
				if (num != 0f && _cooldownStopwatch.Elapsed.TotalSeconds >= (double)_cooldown)
				{
					base.TargetValue += ((num > 0f) ? _stepSize : (0f - _stepSize));
					_cooldownStopwatch.Restart();
				}
			}
		}
	}
}
