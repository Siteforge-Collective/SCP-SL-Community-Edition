namespace InventorySystem.Items.Radio
{
	public class RadioPickup : global::InventorySystem.Items.Pickups.CollisionDetectionPickup, global::InventorySystem.Items.IUpgradeTrigger
	{
		[global::Mirror.SyncVar]
		public bool SavedEnabled;

		[global::Mirror.SyncVar]
		public byte SavedRange;

		public float SavedBattery;

		private static global::InventorySystem.Items.Radio.RadioItem _radioCache;

		private static bool _radioCacheSet;

		public bool NetworkSavedEnabled
		{
			get
			{
				return SavedEnabled;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref SavedEnabled))
				{
					bool savedEnabled = SavedEnabled;
					SetSyncVar(value, ref SavedEnabled, 1uL);
				}
			}
		}

		public byte NetworkSavedRange
		{
			get
			{
				return SavedRange;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref SavedRange))
				{
					byte savedRange = SavedRange;
					SetSyncVar(value, ref SavedRange, 2uL);
				}
			}
		}

		protected override void Awake()
		{
			base.Awake();
			if (!_radioCacheSet)
			{
				_radioCacheSet = global::InventorySystem.InventoryItemLoader.TryGetItem<global::InventorySystem.Items.Radio.RadioItem>(ItemType.Radio, out _radioCache);
			}
		}

		private void LateUpdate()
		{
			if (global::Mirror.NetworkServer.active && SavedEnabled && _radioCacheSet)
			{
				float num = _radioCache.Ranges[SavedRange].MinuteCostWhenIdle / 60f;
				float num2 = SavedBattery - global::UnityEngine.Time.deltaTime * num / 100f;
				if (num2 <= 0f)
				{
					NetworkSavedEnabled = false;
					num2 = 0f;
				}
				SavedBattery = num2;
			}
		}

		public void ServerOnUpgraded(global::Scp914.Scp914KnobSetting setting)
		{
			SavedBattery = 1f;
		}

		private void MirrorProcessed()
		{
		}

		public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
		{
			bool result = base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, SavedEnabled);
				global::Mirror.NetworkWriterExtensions.WriteByte(writer, SavedRange);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, SavedEnabled);
				result = true;
			}
			if ((base.syncVarDirtyBits & 2L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteByte(writer, SavedRange);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				bool savedEnabled = SavedEnabled;
				NetworkSavedEnabled = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
				byte savedRange = SavedRange;
				NetworkSavedRange = global::Mirror.NetworkReaderExtensions.ReadByte(reader);
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 1L) != 0L)
			{
				bool savedEnabled2 = SavedEnabled;
				NetworkSavedEnabled = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
			}
			if ((num & 2L) != 0L)
			{
				byte savedRange2 = SavedRange;
				NetworkSavedRange = global::Mirror.NetworkReaderExtensions.ReadByte(reader);
			}
		}
	}
}
