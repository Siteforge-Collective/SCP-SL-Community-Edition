namespace PlayerRoles.PlayableScps.Subroutines
{
	public abstract class ScpStandardSubroutine<T> : global::PlayerRoles.PlayableScps.Subroutines.ScpSubroutineBase, global::GameObjectPools.IPoolSpawnable, global::GameObjectPools.IPoolResettable where T : global::PlayerRoles.PlayerRoleBase
	{
		public ReferenceHub Owner { get; private set; }

		public T ScpRole { get; private set; }

		protected override void Awake()
		{
			base.Awake();
			ScpRole = base.Role as T;
		}

		protected void GetSubroutine<TSubroutine>(out TSubroutine sr) where TSubroutine : global::PlayerRoles.PlayableScps.Subroutines.ScpSubroutineBase
		{
			(base.Role as global::PlayerRoles.PlayableScps.Subroutines.ISubroutinedScpRole).SubroutineModule.TryGetSubroutine<TSubroutine>(out sr);
		}

		public virtual void SpawnObject()
		{
			if (!base.Role.TryGetOwner(out var hub))
			{
				throw new global::System.InvalidOperationException("Subroutine " + base.name + " of type " + GetType().FullName + " spawned with no valid owner!");
			}
			Owner = hub;
		}

		public virtual void ResetObject()
		{
		}
	}
}
