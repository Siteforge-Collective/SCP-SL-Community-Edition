namespace PlayerRoles.PlayableScps
{
	public interface ISpawnableScp
	{
		float GetSpawnChance(global::System.Collections.Generic.List<global::PlayerRoles.RoleTypeId> alreadySpawned);
	}
}
