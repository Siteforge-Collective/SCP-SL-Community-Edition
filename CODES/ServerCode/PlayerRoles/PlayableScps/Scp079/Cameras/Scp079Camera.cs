namespace PlayerRoles.PlayableScps.Scp079.Cameras
{
	public class Scp079Camera : global::PlayerRoles.PlayableScps.Scp079.Scp079InteractableBase, global::PlayerRoles.IAdvancedCameraController, global::PlayerRoles.ICameraController
	{
		public bool IsMain;

		public string Label;

		public global::PlayerRoles.PlayableScps.Scp079.Cameras.CameraRotationAxis VerticalAxis;

		public global::PlayerRoles.PlayableScps.Scp079.Cameras.CameraRotationAxis HorizontalAxis;

		public global::PlayerRoles.PlayableScps.Scp079.Cameras.CameraZoomAxis ZoomAxis;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Transform _cameraAnchor;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Renderer[] _targetRenderers;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Material _offlineMat;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Material _onlineMat;

		private bool _isActive;

		public bool IsActive
		{
			get
			{
				return _isActive;
			}
			set
			{
				if (value != _isActive)
				{
					_isActive = value;
					global::UnityEngine.Renderer[] targetRenderers = _targetRenderers;
					for (int i = 0; i < targetRenderers.Length; i++)
					{
						targetRenderers[i].sharedMaterial = (_isActive ? _onlineMat : _offlineMat);
					}
					global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera.OnAnyCameraStateChanged?.Invoke(this);
				}
			}
		}

		public bool IsUsedByLocalPlayer
		{
			get
			{
				if (global::PlayerRoles.PlayableScps.Scp079.Scp079Role.LocalInstanceActive)
				{
					return global::PlayerRoles.PlayableScps.Scp079.Scp079Role.LocalInstance.CurrentCamera == this;
				}
				return false;
			}
		}

		public global::UnityEngine.Vector3 CameraPosition { get; private set; }

		public float VerticalRotation { get; private set; }

		public float HorizontalRotation { get; private set; }

		public float RollRotation { get; private set; }

		public static event global::System.Action<global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera> OnAnyCameraStateChanged;

		protected override void Awake()
		{
			base.Awake();
			VerticalAxis.Awake(this);
			HorizontalAxis.Awake(this);
			ZoomAxis.Awake(this);
		}

		internal void WriteAxes(global::Mirror.NetworkWriter writer)
		{
			global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, VerticalAxis.Value16BitCompression);
			global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, HorizontalAxis.Value16BitCompression);
			writer.WriteByte(ZoomAxis.Value8BitCompression);
		}

		internal void ApplyAxes(global::Mirror.NetworkReader reader)
		{
			if (!IsUsedByLocalPlayer)
			{
				VerticalAxis.Value16BitCompression = global::Mirror.NetworkReaderExtensions.ReadUInt16(reader);
				HorizontalAxis.Value16BitCompression = global::Mirror.NetworkReaderExtensions.ReadUInt16(reader);
				ZoomAxis.Value8BitCompression = reader.ReadByte();
			}
		}

		protected virtual void Update()
		{
			VerticalAxis.Update(this);
			HorizontalAxis.Update(this);
			ZoomAxis.Update(this);
			if (IsActive)
			{
				if (global::Utils.NonAllocLINQ.HashsetExtensions.All(global::PlayerRoles.PlayableScps.Scp079.Scp079Role.ActiveInstances, (global::PlayerRoles.PlayableScps.Scp079.Scp079Role x) => x.CurrentCamera != this))
				{
					IsActive = false;
					return;
				}
				global::UnityEngine.Vector3 eulerAngles = _cameraAnchor.rotation.eulerAngles;
				VerticalRotation = eulerAngles.x;
				HorizontalRotation = eulerAngles.y;
				RollRotation = eulerAngles.z;
				CameraPosition = _cameraAnchor.position;
			}
		}
	}
}
