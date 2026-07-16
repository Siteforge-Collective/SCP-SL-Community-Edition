public sealed class ReferenceHub : global::Mirror.NetworkBehaviour, global::System.IEquatable<ReferenceHub>, global::PluginAPI.Core.Interfaces.IGameComponent
{
	private class GameObjectComparer : global::System.Collections.Generic.EqualityComparer<global::UnityEngine.GameObject>
	{
		public override bool Equals(global::UnityEngine.GameObject x, global::UnityEngine.GameObject y)
		{
			return x == y;
		}

		public override int GetHashCode(global::UnityEngine.GameObject obj)
		{
			if (!(obj == null))
			{
				return obj.GetHashCode();
			}
			return 0;
		}
	}

	public static global::System.Action<ReferenceHub> OnPlayerAdded;

	public static global::System.Action<ReferenceHub> OnPlayerRemoved;

	private static readonly global::System.Collections.Generic.Dictionary<global::UnityEngine.GameObject, ReferenceHub> HubsByGameObjects = new global::System.Collections.Generic.Dictionary<global::UnityEngine.GameObject, ReferenceHub>(20, new ReferenceHub.GameObjectComparer());

	private static readonly global::System.Collections.Generic.Dictionary<int, ReferenceHub> HubByPlayerIds = new global::System.Collections.Generic.Dictionary<int, ReferenceHub>(20);

	private static bool _localHubSet;

	private static bool _hostHubSet;

	private static ReferenceHub _localHub;

	private static ReferenceHub _hostHub;

	[global::Mirror.SyncVar]
	private RecyclablePlayerId _playerId;

	public global::UnityEngine.Transform PlayerCameraReference;

	public global::Mirror.NetworkIdentity networkIdentity;

	public CharacterClassManager characterClassManager;

	public global::PlayerRoles.PlayerRoleManager roleManager;

	public global::PlayerStatsSystem.PlayerStats playerStats;

	public global::InventorySystem.Inventory inventory;

	public global::InventorySystem.Searching.SearchCoordinator searchCoordinator;

	public ServerRoles serverRoles;

	public global::RemoteAdmin.QueryProcessor queryProcessor;

	public NicknameSync nicknameSync;

	public PlayerInteract playerInteract;

	public global::Interactables.InteractionCoordinator interCoordinator;

	public PlayerEffectsController playerEffectsController;

	public global::Hints.HintDisplay hints;

	public AspectRatioSync aspectRatioSync;

	public global::Security.PlayerRateLimitHandler playerRateLimitHandler;

	public GameConsoleTransmission gameConsoleTransmission;

	internal FriendlyFireHandler FriendlyFireHandler;

	public static global::System.Collections.Generic.HashSet<ReferenceHub> AllHubs { get; private set; } = new global::System.Collections.Generic.HashSet<ReferenceHub>();

	public static ReferenceHub HostHub
	{
		get
		{
			if (!TryGetHostHub(out var hub))
			{
				return null;
			}
			return hub;
		}
	}

	public static ReferenceHub LocalHub
	{
		get
		{
			if (!TryGetLocalHub(out var hub))
			{
				return null;
			}
			return hub;
		}
	}

	public int PlayerId => _playerId.Value;

	public ClientInstanceMode Mode => characterClassManager.InstanceMode;

	public RecyclablePlayerId Network_playerId
	{
		get
		{
			return _playerId;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref _playerId))
			{
				RecyclablePlayerId playerId = _playerId;
				SetSyncVar(value, ref _playerId, 1uL);
			}
		}
	}

	private void Awake()
	{
		AllHubs.Add(this);
		HubsByGameObjects[base.gameObject] = this;
		if (global::Mirror.NetworkServer.active)
		{
			Network_playerId = new RecyclablePlayerId(useMinQueue: true);
			FriendlyFireHandler = new FriendlyFireHandler(this);
		}
	}

	private void Start()
	{
		OnPlayerAdded?.Invoke(this);
		if (!CharacterClassManager.OnlineMode)
		{
			global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerJoined, this);
		}
	}

	private void OnDestroy()
	{
		global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerLeft, this);
		AllHubs.Remove(this);
		HubsByGameObjects.Remove(base.gameObject);
		HubByPlayerIds.Remove(PlayerId);
		_playerId.Destroy();
		if (_hostHub == this)
		{
			_hostHub = null;
			_hostHubSet = false;
		}
		if (_localHub == this)
		{
			_localHub = null;
			_localHubSet = false;
		}
		OnPlayerRemoved?.Invoke(this);
	}

	public override string ToString()
	{
		return string.Format("{0} (Name='{1}', NetID='{2}', PlayerID='{3}')", "ReferenceHub", base.name, base.netId, PlayerId);
	}

	public static ReferenceHub GetHub(global::UnityEngine.GameObject player)
	{
		if (!TryGetHub(player, out var hub))
		{
			return null;
		}
		return hub;
	}

	public static ReferenceHub GetHub(global::UnityEngine.MonoBehaviour player)
	{
		if (!TryGetHub(player.gameObject, out var hub))
		{
			return null;
		}
		return hub;
	}

	public static bool TryGetHub(global::UnityEngine.GameObject player, out ReferenceHub hub)
	{
		if (player == null)
		{
			hub = null;
			return false;
		}
		if (!HubsByGameObjects.TryGetValue(player, out hub))
		{
			return player.TryGetComponent<ReferenceHub>(out hub);
		}
		return true;
	}

	public static bool TryGetHubNetID(uint netId, out ReferenceHub hub)
	{
		if (global::Mirror.NetworkIdentity.spawned.TryGetValue(netId, out var value) && TryGetHub(value.gameObject, out hub))
		{
			return true;
		}
		hub = null;
		return false;
	}

	public static bool TryGetLocalHub(out ReferenceHub hub)
	{
		if (_localHubSet)
		{
			hub = _localHub;
			return true;
		}
		foreach (ReferenceHub allHub in AllHubs)
		{
			if (allHub.isLocalPlayer)
			{
				hub = allHub;
				_localHub = allHub;
				_localHubSet = true;
				return true;
			}
		}
		hub = null;
		return false;
	}

	public static bool TryGetHostHub(out ReferenceHub hub)
	{
		if (_hostHubSet)
		{
			hub = _hostHub;
			return true;
		}
		foreach (ReferenceHub allHub in AllHubs)
		{
			if (allHub.queryProcessor.IsHost)
			{
				hub = allHub;
				_hostHub = allHub;
				_hostHubSet = true;
				global::PluginAPI.Core.Server.Init();
				return true;
			}
		}
		hub = null;
		return false;
	}

	public static ReferenceHub GetHub(int playerId)
	{
		if (!TryGetHub(playerId, out var hub))
		{
			return null;
		}
		return hub;
	}

	public static bool TryGetHub(int playerId, out ReferenceHub hub)
	{
		if (playerId > 0)
		{
			if (HubByPlayerIds.TryGetValue(playerId, out hub))
			{
				return true;
			}
			foreach (ReferenceHub allHub in AllHubs)
			{
				if (allHub.PlayerId == playerId)
				{
					HubByPlayerIds[playerId] = allHub;
					hub = allHub;
					return true;
				}
			}
		}
		hub = null;
		return false;
	}

	private void OnValidate()
	{
		if (networkIdentity == null)
		{
			networkIdentity = GetComponent<global::Mirror.NetworkIdentity>();
		}
		if (characterClassManager == null)
		{
			characterClassManager = GetComponent<CharacterClassManager>();
		}
		if (roleManager == null)
		{
			roleManager = GetComponent<global::PlayerRoles.PlayerRoleManager>();
		}
		if (inventory == null)
		{
			inventory = GetComponent<global::InventorySystem.Inventory>();
		}
		if (serverRoles == null)
		{
			serverRoles = GetComponent<ServerRoles>();
		}
		if (queryProcessor == null)
		{
			queryProcessor = GetComponent<global::RemoteAdmin.QueryProcessor>();
		}
		if (nicknameSync == null)
		{
			nicknameSync = GetComponent<NicknameSync>();
		}
		if (playerStats == null)
		{
			playerStats = GetComponent<global::PlayerStatsSystem.PlayerStats>();
		}
		if (playerInteract == null)
		{
			playerInteract = GetComponent<PlayerInteract>();
		}
		if (interCoordinator == null)
		{
			interCoordinator = GetComponent<global::Interactables.InteractionCoordinator>();
		}
		if (playerEffectsController == null)
		{
			playerEffectsController = GetComponent<PlayerEffectsController>();
		}
		if (searchCoordinator == null)
		{
			searchCoordinator = GetComponent<global::InventorySystem.Searching.SearchCoordinator>();
		}
		if (hints == null)
		{
			hints = GetComponent<global::Hints.HintDisplay>();
		}
		if (playerRateLimitHandler == null)
		{
			playerRateLimitHandler = GetComponent<global::Security.PlayerRateLimitHandler>();
		}
		if (aspectRatioSync == null)
		{
			aspectRatioSync = GetComponent<AspectRatioSync>();
		}
		if (gameConsoleTransmission == null)
		{
			gameConsoleTransmission = GetComponent<GameConsoleTransmission>();
		}
	}

	public bool Equals(ReferenceHub other)
	{
		return this == other;
	}

	public override bool Equals(object obj)
	{
		if (obj is ReferenceHub referenceHub)
		{
			return this == referenceHub;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.gameObject.GetHashCode();
	}

	public static bool operator ==(ReferenceHub left, ReferenceHub right)
	{
		return (global::UnityEngine.Object)left == (global::UnityEngine.Object)right;
	}

	public static bool operator !=(ReferenceHub left, ReferenceHub right)
	{
		return (global::UnityEngine.Object)left != (global::UnityEngine.Object)right;
	}

	private void MirrorProcessed()
	{
	}

	public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
	{
		bool result = base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteRecyclablePlayerId(_playerId);
			return true;
		}
		global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1L) != 0L)
		{
			writer.WriteRecyclablePlayerId(_playerId);
			result = true;
		}
		return result;
	}

	public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			RecyclablePlayerId playerId = _playerId;
			Network_playerId = reader.ReadRecyclablePlayerId();
			return;
		}
		long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
		if ((num & 1L) != 0L)
		{
			RecyclablePlayerId playerId2 = _playerId;
			Network_playerId = reader.ReadRecyclablePlayerId();
		}
	}
}
