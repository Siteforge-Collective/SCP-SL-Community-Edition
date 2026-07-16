namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096VisibilityController : global::PlayerRoles.FirstPersonControl.FpcVisibilityController
	{
		private global::PlayerRoles.PlayableScps.Scp096.Scp096Role _role;

		private global::PlayerRoles.PlayableScps.Scp096.Scp096TargetsTracker _targetsTracker;

		public override global::PlayerRoles.Visibility.InvisibilityFlags IgnoredFlags
		{
			get
			{
				global::PlayerRoles.Visibility.InvisibilityFlags invisibilityFlags = base.IgnoredFlags;
				if (HideNonTargets)
				{
					invisibilityFlags |= (global::PlayerRoles.Visibility.InvisibilityFlags)3u;
				}
				return invisibilityFlags;
			}
		}

		private bool HideNonTargets
		{
			get
			{
				if (!_role.IsRageState(global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Distressed))
				{
					return _role.IsRageState(global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Enraged);
				}
				return true;
			}
		}

		public override bool ValidateVisibility(ReferenceHub target)
		{
			if (HideNonTargets)
			{
				if (_targetsTracker.HasTarget(target))
				{
					return base.ValidateVisibility(target);
				}
				return false;
			}
			return base.ValidateVisibility(target);
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			if (global::Mirror.NetworkServer.active)
			{
				_role = base.Role as global::PlayerRoles.PlayableScps.Scp096.Scp096Role;
				_role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp096.Scp096TargetsTracker>(out _targetsTracker);
			}
		}
	}
}
