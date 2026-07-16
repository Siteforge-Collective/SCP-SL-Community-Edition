namespace CustomPlayerEffects
{
	public abstract class ParentEffectBase<T> : global::CustomPlayerEffects.StatusEffectBase where T : global::CustomPlayerEffects.SubEffectBase
	{
		public T[] SubEffects { get; private set; }

		internal override void OnRoleChanged(global::PlayerRoles.PlayerRoleBase previousRole, global::PlayerRoles.PlayerRoleBase newRole)
		{
			base.OnRoleChanged(previousRole, newRole);
			T[] subEffects = SubEffects;
			for (int i = 0; i < subEffects.Length; i++)
			{
				subEffects[i].DisableEffect();
			}
		}

		public override void OnStopSpectating()
		{
			base.OnStopSpectating();
			T[] subEffects = SubEffects;
			for (int i = 0; i < subEffects.Length; i++)
			{
				subEffects[i].DisableEffect();
			}
		}

		protected override void OnAwake()
		{
			base.OnAwake();
			T[] subEffects = SubEffects;
			for (int i = 0; i < subEffects.Length; i++)
			{
				subEffects[i].Init(this);
			}
		}

		protected virtual void UpdateSubEffects()
		{
			T[] subEffects = SubEffects;
			for (int i = 0; i < subEffects.Length; i++)
			{
				subEffects[i].UpdateEffect();
			}
		}
	}
}
