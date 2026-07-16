using Interactables.Interobjects.DoorUtils;
using Mirror;
using UnityEngine;
using TMPro;

namespace Interactables.Interobjects
{
    public class CheckpointDoor : DoorVariant, IDamageableDoor
    {
        private enum CheckpointSequenceStage
        {
            Idle = 0,
            Granted = 1,
            Open = 2,
            Closing = 3
        }

        private enum CheckpointErrorType : byte
        {
            Denied = 0,
            LockedDown = 1,
            Destroyed = 2
        }

        [Header("Checkpoint Settings")]
        [SerializeField] private DoorVariant[] _subDoors;
        [SerializeField] private float _openingTime;
        [SerializeField] private float _waitTime;
        [SerializeField] private float _warningTime;
        [SerializeField] private PanelVisualSettings _panelSettings;
        [SerializeField] private Material _warningMat;
        [SerializeField] private RegularDoorButton[] _buttons; 
        [SerializeField] private float _anticheatPassableThreshold = 0.2f;

        [Header("Audio")]
        [SerializeField] private AudioClip _warningSound;
        [SerializeField] private AudioClip _loudThingySound;
        [SerializeField] private AudioClip _beepAccess;
        [SerializeField] private AudioClip _beepDenied;
        [SerializeField] private AudioSource _loudSource;
        [SerializeField] private AudioSource _quietSource;

        private float _remainingBeepCooldown;
        private float _mainTimer;
        private bool _permanentDestroyment;
        private CheckpointSequenceStage _currentSequence;
        private bool _prevDestroyed;

        public DoorVariant[] SubDoors => _subDoors;

        public bool IsDestroyed
        {
            get
            {
                if (_subDoors == null) return false;
                foreach (var door in _subDoors)
                {
                    if (door is IDamageableDoor dam && !dam.IsDestroyed) return false;
                }
                return true;
            }
            set { }
        }

        protected override void Start()
        {
            base.Start();
            SetButtons(_panelSettings.TextClosed, _panelSettings.PanelClosedMat);
        }

        public override bool AllowInteracting(ReferenceHub ply, byte colliderId)
        {
            if (IsDestroyed)
            {
                RpcPlayBeepSound((byte)CheckpointErrorType.Destroyed);
                return false;
            }

            foreach (var door in _subDoors)
            {
                if (!door.AllowInteracting(ply, colliderId)) return false;
            }

            return _currentSequence == CheckpointSequenceStage.Idle;
        }

        protected override void Update()
        {
            base.Update();

            if (_remainingBeepCooldown > 0)
                _remainingBeepCooldown -= Time.deltaTime;

            if (_remainingBeepCooldown <= 0 && _currentSequence == CheckpointSequenceStage.Idle && !_permanentDestroyment)
            {
                if (ActiveLocks > 0)
                    SetButtons(_panelSettings.TextLockedDown, _panelSettings.PanelErrorMat);
                else
                    SetButtons(_panelSettings.TextClosed, _panelSettings.PanelClosedMat);
            }

            UpdateSequence();

            // Проверка на уничтожение
            if (IsDestroyed && !_prevDestroyed)
            {
                _prevDestroyed = true;
                ClientDestroyEffects();
            }
        }

