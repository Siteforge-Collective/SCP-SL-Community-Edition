namespace Achievements
{
	public readonly struct Achievement
	{
		private readonly string _steamName;

		private readonly string _steamProgress;

		private readonly long _discordId;

		private readonly int _maxValue;

		public readonly bool ActivatedByServer;

		public Achievement(string steamName, long discordId, bool byServer = false)
		{
			_steamName = null;
			_steamProgress = null;
			_discordId = 0L;
			_maxValue = 0;
			ActivatedByServer = false;
		}

		public Achievement(string steamName, string steamParameter, long discordId, int maxValue, bool byServer = false)
		{
			_steamName = null;
			_steamProgress = null;
			_discordId = 0L;
			_maxValue = 0;
			ActivatedByServer = false;
		}

		public void Achieve()
		{
		}

		public void AddProgress(int amt = 1)
		{
		}

		public void Reset()
		{
		}

		private void SetAchievementDiscord(bool state)
		{
		}

		private void SetAchievementSteam(bool state)
		{
		}

		private void AddProgressDiscord(int progress)
		{
		}

		private void AddProgressSteam(int progress)
		{
		}
	}
}
