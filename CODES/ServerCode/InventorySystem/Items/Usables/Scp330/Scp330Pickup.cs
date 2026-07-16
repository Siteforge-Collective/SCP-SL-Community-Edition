namespace InventorySystem.Items.Usables.Scp330
{
	public class Scp330Pickup : global::InventorySystem.Items.Pickups.CollisionDetectionPickup
	{
		[global::System.Serializable]
		private struct IndividualCandy
		{
			[global::UnityEngine.SerializeField]
			private global::InventorySystem.Items.Usables.Scp330.CandyKindID _kind;

			[global::UnityEngine.SerializeField]
			private global::UnityEngine.GameObject _candyObject;

			public void Refresh(global::InventorySystem.Items.Usables.Scp330.CandyKindID exposed)
			{
				_candyObject.SetActive(exposed == _kind);
			}
		}

		public global::System.Collections.Generic.List<global::InventorySystem.Items.Usables.Scp330.CandyKindID> StoredCandies = new global::System.Collections.Generic.List<global::InventorySystem.Items.Usables.Scp330.CandyKindID>();

		[global::Mirror.SyncVar]
		public global::InventorySystem.Items.Usables.Scp330.CandyKindID ExposedCandy;

		[global::UnityEngine.SerializeField]
		private global::InventorySystem.Items.Usables.Scp330.Scp330Pickup.IndividualCandy[] _candyTypes;

		private int _prevExposed = -1;

		public global::InventorySystem.Items.Usables.Scp330.CandyKindID NetworkExposedCandy
		{
			get
			{
				return ExposedCandy;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref ExposedCandy))
				{
					global::InventorySystem.Items.Usables.Scp330.CandyKindID exposedCandy = ExposedCandy;
					SetSyncVar(value, ref ExposedCandy, 1uL);
				}
			}
		}

		private void Update()
		{
			int exposedCandy = (int)ExposedCandy;
			if (_prevExposed != exposedCandy)
			{
				global::InventorySystem.Items.Usables.Scp330.Scp330Pickup.IndividualCandy[] candyTypes = _candyTypes;
				foreach (global::InventorySystem.Items.Usables.Scp330.Scp330Pickup.IndividualCandy individualCandy in candyTypes)
				{
					individualCandy.Refresh(ExposedCandy);
				}
				_prevExposed = exposedCandy;
				if (global::Mirror.NetworkServer.active && StoredCandies.Count == 0)
				{
					DestroySelf();
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
				global::Mirror.GeneratedNetworkCode._Write_InventorySystem_002EItems_002EUsables_002EScp330_002ECandyKindID(writer, ExposedCandy);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1L) != 0L)
			{
				global::Mirror.GeneratedNetworkCode._Write_InventorySystem_002EItems_002EUsables_002EScp330_002ECandyKindID(writer, ExposedCandy);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				global::InventorySystem.Items.Usables.Scp330.CandyKindID exposedCandy = ExposedCandy;
				NetworkExposedCandy = global::Mirror.GeneratedNetworkCode._Read_InventorySystem_002EItems_002EUsables_002EScp330_002ECandyKindID(reader);
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 1L) != 0L)
			{
				global::InventorySystem.Items.Usables.Scp330.CandyKindID exposedCandy2 = ExposedCandy;
				NetworkExposedCandy = global::Mirror.GeneratedNetworkCode._Read_InventorySystem_002EItems_002EUsables_002EScp330_002ECandyKindID(reader);
			}
		}
	}
}
