using Interactables.Interobjects.DoorUtils;
using Interactables.Verification;
using Mirror;
using UnityEngine;

namespace Interactables.Interobjects
{
    public class AirlockController : NetworkBehaviour, IServerInteractable, IInteractable
    {
        private static readonly int AnimationTriggerHash = Animator.StringToHash("Lockdown");

        public bool AirlockDisabled;

        [SerializeField] private DoorVariant _doorA;
        [SerializeField] private DoorVariant _doorB;
        [SerializeField] private float _lockdownDuration = 10f;
        [SerializeField] private float _lockdownCooldown = 5f;
        [SerializeField] private Animator _targetAnimator;

        private float _lockdownCombinedTimer;
        private byte _frameCooldownTimer;
        private bool _targetStateA;
        private bool _doorsLocked;
        private bool _warheadInProgress;
        private bool _readyToUse;

        public IVerificationRule VerificationRule => StandardDistanceVerification.Default;

        private void Start()
        {
            if (NetworkServer.active)
            {
                DoorEvents.OnDoorAction += OnDoorAction;
                DoorEventOpenerExtension.OnDoorsTriggerred += EventTriggerred;
            }
        }

        private void OnDestroy()
        {
            if (NetworkServer.active)
            {
                DoorEvents.OnDoorAction -= OnDoorAction;
                DoorEventOpenerExtension.OnDoorsTriggerred -= EventTriggerred;
            }
        }

        private void OnDoorAction(DoorVariant door, DoorAction action, ReferenceHub ply)
        {
            if (door != _doorA && door != _doorB)
                return;

            if (action == DoorAction.Opened || action == DoorAction.Closed)
            {
                _targetStateA = _doorA.TargetState;
            }
        }

        private void EventTriggerred(DoorEventOpenerExtension.OpenerEventType eventType)
        {
            switch (eventType)
            {
                case DoorEventOpenerExtension.OpenerEventType.WarheadStart:
                case DoorEventOpenerExtension.OpenerEventType.WarheadCancel:
                    _warheadInProgress = eventType == DoorEventOpenerExtension.OpenerEventType.WarheadStart;
                    _doorA.ServerChangeLock(DoorLockReason.Warhead, _warheadInProgress);
                    _doorB.ServerChangeLock(DoorLockReason.Warhead, _warheadInProgress);

                    if (_warheadInProgress)
                    {
                        _doorA.TargetState = true;
                        _doorB.TargetState = true;
                        _frameCooldownTimer = 5;
                    }
                    else
                    {
                        ToggleAirlock();
                    }
                    break;

                case DoorEventOpenerExtension.OpenerEventType.DeconFinish:
                    _doorsLocked = true;
                    _doorA.ServerChangeLock(DoorLockReason.DecontLockdown, true);
                    _doorB.ServerChangeLock(DoorLockReason.DecontLockdown, true);
                    _doorA.TargetState = false;
                    _doorB.TargetState = false;
                    _lockdownCombinedTimer = 65535f;
                    break;
            }
        }

        private void ToggleAirlock()
        {
            _targetStateA = !_targetStateA;
            _doorA.TargetState = _targetStateA;
            _doorB.TargetState = !_targetStateA;
        }

        private void Update()
        {
            if (NetworkServer.active)
            {
                if (_lockdownCombinedTimer > -Mathf.Abs(_lockdownCooldown))
                {
                    _lockdownCombinedTimer -= Time.deltaTime;

                    if (_doorsLocked && _lockdownCombinedTimer <= 0f)
                    {
                        _doorsLocked = false;
                        _doorA.ServerChangeLock(DoorLockReason.SpecialDoorFeature, false);
                        _doorB.ServerChangeLock(DoorLockReason.SpecialDoorFeature, false);
                        _doorA.TargetState = _targetStateA;
                        _doorB.TargetState = !_targetStateA;
                    }
                }
            }
            else if (_frameCooldownTimer > 0)
            {
                _frameCooldownTimer--;

                if (_frameCooldownTimer == 0)
                {
                    if (Mathf.Approximately(_doorA.GetExactState(), _doorB.GetExactState()))
                    {
                        ToggleAirlock();
                        _frameCooldownTimer = 200;
                    }
                    else
                    {
                        _readyToUse = true;
                        _frameCooldownTimer = 10;
                    }
                }
            }
        }

        public void ServerInteract(ReferenceHub ply, byte colliderId)
        {
            if (NetworkServer.active && !AirlockDisabled && !(_lockdownCombinedTimer > -Mathf.Abs(_lockdownCooldown)))
            {
                _lockdownCombinedTimer = Mathf.Abs(_lockdownDuration);
                _doorsLocked = true;
                _doorA.ServerChangeLock(DoorLockReason.SpecialDoorFeature, true);
                _doorB.ServerChangeLock(DoorLockReason.SpecialDoorFeature, true);
                _doorA.TargetState = false;
                _doorB.TargetState = false;
                RpcAlarm();
            }
        }

        [ClientRpc]
        private void RpcAlarm()
        {
            if (_targetAnimator != null)
                _targetAnimator.SetTrigger(AnimationTriggerHash);
        }
    }
}