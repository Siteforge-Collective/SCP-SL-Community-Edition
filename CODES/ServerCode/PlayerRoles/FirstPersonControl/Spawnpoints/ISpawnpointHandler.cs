namespace PlayerRoles.FirstPersonControl.Spawnpoints
{
	public interface ISpawnpointHandler
	{
		bool TryGetSpawnpoint(out global::UnityEngine.Vector3 position, out float horizontalRot);
	}
}