        private void UpdateSequence()
        {
            if (TargetState && _currentSequence == CheckpointSequenceStage.Idle)
            {
                if (NetworkServer.active) ToggleAllDoors(true);

                _currentSequence = CheckpointSequenceStage.Granted;
                _mainTimer = 0f;

                if (_quietSource && _beepAccess) _quietSource.PlayOneShot(_beepAccess);
                if (_loudSource && _loudThingySound) _loudSource.PlayOneShot(_loudThingySound);

                SetButtons(_panelSettings.TextMoving, _panelSettings.PanelMovingMat);
                return;
            }

            switch (_currentSequence)
            {
                case CheckpointSequenceStage.Granted:
                    _mainTimer += Time.deltaTime;
                    if (_mainTimer > _openingTime)
                    {
                        _currentSequence = CheckpointSequenceStage.Open;
                        _mainTimer = 0f;
                    }
                    break;

                case CheckpointSequenceStage.Open:
                    if (NetworkServer.active)
                    {
                        _mainTimer += Time.deltaTime;
                        if (_mainTimer > _waitTime) TargetState = false;
                    }

                    if (!TargetState)
                    {
                        _currentSequence = CheckpointSequenceStage.Closing;
                        _mainTimer = 0f;

                        if (_loudSource && _warningSound)
                        {
                            _loudSource.clip = _warningSound;
                            _loudSource.loop = true;
                            _loudSource.Play();
                        }

                        SetButtons(_panelSettings.TextClosed, _panelSettings.PanelMovingMat);
                    }
                    break;

                case CheckpointSequenceStage.Closing:
                    _mainTimer += Time.deltaTime;

                    if (_loudSource && !_loudSource.isPlaying && _warningSound)
                    {
                        _loudSource.PlayOneShot(_warningSound);
                    }

                    if (NetworkServer.active && _mainTimer > _warningTime)
                    {
                        ToggleAllDoors(false);
                        _currentSequence = CheckpointSequenceStage.Idle;

                        if (_loudSource) _loudSource.Stop();

                        SetButtons(_panelSettings.TextClosed, _panelSettings.PanelClosedMat);
                    }
                    break;
            }
        }

        private void ToggleAllDoors(bool newState)
        {
            foreach (var door in _subDoors)
            {
                if (door is IDamageableDoor dam && dam.IsDestroyed) continue;
                door.TargetState = newState;
            }
        }

        protected void SetButtons(string text, Material mat)
        {
            if (_buttons == null || _permanentDestroyment) return;

            foreach (var button in _buttons)
            {
                if (button != null)
                {
                    button.SetupButton(text, mat);
                }
            }
        }

        [ClientRpc]
        private void RpcPlayBeepSound(byte deniedType)
        {
            if (_remainingBeepCooldown > 0) return;
            _remainingBeepCooldown = 1f;

            if (_quietSource && _beepDenied) _quietSource.PlayOneShot(_beepDenied);

            if (deniedType == (byte)CheckpointErrorType.Destroyed)
            {
                _permanentDestroyment = true;
                foreach (var b in _buttons) b?.SetupButton(_panelSettings.TextError, _panelSettings.PanelErrorMat);
            }
            else if (deniedType == (byte)CheckpointErrorType.Denied)
            {
                SetButtons(_panelSettings.TextDenied, _panelSettings.PanelDeniedMat);
            }
            else if (deniedType == (byte)CheckpointErrorType.LockedDown)
            {
                SetButtons(_panelSettings.TextLockedDown, _panelSettings.PanelErrorMat);
            }
        }

        public bool ServerDamage(float hp, DoorDamageType type)
        {
            bool anyDamaged = false;
            foreach (var door in _subDoors)
            {
                if (door is IDamageableDoor dam) anyDamaged |= dam.ServerDamage(hp, type);
            }

            if (IsDestroyed && !_prevDestroyed)
            {
                _prevDestroyed = true;
                RpcPlayBeepSound((byte)CheckpointErrorType.Destroyed);
            }
            return anyDamaged;
        }

        public void ClientDestroyEffects()
        {
            _permanentDestroyment = true;
            foreach (var b in _buttons) b?.SetupButton(_panelSettings.TextError, _panelSettings.PanelErrorMat);
        }

        public float GetHealthPercent()
        {
            float total = 1f;
            foreach (var d in _subDoors)
            {
                if (d is IDamageableDoor dam) total *= dam.GetHealthPercent();
            }
            return total;
        }

        public override float GetExactState()
        {
            float max = 0f;
            foreach (var d in _subDoors) max = Mathf.Max(max, d.GetExactState());
            return max;
        }

        public override bool IsConsideredOpen()
        {
            foreach (var d in _subDoors) if (d.IsConsideredOpen()) return true;
            return false;
        }

        public override void LockBypassDenied(ReferenceHub ply, byte colliderId) => RpcPlayBeepSound((byte)CheckpointErrorType.LockedDown);
        public override void PermissionsDenied(ReferenceHub ply, byte colliderId) => RpcPlayBeepSound((byte)CheckpointErrorType.Denied);
        public override bool AnticheatPassageApproved()
        {
            if (IsConsideredOpen())
                return true;

            if (!TargetState)
                return GetExactState() > _anticheatPassableThreshold;

            return false;
        }
    }
}