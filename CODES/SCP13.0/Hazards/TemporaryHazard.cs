using Elevators;
using Mirror;
using UnityEngine;

namespace Hazards
{
    [RequireComponent(typeof(TransformElevatorFollower))]
    public abstract class TemporaryHazard : EnvironmentalHazard
    {
        private bool _destroyed;

        private float _elapsed;

        public override bool IsActive => !_destroyed;

        protected abstract float HazardDuration { get; }

        protected virtual float DecaySpeed { get; } = 1f;

        [Server]
        public virtual void ServerDestroy()
        {
            if (!NetworkServer.active)
            {
                Debug.LogWarning("[Server] function 'System.Void Hazards.TemporaryHazard::ServerDestroy()' called when server was not active");
                return;
            }
            _destroyed = true;
            base.AffectedPlayers.ToArray().ForEach(OnExit);
        }

        protected override void Update()
        {
            base.Update();
            if (NetworkServer.active && IsActive)
            {
                if (_elapsed > HazardDuration)
                {
                    ServerDestroy();
                }
                else
                {
                    _elapsed += DecaySpeed * Time.deltaTime;
                }
            }
        }
    }
}
