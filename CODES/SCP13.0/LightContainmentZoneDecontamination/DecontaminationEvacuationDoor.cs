using System;
using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;
using UnityEngine;

namespace LightContainmentZoneDecontamination
{
    [Obsolete("Replaced by the new door system", true)]
    public class DecontaminationEvacuationDoor : MonoBehaviour
    {
        public static List<DecontaminationEvacuationDoor> Instances = new List<DecontaminationEvacuationDoor>();

        public bool ShouldBeOpened;

        public bool ShouldBeClosed;

        [SerializeField]
        private DoorVariant _door;

        private void Awake()
        {
            Instances.Add(this);
        }

        private void OnDestroy()
        {
            Instances.Remove(this);
        }

        public void Open()
        {
            if (ShouldBeOpened)
            {
                _door.TargetState = true;
                _door.ServerChangeLock(DoorLockReason.AdminCommand, true);
            }
        }

        public void Close()
        {
            if (ShouldBeClosed)
            {
                _door.TargetState = false;
                _door.ServerChangeLock(DoorLockReason.AdminCommand, false);
                _door.ServerChangeLock(DoorLockReason.Regular079, true);
            }
        }
    }
}