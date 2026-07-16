namespace PlayerRoles.PlayableScps.Scp106
{
	public class Scp106VisibilityController : global::PlayerRoles.FirstPersonControl.FpcVisibilityController
	{
		private global::PlayerRoles.PlayableScps.Scp106.Scp106Role _role106;

		private global::PlayerRoles.PlayableScps.Scp106.Scp106StalkVisibilityController _visSubroutine;

		private bool CheckPlayer(ReferenceHub observer)
		{
			if (observer.IsHuman())
			{
				return _visSubroutine.SyncDamage.ContainsKey(observer.PlayerId);
			}
			return true;
		}

		public override global::PlayerRoles.Visibility.InvisibilityFlags GetActiveFlags(ReferenceHub observer)
		{
			global::PlayerRoles.Visibility.InvisibilityFlags invisibilityFlags = base.GetActiveFlags(observer);
			if (_role106.Sinkhole.IsHidden && !CheckPlayer(observer))
			{
				invisibilityFlags |= global::PlayerRoles.Visibility.InvisibilityFlags.Scp106Sinkhole;
			}
			return invisibilityFlags;
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			if (global::Mirror.NetworkServer.active)
			{
				_role106 = base.Role as global::PlayerRoles.PlayableScps.Scp106.Scp106Role;
				_role106.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp106.Scp106StalkVisibilityController>(out _visSubroutine);
			}
		}
	}
}
