namespace MapGeneration.Distributors
{
	public class StructurePositionSync : global::Mirror.NetworkBehaviour
	{
		private const float ConversionRate = 5.625f;

		[global::Mirror.SyncVar]
		private sbyte _rotationY;

		[global::Mirror.SyncVar]
		private global::UnityEngine.Vector3 _position;

		public sbyte Network_rotationY
		{
			get
			{
				return _rotationY;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref _rotationY))
				{
					sbyte rotationY = _rotationY;
					SetSyncVar(value, ref _rotationY, 1uL);
				}
			}
		}

		public global::UnityEngine.Vector3 Network_position
		{
			get
			{
				return _position;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref _position))
				{
					global::UnityEngine.Vector3 position = _position;
					SetSyncVar(value, ref _position, 2uL);
				}
			}
		}

		private void Start()
		{
			if (global::Mirror.NetworkServer.active)
			{
				Network_position = base.transform.position;
				Network_rotationY = (sbyte)global::UnityEngine.Mathf.RoundToInt(base.transform.rotation.eulerAngles.y / 5.625f);
				base.enabled = false;
			}
		}

		private void Update()
		{
			if (_position != global::UnityEngine.Vector3.zero)
			{
				base.transform.position = _position;
				base.transform.rotation = global::UnityEngine.Quaternion.Euler(global::UnityEngine.Vector3.up * _rotationY * 5.625f);
				base.enabled = false;
			}
		}

		private void MirrorProcessed()
		{
		}

		public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
		{
			bool result = base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				global::Mirror.NetworkWriterExtensions.WriteSByte(writer, _rotationY);
				global::Mirror.NetworkWriterExtensions.WriteVector3(writer, _position);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteSByte(writer, _rotationY);
				result = true;
			}
			if ((base.syncVarDirtyBits & 2L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteVector3(writer, _position);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				sbyte rotationY = _rotationY;
				Network_rotationY = global::Mirror.NetworkReaderExtensions.ReadSByte(reader);
				global::UnityEngine.Vector3 position = _position;
				Network_position = global::Mirror.NetworkReaderExtensions.ReadVector3(reader);
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 1L) != 0L)
			{
				sbyte rotationY2 = _rotationY;
				Network_rotationY = global::Mirror.NetworkReaderExtensions.ReadSByte(reader);
			}
			if ((num & 2L) != 0L)
			{
				global::UnityEngine.Vector3 position2 = _position;
				Network_position = global::Mirror.NetworkReaderExtensions.ReadVector3(reader);
			}
		}
	}
}
