namespace Respawning
{
	public abstract class SpawnableTeamHandlerBase
	{
		public abstract int MaxWaveSize { get; }

		public abstract int StartTokens { get; }

		public abstract float EffectTime { get; }

		public abstract void GenerateQueue(global::System.Collections.Generic.Queue<global::PlayerRoles.RoleTypeId> queueToFill, int playersToSpawn);
	}
}
