namespace InventorySystem
{
	public class Inventory : global::Mirror.NetworkBehaviour, global::PlayerRoles.FirstPersonControl.IStaminaModifier, global::PlayerRoles.FirstPersonControl.IMovementSpeedModifier
	{
		public const int MaxSlots = 8;

		public global::InventorySystem.InventoryInfo UserInventory = new global::InventorySystem.InventoryInfo();

		[global::Mirror.SyncVar(hook = "OnItemUpdated")]
		public global::InventorySystem.Items.ItemIdentifier CurItem = global::InventorySystem.Items.ItemIdentifier.None;

		public bool SendItemsNextFrame;

		public bool SendAmmoNextFrame;

		private global::UnityEngine.KeyCode _throwKey;

		private global::InventorySystem.Items.ItemIdentifier _prevCurItem;

		internal ReferenceHub _hub;

		private global::InventorySystem.Items.ItemBase _curInstance;

		private float _staminaModifier;

		private float _movementLimiter;

		private float _movementMultiplier;

		private bool _sprintingDisabled;

		private readonly global::System.Diagnostics.Stopwatch _lastEquipSw = global::System.Diagnostics.Stopwatch.StartNew();

		[global::Mirror.SyncVar]
		private float _syncStaminaModifier;

		[global::Mirror.SyncVar]
		private float _syncMovementLimiter;

		[global::Mirror.SyncVar]
		private float _syncMovementMultiplier;

		[global::UnityEngine.HideInInspector]
		public global::InventorySystem.Items.ItemBase CurInstance
		{
			get
			{
				return _curInstance;
			}
			set
			{
				if (!(value == _curInstance))
				{
					global::InventorySystem.Items.ItemBase curInstance = _curInstance;
					_curInstance = value;
					_ = _curInstance == null;
					if (curInstance != null)
					{
						curInstance.OnHolstered();
						curInstance.IsEquipped = false;
					}
					if (_curInstance != null)
					{
						_curInstance.OnEquipped();
						_curInstance.IsEquipped = true;
					}
				}
			}
		}

		public float LastItemSwitch => (float)_lastEquipSw.Elapsed.TotalSeconds;

		private global::UnityEngine.Transform ItemWorkspace => global::InventorySystem.Items.SharedHandsController.Singleton.transform;

		public bool StaminaModifierActive => true;

		public bool MovementModifierActive => true;

		public float StaminaUsageMultiplier
		{
			get
			{
				if (!IsObserver)
				{
					return _staminaModifier;
				}
				return _syncStaminaModifier;
			}
		}

		public float StaminaRegenMultiplier => 1f;

		public bool SprintingDisabled
		{
			get
			{
				if (!IsObserver)
				{
					return _sprintingDisabled;
				}
				return false;
			}
		}

		public float MovementSpeedMultiplier
		{
			get
			{
				if (!IsObserver)
				{
					return _movementMultiplier;
				}
				return _syncMovementMultiplier;
			}
		}

		public float MovementSpeedLimit
		{
			get
			{
				if (!IsObserver)
				{
					return _movementLimiter;
				}
				return _syncMovementLimiter;
			}
		}

		private bool IsObserver
		{
			get
			{
				if (!global::Mirror.NetworkServer.active)
				{
					return !base.isLocalPlayer;
				}
				return false;
			}
		}

