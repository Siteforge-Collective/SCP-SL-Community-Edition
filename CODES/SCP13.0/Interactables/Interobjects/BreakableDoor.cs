using Interactables.Interobjects.DoorUtils;
using Mirror;
using UnityEngine;

namespace Interactables.Interobjects
{
    public class BreakableDoor : BasicDoor, IDamageableDoor, INonInteractableDoor, IScp106PassableDoor
    {
        [SyncVar] private bool _destroyed;
        [SyncVar] private bool _restrict106WhileLocked;

        private bool _prevDestroyed;

        [Header("Breakable Door Settings")]
        [SerializeField] private float _maxHealth = 1000f;
        [SerializeField] private BrokenDoor _brokenPrefab;
        [SerializeField] private DoorDamageType _ignoredDamageSources;
        [SerializeField] private GameObject _objectToReplace;
        [SerializeField] private bool _nonInteractable;

        public float RemainingHealth { get; private set; }

        public float MaxHealth
        {
            get => _maxHealth;
            set => _maxHealth = value;
        }

        public DoorDamageType IgnoredDamageSources
        {
            get => _ignoredDamageSources;
            set => _ignoredDamageSources = value;
        }

        public bool IsDestroyed
        {
            get => _destroyed;
            set
            {
                if (!value)
                {
                    Debug.LogError("You cannot unset the IsDestroyed value.");
                    return;
                }
                ServerDamage(_maxHealth, DoorDamageType.ServerCommand);
            }
        }

        public bool IgnoreLockdowns => _nonInteractable;

        public bool IgnoreRemoteAdmin => _nonInteractable;

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

        protected override void Start()
        {
            base.Start();
            RemainingHealth = _maxHealth;
        }

        [Server]
        public bool ServerDamage(float hp, DoorDamageType type)
        {
            if (!NetworkServer.active)
            {
                Debug.LogWarning("[Server] function 'System.Boolean Interactables.Interobjects.BreakableDoor::ServerDamage(System.Single,Interactables.Interobjects.DoorUtils.DoorDamageType)' called when server was not active");
                return false;
            }

            if (_destroyed)
                return false;

            if ((_ignoredDamageSources & type) != 0)
                return false;

            if (_brokenPrefab == null || _objectToReplace == null)
                return false;

            RemainingHealth -= hp;

            if (RemainingHealth <= 0f)
            {
                RemainingHealth = 0f;
                _destroyed = true;
                DoorEvents.TriggerAction(this, DoorAction.Destroyed, null);
                return true;
            }

            return false;
        }

        public override float GetExactState()
        {
            if (_destroyed)
                return 1f;

            return base.GetExactState();
        }

        public override bool AllowInteracting(ReferenceHub ply, byte colliderId)
        {
            if (_destroyed)
                return false;

            return base.AllowInteracting(ply, colliderId);
        }

        internal override void TargetStateChanged()
        {
            base.TargetStateChanged();
            if (_destroyed)
                return;

            if (MainSource != null && doorAudioSettings != null)
            {
                AudioClip clip = TargetState ? doorAudioSettings.RandomOpeningSound : doorAudioSettings.RandomClosingSound;
                MainSource.PlayOneShot(clip);
            }

            if (RequiredPermissions.RequiredPermissions != KeycardPermissions.None)
            {
                if (MainSource != null && doorAudioSettings != null)
                    MainSource.PlayOneShot(doorAudioSettings.AccessGranted);
            }

            UpdateAnimations = true;
        }

        protected override void Update()
        {
            base.Update();

            if (!_prevDestroyed && _destroyed)
            {
                _prevDestroyed = true;
                ClientDestroyEffects();
            }
        }

        public void ClientDestroyEffects()
        {
            if (_objectToReplace != null)
                _objectToReplace.SetActive(false);

            if (panelSettings != null)
            {
                string textError = panelSettings.TextError;
                Material panelErrorMat = panelSettings.PanelErrorMat;
                SetButtons(textError, panelErrorMat, true);
            }

            if (_brokenPrefab != null && _objectToReplace != null)
            {
                BrokenDoor brokenDoor = Object.Instantiate(_brokenPrefab, _objectToReplace.transform.parent);
                brokenDoor.transform.position = _objectToReplace.transform.position;
                brokenDoor.transform.rotation = _objectToReplace.transform.rotation;
                brokenDoor.transform.localScale = _objectToReplace.transform.localScale;
            }
        }

        public float GetHealthPercent()
        {
            if (_maxHealth <= 0f)
                return 0f;

            return Mathf.Clamp01(RemainingHealth / _maxHealth);
        }
    }
}