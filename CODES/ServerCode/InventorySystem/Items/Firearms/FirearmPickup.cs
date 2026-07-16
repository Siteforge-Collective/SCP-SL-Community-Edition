namespace InventorySystem.Items.Firearms
{
	public class FirearmPickup : global::InventorySystem.Items.Pickups.CollisionDetectionPickup, global::InventorySystem.Items.Pickups.IPickupDistributorTrigger
	{
		[global::System.NonSerialized]
		public bool Distributed;

		[global::Mirror.SyncVar]
		public global::InventorySystem.Items.Firearms.FirearmStatus Status;

		[global::UnityEngine.SerializeField]
		private global::InventorySystem.Items.Firearms.FirearmWorldmodel _worldmodel;

		public global::InventorySystem.Items.Firearms.FirearmStatus NetworkStatus
		{
			get
			{
				return Status;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref Status))
				{
					global::InventorySystem.Items.Firearms.FirearmStatus status = Status;
					SetSyncVar(value, ref Status, 1uL);
				}
			}
		}

		public void OnDistributed()
		{
			Distributed = true;
			NetworkStatus = new global::InventorySystem.Items.Firearms.FirearmStatus(2, global::InventorySystem.Items.Firearms.FirearmStatusFlags.MagazineInserted, global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.GetRandomAttachmentsCode(Info.ItemId));
		}

		private void Update()
		{
			if (_worldmodel.ApplyStatus(Status, Info.ItemId))
			{
				Rb.ResetCenterOfMass();
				Rb.ResetInertiaTensor();
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
				global::Mirror.GeneratedNetworkCode._Write_InventorySystem_002EItems_002EFirearms_002EFirearmStatus(writer, Status);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1L) != 0L)
			{
				global::Mirror.GeneratedNetworkCode._Write_InventorySystem_002EItems_002EFirearms_002EFirearmStatus(writer, Status);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				global::InventorySystem.Items.Firearms.FirearmStatus status = Status;
				NetworkStatus = global::Mirror.GeneratedNetworkCode._Read_InventorySystem_002EItems_002EFirearms_002EFirearmStatus(reader);
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 1L) != 0L)
			{
				global::InventorySystem.Items.Firearms.FirearmStatus status2 = Status;
				NetworkStatus = global::Mirror.GeneratedNetworkCode._Read_InventorySystem_002EItems_002EFirearms_002EFirearmStatus(reader);
			}
		}
	}
}
