using Interactables.Interobjects.DoorUtils;
using UnityEngine;

namespace Interactables.Interobjects
{
    public class DummyDoor : DoorVariant, IDamageableDoor, INonInteractableDoor
    {
        [field: SerializeField]
        public bool IgnoreLockdowns { get; private set; }

        [field: SerializeField]
        public bool IgnoreRemoteAdmin { get; private set; }

        public bool IsDestroyed
        {
            get => false;
            set { } 
        }


        public override bool AllowInteracting(ReferenceHub ply, byte colliderId)
        {
            return false;
        }

        public override bool AnticheatPassageApproved()
        {
            return false;
        }

        public override float GetExactState()
        {
            return 0f;
        }

        public float GetHealthPercent()
        {
            return 0f;
        }

        public override bool IsConsideredOpen()
        {
            return TargetState;
        }

        public bool ServerDamage(float hp, DoorDamageType type)
        {
            return false;
        }

        public void ClientDestroyEffects()
        {
        }

        public override void LockBypassDenied(ReferenceHub ply, byte colliderId) { }
        public override void PermissionsDenied(ReferenceHub ply, byte colliderId) { }
    }
}