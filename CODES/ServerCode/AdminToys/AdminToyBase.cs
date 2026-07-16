namespace AdminToys
{
	public abstract class AdminToyBase : global::Mirror.NetworkBehaviour
	{
		[global::Mirror.SyncVar]
		public global::UnityEngine.Vector3 Position;

		[global::Mirror.SyncVar]
		public LowPrecisionQuaternion Rotation;

		[global::Mirror.SyncVar]
		public global::UnityEngine.Vector3 Scale;

		[global::Mirror.SyncVar]
		public byte MovementSmoothing;

		private const float SmoothingMultiplier = 0.3f;

		public abstract string CommandName { get; }

		public global::Footprinting.Footprint SpawnerFootprint { get; private set; }

		public global::UnityEngine.Vector3 NetworkPosition
		{
			get
			{
				return Position;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref Position))
				{
					global::UnityEngine.Vector3 position = Position;
					SetSyncVar(value, ref Position, 1uL);
				}
			}
		}

		public LowPrecisionQuaternion NetworkRotation
		{
			get
			{
				return Rotation;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref Rotation))
				{
					LowPrecisionQuaternion rotation = Rotation;
					SetSyncVar(value, ref Rotation, 2uL);
				}
			}
		}

		public global::UnityEngine.Vector3 NetworkScale
		{
			get
			{
				return Scale;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref Scale))
				{
					global::UnityEngine.Vector3 scale = Scale;
					SetSyncVar(value, ref Scale, 4uL);
				}
			}
		}

		public byte NetworkMovementSmoothing
		{
			get
			{
				return MovementSmoothing;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref MovementSmoothing))
				{
					byte movementSmoothing = MovementSmoothing;
					SetSyncVar(value, ref MovementSmoothing, 8uL);
				}
			}
		}

		protected virtual void LateUpdate()
		{
			if (global::Mirror.NetworkServer.active)
			{
				UpdatePositionServer();
			}
			else
			{
				UpdatePositionClient();
			}
		}

		public virtual void OnSpawned(ReferenceHub admin, global::System.ArraySegment<string> arguments)
		{
			SpawnerFootprint = new global::Footprinting.Footprint(admin);
			global::Mirror.NetworkServer.Spawn(base.gameObject);
			ServerLogs.AddLog(ServerLogs.Modules.Administrative, $"{admin.LoggedNameFromRefHub()} spawned an admin toy: {CommandName} with NetID {base.netId}.", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
		}

		private void UpdatePositionServer()
		{
			NetworkPosition = base.transform.position;
			NetworkRotation = new LowPrecisionQuaternion(base.transform.rotation);
			NetworkScale = base.transform.localScale;
		}

		private void UpdatePositionClient()
		{
			float num = global::UnityEngine.Time.deltaTime * (float)(int)MovementSmoothing * 0.3f;
			if (num == 0f)
			{
				num = 1f;
			}
			base.transform.position = global::UnityEngine.Vector3.Lerp(base.transform.position, Position, num);
			base.transform.rotation = global::UnityEngine.Quaternion.Lerp(base.transform.rotation, Rotation.Value, num);
			base.transform.localScale = global::UnityEngine.Vector3.Lerp(base.transform.localScale, Scale, num);
		}

		private void MirrorProcessed()
		{
		}

		public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
		{
			bool result = base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				global::Mirror.NetworkWriterExtensions.WriteVector3(writer, Position);
				writer.WriteLowPrecisionQuaternion(Rotation);
				global::Mirror.NetworkWriterExtensions.WriteVector3(writer, Scale);
				global::Mirror.NetworkWriterExtensions.WriteByte(writer, MovementSmoothing);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteVector3(writer, Position);
				result = true;
			}
			if ((base.syncVarDirtyBits & 2L) != 0L)
			{
				writer.WriteLowPrecisionQuaternion(Rotation);
				result = true;
			}
			if ((base.syncVarDirtyBits & 4L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteVector3(writer, Scale);
				result = true;
			}
			if ((base.syncVarDirtyBits & 8L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteByte(writer, MovementSmoothing);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				global::UnityEngine.Vector3 position = Position;
				NetworkPosition = global::Mirror.NetworkReaderExtensions.ReadVector3(reader);
				LowPrecisionQuaternion rotation = Rotation;
				NetworkRotation = reader.ReadLowPrecisionQuaternion();
				global::UnityEngine.Vector3 scale = Scale;
				NetworkScale = global::Mirror.NetworkReaderExtensions.ReadVector3(reader);
				byte movementSmoothing = MovementSmoothing;
				NetworkMovementSmoothing = global::Mirror.NetworkReaderExtensions.ReadByte(reader);
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 1L) != 0L)
			{
				global::UnityEngine.Vector3 position2 = Position;
				NetworkPosition = global::Mirror.NetworkReaderExtensions.ReadVector3(reader);
			}
			if ((num & 2L) != 0L)
			{
				LowPrecisionQuaternion rotation2 = Rotation;
				NetworkRotation = reader.ReadLowPrecisionQuaternion();
			}
			if ((num & 4L) != 0L)
			{
				global::UnityEngine.Vector3 scale2 = Scale;
				NetworkScale = global::Mirror.NetworkReaderExtensions.ReadVector3(reader);
			}
			if ((num & 8L) != 0L)
			{
				byte movementSmoothing2 = MovementSmoothing;
				NetworkMovementSmoothing = global::Mirror.NetworkReaderExtensions.ReadByte(reader);
			}
		}
	}
}
