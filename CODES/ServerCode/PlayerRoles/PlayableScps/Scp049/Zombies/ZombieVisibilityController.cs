namespace PlayerRoles.PlayableScps.Scp049.Zombies
{
	public class ZombieVisibilityController : global::PlayerRoles.FirstPersonControl.FpcVisibilityController
	{
		public override bool ValidateVisibility(ReferenceHub hub)
		{
			if (!base.ValidateVisibility(hub))
			{
				return hub.roleManager.CurrentRole is global::PlayerRoles.PlayableScps.Scp049.Scp049Role;
			}
			return true;
		}
	}
}
