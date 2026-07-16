using System.Runtime.InteropServices;
using DeathAnimations;
using Mirror;
using RoundRestarting;
using UnityEngine;

namespace PlayerRoles.Ragdolls
{
    public class BasicRagdoll : NetworkBehaviour
    {
        [SyncVar]
        public RagdollData Info;

        public DeathAnimation[] AllDeathAnimations;

        [SerializeField]
        private Transform _originPoint;

        private bool _frozen;

        private bool _roundRestartEventSet;

        public virtual Transform CenterPoint
        {
            get
            {
                if (!(_originPoint != null))
                {
                    return base.transform;
                }
                return _originPoint;
            }
        }

        private void OnRestartTriggered()
        {
            NetworkServer.Destroy(base.gameObject);
        }

        public virtual void FreezeRagdoll()
        {
            _frozen = true;
        }

        protected virtual void Start()
        {
            base.transform.SetPositionAndRotation(Info.StartPosition, Info.StartRotation);
            Info.Handler.ProcessRagdoll(this);
            RagdollManager.OnSpawnedRagdoll(this);
            if (NetworkServer.active)
            {
                _roundRestartEventSet = true;
                RoundRestart.OnRestartTriggered += OnRestartTriggered;
            }
        }

        protected virtual void OnDestroy()
        {
            RagdollManager.OnRemovedRagdoll(this);
            if (_roundRestartEventSet)
            {
                RoundRestart.OnRestartTriggered -= OnRestartTriggered;
            }
        }

        protected virtual void Update()
        {
            UpdateFreeze();
        }

        private void UpdateFreeze()
        {
            if (!_frozen && !(Info.ExistenceTime < (float)RagdollManager.FreezeTime))
            {
                FreezeRagdoll();
            }
        }
    }
}
