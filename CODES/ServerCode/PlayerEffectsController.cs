public class PlayerEffectsController : global::Mirror.NetworkBehaviour
{
	public global::UnityEngine.Audio.AudioMixer mixer;

	public global::UnityEngine.GameObject effectsGameObject;

	private readonly global::System.Collections.Generic.Dictionary<global::System.Type, global::CustomPlayerEffects.StatusEffectBase> _effectsByType = new global::System.Collections.Generic.Dictionary<global::System.Type, global::CustomPlayerEffects.StatusEffectBase>();

	private readonly global::Mirror.SyncList<byte> _syncEffectsIntensity = new global::Mirror.SyncList<byte>();

	private bool _wasSpectated;

	private ReferenceHub _hub;

	public global::CustomPlayerEffects.StatusEffectBase[] AllEffects { get; private set; }

	public int EffectsLength { get; private set; }

	public bool TryGetEffect(string effectName, out global::CustomPlayerEffects.StatusEffectBase playerEffect)
	{
		global::CustomPlayerEffects.StatusEffectBase[] allEffects = AllEffects;
		foreach (global::CustomPlayerEffects.StatusEffectBase statusEffectBase in allEffects)
		{
			if (statusEffectBase.ToString().StartsWith(effectName, global::System.StringComparison.InvariantCultureIgnoreCase))
			{
				playerEffect = statusEffectBase;
				return true;
			}
		}
		playerEffect = null;
		return false;
	}

	public bool TryGetEffect<T>(out T playerEffect) where T : global::CustomPlayerEffects.StatusEffectBase
	{
		if (_effectsByType.TryGetValue(typeof(T), out var value) && value is T val)
		{
			playerEffect = val;
			return true;
		}
		playerEffect = null;
		return false;
	}

	[global::Mirror.Server]
	public void UseMedicalItem(global::InventorySystem.Items.ItemBase item)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void PlayerEffectsController::UseMedicalItem(InventorySystem.Items.ItemBase)' called when server was not active");
			return;
		}
		global::CustomPlayerEffects.StatusEffectBase[] allEffects = AllEffects;
		foreach (global::CustomPlayerEffects.StatusEffectBase statusEffectBase in allEffects)
		{
			if (statusEffectBase is global::CustomPlayerEffects.IHealablePlayerEffect healablePlayerEffect && healablePlayerEffect.IsHealable(item.ItemTypeId))
			{
				statusEffectBase.IsEnabled = false;
			}
		}
	}

	[global::Mirror.Server]
	public global::CustomPlayerEffects.StatusEffectBase ChangeState(string effectName, byte intensity, float duration = 0f, bool addDuration = false)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogWarning("[Server] function 'CustomPlayerEffects.StatusEffectBase PlayerEffectsController::ChangeState(System.String,System.Byte,System.Single,System.Boolean)' called when server was not active");
			return null;
		}
		if (TryGetEffect(effectName, out var playerEffect))
		{
			playerEffect.ServerSetState(intensity, duration, addDuration);
		}
		return playerEffect;
	}

	[global::Mirror.Server]
	public T ChangeState<T>(byte intensity, float duration = 0f, bool addDuration = false) where T : global::CustomPlayerEffects.StatusEffectBase
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogWarning("[Server] function 'T PlayerEffectsController::ChangeState(System.Byte,System.Single,System.Boolean)' called when server was not active");
			return null;
		}
		if (TryGetEffect<T>(out var playerEffect))
		{
			playerEffect.ServerSetState(intensity, duration, addDuration);
		}
		return playerEffect;
	}

	[global::Mirror.Server]
	public T EnableEffect<T>(float duration = 0f, bool addDuration = false) where T : global::CustomPlayerEffects.StatusEffectBase
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogWarning("[Server] function 'T PlayerEffectsController::EnableEffect(System.Single,System.Boolean)' called when server was not active");
			return null;
		}
		return ChangeState<T>(1, duration, addDuration);
	}

	[global::Mirror.Server]
	public T DisableEffect<T>() where T : global::CustomPlayerEffects.StatusEffectBase
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogWarning("[Server] function 'T PlayerEffectsController::DisableEffect()' called when server was not active");
			return null;
		}
		return ChangeState<T>(0);
	}

	public void DisableAllEffects()
	{
		global::CustomPlayerEffects.StatusEffectBase[] allEffects = AllEffects;
		for (int i = 0; i < allEffects.Length; i++)
		{
			allEffects[i].ServerDisable();
		}
	}

	public T GetEffect<T>() where T : global::CustomPlayerEffects.StatusEffectBase
	{
		if (!TryGetEffect<T>(out var playerEffect))
		{
			return null;
		}
		return playerEffect;
	}

	public string GetAllSpectatorEffects()
	{
		global::System.Text.StringBuilder stringBuilder = global::NorthwoodLib.Pools.StringBuilderPool.Shared.Rent();
		global::CustomPlayerEffects.StatusEffectBase[] allEffects = AllEffects;
		foreach (global::CustomPlayerEffects.StatusEffectBase statusEffectBase in allEffects)
		{
			if (statusEffectBase.IsEnabled && statusEffectBase is global::CustomPlayerEffects.ISpectatorDataPlayerEffect spectatorDataPlayerEffect && spectatorDataPlayerEffect.GetSpectatorText(out var display))
			{
				stringBuilder.AppendFormat("<color=#DC143C>{0}</color>\n", display);
			}
		}
		string result = stringBuilder.ToString();
		global::NorthwoodLib.Pools.StringBuilderPool.Shared.Return(stringBuilder);
		return result;
	}

	[global::Mirror.Server]
	public void ServerSyncEffect(global::CustomPlayerEffects.StatusEffectBase effect)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void PlayerEffectsController::ServerSyncEffect(CustomPlayerEffects.StatusEffectBase)' called when server was not active");
			return;
		}
		for (int i = 0; i < EffectsLength; i++)
		{
			global::CustomPlayerEffects.StatusEffectBase statusEffectBase = AllEffects[i];
			if (statusEffectBase == effect)
			{
				_syncEffectsIntensity[i] = statusEffectBase.Intensity;
				break;
			}
		}
	}

	public void ServerSendPulse<T>() where T : global::CustomPlayerEffects.IPulseEffect
	{
		for (int i = 0; i < EffectsLength; i++)
		{
			if (AllEffects[i] is T)
			{
				byte index = (byte)global::UnityEngine.Mathf.Min(i, 255);
				TargetRpcReceivePulse(_hub.connectionToClient, index);
				global::PlayerRoles.Spectating.SpectatorNetworking.ForeachSpectatorOf(_hub, delegate(ReferenceHub x)
				{
					TargetRpcReceivePulse(x.connectionToClient, index);
				});
				break;
			}
		}
	}

	[global::Mirror.TargetRpc]
	private void TargetRpcReceivePulse(global::Mirror.NetworkConnection _, byte effectIndex)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.NetworkWriterExtensions.WriteByte(writer, effectIndex);
		SendTargetRPCInternal(_, typeof(PlayerEffectsController), "TargetRpcReceivePulse", writer, 0);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	private void Awake()
	{
		_hub = ReferenceHub.GetHub(base.gameObject);
		AllEffects = effectsGameObject.GetComponentsInChildren<global::CustomPlayerEffects.StatusEffectBase>();
		EffectsLength = AllEffects.Length;
		global::CustomPlayerEffects.StatusEffectBase[] allEffects = AllEffects;
		foreach (global::CustomPlayerEffects.StatusEffectBase statusEffectBase in allEffects)
		{
			_effectsByType.Add(statusEffectBase.GetType(), statusEffectBase);
			_syncEffectsIntensity.Add(0);
		}
	}

	private void Update()
	{
	}

	private void Start()
	{
		effectsGameObject.SetActive(value: true);
	}

	private void OnEnable()
	{
		global::PlayerRoles.PlayerRoleManager.OnRoleChanged += OnRoleChanged;
	}

	private void OnDisable()
	{
		global::PlayerRoles.PlayerRoleManager.OnRoleChanged -= OnRoleChanged;
	}

	private void OnRoleChanged(ReferenceHub targetHub, global::PlayerRoles.PlayerRoleBase oldRole, global::PlayerRoles.PlayerRoleBase newRole)
	{
		if (targetHub != _hub)
		{
			return;
		}
		bool flag = oldRole.Team != global::PlayerRoles.Team.Dead && newRole.Team == global::PlayerRoles.Team.Dead;
		global::CustomPlayerEffects.StatusEffectBase[] allEffects = AllEffects;
		foreach (global::CustomPlayerEffects.StatusEffectBase statusEffectBase in allEffects)
		{
			if (flag)
			{
				statusEffectBase.OnDeath(oldRole);
			}
			else
			{
				statusEffectBase.OnRoleChanged(oldRole, newRole);
			}
		}
	}

	[global::UnityEngine.RuntimeInitializeOnLoadMethod]
	private static void Init()
	{
		global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged = (global::System.Action)global::System.Delegate.Combine(global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged, (global::System.Action)delegate
		{
			if (global::Utils.NonAllocLINQ.HashsetExtensions.TryGetFirst(ReferenceHub.AllHubs, (ReferenceHub x) => x.playerEffectsController._wasSpectated, out var first))
			{
				global::CustomPlayerEffects.StatusEffectBase[] allEffects = first.playerEffectsController.AllEffects;
				for (int num = 0; num < allEffects.Length; num++)
				{
					allEffects[num].OnStopSpectating();
				}
				first.playerEffectsController._wasSpectated = false;
			}
			if (global::PlayerRoles.Spectating.SpectatorTargetTracker.TryGetTrackedPlayer(out var hub))
			{
				PlayerEffectsController playerEffectsController = hub.playerEffectsController;
				global::CustomPlayerEffects.StatusEffectBase[] allEffects = playerEffectsController.AllEffects;
				for (int num = 0; num < allEffects.Length; num++)
				{
					allEffects[num].OnBeginSpectating();
				}
				playerEffectsController._wasSpectated = true;
			}
		});
	}

	public PlayerEffectsController()
	{
		InitSyncObject(_syncEffectsIntensity);
	}

	private void MirrorProcessed()
	{
	}

	private void UserCode_TargetRpcReceivePulse(global::Mirror.NetworkConnection _, byte effectIndex)
	{
		int num = global::UnityEngine.Mathf.Min(effectIndex, EffectsLength - 1);
		if (AllEffects[num] is global::CustomPlayerEffects.IPulseEffect pulseEffect)
		{
			pulseEffect.ExecutePulse();
		}
	}

	protected static void InvokeUserCode_TargetRpcReceivePulse(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("TargetRPC TargetRpcReceivePulse called on server.");
		}
		else
		{
			((PlayerEffectsController)obj).UserCode_TargetRpcReceivePulse(global::Mirror.NetworkClient.readyConnection, global::Mirror.NetworkReaderExtensions.ReadByte(reader));
		}
	}

	static PlayerEffectsController()
	{
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(PlayerEffectsController), "TargetRpcReceivePulse", InvokeUserCode_TargetRpcReceivePulse);
	}
}
