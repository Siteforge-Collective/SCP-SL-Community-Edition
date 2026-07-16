public class ServerConfigSynchronizer : global::Mirror.NetworkBehaviour
{
	public enum MainBoolsSettings : byte
	{
		FriendlyFire = 1
	}

	[global::System.Serializable]
	public struct AmmoLimit
	{
		public ItemType AmmoType;

		public ushort Limit;
	}

	public struct PredefinedBanTemplate
	{
		public int Duration;

		public string FormattedDuration;

		public string Reason;
	}

	public static ServerConfigSynchronizer Singleton;

	public static global::System.Action OnRefreshed;

	[global::Mirror.SyncVar]
	public byte MainBoolsSync;

	public global::Mirror.SyncList<sbyte> CategoryLimits = new global::Mirror.SyncList<sbyte>();

	public global::Mirror.SyncList<ServerConfigSynchronizer.AmmoLimit> AmmoLimitsSync = new global::Mirror.SyncList<ServerConfigSynchronizer.AmmoLimit>();

	[global::Mirror.SyncVar]
	public string ServerName;

	[global::Mirror.SyncVar]
	public bool EnableRemoteAdminPredefinedBanTemplates = true;

	[global::Mirror.SyncVar]
	public string RemoteAdminExternalPlayerLookupMode = "disabled";

	[global::Mirror.SyncVar]
	public string RemoteAdminExternalPlayerLookupURL = "";

	[global::System.NonSerialized]
	public string RemoteAdminExternalPlayerLookupToken = string.Empty;

	public readonly global::Mirror.SyncList<ServerConfigSynchronizer.PredefinedBanTemplate> RemoteAdminPredefinedBanTemplates = new global::Mirror.SyncList<ServerConfigSynchronizer.PredefinedBanTemplate>();

	private bool _ready;

	private readonly global::System.Text.RegularExpressions.Regex _regex = new global::System.Text.RegularExpressions.Regex("[^\\w\\d]+", global::System.Text.RegularExpressions.RegexOptions.Compiled);

	public byte NetworkMainBoolsSync
	{
		get
		{
			return MainBoolsSync;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref MainBoolsSync))
			{
				byte mainBoolsSync = MainBoolsSync;
				SetSyncVar(value, ref MainBoolsSync, 1uL);
			}
		}
	}

	public string NetworkServerName
	{
		get
		{
			return ServerName;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref ServerName))
			{
				string serverName = ServerName;
				SetSyncVar(value, ref ServerName, 2uL);
			}
		}
	}

	public bool NetworkEnableRemoteAdminPredefinedBanTemplates
	{
		get
		{
			return EnableRemoteAdminPredefinedBanTemplates;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref EnableRemoteAdminPredefinedBanTemplates))
			{
				bool enableRemoteAdminPredefinedBanTemplates = EnableRemoteAdminPredefinedBanTemplates;
				SetSyncVar(value, ref EnableRemoteAdminPredefinedBanTemplates, 4uL);
			}
		}
	}

	public string NetworkRemoteAdminExternalPlayerLookupMode
	{
		get
		{
			return RemoteAdminExternalPlayerLookupMode;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref RemoteAdminExternalPlayerLookupMode))
			{
				string remoteAdminExternalPlayerLookupMode = RemoteAdminExternalPlayerLookupMode;
				SetSyncVar(value, ref RemoteAdminExternalPlayerLookupMode, 8uL);
			}
		}
	}

	public string NetworkRemoteAdminExternalPlayerLookupURL
	{
		get
		{
			return RemoteAdminExternalPlayerLookupURL;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref RemoteAdminExternalPlayerLookupURL))
			{
				string remoteAdminExternalPlayerLookupURL = RemoteAdminExternalPlayerLookupURL;
				SetSyncVar(value, ref RemoteAdminExternalPlayerLookupURL, 16uL);
			}
		}
	}

	private void Awake()
	{
		Singleton = this;
	}

	public static void RefreshAllConfigs()
	{
		if (!(Singleton == null))
		{
			Singleton.RefreshMainBools();
			Singleton.RefreshCategoryLimits();
			Singleton.RefreshAmmoLimits();
			Singleton.RefreshRAConfigs();
			OnRefreshed?.Invoke();
		}
	}

	[global::Mirror.Server]
	private void RefreshRAConfigs()
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void ServerConfigSynchronizer::RefreshRAConfigs()' called when server was not active");
			return;
		}
		NetworkEnableRemoteAdminPredefinedBanTemplates = ServerStatic.RolesConfig.GetBool("enable_predefined_ban_templates", def: true);
		NetworkRemoteAdminExternalPlayerLookupMode = ServerStatic.RolesConfig.GetString("external_player_lookup_mode", "disabled").Trim().ToLower();
		NetworkRemoteAdminExternalPlayerLookupURL = ServerStatic.RolesConfig.GetString("external_player_lookup_url");
		RemoteAdminExternalPlayerLookupToken = ServerStatic.RolesConfig.GetString("external_player_lookup_token");
		if (!EnableRemoteAdminPredefinedBanTemplates)
		{
			return;
		}
		global::System.Collections.Generic.List<string> stringList = ServerStatic.RolesConfig.GetStringList("PredefinedBanTemplates");
		if (stringList != null)
		{
			ServerConfigSynchronizer.PredefinedBanTemplate item = default(ServerConfigSynchronizer.PredefinedBanTemplate);
			foreach (string item2 in stringList)
			{
				string[] array = YamlConfig.ParseCommaSeparatedString(item2);
				if (array.Length != 2)
				{
					ServerConsole.AddLog("Invalid ban template in RA Config file! Template: " + item2);
					continue;
				}
				if (!int.TryParse(array[0], out var result) || result < 0)
				{
					ServerConsole.AddLog("Invalid ban template in RA Config file - duration must be a non-negative integer. Ban template name: " + item2);
					continue;
				}
				item.Reason = array[1];
				global::System.TimeSpan timeSpan = global::System.TimeSpan.FromSeconds(result);
				item.Duration = (int)timeSpan.TotalMinutes;
				int num = timeSpan.Days / 365;
				if (num > 0)
				{
					item.FormattedDuration = $"{num}y";
				}
				else if (timeSpan.Days > 0)
				{
					item.FormattedDuration = $"{timeSpan.Days}d";
				}
				else if (timeSpan.Hours > 0)
				{
					item.FormattedDuration = $"{timeSpan.Hours}h";
				}
				else if (timeSpan.Minutes > 0)
				{
					item.FormattedDuration = $"{timeSpan.Minutes}m";
				}
				else
				{
					item.FormattedDuration = $"{timeSpan.Seconds}s";
				}
				RemoteAdminPredefinedBanTemplates.Add(item);
			}
			if (RemoteAdminPredefinedBanTemplates.Count == 0)
			{
				NetworkEnableRemoteAdminPredefinedBanTemplates = false;
			}
		}
		else
		{
			NetworkEnableRemoteAdminPredefinedBanTemplates = false;
		}
	}

	[global::Mirror.Server]
	public void RefreshMainBools()
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void ServerConfigSynchronizer::RefreshMainBools()' called when server was not active");
		}
		else
		{
			NetworkMainBoolsSync = Misc.BoolsToByte(ServerConsole.FriendlyFire);
		}
	}

	[global::Mirror.Server]
	private void RefreshCategoryLimits()
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void ServerConfigSynchronizer::RefreshCategoryLimits()' called when server was not active");
			return;
		}
		CategoryLimits.Clear();
		for (int i = 0; global::System.Enum.IsDefined(typeof(ItemCategory), (ItemCategory)i); i++)
		{
			ItemCategory key = (ItemCategory)i;
			if (global::InventorySystem.Configs.InventoryLimits.StandardCategoryLimits.TryGetValue(key, out var value) && value >= 0)
			{
				CategoryLimits.Add(global::GameCore.ConfigFile.ServerConfig.GetSByte("limit_category_" + key.ToString().ToLowerInvariant(), value));
			}
		}
	}

	[global::Mirror.Server]
	private void RefreshAmmoLimits()
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void ServerConfigSynchronizer::RefreshAmmoLimits()' called when server was not active");
			return;
		}
		if (AmmoLimitsSync.Count > 0)
		{
			AmmoLimitsSync.Clear();
		}
		foreach (global::System.Collections.Generic.KeyValuePair<ItemType, ushort> standardAmmoLimit in global::InventorySystem.Configs.InventoryLimits.StandardAmmoLimits)
		{
			ushort uShort = global::GameCore.ConfigFile.ServerConfig.GetUShort("limit_" + standardAmmoLimit.Key.ToString().ToLowerInvariant(), standardAmmoLimit.Value);
			AmmoLimitsSync.Add(new ServerConfigSynchronizer.AmmoLimit
			{
				AmmoType = standardAmmoLimit.Key,
				Limit = uShort
			});
		}
	}

	private void Update()
	{
		if (!_ready)
		{
			ReferenceHub hub;
			if (!global::Mirror.NetworkServer.active)
			{
				_ready = true;
			}
			else if (ReferenceHub.TryGetHostHub(out hub))
			{
				_ready = true;
				RefreshAllConfigs();
			}
		}
	}

	public ServerConfigSynchronizer()
	{
		InitSyncObject(CategoryLimits);
		InitSyncObject(AmmoLimitsSync);
		InitSyncObject(RemoteAdminPredefinedBanTemplates);
	}

	private void MirrorProcessed()
	{
	}

	static ServerConfigSynchronizer()
	{
	}

	public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
	{
		bool result = base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			global::Mirror.NetworkWriterExtensions.WriteByte(writer, MainBoolsSync);
			global::Mirror.NetworkWriterExtensions.WriteString(writer, ServerName);
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, EnableRemoteAdminPredefinedBanTemplates);
			global::Mirror.NetworkWriterExtensions.WriteString(writer, RemoteAdminExternalPlayerLookupMode);
			global::Mirror.NetworkWriterExtensions.WriteString(writer, RemoteAdminExternalPlayerLookupURL);
			return true;
		}
		global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteByte(writer, MainBoolsSync);
			result = true;
		}
		if ((base.syncVarDirtyBits & 2L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteString(writer, ServerName);
			result = true;
		}
		if ((base.syncVarDirtyBits & 4L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, EnableRemoteAdminPredefinedBanTemplates);
			result = true;
		}
		if ((base.syncVarDirtyBits & 8L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteString(writer, RemoteAdminExternalPlayerLookupMode);
			result = true;
		}
		if ((base.syncVarDirtyBits & 0x10L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteString(writer, RemoteAdminExternalPlayerLookupURL);
			result = true;
		}
		return result;
	}

	public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			byte mainBoolsSync = MainBoolsSync;
			NetworkMainBoolsSync = global::Mirror.NetworkReaderExtensions.ReadByte(reader);
			string serverName = ServerName;
			NetworkServerName = global::Mirror.NetworkReaderExtensions.ReadString(reader);
			bool enableRemoteAdminPredefinedBanTemplates = EnableRemoteAdminPredefinedBanTemplates;
			NetworkEnableRemoteAdminPredefinedBanTemplates = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
			string remoteAdminExternalPlayerLookupMode = RemoteAdminExternalPlayerLookupMode;
			NetworkRemoteAdminExternalPlayerLookupMode = global::Mirror.NetworkReaderExtensions.ReadString(reader);
			string remoteAdminExternalPlayerLookupURL = RemoteAdminExternalPlayerLookupURL;
			NetworkRemoteAdminExternalPlayerLookupURL = global::Mirror.NetworkReaderExtensions.ReadString(reader);
			return;
		}
		long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
		if ((num & 1L) != 0L)
		{
			byte mainBoolsSync2 = MainBoolsSync;
			NetworkMainBoolsSync = global::Mirror.NetworkReaderExtensions.ReadByte(reader);
		}
		if ((num & 2L) != 0L)
		{
			string serverName2 = ServerName;
			NetworkServerName = global::Mirror.NetworkReaderExtensions.ReadString(reader);
		}
		if ((num & 4L) != 0L)
		{
			bool enableRemoteAdminPredefinedBanTemplates2 = EnableRemoteAdminPredefinedBanTemplates;
			NetworkEnableRemoteAdminPredefinedBanTemplates = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
		}
		if ((num & 8L) != 0L)
		{
			string remoteAdminExternalPlayerLookupMode2 = RemoteAdminExternalPlayerLookupMode;
			NetworkRemoteAdminExternalPlayerLookupMode = global::Mirror.NetworkReaderExtensions.ReadString(reader);
		}
		if ((num & 0x10L) != 0L)
		{
			string remoteAdminExternalPlayerLookupURL2 = RemoteAdminExternalPlayerLookupURL;
			NetworkRemoteAdminExternalPlayerLookupURL = global::Mirror.NetworkReaderExtensions.ReadString(reader);
		}
	}
}
