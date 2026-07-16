namespace PlayerRoles.PlayableScps.Scp079.Cameras
{
	public class Scp079ForwardCameraSelector : global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079DirectionalCameraSelector, global::GameObjectPools.IPoolResettable
	{
		[global::UnityEngine.SerializeField]
		private float _maxDistance;

		[global::UnityEngine.SerializeField]
		private float _minDot;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _elevatorSwitchDot;

		private bool _switchRequested;

		private global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera _requestedCamera;

		private float _requestedRotation;

		private readonly global::System.Diagnostics.Stopwatch _requestTimer = new global::System.Diagnostics.Stopwatch();

		private const float MinDisSqr = 0.2f;

		private const float RequestTimeout = 4f;

		public static global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera HighlightedCamera { get; private set; }

		private void OnCameraChanged()
		{
			if (!_switchRequested)
			{
				return;
			}
			_switchRequested = false;
			if (!(base.CurrentCamSync.CurrentCamera != _requestedCamera) && !(_requestTimer.Elapsed.TotalSeconds > 4.0))
			{
				global::PlayerRoles.PlayableScps.Scp079.Cameras.CameraRotationAxis horizontalAxis = _requestedCamera.HorizontalAxis;
				float num = (horizontalAxis.TargetValue + _requestedRotation - horizontalAxis.Pivot.eulerAngles.y) % 360f;
				float minValue = horizontalAxis.MinValue;
				float maxValue = horizontalAxis.MaxValue;
				if (num > maxValue || num < minValue)
				{
					float num2 = global::UnityEngine.Mathf.Abs(global::UnityEngine.Mathf.DeltaAngle(num, minValue));
					float num3 = global::UnityEngine.Mathf.Abs(global::UnityEngine.Mathf.DeltaAngle(num, maxValue));
					num = ((num2 < num3) ? minValue : maxValue);
				}
				horizontalAxis.TargetValue = num;
			}
		}

		protected override bool TryGetCamera(out global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera targetCamera)
		{
			bool flag = false;
			float num = _minDot;
			targetCamera = null;
			global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera currentCamera = base.CurrentCamSync.CurrentCamera;
			global::UnityEngine.Vector3 position = MainCameraController.CurrentCamera.position;
			global::UnityEngine.Vector3 forward = MainCameraController.CurrentCamera.forward;
			float num2 = _maxDistance * _maxDistance;
			foreach (global::PlayerRoles.PlayableScps.Scp079.Overcons.CameraOvercon visibleOvercon in global::PlayerRoles.PlayableScps.Scp079.Overcons.CameraOverconRenderer.VisibleOvercons)
			{
				if (visibleOvercon == currentCamera)
				{
					continue;
				}
				global::UnityEngine.Vector3 vector = visibleOvercon.Position - position;
				float sqrMagnitude = vector.sqrMagnitude;
				if (!(sqrMagnitude < 0.2f) && !(sqrMagnitude > num2))
				{
					float num3 = global::UnityEngine.Vector3.Dot(forward, vector / global::UnityEngine.Mathf.Sqrt(sqrMagnitude));
					if (!(num3 < num) && (!visibleOvercon.IsElevator || !(num3 < _elevatorSwitchDot.Evaluate(sqrMagnitude))))
					{
						flag = true;
						num = num3;
						targetCamera = visibleOvercon.Target;
					}
				}
			}
			if (flag || base.TryGetCamera(out targetCamera))
			{
				HighlightedCamera = targetCamera;
				return true;
			}
			HighlightedCamera = null;
			return false;
		}

		protected override void Trigger()
		{
			if (TryGetCamera(out _requestedCamera) && _requestedCamera.Room != base.CurrentCamSync.CurrentCamera.Room)
			{
				global::UnityEngine.Transform obj = _requestedCamera.Room.transform;
				global::UnityEngine.Transform transform = base.CurrentCamSync.CurrentCamera.Room.transform;
				global::UnityEngine.Vector3 to = obj.position - transform.position;
				_requestedRotation = global::UnityEngine.Vector3.SignedAngle(global::UnityEngine.Vector3.forward, to, global::UnityEngine.Vector3.up);
				_switchRequested = true;
				_requestTimer.Restart();
			}
			base.Trigger();
		}

		protected override void Start()
		{
			base.Start();
			base.CurrentCamSync.OnCameraChanged += OnCameraChanged;
		}

		public override void ResetObject()
		{
			base.ResetObject();
			_switchRequested = false;
		}
	}
}
