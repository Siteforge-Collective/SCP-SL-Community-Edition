namespace InventorySystem.Items.Usables.Scp1576
{
	public class Scp1576Pickup : global::InventorySystem.Items.Pickups.CollisionDetectionPickup
	{
		private byte _prevSyncHorn;

		[global::Mirror.SyncVar]
		private byte _syncHorn;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Transform _horn;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Vector3 _posZero;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Vector3 _posOne;

		public float HornPos
		{
			get
			{
				return (float)(int)_syncHorn / 255f;
			}
			set
			{
				Network_syncHorn = (byte)global::UnityEngine.Mathf.Clamp(global::UnityEngine.Mathf.RoundToInt(value * 255f), 0, 255);
			}
		}

		public byte Network_syncHorn
		{
			get
			{
				return _syncHorn;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref _syncHorn))
				{
					byte syncHorn = _syncHorn;
					SetSyncVar(value, ref _syncHorn, 1uL);
				}
			}
		}

		public static event global::System.Action<ushort, float> OnHornPositionUpdated;

		private void Update()
		{
			if (_prevSyncHorn != _syncHorn)
			{
				float hornPos = HornPos;
				_horn.localPosition = global::UnityEngine.Vector3.Lerp(_posZero, _posOne, hornPos);
				global::InventorySystem.Items.Usables.Scp1576.Scp1576Pickup.OnHornPositionUpdated?.Invoke(Info.Serial, hornPos);
				_prevSyncHorn = _syncHorn;
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
				global::Mirror.NetworkWriterExtensions.WriteByte(writer, _syncHorn);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteByte(writer, _syncHorn);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				byte syncHorn = _syncHorn;
				Network_syncHorn = global::Mirror.NetworkReaderExtensions.ReadByte(reader);
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 1L) != 0L)
			{
				byte syncHorn2 = _syncHorn;
				Network_syncHorn = global::Mirror.NetworkReaderExtensions.ReadByte(reader);
			}
		}
	}
}
