using PlayerRoles;

namespace CustomPlayerEffects
{
    public abstract class ParentEffectBase<T> : StatusEffectBase where T : SubEffectBase
    {
        public T[] SubEffects { get; private set; }

        protected override void OnAwake()
        {
            base.OnAwake();

            SubEffects = GetComponents<T>();

            for (int i = 0; i < SubEffects.Length; i++)
            {
                SubEffects[i].Init(this);
            }
        }

        internal override void OnRoleChanged(PlayerRoleBase previousRole, PlayerRoleBase newRole)
        {
            base.OnRoleChanged(previousRole, newRole);
            if (SubEffects == null) return;
            for (int i = 0; i < SubEffects.Length; i++)
                SubEffects[i].DisableEffect();
        }

        public override void OnStopSpectating()
        {
            base.OnStopSpectating();
            if (SubEffects == null) return;
            for (int i = 0; i < SubEffects.Length; i++)
                SubEffects[i].DisableEffect();
        }

        protected virtual void UpdateSubEffects()
        {
            if (SubEffects == null) return;
            for (int i = 0; i < SubEffects.Length; i++)
                SubEffects[i].UpdateEffect();
        }
    }
}