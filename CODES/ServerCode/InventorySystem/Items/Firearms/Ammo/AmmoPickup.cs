namespace InventorySystem.Items.Firearms.Ammo
{
	public class AmmoPickup : global::InventorySystem.Items.Pickups.ItemPickupBase
	{
		[global::Mirror.SyncVar]
		public ushort SavedAmmo;

		[global::UnityEngine.SerializeField]
		private int _minDisplayedValue;

		[global::UnityEngine.SerializeField]
		private int _maxDisplayedValue;

		[global::UnityEngine.SerializeField]
		private int _roundingValue;

		[global::UnityEngine.SerializeField]
		private bool _hideFirstDigitBelow10;

		public int MaxAmmo => _maxDisplayedValue;

		public ushort NetworkSavedAmmo
		{
			get
			{
				return SavedAmmo;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref SavedAmmo))
				{
					ushort savedAmmo = SavedAmmo;
					SetSyncVar(value, ref SavedAmmo, 2uL);
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
				global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, SavedAmmo);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 2L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, SavedAmmo);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				ushort savedAmmo = SavedAmmo;
				NetworkSavedAmmo = global::Mirror.NetworkReaderExtensions.ReadUInt16(reader);
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 2L) != 0L)
			{
				ushort savedAmmo2 = SavedAmmo;
				NetworkSavedAmmo = global::Mirror.NetworkReaderExtensions.ReadUInt16(reader);
			}
		}
	}
}
