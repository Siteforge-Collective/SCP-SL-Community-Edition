namespace InventorySystem.Items.MicroHID
{
	public class MicroHIDPickup : global::InventorySystem.Items.Pickups.CollisionDetectionPickup
	{
		[global::Mirror.SyncVar]
		public float Energy;

		public float NetworkEnergy
		{
			get
			{
				return Energy;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref Energy))
				{
					float energy = Energy;
					SetSyncVar(value, ref Energy, 1uL);
				}
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
				global::Mirror.NetworkWriterExtensions.WriteSingle(writer, Energy);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteSingle(writer, Energy);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				float energy = Energy;
				NetworkEnergy = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 1L) != 0L)
			{
				float energy2 = Energy;
				NetworkEnergy = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
			}
		}
	}
}
