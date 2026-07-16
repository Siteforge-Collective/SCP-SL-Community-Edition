using PlayerStatsSystem;

internal class FriendlyFireHandler
{
	private static bool _eventsAssigned;

	internal readonly RoundFriendlyFireDetector Round;

	internal readonly LifeFriendlyFireDetector Life;

	internal readonly WindowFriendlyFireDetector Window;

	internal readonly RespawnFriendlyFireDetector Respawn;

	internal FriendlyFireHandler(ReferenceHub hub)
	{
	}

	private static void OnAnyDied(ReferenceHub deadPlayer, DamageHandlerBase handler)
	{
	}

	private static void OnAnyDamaged(ReferenceHub damagedPlayer, DamageHandlerBase handler)
	{
	}

	private static bool IsFriendlyFire(ReferenceHub damagedPlayer, DamageHandlerBase handler, out AttackerDamageHandler attackerHandler)
	{
		attackerHandler = null;
		return false;
	}
}
