using System;
using System.Collections.Generic;
using CentralAuth;
using GameCore;
using MEC;
using Mirror;
using TMPro;
using ToggleableMenus;
using UnityEngine;
using Utils.ConfigHandler;

public class PlayerList : SimpleToggleableMenu
{
	[Serializable]
	public class Instance
	{
		public ReferenceHub owner;

		public PlayerListElement listElementReference;
	}

    private static readonly ConfigEntry<float> _refreshRate = new ConfigEntry<float>("player_list_title_rate", 5f, "Player List Title Refresh Rate", "The amount of time (in seconds) between refreshing the title of the player list");

    public static readonly ConfigEntry<string> Title = new ConfigEntry<string>("player_list_title", null, "Player List Title", "The title at the top of the player list menu.");

    public Transform parent;

	public Transform template;

	public GameObject mainPanel;

	public GameObject reportForm;

	public GameObject reportPopup;

	public TextMeshProUGUI badgeText;

	public TextMeshProUGUI timerText;

	public TextMeshProUGUI serverNameText;

	public TextMeshProUGUI reportPopupText;

	public TextMeshProUGUI PlayerCount;

	private static bool _eventsAssigned;

	private static readonly HashSet<ReferenceHub> _alreadyAddedPlayers;

	public static InterfaceColorAdjuster ica;

	public static PlayerList singleton;

	private int _timer;

	private static Transform s_parent;

	private static Transform s_template;

	private static bool _anyAdminOnServer;

    public static readonly List<Instance> instances = new List<Instance>();


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
                ServerConfigSynchronizer.Singleton.ServerName = value;
            }
        }
    }

    public override bool CanToggle => false;

	public override bool LockMovement => false;

    private void Update()
    {
        RectTransform component = GetComponent<RectTransform>();
        component.localPosition = Vector3.zero;
        component.sizeDelta = Vector2.zero;
    }

    private void Start()
    {
        _anyAdminOnServer = false;
        if (NetworkServer.active)
        {
            ConfigFile.ServerConfig.UpdateConfigValue(_refreshRate);
            ConfigFile.ServerConfig.UpdateConfigValue(Title);
            Timing.RunCoroutine(_RefreshTitleLoop(), Segment.FixedUpdate);
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

    protected override void OnToggled()
	{
	}

	internal static bool DisplayProfileButton(PlayerAuthenticationManager pam)
	{
		return false;
	}

	internal static void AddPlayer(ReferenceHub instance, ClientInstanceMode mode)
	{
	}

	internal static void UpdatePlayerCount()
	{
	}

	internal static void RefreshPlayerId(GameObject instance)
	{
	}

	private static void SetPlayerCountText(int current, int max)
	{
	}

	[RuntimeInitializeOnLoadMethod]
	private static void Init()
	{
	}

    public static void UpdatePlayerNickname(ReferenceHub instance)
    {
        foreach (Instance instance2 in instances)
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
                    Debug.LogWarning("UpdatePlayerNickname: PlayerList Instance either has a null list element or is updating for an unknown player.");
                }
                break;
            }
        }
    }

    public static void UpdatePlayerRole(ReferenceHub instance)
    {
        _anyAdminOnServer = false;
        bool flag = instance == null;
        foreach (Instance instance2 in instances)
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
            catch (Exception ex)
            {
                GameCore.Console.AddLog("Exception caught (UpdatePlayerRole in PlayerList): " + ex.Message, Color.red);
                Debug.LogError("Exception caught (UpdatePlayerRole in PlayerList): " + ex.Message);
            }
        }
    }

    public static void UpdateColors()
	{
	}

	public static void DestroyPlayer(ReferenceHub instance)
	{
	}

	public void Report(bool toGM = false)
	{
	}

	public void CloseForm()
	{
	}

	public void ShowReportResponse(string response)
	{
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

    private IEnumerator<float> _RefreshTitleLoop()
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
