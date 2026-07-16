namespace Respawning
{
	public abstract class ConfigBasedTeamSpawnHandler : global::Respawning.SpawnableTeamHandlerBase
	{
		private readonly string _maxWaveSizeConfig;

		private readonly string _startTokensConfig;

		private readonly int _defaultMaxWaveSize;

		private readonly int _defaultStartTokens;

		private int _maxWaveSize;

		private int _startTokens;

		public override int MaxWaveSize => _maxWaveSize;

		public override int StartTokens => _startTokens;

		public ConfigBasedTeamSpawnHandler(string maxWaveSizeConfig, int defaultMaxWaveSize, string startTokensConfig, int defaultStartTokens)
		{
			_maxWaveSizeConfig = maxWaveSizeConfig;
			_defaultMaxWaveSize = defaultMaxWaveSize;
			_startTokensConfig = startTokensConfig;
			_defaultStartTokens = defaultStartTokens;
			RefreshConfigs();
			global::GameCore.ConfigFile.OnConfigReloaded = (global::System.Action)global::System.Delegate.Combine(global::GameCore.ConfigFile.OnConfigReloaded, new global::System.Action(RefreshConfigs));
		}

		private void RefreshConfigs()
		{
			_maxWaveSize = global::GameCore.ConfigFile.ServerConfig.GetInt(_maxWaveSizeConfig, _defaultMaxWaveSize);
			_startTokens = global::GameCore.ConfigFile.ServerConfig.GetInt(_startTokensConfig, _defaultStartTokens);
		}
	}
}
