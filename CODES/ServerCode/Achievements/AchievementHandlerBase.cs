namespace Achievements
{
	public abstract class AchievementHandlerBase
	{
		internal virtual void OnInitialize()
		{
		}

		internal virtual void OnRoundStarted()
		{
		}

		public static void ClientAchieve(global::Achievements.AchievementName targetAchievement)
		{
			if (global::Achievements.AchievementManager.Achievements.TryGetValue(targetAchievement, out var value))
			{
				value.Achieve();
			}
		}

		public static void ServerAchieve(global::Mirror.NetworkConnection conn, global::Achievements.AchievementName targetAchievement)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				throw new global::System.InvalidOperationException("Method ServerAchieve can only be executed on the server.");
			}
			if (!conn.identity.isLocalPlayer)
			{
				conn.Send(new global::Achievements.AchievementManager.AchievementMessage
				{
					AchievementId = (byte)targetAchievement
				});
			}
		}
	}
}
