using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;
using Mirror;
using UnityEngine;

namespace Interactables.Interobjects
{
    public class BasicDoor : DoorVariant
    {
        private static readonly int _animHash = Animator.StringToHash("isOpen");

        [Header("General settings")]
        [SerializeField] private Animator _mainAnimator;
        [SerializeField] private AudioSource _mainSource;
        [SerializeField] private float _cooldownDuration = 0.5f;
        [SerializeField] private float _consideredOpenThreshold = 0.7f;
        [SerializeField] private float _anticheatPassableThreshold = 0.2f;

        [Header("These values are used to get the exact state")]
        [SerializeField] private Transform _stateMoveable;
        [SerializeField] private Transform _stateStator;
        [SerializeField] private float _stateMinDis;
        [SerializeField] private float _stateMaxDis;

        [SerializeField] private RegularDoorButton[] _buttons;
        [SerializeField] private PanelVisualSettings PanelSettings;
        [SerializeField] private DoorAudioSettings AudioSettings;

        public List<Collider> Scp106Colliders = new List<Collider>();

        public bool UpdateAnimations;
        public Animator MainAnimator => _mainAnimator;
        public AudioSource MainSource => _mainSource;
        public DoorAudioSettings doorAudioSettings => AudioSettings;
        public PanelVisualSettings panelSettings => PanelSettings;

        private float _remainingAnimCooldown;
        private float _remainingBeepCooldown;

        public override bool AllowInteracting(ReferenceHub ply, byte colliderId)
        {
            return _remainingAnimCooldown <= 0f;
        }

        public override float GetExactState()
        {
            if (_stateMoveable == null || _stateStator == null)
                return TargetState ? 1f : 0f;

            Vector3 posMove = _stateMoveable.position;
            Vector3 posStat = _stateStator.position;

            float distance = Mathf.Abs(posMove.x - posStat.x) +
                             Mathf.Abs(posMove.y - posStat.y) +
                             Mathf.Abs(posMove.z - posStat.z);

            return Mathf.Clamp01(Mathf.InverseLerp(_stateMinDis, _stateMaxDis, distance));
        }

        public override bool IsConsideredOpen()
        {
            return GetExactState() > _consideredOpenThreshold;
        }

        public override bool AnticheatPassageApproved()
        {
            if (IsConsideredOpen())
                return true;

            if (!TargetState)
                return GetExactState() > _anticheatPassableThreshold;

            return false;
        }

        public override void LockBypassDenied(ReferenceHub ply, byte colliderId)
        {
            RpcPlayBeepSound(false);
        }

        public override void PermissionsDenied(ReferenceHub ply, byte colliderId)
        {
            RpcPlayBeepSound(true);
        }

        [ClientRpc]
        protected void RpcPlayBeepSound(bool setDeniedButtons)
        {
            if (_remainingBeepCooldown > 0f)
                return;

            _remainingBeepCooldown = 1f;

            if (AudioSettings != null && _mainSource != null)
            {
                _mainSource.PlayOneShot(AudioSettings.AccessDenied);
            }

            if (setDeniedButtons && PanelSettings != null && _buttons != null)
            {
                SetButtons(PanelSettings.TextDenied, PanelSettings.PanelDeniedMat, true);
            }
        }

        protected override void Update()
        {
            base.Update();

            if (NetworkServer.active && _remainingAnimCooldown > 0f)
            {
                _remainingAnimCooldown -= Time.deltaTime;
            }
        }

        internal override void TargetStateChanged()
        {
            if (_mainAnimator != null)
            {
                _mainAnimator.SetBool(_animHash, TargetState);
            }

            if (NetworkServer.active)
            {
                _remainingAnimCooldown = _cooldownDuration;
            }
        }

        protected override void LockChanged(ushort prevValue)
        {
            UpdateAnimations = true;
            UpdateButtonVisuals();
        }

        private void UpdateButtonVisuals()
        {
            if (_buttons == null || PanelSettings == null)
                return;

            string text = TargetState ? PanelSettings.TextOpen : PanelSettings.TextClosed;
            Material mat = TargetState ? PanelSettings.PanelOpenMat : PanelSettings.PanelClosedMat;

            foreach (var button in _buttons)
            {
                if (button != null)
                {
                    button.SetupButton(text, mat);
                }
            }
        }

        protected void SetButtons(string text, Material mat, bool isError)
        {
            if (_buttons == null) return;

            foreach (var button in _buttons)
            {
                if (button != null)
                {
                    button.SetupButton(text, mat);
                }
            }
        }
    }
}