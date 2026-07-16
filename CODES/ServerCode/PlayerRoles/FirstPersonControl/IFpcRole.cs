namespace PlayerRoles.FirstPersonControl
{
	public interface IFpcRole
	{
		global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule FpcModule { get; }

		global::PlayerRoles.FirstPersonControl.Spawnpoints.ISpawnpointHandler SpawnpointHandler { get; }
	}
}
