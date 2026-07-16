using System;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.Cameras
{
	[Serializable]
	public class CameraRotationAxis : CameraAxisBase
	{
		[SerializeField]
		private Transform _pivot;

		[SerializeField]
		private bool _isVertical;

		private const string HorizontalAxis = "Mouse X";

		private const string VerticalAxis = "Mouse Y";

		private const float OverallSensMultiplier = 2f;

		private const float MoveSpeed = 150f;

		private const float BeginMovePercent = 0.93f;

		private const float FullSpeedPercent = 0.98f;

		private const int EdgeThreshold = 2;

		public Transform Pivot => null;

		private float MouseInput => 0f;

		protected override float SpectatorLerpMultiplier => 0f;

		internal override void Update(Scp079Camera cam)
		{
		}

		protected override void OnValueChanged(float newValue, Scp079Camera cam)
		{
		}

		internal override void Awake(Scp079Camera cam)
		{
		}
	}
}
