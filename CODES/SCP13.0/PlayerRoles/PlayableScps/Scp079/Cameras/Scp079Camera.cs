using System;
using System.Runtime.CompilerServices;
using MapGeneration;
using Mirror;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.Cameras
{
	public class Scp079Camera : Scp079InteractableBase, IAdvancedCameraController, ICameraController
	{
		public bool IsMain;

		public string Label;

		public CameraRotationAxis VerticalAxis;

		public CameraRotationAxis HorizontalAxis;

		public CameraZoomAxis ZoomAxis;

		[SerializeField]
		private Transform _cameraAnchor;

		[SerializeField]
		private Renderer[] _targetRenderers;

		[SerializeField]
		private Material _offlineMat;

		[SerializeField]
		private Material _onlineMat;

		private bool _isActive;

		public bool IsActive
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool IsUsedByLocalPlayer => false;

		public Vector3 CameraPosition { get; private set; }

		public float VerticalRotation { get; private set; }

		public float HorizontalRotation { get; private set; }

		public float RollRotation { get; private set; }

		public static event Action<Scp079Camera> OnAnyCameraStateChanged
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		protected override void Awake()
		{
		}

		internal void WriteAxes(NetworkWriter writer)
		{
		}

		internal void ApplyAxes(NetworkReader reader)
		{
		}

		protected virtual void Update()
		{
		}

		public static bool TryGetClosestCamera(Vector3 pos, Func<Scp079Camera, bool> validator, out Scp079Camera closest)
		{
			closest = null;
			return false;
		}

		public static bool TryGetMainCamera(RoomIdentifier room, out Scp079Camera main)
		{
			main = null;
			return false;
		}
	}
}
