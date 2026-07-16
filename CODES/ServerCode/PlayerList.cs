public class PlayerList : global::ToggleableMenus.SimpleToggleableMenu
{
	[global::System.Serializable]
	public class Instance
	{
		public ReferenceHub owner;

		public PlayerListElement listElementReference;
	}

	private static readonly global::Utils.ConfigHandler.ConfigEntry<float> _refreshRate = new global::Utils.ConfigHandler.ConfigEntry<float>("player_list_title_rate", 5f, "Player List Title Refresh Rate", "The amount of time (in seconds) between refreshing the title of the player list");

	public static readonly global::Utils.ConfigHandler.ConfigEntry<string> Title = new global::Utils.ConfigHandler.ConfigEntry<string>("player_list_title", null, "Player List Title", "The title at the top of the player list menu.");

	public global::UnityEngine.Transform parent;

	public global::UnityEngine.Transform template;

	public global::UnityEngine.GameObject mainPanel;

	public global::UnityEngine.GameObject reportForm;

	public global::UnityEngine.GameObject reportPopup;

	public static InterfaceColorAdjuster ica;

	public static PlayerList singleton;

	private int _timer;

	private static global::UnityEngine.Transform s_parent;

	private static global::UnityEngine.Transform s_template;

	private static bool _anyAdminOnServer;

	public static readonly global::System.Collections.Generic.List<PlayerList.Instance> instances = new global::System.Collections.Generic.List<PlayerList.Instance>();

	private static string ServerName
	{
		get
		{
			ServerConfigSynchronizer serverConfigSynchronizer = ServerConfigSynchronizer.Singleton;
			if (!(serverConfigSynchronizer == null))
			{
				return serverConfigSynchronizer.ServerName;
			}
			return null;
		}
		set
		{
			if (!(ServerConfigSynchronizer.Singleton == null))
			{
				ServerConfigSynchronizer.Singleton.NetworkServerName = value;
			}
		}
	}

	private void Update()
	{
		global::UnityEngine.RectTransform component = GetComponent<global::UnityEngine.RectTransform>();
		component.localPosition = global::UnityEngine.Vector3.zero;
		component.sizeDelta = global::UnityEngine.Vector2.zero;
	}

	private void Start()
	{
		_anyAdminOnServer = false;
		if (global::Mirror.NetworkServer.active)
		{
			global::GameCore.ConfigFile.ServerConfig.UpdateConfigValue(_refreshRate);
			global::GameCore.ConfigFile.ServerConfig.UpdateConfigValue(Title);
			global::MEC.Timing.RunCoroutine(_RefreshTitleLoop(), global::MEC.Segment.FixedUpdate);
		}
	}

	protected override void Awake()
	{
		base.Awake();
		instances.Clear();
		singleton = this;
		s_parent = parent;
		s_template = template;
	}

	public static void UpdatePlayerNickname(ReferenceHub instance)
	{
		foreach (PlayerList.Instance instance2 in instances)
		{
			if (!(instance2.owner == null) && !(instance2.owner != instance))
			{
				ReferenceHub hub = ReferenceHub.GetHub(instance2.owner);
				if (instance2.listElementReference != null && hub != null)
				{
					instance2.listElementReference.TextNick.text = hub.nicknameSync.DisplayName;
				}
				else
				{
					global::UnityEngine.Debug.LogWarning("UpdatePlayerNickname: PlayerList Instance either has a null list element or is updating for an unknown player.");
				}
				break;
			}
		}
	}

	public static void UpdatePlayerRole(ReferenceHub instance)
	{
		_anyAdminOnServer = false;
		bool flag = instance == null;
		foreach (PlayerList.Instance instance2 in instances)
		{
			try
			{
				if (instance2 != null)
				{
					if (!_anyAdminOnServer && !string.IsNullOrEmpty(instance.serverRoles.GetUncoloredRoleString()))
					{
						_anyAdminOnServer = true;
					}
					if (!flag)
					{
						_ = instance != instance2.owner;
					}
				}
			}
			catch (global::System.Exception ex)
			{
				global::GameCore.Console.AddLog("Exception caught (UpdatePlayerRole in PlayerList): " + ex.Message, global::UnityEngine.Color.red);
				global::UnityEngine.Debug.LogError("Exception caught (UpdatePlayerRole in PlayerList): " + ex.Message);
			}
		}
	}

	public void RefreshTitleSafe()
	{
		string result;
		if (string.IsNullOrEmpty(Title.Value))
		{
			ServerName = ServerConsole.singleton.RefreshServerNameSafe();
		}
		else if (!ServerConsole.singleton.NameFormatter.TryProcessExpression(Title.Value, "player list title", out result))
		{
			ServerConsole.AddLog(result);
		}
		else
		{
			ServerName = result;
		}
	}

	public void RefreshTitle()
	{
		ServerName = (string.IsNullOrEmpty(Title.Value) ? ServerConsole.singleton.RefreshServerName() : ServerConsole.singleton.NameFormatter.ProcessExpression(Title.Value));
	}

	private global::System.Collections.Generic.IEnumerator<float> _RefreshTitleLoop()
	{
		while (this != null)
		{
			RefreshTitleSafe();
			ushort i = 0;
			while ((float)(int)i < 50f * _refreshRate.Value)
			{
				yield return 0f;
				i++;
			}
		}
	}
}