		public global::InventorySystem.Items.ItemIdentifier NetworkCurItem
		{
			get
			{
				return CurItem;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref CurItem))
				{
					global::InventorySystem.Items.ItemIdentifier curItem = CurItem;
					SetSyncVar(value, ref CurItem, 1uL);
					if (global::Mirror.NetworkServer.localClientActive && !getSyncVarHookGuard(1uL))
					{
						setSyncVarHookGuard(1uL, value: true);
						OnItemUpdated(curItem, value);
						setSyncVarHookGuard(1uL, value: false);
					}
				}
			}
		}

		public float Network_syncStaminaModifier
		{
			get
			{
				return _syncStaminaModifier;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref _syncStaminaModifier))
				{
					float syncStaminaModifier = _syncStaminaModifier;
					SetSyncVar(value, ref _syncStaminaModifier, 2uL);
				}
			}
		}

		public float Network_syncMovementLimiter
		{
			get
			{
				return _syncMovementLimiter;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref _syncMovementLimiter))
				{
					float syncMovementLimiter = _syncMovementLimiter;
					SetSyncVar(value, ref _syncMovementLimiter, 4uL);
				}
			}
		}

		public float Network_syncMovementMultiplier
		{
			get
			{
				return _syncMovementMultiplier;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref _syncMovementMultiplier))
				{
					float syncMovementMultiplier = _syncMovementMultiplier;
					SetSyncVar(value, ref _syncMovementMultiplier, 8uL);
				}
			}
		}

		public static event global::System.Action<ReferenceHub> OnItemsModified;

		public static event global::System.Action<ReferenceHub> OnAmmoModified;

		public static event global::System.Action OnServerStarted;

		public static event global::System.Action OnLocalClientStarted;

		public static event global::System.Action<ReferenceHub, global::InventorySystem.Items.ItemIdentifier, global::InventorySystem.Items.ItemIdentifier> OnCurrentItemChanged;

		private void OnItemUpdated(global::InventorySystem.Items.ItemIdentifier prev, global::InventorySystem.Items.ItemIdentifier cur)
		{
			if (prev != cur)
			{
				_lastEquipSw.Restart();
			}
		}

		private void Awake()
		{
			_hub = ReferenceHub.GetHub(base.gameObject);
		}

		private void Start()
		{
			if ((base.isLocalPlayer || global::Mirror.NetworkServer.active) && base.isLocalPlayer)
			{
				_throwKey = NewInput.GetKey(ActionName.ThrowItem, global::UnityEngine.KeyCode.T);
				if (global::Mirror.NetworkServer.active)
				{
					global::InventorySystem.Inventory.OnServerStarted();
					CustomNetworkManager.InvokeOnClientReady();
				}
				global::InventorySystem.Inventory.OnLocalClientStarted?.Invoke();
			}
		}

		private void Update()
		{
			if (global::Mirror.NetworkServer.active)
			{
				if (SendItemsNextFrame)
				{
					SendItemsNextFrame = false;
					global::InventorySystem.Inventory.OnItemsModified?.Invoke(_hub);
					ServerSendItems();
				}
				if (SendAmmoNextFrame)
				{
					SendAmmoNextFrame = false;
					global::InventorySystem.Inventory.OnAmmoModified?.Invoke(_hub);
					ServerSendAmmo();
				}
			}
			if (_prevCurItem != CurItem)
			{
				global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerChangeItem, _hub, _prevCurItem.SerialNumber, CurItem.SerialNumber);
				global::InventorySystem.Inventory.OnCurrentItemChanged?.Invoke(_hub, _prevCurItem, CurItem);
				_prevCurItem = new global::InventorySystem.Items.ItemIdentifier(CurItem.TypeId, CurItem.SerialNumber);
			}
			if (IsObserver)
			{
				return;
			}
			if (CurInstance != null)
			{
				CurInstance.EquipUpdate();
			}
			foreach (global::InventorySystem.Items.ItemBase value in UserInventory.Items.Values)
			{
				value.AlwaysUpdate();
			}
			RefreshModifiers();
		}

		private void RefreshModifiers()
		{
			_staminaModifier = 1f;
			_movementLimiter = float.MaxValue;
			_movementMultiplier = 1f;
			_sprintingDisabled = false;
			foreach (global::System.Collections.Generic.KeyValuePair<ushort, global::InventorySystem.Items.ItemBase> item in UserInventory.Items)
			{
				if (item.Value is global::PlayerRoles.FirstPersonControl.IStaminaModifier staminaModifier && staminaModifier.StaminaModifierActive)
				{
					_staminaModifier *= staminaModifier.StaminaUsageMultiplier;
					_sprintingDisabled |= staminaModifier.SprintingDisabled;
				}
				if (item.Value is global::PlayerRoles.FirstPersonControl.IMovementSpeedModifier movementSpeedModifier && movementSpeedModifier.MovementModifierActive)
				{
					_movementLimiter = global::UnityEngine.Mathf.Min(_movementLimiter, movementSpeedModifier.MovementSpeedLimit);
					_movementMultiplier *= movementSpeedModifier.MovementSpeedMultiplier;
				}
			}
			if (global::Mirror.NetworkServer.active)
			{
				Network_syncStaminaModifier = _staminaModifier;
				Network_syncMovementMultiplier = _movementMultiplier;
				Network_syncMovementLimiter = _movementLimiter;
			}
		}

		[global::Mirror.Server]
		public void ServerSelectItem(ushort itemSerial)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void InventorySystem.Inventory::ServerSelectItem(System.UInt16)' called when server was not active");
			}
			else
			{
				if (itemSerial == CurItem.SerialNumber)
				{
					return;
				}
				global::InventorySystem.Items.ItemBase value = null;
				global::InventorySystem.Items.ItemBase value2 = null;
				bool flag = CurItem.SerialNumber == 0 || (UserInventory.Items.TryGetValue(CurItem.SerialNumber, out value) && CurInstance != null);
				if (itemSerial == 0 || UserInventory.Items.TryGetValue(itemSerial, out value2))
				{
					if ((CurItem.SerialNumber != 0 && flag && !global::InventorySystem.Items.EquipDequipModifierExtensions.CanHolster(value)) || (itemSerial != 0 && !global::InventorySystem.Items.EquipDequipModifierExtensions.CanEquip(value2)))
					{
						return;
					}
					if (itemSerial == 0)
					{
						NetworkCurItem = global::InventorySystem.Items.ItemIdentifier.None;
						if (!base.isLocalPlayer)
						{
							CurInstance = null;
						}
					}
					else
					{
						NetworkCurItem = new global::InventorySystem.Items.ItemIdentifier(value2.ItemTypeId, itemSerial);
						if (!base.isLocalPlayer)
						{
							CurInstance = value2;
						}
					}
				}
				else if (!flag)
				{
					NetworkCurItem = global::InventorySystem.Items.ItemIdentifier.None;
					if (!base.isLocalPlayer)
					{
						CurInstance = null;
					}
				}
			}
		}

		[global::Mirror.Server]
		private void ServerSendItems()
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void InventorySystem.Inventory::ServerSendItems()' called when server was not active");
			}
			else
			{
				if (base.isLocalPlayer)
				{
					return;
				}
				global::System.Collections.Generic.HashSet<global::InventorySystem.Items.ItemIdentifier> hashSet = global::NorthwoodLib.Pools.HashSetPool<global::InventorySystem.Items.ItemIdentifier>.Shared.Rent();
				foreach (global::System.Collections.Generic.KeyValuePair<ushort, global::InventorySystem.Items.ItemBase> item in UserInventory.Items)
				{
					hashSet.Add(new global::InventorySystem.Items.ItemIdentifier(item.Value.ItemTypeId, item.Key));
				}
				TargetRefreshItems(global::System.Linq.Enumerable.ToArray(hashSet));
				global::NorthwoodLib.Pools.HashSetPool<global::InventorySystem.Items.ItemIdentifier>.Shared.Return(hashSet);
			}
		}

		[global::Mirror.Server]
		private void ServerSendAmmo()
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void InventorySystem.Inventory::ServerSendAmmo()' called when server was not active");
			}
			else
			{
				if (base.isLocalPlayer)
				{
					return;
				}
				global::System.Collections.Generic.List<byte> list = global::NorthwoodLib.Pools.ListPool<byte>.Shared.Rent();
				global::System.Collections.Generic.List<ushort> list2 = global::NorthwoodLib.Pools.ListPool<ushort>.Shared.Rent();
				foreach (global::System.Collections.Generic.KeyValuePair<ItemType, ushort> item in UserInventory.ReserveAmmo)
				{
					list.Add((byte)item.Key);
					list2.Add(item.Value);
				}
				TargetRefreshAmmo(list.ToArray(), list2.ToArray());
				global::NorthwoodLib.Pools.ListPool<byte>.Shared.Return(list);
				global::NorthwoodLib.Pools.ListPool<ushort>.Shared.Return(list2);
			}
		}

		[global::Mirror.TargetRpc]
		private void TargetRefreshItems(global::InventorySystem.Items.ItemIdentifier[] ids)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.GeneratedNetworkCode._Write_InventorySystem_002EItems_002EItemIdentifier_005B_005D(writer, ids);
			SendTargetRPCInternal(null, typeof(global::InventorySystem.Inventory), "TargetRefreshItems", writer, 0);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		[global::Mirror.TargetRpc]
		private void TargetRefreshAmmo(byte[] keys, ushort[] values)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.NetworkWriterExtensions.WriteBytesAndSize(writer, keys);
			global::Mirror.GeneratedNetworkCode._Write_System_002EUInt16_005B_005D(writer, values);
			SendTargetRPCInternal(null, typeof(global::InventorySystem.Inventory), "TargetRefreshAmmo", writer, 0);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		[global::Mirror.Command]
		public void CmdSelectItem(ushort itemSerial)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, itemSerial);
			SendCommandInternal(typeof(global::InventorySystem.Inventory), "CmdSelectItem", writer, 0);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		[global::Mirror.Command]
		public void CmdProcessHotkey(ActionName hotkeyButtonPressed, ushort clientsideDesiredItem)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.GeneratedNetworkCode._Write_ActionName(writer, hotkeyButtonPressed);
			global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, clientsideDesiredItem);
			SendCommandInternal(typeof(global::InventorySystem.Inventory), "CmdProcessHotkey", writer, 0);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		[global::Mirror.Command(channel = 4)]
		private void CmdConfirmAcquisition(ushort[] itemSerials)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.GeneratedNetworkCode._Write_System_002EUInt16_005B_005D(writer, itemSerials);
			SendCommandInternal(typeof(global::InventorySystem.Inventory), "CmdConfirmAcquisition", writer, 4);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		[global::Mirror.Command(channel = 4)]
		public void CmdDropItem(ushort itemSerial, bool tryThrow)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, itemSerial);
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, tryThrow);
			SendCommandInternal(typeof(global::InventorySystem.Inventory), "CmdDropItem", writer, 4);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		[global::Mirror.Command(channel = 4)]
		public void CmdDropAmmo(byte ammoType, ushort amount)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.NetworkWriterExtensions.WriteByte(writer, ammoType);
			global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, amount);
			SendCommandInternal(typeof(global::InventorySystem.Inventory), "CmdDropAmmo", writer, 4);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		public global::InventorySystem.Items.ItemBase CreateItemInstance(global::InventorySystem.Items.ItemIdentifier identifier, bool updateViewmodel)
		{
			if (!global::InventorySystem.InventoryItemLoader.AvailableItems.TryGetValue(identifier.TypeId, out var value))
			{
				return null;
			}
			global::InventorySystem.Items.ItemBase itemBase = global::UnityEngine.Object.Instantiate(value, ItemWorkspace);
			itemBase.transform.localPosition = global::UnityEngine.Vector3.zero;
			itemBase.transform.localRotation = global::UnityEngine.Quaternion.identity;
			itemBase.Owner = _hub;
			itemBase.ItemSerial = identifier.SerialNumber;
			return itemBase;
		}

		public bool DestroyItemInstance(ushort targetInstance, global::InventorySystem.Items.Pickups.ItemPickupBase pickup, out global::InventorySystem.Items.ItemBase foundItem)
		{
			if (!UserInventory.Items.TryGetValue(targetInstance, out foundItem))
			{
				return false;
			}
			foundItem.OnRemoved(pickup);
			if (CurInstance == foundItem)
			{
				CurInstance = null;
			}
			global::UnityEngine.Object.Destroy(foundItem.gameObject);
			return true;
		}

		static Inventory()
		{
			global::InventorySystem.Inventory.OnItemsModified = delegate
			{
			};
			global::InventorySystem.Inventory.OnAmmoModified = delegate
			{
			};
			global::InventorySystem.Inventory.OnCurrentItemChanged = delegate
			{
			};
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(global::InventorySystem.Inventory), "CmdSelectItem", InvokeUserCode_CmdSelectItem, requiresAuthority: true);
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(global::InventorySystem.Inventory), "CmdProcessHotkey", InvokeUserCode_CmdProcessHotkey, requiresAuthority: true);
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(global::InventorySystem.Inventory), "CmdConfirmAcquisition", InvokeUserCode_CmdConfirmAcquisition, requiresAuthority: true);
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(global::InventorySystem.Inventory), "CmdDropItem", InvokeUserCode_CmdDropItem, requiresAuthority: true);
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(global::InventorySystem.Inventory), "CmdDropAmmo", InvokeUserCode_CmdDropAmmo, requiresAuthority: true);
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(global::InventorySystem.Inventory), "TargetRefreshItems", InvokeUserCode_TargetRefreshItems);
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(global::InventorySystem.Inventory), "TargetRefreshAmmo", InvokeUserCode_TargetRefreshAmmo);
		}

		private void MirrorProcessed()
		{
		}

		private void UserCode_TargetRefreshItems(global::InventorySystem.Items.ItemIdentifier[] ids)
		{
			global::System.Collections.Generic.Queue<global::InventorySystem.Items.ItemIdentifier> queue = new global::System.Collections.Generic.Queue<global::InventorySystem.Items.ItemIdentifier>();
			global::System.Collections.Generic.List<ushort> list = global::System.Linq.Enumerable.ToList(UserInventory.Items.Keys);
			int num = 0;
			for (int i = 0; i < ids.Length; i++)
			{
				global::InventorySystem.Items.ItemIdentifier item = ids[i];
				if (!global::System.Linq.Enumerable.Contains(UserInventory.Items.Keys, item.SerialNumber))
				{
					queue.Enqueue(item);
				}
				if (list.Contains(item.SerialNumber))
				{
					list.Remove(item.SerialNumber);
				}
			}
			while (list.Count > 0)
			{
				DestroyItemInstance(list[0], null, out var _);
				UserInventory.Items.Remove(list[0]);
				list.RemoveAt(0);
				num++;
			}
			global::System.Collections.Generic.List<ushort> list2 = global::NorthwoodLib.Pools.ListPool<ushort>.Shared.Rent();
			while (queue.Count > 0)
			{
				global::InventorySystem.Items.ItemIdentifier identifier = queue.Dequeue();
				global::InventorySystem.Items.ItemBase itemBase = CreateItemInstance(identifier, updateViewmodel: true);
				UserInventory.Items[identifier.SerialNumber] = itemBase;
				itemBase.OnAdded(null);
				if (itemBase is global::InventorySystem.Items.IAcquisitionConfirmationTrigger)
				{
					list2.Add(identifier.SerialNumber);
				}
				num++;
			}
			if (list2.Count > 0)
			{
				CmdConfirmAcquisition(list2.ToArray());
			}
			global::NorthwoodLib.Pools.ListPool<ushort>.Shared.Return(list2);
		}

		protected static void InvokeUserCode_TargetRefreshItems(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkClient.active)
			{
				global::UnityEngine.Debug.LogError("TargetRPC TargetRefreshItems called on server.");
			}
			else
			{
				((global::InventorySystem.Inventory)obj).UserCode_TargetRefreshItems(global::Mirror.GeneratedNetworkCode._Read_InventorySystem_002EItems_002EItemIdentifier_005B_005D(reader));
			}
		}

		private void UserCode_TargetRefreshAmmo(byte[] keys, ushort[] values)
		{
			if (keys.Length == values.Length)
			{
				UserInventory.ReserveAmmo.Clear();
				for (int i = 0; i < keys.Length; i++)
				{
					UserInventory.ReserveAmmo[(ItemType)keys[i]] = values[i];
				}
				global::InventorySystem.Inventory.OnAmmoModified?.Invoke(_hub);
			}
		}

		protected static void InvokeUserCode_TargetRefreshAmmo(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkClient.active)
			{
				global::UnityEngine.Debug.LogError("TargetRPC TargetRefreshAmmo called on server.");
			}
			else
			{
				((global::InventorySystem.Inventory)obj).UserCode_TargetRefreshAmmo(global::Mirror.NetworkReaderExtensions.ReadBytesAndSize(reader), global::Mirror.GeneratedNetworkCode._Read_System_002EUInt16_005B_005D(reader));
			}
		}

		public void UserCode_CmdSelectItem(ushort itemSerial)
		{
			if (!_hub.interCoordinator.AnyBlocker(global::InventorySystem.Items.BlockedInteraction.OpenInventory))
			{
				ServerSelectItem(itemSerial);
			}
		}

		protected static void InvokeUserCode_CmdSelectItem(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogError("Command CmdSelectItem called on client.");
			}
			else
			{
				((global::InventorySystem.Inventory)obj).UserCode_CmdSelectItem(global::Mirror.NetworkReaderExtensions.ReadUInt16(reader));
			}
		}

		public void UserCode_CmdProcessHotkey(ActionName hotkeyButtonPressed, ushort clientsideDesiredItem)
		{
			if (!_hub.interCoordinator.AnyBlocker(global::InventorySystem.Items.BlockedInteraction.OpenInventory) && global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerUseHotkey, _hub, hotkeyButtonPressed))
			{
				ServerSelectItem(clientsideDesiredItem);
			}
		}

		protected static void InvokeUserCode_CmdProcessHotkey(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogError("Command CmdProcessHotkey called on client.");
			}
			else
			{
				((global::InventorySystem.Inventory)obj).UserCode_CmdProcessHotkey(global::Mirror.GeneratedNetworkCode._Read_ActionName(reader), global::Mirror.NetworkReaderExtensions.ReadUInt16(reader));
			}
		}

		private void UserCode_CmdConfirmAcquisition(ushort[] itemSerials)
		{
			foreach (ushort key in itemSerials)
			{
				if (UserInventory.Items.TryGetValue(key, out var value) && value is global::InventorySystem.Items.IAcquisitionConfirmationTrigger acquisitionConfirmationTrigger && !acquisitionConfirmationTrigger.AcquisitionAlreadyReceived)
				{
					acquisitionConfirmationTrigger.ServerConfirmAcqusition();
					acquisitionConfirmationTrigger.AcquisitionAlreadyReceived = true;
				}
			}
		}

		protected static void InvokeUserCode_CmdConfirmAcquisition(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogError("Command CmdConfirmAcquisition called on client.");
			}
			else
			{
				((global::InventorySystem.Inventory)obj).UserCode_CmdConfirmAcquisition(global::Mirror.GeneratedNetworkCode._Read_System_002EUInt16_005B_005D(reader));
			}
		}

		public void UserCode_CmdDropItem(ushort itemSerial, bool tryThrow)
		{
			if (!UserInventory.Items.TryGetValue(itemSerial, out var value) || !global::InventorySystem.Items.EquipDequipModifierExtensions.CanHolster(value) || !global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerDropItem, _hub, value))
			{
				return;
			}
			global::InventorySystem.Items.Pickups.ItemPickupBase itemPickupBase = this.ServerDropItem(itemSerial);
			SendItemsNextFrame = true;
			if (tryThrow && !(itemPickupBase == null) && itemPickupBase.TryGetComponent<global::UnityEngine.Rigidbody>(out var component) && global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerThrowItem, _hub, value, component))
			{
				global::UnityEngine.Vector3 velocity = global::PlayerRoles.FirstPersonControl.FpcExtensionMethods.GetVelocity(_hub);
				global::UnityEngine.Vector3 velocity2 = velocity / 3f + _hub.PlayerCameraReference.forward * 6f * (global::UnityEngine.Mathf.Clamp01(global::UnityEngine.Mathf.InverseLerp(7f, 0.1f, component.mass)) + 0.3f);
				velocity2.x = global::UnityEngine.Mathf.Max(global::UnityEngine.Mathf.Abs(velocity.x), global::UnityEngine.Mathf.Abs(velocity2.x)) * (float)((!(velocity2.x < 0f)) ? 1 : (-1));
				velocity2.y = global::UnityEngine.Mathf.Max(global::UnityEngine.Mathf.Abs(velocity.y), global::UnityEngine.Mathf.Abs(velocity2.y)) * (float)((!(velocity2.y < 0f)) ? 1 : (-1));
				velocity2.z = global::UnityEngine.Mathf.Max(global::UnityEngine.Mathf.Abs(velocity.z), global::UnityEngine.Mathf.Abs(velocity2.z)) * (float)((!(velocity2.z < 0f)) ? 1 : (-1));
				component.position = _hub.PlayerCameraReference.position;
				component.velocity = velocity2;
				component.angularVelocity = global::UnityEngine.Vector3.Lerp(value.ThrowSettings.RandomTorqueA, value.ThrowSettings.RandomTorqueB, global::UnityEngine.Random.value);
				float magnitude = component.angularVelocity.magnitude;
				if (magnitude > component.maxAngularVelocity)
				{
					component.maxAngularVelocity = magnitude;
				}
			}
		}

		protected static void InvokeUserCode_CmdDropItem(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogError("Command CmdDropItem called on client.");
			}
			else
			{
				((global::InventorySystem.Inventory)obj).UserCode_CmdDropItem(global::Mirror.NetworkReaderExtensions.ReadUInt16(reader), global::Mirror.NetworkReaderExtensions.ReadBoolean(reader));
			}
		}

		public void UserCode_CmdDropAmmo(byte ammoType, ushort amount)
		{
			this.ServerDropAmmo((ItemType)ammoType, amount, checkMinimals: true);
		}

		protected static void InvokeUserCode_CmdDropAmmo(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogError("Command CmdDropAmmo called on client.");
			}
			else
			{
				((global::InventorySystem.Inventory)obj).UserCode_CmdDropAmmo(global::Mirror.NetworkReaderExtensions.ReadByte(reader), global::Mirror.NetworkReaderExtensions.ReadUInt16(reader));
			}
		}

		public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
		{
			bool result = base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				global::Mirror.GeneratedNetworkCode._Write_InventorySystem_002EItems_002EItemIdentifier(writer, CurItem);
				global::Mirror.NetworkWriterExtensions.WriteSingle(writer, _syncStaminaModifier);
				global::Mirror.NetworkWriterExtensions.WriteSingle(writer, _syncMovementLimiter);
				global::Mirror.NetworkWriterExtensions.WriteSingle(writer, _syncMovementMultiplier);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1L) != 0L)
			{
				global::Mirror.GeneratedNetworkCode._Write_InventorySystem_002EItems_002EItemIdentifier(writer, CurItem);
				result = true;
			}
			if ((base.syncVarDirtyBits & 2L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteSingle(writer, _syncStaminaModifier);
				result = true;
			}
			if ((base.syncVarDirtyBits & 4L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteSingle(writer, _syncMovementLimiter);
				result = true;
			}
			if ((base.syncVarDirtyBits & 8L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteSingle(writer, _syncMovementMultiplier);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				global::InventorySystem.Items.ItemIdentifier curItem = CurItem;
				NetworkCurItem = global::Mirror.GeneratedNetworkCode._Read_InventorySystem_002EItems_002EItemIdentifier(reader);
				if (!SyncVarEqual(curItem, ref CurItem))
				{
					OnItemUpdated(curItem, CurItem);
				}
				float syncStaminaModifier = _syncStaminaModifier;
				Network_syncStaminaModifier = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
				float syncMovementLimiter = _syncMovementLimiter;
				Network_syncMovementLimiter = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
				float syncMovementMultiplier = _syncMovementMultiplier;
				Network_syncMovementMultiplier = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 1L) != 0L)
			{
				global::InventorySystem.Items.ItemIdentifier curItem2 = CurItem;
				NetworkCurItem = global::Mirror.GeneratedNetworkCode._Read_InventorySystem_002EItems_002EItemIdentifier(reader);
				if (!SyncVarEqual(curItem2, ref CurItem))
				{
					OnItemUpdated(curItem2, CurItem);
				}
			}
			if ((num & 2L) != 0L)
			{
				float syncStaminaModifier2 = _syncStaminaModifier;
				Network_syncStaminaModifier = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
			}
			if ((num & 4L) != 0L)
			{
				float syncMovementLimiter2 = _syncMovementLimiter;
				Network_syncMovementLimiter = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
			}
			if ((num & 8L) != 0L)
			{
				float syncMovementMultiplier2 = _syncMovementMultiplier;
				Network_syncMovementMultiplier = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
			}
		}
	}
}
