using PlayerRoles;
using PlayerRoles.Spectating;

namespace DeathAnimations
{
    public abstract class FirstpersonDeathAnimation : DeathAnimation
    {
        private bool _eventAssigned;

        protected bool IsFirstperson => ReferenceHub.LocalHub == TargetRagdoll.Info.OwnerHub || (ReferenceHub.LocalHub?.roleManager.CurrentRole is SpectatorRole && ReferenceHub.LocalHub.IsSpectatedBy(TargetRagdoll.Info.OwnerHub));

        public bool EventAssigned
        {
            set
            {
                if (value)
                {
                    PlayerRoleManager.OnRoleChanged += OnRoleChanged;
                    SpectatorTargetTracker.OnTargetChanged += OnTargetChanged;
                }
                else
                {
                    PlayerRoleManager.OnRoleChanged -= OnRoleChanged;
                    SpectatorTargetTracker.OnTargetChanged -= OnTargetChanged;
                }
                _eventAssigned = value;
            }
        }

        protected override void OnAnimationStarted()
        {
            if (IsFirstperson)
            {
                EventAssigned = true;
            }
        }

        protected override void OnAnimationEnded()
        {
            if (_eventAssigned)
            {
                EventAssigned = false;
            }
        }

        protected virtual void OnDestroy()
        {
            if (_eventAssigned)
            {
                EventAssigned = false;
            }
        }

        private void OnTargetChanged()
        {
            if (IsPlaying && !IsFirstperson)
            {
                IsPlaying = false;
                OnAnimationEnded();
            }
        }

        private void OnRoleChanged(ReferenceHub hub, PlayerRoleBase prevRole, PlayerRoleBase newRole)
        {
            if (hub.isLocalPlayer && newRole.Team != Team.Dead)
            {
                if (IsPlaying)
                {
                    IsPlaying = false;
                    OnAnimationEnded();
                }
            }
        }
    }
}
