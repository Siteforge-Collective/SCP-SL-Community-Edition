namespace Respawning
{
	public abstract class ConfigBasedTeamSpawnHandler : SpawnableTeamHandlerBase
	{
		private readonly string _maxWaveSizeConfig;

		private readonly string _startTokensConfig;

		private readonly int _defaultMaxWaveSize;

		private readonly int _defaultStartTokens;

		private int _maxWaveSize;

		private int _startTokens;

		public override int MaxWaveSize => 0;

		public override int StartTokens => 0;

		public ConfigBasedTeamSpawnHandler(string maxWaveSizeConfig, int defaultMaxWaveSize, string startTokensConfig, int defaultStartTokens)
		{
		}

		private void RefreshConfigs()
		{
		}
	}
}
