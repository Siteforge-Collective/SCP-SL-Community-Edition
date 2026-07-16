namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096TryNotToCryAbility : global::PlayerRoles.PlayableScps.Subroutines.ScpKeySubroutine<global::PlayerRoles.PlayableScps.Scp096.Scp096Role>, global::CursorManagement.ICursorOverride
	{
		[global::UnityEngine.SerializeField]
		private float _clientDotTolerance;

		[global::UnityEngine.SerializeField]
		private float _serverDotTolerance;

		[global::UnityEngine.SerializeField]
		private float _clientDisTolerance;

		[global::UnityEngine.SerializeField]
		private float _serverDisTolerance;

		[global::UnityEngine.SerializeField]
		private float _maxVerticalAngle;

		[global::UnityEngine.SerializeField]
		private float _maxDistance;

		[global::UnityEngine.SerializeField]
		private float _minWidth;

		[global::UnityEngine.SerializeField]
		private float _sideOffset;

		[global::UnityEngine.SerializeField]
		private float _groundLevelMaxDiff;

		private global::RelativePositioning.RelativePosition _syncPoint;

		private global::UnityEngine.Quaternion _syncRot;

		private bool _cancelled;

		private readonly global::System.Diagnostics.Stopwatch _freezeSw = new global::System.Diagnostics.Stopwatch();

		private const float AbsFreezeDuration = 0.1f;

		private const float RadiusTolerance = 0.9f;

		private static readonly global::UnityEngine.Quaternion[] RotationAngles = new global::UnityEngine.Quaternion[2]
		{
			global::UnityEngine.Quaternion.Euler(global::UnityEngine.Vector3.up * 90f),
			global::UnityEngine.Quaternion.Euler(global::UnityEngine.Vector3.down * 90f)
		};

		private static readonly ActionName[] CancelKeys = new ActionName[5]
		{
			ActionName.MoveBackward,
			ActionName.MoveForward,
			ActionName.MoveLeft,
			ActionName.MoveRight,
			ActionName.Jump
		};

		private static readonly CachedLayerMask Mask = new CachedLayerMask("Door", "Glass", "Default");

		private static readonly float[] Heights = new float[3] { 0f, -0.4f, -0.9f };

		private static readonly global::UnityEngine.Vector3[] Offsets = new global::UnityEngine.Vector3[RotationAngles.Length + 1];

		private static readonly global::UnityEngine.Vector3[] GroundPoints = new global::UnityEngine.Vector3[4];

		public global::CursorManagement.CursorOverrideMode CursorOverride => global::CursorManagement.CursorOverrideMode.NoOverride;

		public bool LockMovement
		{
			get
			{
				if (!base.Owner.isLocalPlayer || _cancelled)
				{
					return false;
				}
				if (IsActive)
				{
					return true;
				}
				if (!_freezeSw.IsRunning)
				{
					return false;
				}
				double num = global::Mirror.NetworkTime.rtt + 0.10000000149011612;
				return _freezeSw.Elapsed.TotalSeconds < num;
			}
		}

		protected override ActionName TargetKey => ActionName.Zoom;

		private bool IsActive
		{
			get
			{
				return base.ScpRole.IsAbilityState(global::PlayerRoles.PlayableScps.Scp096.Scp096AbilityState.TryingNotToCry);
			}
			set
			{
				if (!global::Mirror.NetworkServer.active)
				{
					throw new global::System.InvalidOperationException(string.Format("Cannot set {0}.{1} as client.", this, "IsActive"));
				}
				if (IsActive == value)
				{
					return;
				}
				if (value)
				{
					base.Role.TryGetOwner(out var hub);
					if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp096TryNotCry, hub))
					{
						base.ScpRole.StateController.SetAbilityState(global::PlayerRoles.PlayableScps.Scp096.Scp096AbilityState.TryingNotToCry);
					}
				}
				else if (IsActive)
				{
					base.Role.TryGetOwner(out var hub2);
					if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp096StartCrying, hub2))
					{
						base.ScpRole.ResetAbilityState();
					}
				}
			}
		}

		protected override void Update()
		{
			base.Update();
			if (IsActive)
			{
				if (base.Owner.isLocalPlayer)
				{
					UpdateClient();
				}
				if (global::Mirror.NetworkServer.active && !ServerValidate())
				{
					IsActive = false;
				}
			}
		}

		protected override void OnKeyDown()
		{
			base.OnKeyDown();
			if (!IsActive && ValidatePoint())
			{
				_cancelled = false;
				_freezeSw.Restart();
				ClientSendCmd();
			}
		}

		public override void ClientWriteCmd(global::Mirror.NetworkWriter writer)
		{
			base.ClientWriteCmd(writer);
			if (!_cancelled)
			{
				global::RelativePositioning.RelativePosition msg = new global::RelativePositioning.RelativePosition(base.ScpRole.FpcModule.Position);
				if (global::RelativePositioning.WaypointBase.TryGetWaypoint(msg.WaypointId, out var wp))
				{
					global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, msg);
					global::Mirror.NetworkWriterExtensions.WriteQuaternion(writer, wp.GetRelativeRotation(base.Owner.PlayerCameraReference.rotation));
				}
			}
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			base.ServerProcessCmd(reader);
			if (reader.Position >= reader.Length)
			{
				IsActive = false;
				return;
			}
			_syncPoint = global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition(reader);
			_syncRot = global::Mirror.NetworkReaderExtensions.ReadQuaternion(reader);
			IsActive = ServerValidate();
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			global::CursorManagement.CursorManager.Register(this);
		}

		public override void ResetObject()
		{
			base.ResetObject();
			_freezeSw.Reset();
			global::CursorManagement.CursorManager.Unregister(this);
		}

		private void UpdateClient()
		{
			if (_cancelled)
			{
				return;
			}
			ActionName[] cancelKeys = CancelKeys;
			for (int i = 0; i < cancelKeys.Length; i++)
			{
				if (global::UnityEngine.Input.GetKeyDown(NewInput.GetKey(cancelKeys[i])))
				{
					_cancelled = true;
					ClientSendCmd();
					break;
				}
			}
		}

		private bool ServerValidate()
		{
			if (base.ScpRole.StateController.RageState != global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Docile)
			{
				return false;
			}
			if (!global::RelativePositioning.WaypointBase.TryGetWaypoint(_syncPoint.WaypointId, out var wp))
			{
				return false;
			}
			global::UnityEngine.Vector3 worldspacePosition = wp.GetWorldspacePosition(_syncPoint.Relative);
			global::UnityEngine.Quaternion worldspaceRotation = wp.GetWorldspaceRotation(_syncRot);
			using (new global::PlayerRoles.FirstPersonControl.FpcBacktracker(base.Owner, worldspacePosition, worldspaceRotation))
			{
				return ValidatePoint();
			}
		}

		private bool ValidatePoint()
		{
			if (ValidateGround())
			{
				return ValidateWall();
			}
			return false;
		}

		private bool ValidateGround()
		{
			if (!base.ScpRole.FpcModule.IsGrounded)
			{
				return false;
			}
			global::UnityEngine.Transform transform = base.Owner.transform;
			float height = base.ScpRole.FpcModule.CharController.height;
			float num = base.ScpRole.FpcModule.CharController.radius * 0.9f;
			GroundPoints[0] = transform.position + transform.forward * num;
			GroundPoints[1] = transform.position + transform.right * num;
			GroundPoints[2] = transform.position - transform.forward * num;
			GroundPoints[3] = transform.position - transform.right * num;
			float num2 = float.MaxValue;
			float num3 = 0f;
			for (int i = 0; i < GroundPoints.Length; i++)
			{
				if (!global::UnityEngine.Physics.Raycast(GroundPoints[i], global::UnityEngine.Vector3.down, out var hitInfo, height, Mask))
				{
					return false;
				}
				float distance = hitInfo.distance;
				if (distance < num2)
				{
					num2 = distance;
				}
				if (distance > num3)
				{
					num3 = distance;
				}
				if (num3 - num2 > _groundLevelMaxDiff)
				{
					return false;
				}
			}
			return true;
		}

		private bool ValidateWall()
		{
			global::UnityEngine.Vector3 position = base.Owner.PlayerCameraReference.position;
			global::UnityEngine.Vector3 forward = base.Owner.PlayerCameraReference.forward;
			if (base.Owner.isLocalPlayer && global::UnityEngine.Mathf.Abs(base.ScpRole.FpcModule.MouseLook.CurrentVertical) > _maxVerticalAngle)
			{
				return false;
			}
			forward.y = 0f;
			float magnitude = forward.magnitude;
			if (magnitude == 0f)
			{
				return false;
			}
			forward /= magnitude;
			if (!global::UnityEngine.Physics.Raycast(position, forward, out var hitInfo, _maxDistance, Mask))
			{
				return false;
			}
			float num = (base.Owner.isLocalPlayer ? _clientDotTolerance : _serverDotTolerance);
			global::UnityEngine.Vector3 normal = hitInfo.normal;
			if (global::UnityEngine.Vector3.Dot(forward, normal) > num)
			{
				return false;
			}
			global::UnityEngine.Vector3 vector = position + normal * _minWidth;
			global::UnityEngine.Vector3 start = vector + global::UnityEngine.Vector3.down * _sideOffset;
			global::UnityEngine.Vector3 end = vector + global::UnityEngine.Vector3.up * (Heights[Heights.Length - 1] + _sideOffset);
			if (global::UnityEngine.Physics.CheckCapsule(start, end, _sideOffset, Mask))
			{
				return false;
			}
			float num2 = (base.Owner.isLocalPlayer ? _clientDisTolerance : _serverDisTolerance);
			float num3 = float.MaxValue;
			float num4 = 0f;
			for (int i = 0; i < RotationAngles.Length; i++)
			{
				Offsets[i] = RotationAngles[i] * normal;
			}
			for (int j = 0; j < Offsets.Length; j++)
			{
				for (int k = 0; k < Heights.Length; k++)
				{
					if (!global::UnityEngine.Physics.Raycast(Offsets[j] * _sideOffset + normal + position + global::UnityEngine.Vector3.up * Heights[k], -normal, out var hitInfo2, _maxDistance, Mask))
					{
						return false;
					}
					if (global::UnityEngine.Vector3.Dot(forward, normal) > num)
					{
						return false;
					}
					float distance = hitInfo2.distance;
					if (distance < num3)
					{
						num3 = distance;
					}
					if (distance > num4)
					{
						num4 = distance;
					}
					if (num4 - num3 > num2)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
