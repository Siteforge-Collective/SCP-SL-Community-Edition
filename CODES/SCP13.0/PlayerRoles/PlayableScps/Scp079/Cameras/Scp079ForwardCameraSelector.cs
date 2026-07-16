using System.Diagnostics;
using GameObjectPools;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.Cameras
{
	public class Scp079ForwardCameraSelector : Scp079DirectionalCameraSelector, IPoolResettable
	{
		[SerializeField]
		private float _maxDistance;

		[SerializeField]
		private float _minDot;

		[SerializeField]
		private AnimationCurve _elevatorSwitchDot;

		private bool _switchRequested;

		private Scp079Camera _requestedCamera;

		private float _requestedRotation;

		private readonly Stopwatch _requestTimer;

		private const float MinDisSqr = 0.2f;

		private const float RequestTimeout = 4f;

		public static Scp079Camera HighlightedCamera { get; private set; }

		private void OnCameraChanged()
		{
		}

		protected override bool TryGetCamera(out Scp079Camera targetCamera)
		{
			targetCamera = null;
			return false;
		}

		protected override void Trigger()
		{
		}

		protected override void Start()
		{
		}

		public override void ResetObject()
		{
		}
	}
}
