using System;
using PlayerRoles.FirstPersonControl;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp106
{
	public class Scp106MovementModule : FirstPersonMovementModule, IFpcCollisionModifier
	{
		[SerializeField]
		private float _stalkSpeed;

		private const float SubmergingLerp = 1.5f;

		private const int GlassLayer = 14;

		private const int DoorLayer = 27;

		private const float SlowdownTransitionSpeed = 5.5f;

		private float _slowndownTarget;

		private float _slowndownSpeed;

		private float _normalSpeed;

		private Scp106SinkholeController _sinkhole;

		public static readonly int PassableDetectionMask;

		private float MovementSpeed
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public float CurSlowdown { get; private set; }

		public LayerMask DetectionMask => default(LayerMask);

		private void Awake()
		{
		}

		private void Update()
		{
		}

		public override void SpawnObject()
		{
		}

		public void ProcessColliders(ArraySegment<Collider> detections)
		{
		}

		public static float GetSlowdownFromCollider(Collider col)
		{
			return 0f;
		}
	}
}
