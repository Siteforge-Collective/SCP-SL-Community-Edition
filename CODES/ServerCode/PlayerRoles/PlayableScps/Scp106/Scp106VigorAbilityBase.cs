namespace PlayerRoles.PlayableScps.Scp106
{
	public abstract class Scp106VigorAbilityBase : global::PlayerRoles.PlayableScps.Subroutines.ScpKeySubroutine<global::PlayerRoles.PlayableScps.Scp106.Scp106Role>
	{
		private static int _vigorId;

		private static bool _vigorIdSet;

		private global::PlayerRoles.PlayableScps.Scp106.Scp106Vigor _vigor;

		private bool _vigorSet;

		private int VigorId
		{
			get
			{
				if (_vigorIdSet)
				{
					return _vigorId;
				}
				for (int i = 0; i < AllSubroutines.Length; i++)
				{
					if (AllSubroutines[i] is global::PlayerRoles.PlayableScps.Scp106.Scp106Vigor)
					{
						_vigorId = i;
						_vigorIdSet = true;
						return i;
					}
				}
				throw new global::System.InvalidOperationException(string.Format("{0} has no {1} subroutine!", base.Role, "Scp106Vigor"));
			}
		}

		private global::PlayerRoles.PlayableScps.Subroutines.ScpSubroutineBase[] AllSubroutines => base.ScpRole.SubroutineModule.AllSubroutines;

		public virtual bool IsSubmerged => false;

		public virtual bool ForceHumanAnimations => false;

		protected global::PlayerRoles.PlayableScps.Scp106.Scp106Vigor Vigor
		{
			get
			{
				if (!_vigorSet)
				{
					_vigorSet = true;
					_vigor = AllSubroutines[VigorId] as global::PlayerRoles.PlayableScps.Scp106.Scp106Vigor;
				}
				return _vigor;
			}
		}
	}
}
