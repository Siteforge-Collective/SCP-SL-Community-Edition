namespace PlayerRoles.PlayableScps.Scp106
{
	public class Scp106HumeShieldController : global::PlayerRoles.PlayableScps.HumeShield.DynamicHumeShieldController
	{
		private global::PlayerRoles.PlayableScps.Scp106.Scp106Role _role106;

		private global::PlayerRoles.PlayableScps.Scp106.Scp106StalkAbility _stalk;

		public override float HsRegeneration
		{
			get
			{
				if (!_stalk.IsActive || !_role106.Sinkhole.IsHidden)
				{
					return 0f;
				}
				return RegenerationRate * HsMax;
			}
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			_role106 = base.Role as global::PlayerRoles.PlayableScps.Scp106.Scp106Role;
			_role106.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp106.Scp106StalkAbility>(out _stalk);
		}
	}
}
