using Interactables.Interobjects.DoorUtils;
using Mirror;
using PlayerRoles;
using UnityEngine;

namespace Interactables.Interobjects
{
    public class PryableDoor : BasicDoor, IScp106PassableDoor
    {
        private static readonly int PryAnimHash = Animator.StringToHash("Pry");

        [SerializeField] private AudioClip _prySound;
        [SerializeField] private DoorLockReason _blockPryingMask = DoorLockReason.None;
        [SerializeField] private float _pryAnimDuration = 2f;
        [SerializeField] public Transform[] PryPositions;
        private float _remainingPryCooldown;
        [SyncVar] private bool _restrict106WhileLocked;

        public bool IsScp106Passable
        {
            get
            {
                if (_restrict106WhileLocked && ActiveLocks != 0)
                    return TargetState;
                return true;
            }
            set
            {
                if (NetworkServer.active)
                    _restrict106WhileLocked = !value;
            }
        }

        public bool TryPryGate(ReferenceHub player)
        {
            if (_remainingPryCooldown > 0f || TargetState || (ActiveLocks & (ushort)_blockPryingMask) != 0 || !player.IsSCP())
                return false;

            RpcPryGate();
            TargetState = true;
            _remainingPryCooldown = _pryAnimDuration;

            return true;
        }

        public override void LockBypassDenied(ReferenceHub ply, byte colliderId)
        {
            RpcPlayBeepSound(false);
        }

        public override void PermissionsDenied(ReferenceHub ply, byte colliderId)
        {
            RpcPlayBeepSound(true);
        }

        protected override void Update()
        {
            base.Update();

            if (_remainingPryCooldown > 0f)
                _remainingPryCooldown -= Time.deltaTime;
        }

        [ClientRpc]
        private void RpcPryGate()
        {
            if (MainSource != null && _prySound != null)
                MainSource.PlayOneShot(_prySound);

            if (MainAnimator != null)
                MainAnimator.SetTrigger(PryAnimHash);
        }
    }
}