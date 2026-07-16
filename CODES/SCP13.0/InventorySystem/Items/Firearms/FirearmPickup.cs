using System;
using System.Runtime.InteropServices;
using InventorySystem.Items.Firearms.Attachments;
using InventorySystem.Items.Pickups;
using Mirror;
using UnityEngine;

namespace InventorySystem.Items.Firearms
{
	public class FirearmPickup : CollisionDetectionPickup, IPickupDistributorTrigger
	{
		[NonSerialized]
		public bool Distributed;

		[SyncVar]
		public FirearmStatus Status;

		[SerializeField]
		private FirearmWorldmodel _worldmodel;

        private Rigidbody Rb => (base.PhysicsModule as PickupStandardPhysics).Rb;

        public void OnDistributed()
        {
            Distributed = true;
            Status = new FirearmStatus(2, FirearmStatusFlags.MagazineInserted, AttachmentsUtils.GetRandomAttachmentsCode(Info.ItemId));
        }

        private void Update()
        {
            if (_worldmodel.ApplyStatus(Status, new ItemIdentifier(Info.ItemId, Info.Serial)))
            {
                Rb.ResetCenterOfMass();
                Rb.ResetInertiaTensor();
            }
        }
	}
}
