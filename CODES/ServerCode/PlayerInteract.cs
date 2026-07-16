public class PlayerInteract : global::Mirror.NetworkBehaviour
{
	private enum AlphaPanelOperations : byte
	{
		Cancel = 0,
		Lever = 1
	}

	internal enum Generator079Operations : byte
	{
		Door = 0,
		Tablet = 1,
		Cancel = 2
	}

	internal static bool Scp096DestroyLockedDoors;

	internal static bool CanDisarmedInteract;

	private const float ActivationTokenReward = 1f;

	public global::UnityEngine.LayerMask mask;

	public float raycastMaxDistance;

	private ServerRoles _sr;

	private global::InventorySystem.Inventory _inv;

	private string _uiToggleKey = "numlock";

	private bool _enableUiToggle;

	private global::CustomPlayerEffects.Invisible _invisible;

	private global::Security.RateLimit _playerInteractRateLimit;

	private ReferenceHub _hub;

	private global::UnityEngine.KeyCode _interactKey;

	private bool CanInteract
	{
		get
		{
			if (_playerInteractRateLimit.CanExecute() && (!global::InventorySystem.Disarming.DisarmedPlayers.IsDisarmed(_hub.inventory) || CanDisarmedInteract))
			{
				return !_hub.interCoordinator.AnyBlocker(global::InventorySystem.Items.BlockedInteraction.GeneralInteractions);
			}
			return false;
		}
	}

	private void Start()
	{
		_hub = GetComponent<ReferenceHub>();
		_playerInteractRateLimit = _hub.playerRateLimitHandler.RateLimits[0];
		_sr = _hub.serverRoles;
		_inv = _hub.inventory;
		_invisible = _hub.playerEffectsController.GetEffect<global::CustomPlayerEffects.Invisible>();
	}

	private void Update()
	{
	}

	[global::Mirror.Command(channel = 4)]
	private void CmdUsePanel(PlayerInteract.AlphaPanelOperations n)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.GeneratedNetworkCode._Write_PlayerInteract_002FAlphaPanelOperations(writer, n);
		SendCommandInternal(typeof(PlayerInteract), "CmdUsePanel", writer, 4);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.ClientRpc]
	private void RpcLeverSound()
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		SendRPCInternal(typeof(PlayerInteract), "RpcLeverSound", writer, 0, includeOwner: true);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.Command(channel = 4)]
	private void CmdSwitchAWButton()
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		SendCommandInternal(typeof(PlayerInteract), "CmdSwitchAWButton", writer, 4);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.Command(channel = 4)]
	private void CmdDetonateWarhead()
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		SendCommandInternal(typeof(PlayerInteract), "CmdDetonateWarhead", writer, 4);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	private bool ChckDis(global::UnityEngine.Vector3 pos)
	{
		return global::UnityEngine.Vector3.Distance(base.transform.position, pos) < raycastMaxDistance * 1.5f;
	}

	private void OnInteract()
	{
		_invisible.ServerDisable();
	}

	private void MirrorProcessed()
	{
	}

	private void UserCode_CmdUsePanel(PlayerInteract.AlphaPanelOperations n)
	{
		if (!CanInteract)
		{
			return;
		}
		ReferenceHub component = GetComponent<ReferenceHub>();
		AlphaWarheadNukesitePanel nukeside = AlphaWarheadOutsitePanel.nukeside;
		if (!ChckDis(nukeside.transform.position))
		{
			return;
		}
		switch (n)
		{
		case PlayerInteract.AlphaPanelOperations.Cancel:
			OnInteract();
			AlphaWarheadController.Singleton.CancelDetonation(_hub);
			ServerLogs.AddLog(ServerLogs.Modules.Warhead, component.LoggedNameFromRefHub() + " cancelled the Alpha Warhead detonation.", ServerLogs.ServerLogType.GameEvent);
			break;
		case PlayerInteract.AlphaPanelOperations.Lever:
			OnInteract();
			if (nukeside.AllowChangeLevelState())
			{
				nukeside.Networkenabled = !nukeside.enabled;
				RpcLeverSound();
				ServerLogs.AddLog(ServerLogs.Modules.Warhead, component.LoggedNameFromRefHub() + " set the Alpha Warhead status to " + nukeside.enabled + ".", ServerLogs.ServerLogType.GameEvent);
			}
			break;
		}
	}

	protected static void InvokeUserCode_CmdUsePanel(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogError("Command CmdUsePanel called on client.");
		}
		else
		{
			((PlayerInteract)obj).UserCode_CmdUsePanel(global::Mirror.GeneratedNetworkCode._Read_PlayerInteract_002FAlphaPanelOperations(reader));
		}
	}

	private void UserCode_RpcLeverSound()
	{
	}

	protected static void InvokeUserCode_RpcLeverSound(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("RPC RpcLeverSound called on server.");
		}
		else
		{
			((PlayerInteract)obj).UserCode_RpcLeverSound();
		}
	}

	private void UserCode_CmdSwitchAWButton()
	{
		if (!CanInteract)
		{
			return;
		}
		global::UnityEngine.GameObject gameObject = global::UnityEngine.GameObject.Find("OutsitePanelScript");
		if (!ChckDis(gameObject.transform.position) || (!_sr.BypassMode && (!(_inv.CurInstance is global::InventorySystem.Items.Keycards.KeycardItem keycardItem) || !keycardItem.Permissions.HasFlag(global::Interactables.Interobjects.DoorUtils.KeycardPermissions.AlphaWarhead))))
		{
			return;
		}
		AlphaWarheadOutsitePanel componentInParent = gameObject.GetComponentInParent<AlphaWarheadOutsitePanel>();
		if (!(componentInParent == null) && !componentInParent.keycardEntered)
		{
			OnInteract();
			componentInParent.NetworkkeycardEntered = true;
			if (global::Respawning.RespawnTokensManager.TryGetAssignedSpawnableTeam(_hub, out var stt))
			{
				global::Respawning.RespawnTokensManager.GrantTokens(stt, 1f);
			}
		}
	}

	protected static void InvokeUserCode_CmdSwitchAWButton(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogError("Command CmdSwitchAWButton called on client.");
		}
		else
		{
			((PlayerInteract)obj).UserCode_CmdSwitchAWButton();
		}
	}

	private void UserCode_CmdDetonateWarhead()
	{
		if (CanInteract && (_playerInteractRateLimit.CanExecute() || !AlphaWarheadController.Singleton.IsLocked))
		{
			global::UnityEngine.GameObject gameObject = global::UnityEngine.GameObject.Find("OutsitePanelScript");
			if (ChckDis(gameObject.transform.position) && AlphaWarheadOutsitePanel.nukeside.enabled && gameObject.GetComponent<AlphaWarheadOutsitePanel>().keycardEntered)
			{
				ReferenceHub component = GetComponent<ReferenceHub>();
				AlphaWarheadController.Singleton.StartDetonation(isAutomatic: false, suppressSubtitles: false, component);
				ServerLogs.AddLog(ServerLogs.Modules.Warhead, component.LoggedNameFromRefHub() + " started the Alpha Warhead detonation.", ServerLogs.ServerLogType.GameEvent);
				OnInteract();
			}
		}
	}

	protected static void InvokeUserCode_CmdDetonateWarhead(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogError("Command CmdDetonateWarhead called on client.");
		}
		else
		{
			((PlayerInteract)obj).UserCode_CmdDetonateWarhead();
		}
	}

	static PlayerInteract()
	{
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(PlayerInteract), "CmdUsePanel", InvokeUserCode_CmdUsePanel, requiresAuthority: true);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(PlayerInteract), "CmdSwitchAWButton", InvokeUserCode_CmdSwitchAWButton, requiresAuthority: true);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(PlayerInteract), "CmdDetonateWarhead", InvokeUserCode_CmdDetonateWarhead, requiresAuthority: true);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(PlayerInteract), "RpcLeverSound", InvokeUserCode_RpcLeverSound);
	}
}
