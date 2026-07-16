using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

namespace InventorySystem.Items.ThrowableProjectiles
{
    public abstract class TimeGrenade : ThrownProjectile
    {
        [SerializeField]
        private float _fuseTime;

        [SyncVar]
        private double _syncTargetTime;

        private bool _alreadyDetonated;

        public double TargetTime
        {
            get
            {
                return _syncTargetTime;
            }
            protected set
            {
                _syncTargetTime = value;
            }
        }

        protected abstract void ServerFuseEnd();

        public override void ServerActivate()
        {
            _syncTargetTime = NetworkTime.time + (double)_fuseTime;
        }

        protected virtual void Update()
        {
            if (NetworkServer.active && !_alreadyDetonated && TargetTime != 0.0 && !(NetworkTime.time < TargetTime))
            {
                ServerFuseEnd();
                _alreadyDetonated = true;
            }
        }
    }
}
