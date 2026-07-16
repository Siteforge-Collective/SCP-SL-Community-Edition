using System;
using System.Diagnostics;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.Cameras
{
	[Serializable]
	public class CameraZoomAxis : CameraAxisBase
	{
		private const string ScrollAxis = "Mouse ScrollWheel";

		private readonly Stopwatch _cooldownStopwatch;

		private float _lastSoundZoom;

		private Offset _unzoomedOffset;

		[SerializeField]
		private Transform _zoomBone;

		[SerializeField]
		private Offset _zoomedOffset;

		[SerializeField]
		private AnimationCurve _magnificationCurve;

		[SerializeField]
		private float _stepSize;

		[SerializeField]
		private float _cooldown;

		public float CurrentZoom => 0f;

		internal override void Update(Scp079Camera cam)
		{
		}

		internal override void Awake(Scp079Camera cam)
		{
		}

		protected override void OnValueChanged(float newValue, Scp079Camera cam)
		{
		}

		private void UpdateInputs()
		{
		}
	}
}
