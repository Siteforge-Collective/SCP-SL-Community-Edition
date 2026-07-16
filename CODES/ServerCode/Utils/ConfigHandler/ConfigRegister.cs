namespace Utils.ConfigHandler
{
	public abstract class ConfigRegister
	{
		protected readonly global::System.Collections.Generic.List<global::Utils.ConfigHandler.ConfigEntry> registeredConfigs = new global::System.Collections.Generic.List<global::Utils.ConfigHandler.ConfigEntry>();

		public global::Utils.ConfigHandler.ConfigEntry[] GetRegisteredConfigs()
		{
			return registeredConfigs.ToArray();
		}

		public global::Utils.ConfigHandler.ConfigEntry GetRegisteredConfig(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return null;
			}
			foreach (global::Utils.ConfigHandler.ConfigEntry registeredConfig in registeredConfigs)
			{
				if (string.Equals(key, registeredConfig.Key, global::System.StringComparison.OrdinalIgnoreCase))
				{
					return registeredConfig;
				}
			}
			return null;
		}

		public void RegisterConfig(global::Utils.ConfigHandler.ConfigEntry configEntry, bool updateValue = true)
		{
			if (configEntry != null && !string.IsNullOrEmpty(configEntry.Key))
			{
				registeredConfigs.Add(configEntry);
				if (updateValue)
				{
					UpdateConfigValue(configEntry);
				}
			}
		}

		public void RegisterConfigs(global::Utils.ConfigHandler.ConfigEntry[] configEntries, bool updateValue = true)
		{
			if (configEntries != null)
			{
				foreach (global::Utils.ConfigHandler.ConfigEntry configEntry in configEntries)
				{
					RegisterConfig(configEntry, updateValue);
				}
			}
		}

		public void UnRegisterConfig(global::Utils.ConfigHandler.ConfigEntry configEntry)
		{
			if (configEntry != null && !string.IsNullOrEmpty(configEntry.Key))
			{
				registeredConfigs.Remove(configEntry);
			}
		}

		public void UnRegisterConfig(string key)
		{
			UnRegisterConfig(GetRegisteredConfig(key));
		}

		public void UnRegisterConfigs(params global::Utils.ConfigHandler.ConfigEntry[] configEntries)
		{
			if (configEntries != null)
			{
				foreach (global::Utils.ConfigHandler.ConfigEntry configEntry in configEntries)
				{
					UnRegisterConfig(configEntry);
				}
			}
		}

		public void UnRegisterConfigs(params string[] keys)
		{
			if (keys != null)
			{
				foreach (string key in keys)
				{
					UnRegisterConfig(key);
				}
			}
		}

		public void UnRegisterConfigs()
		{
			foreach (global::Utils.ConfigHandler.ConfigEntry registeredConfig in registeredConfigs)
			{
				UnRegisterConfig(registeredConfig);
			}
		}

		public abstract void UpdateConfigValue(global::Utils.ConfigHandler.ConfigEntry configEntry);

		public void UpdateConfigValues(params global::Utils.ConfigHandler.ConfigEntry[] configEntries)
		{
			if (configEntries != null)
			{
				foreach (global::Utils.ConfigHandler.ConfigEntry configEntry in configEntries)
				{
					UpdateConfigValue(configEntry);
				}
			}
		}

		public void UpdateRegisteredConfigValues()
		{
			foreach (global::Utils.ConfigHandler.ConfigEntry registeredConfig in registeredConfigs)
			{
				UpdateConfigValue(registeredConfig);
			}
		}
	}
}
