using InventorySystem.Items;
using InventorySystem.Items.Pickups;
using Mirror;
using NorthwoodLib.Pools;
using PlayerRoles.FirstPersonControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace InventorySystem
{
	public class Inventory : NetworkBehaviour, IStaminaModifier, IMovementSpeedModifier
	{
		public const int MaxSlots = 8;

		public InventoryInfo UserInventory = new();

        [SyncVar(hook = nameof(OnItemUpdated))]
        public ItemIdentifier CurItem = ItemIdentifier.None;

        public bool SendItemsNextFrame;

		public bool SendAmmoNextFrame;

		private ItemIdentifier _prevCurItem;

		internal ReferenceHub _hub;

		private ItemBase _curInstance;

		private float _staminaModifier;

		private float _movementLimiter;

		private float _movementMultiplier;

		private bool _sprintingDisabled;

        private readonly Stopwatch _lastEquipSw = Stopwatch.StartNew();

        [SyncVar]
		private float _syncStaminaModifier;

		[SyncVar]
		private float _syncMovementLimiter;

		[SyncVar]
		private float _syncMovementMultiplier;

        [HideInInspector]
        public ItemBase CurInstance
        {
            get
            {
                return _curInstance;
            }
            set
            {
                if (!(value == _curInstance))
                {
                    ItemBase curInstance = _curInstance;
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

        private Transform ItemWorkspace => SharedHandsController.Singleton.transform;

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
                if (!NetworkServer.active)
                {
                    return !base.isLocalPlayer;
                }
                return false;
            }
        }

        private bool HasViewmodel => false;


        public static event Action<ReferenceHub> OnItemsModified;

        public static event Action<ReferenceHub> OnAmmoModified;

        public static event Action OnServerStarted;

        public static event Action OnLocalClientStarted;

        public static event Action<ReferenceHub, ItemIdentifier, ItemIdentifier> OnCurrentItemChanged;

        private void OnItemUpdated(ItemIdentifier prev, ItemIdentifier cur)
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

        public override void OnStartLocalPlayer()
        {
            Inventory.OnLocalClientStarted?.Invoke();

            if (isServer)
            {
                Inventory.OnServerStarted();
                CustomNetworkManager.InvokeOnClientReady();
            }
        }

        private void Update()
        {
            if (NetworkServer.active)
            {
                if (SendItemsNextFrame)
                {
                    SendItemsNextFrame = false;
                    Inventory.OnItemsModified?.Invoke(_hub);
                    ServerSendItems();
                }
                if (SendAmmoNextFrame)
                {
                    SendAmmoNextFrame = false;
                    Inventory.OnAmmoModified?.Invoke(_hub);
                    ServerSendAmmo();
                }
            }
            if (_prevCurItem != CurItem)
            {
                Inventory.OnCurrentItemChanged?.Invoke(_hub, _prevCurItem, CurItem);
                _prevCurItem = new ItemIdentifier(CurItem.TypeId, CurItem.SerialNumber);
            }
            if (IsObserver)
            {
                return;
            }
            if (CurInstance != null && CurInstance.enabled)
            {
                CurInstance.EquipUpdate();
            }
            foreach (ItemBase value in UserInventory.Items.Values)
            {
                if (value.enabled)
                {
                    value.AlwaysUpdate();
                }
            }
            RefreshModifiers();
        }

        private void RefreshModifiers()
        {
            _staminaModifier = 1f;
            _movementLimiter = float.MaxValue;
            _movementMultiplier = 1f;
            _sprintingDisabled = false;
            foreach (KeyValuePair<ushort, ItemBase> item in UserInventory.Items)
            {
                if (item.Value is IStaminaModifier { StaminaModifierActive: not false } staminaModifier)
                {
                    _staminaModifier *= staminaModifier.StaminaUsageMultiplier;
                    _sprintingDisabled |= staminaModifier.SprintingDisabled;
                }
                if (item.Value is IMovementSpeedModifier { MovementModifierActive: not false } movementSpeedModifier)
                {
                    _movementLimiter = Mathf.Min(_movementLimiter, movementSpeedModifier.MovementSpeedLimit);
                    _movementMultiplier *= movementSpeedModifier.MovementSpeedMultiplier;
                }
            }
            if (NetworkServer.active)
            {
                _syncStaminaModifier = _staminaModifier;
                _syncMovementMultiplier = _movementMultiplier;
                _syncMovementLimiter = _movementLimiter;
            }
        }

        [Server]
        public void ServerSelectItem(ushort itemSerial)
        {
            if (!NetworkServer.active)
            {
                UnityEngine.Debug.LogWarning("[Server] function 'System.Void InventorySystem.Inventory::ServerSelectItem(System.UInt16)' called when server was not active");
            }
            else
            {
                if (itemSerial == CurItem.SerialNumber)
                {
                    return;
                }
                ItemBase value = null;
                ItemBase value2 = null;
                bool flag = CurItem.SerialNumber == 0 || (UserInventory.Items.TryGetValue(CurItem.SerialNumber, out value) && CurInstance != null);
                if (itemSerial == 0 || UserInventory.Items.TryGetValue(itemSerial, out value2))
                {
                    if ((CurItem.SerialNumber != 0 && flag && !value.CanHolster()) || (itemSerial != 0 && !value2.CanEquip()))
                    {
                        return;
                    }
                    if (itemSerial == 0)
                    {
                        CurItem = ItemIdentifier.None;
                        if (!base.isLocalPlayer)
                        {
                            CurInstance = null;
                        }
                    }
                    else
                    {
                        CurItem = new ItemIdentifier(value2.ItemTypeId, itemSerial);
                        if (!base.isLocalPlayer)
                        {
                            CurInstance = value2;
                        }
                    }
                }
                else if (!flag)
                {
                    CurItem = ItemIdentifier.None;
                    if (!base.isLocalPlayer)
                    {
                        CurInstance = null;
                    }
                }
            }
        }

        [Server]
        private void ServerSendItems()
        {
            if (!NetworkServer.active)
            {
                UnityEngine.Debug.LogWarning("[Server] function 'System.Void InventorySystem.Inventory::ServerSendItems()' called when server was not active");
            }
            else
            {
                if (base.isLocalPlayer)
                {
                    return;
                }
                HashSet<ItemIdentifier> hashSet = HashSetPool<ItemIdentifier>.Shared.Rent();
                foreach (KeyValuePair<ushort, ItemBase> item in UserInventory.Items)
                {
                    hashSet.Add(new ItemIdentifier(item.Value.ItemTypeId, item.Key));
                }
                TargetRefreshItems(hashSet.ToArray());
                HashSetPool<ItemIdentifier>.Shared.Return(hashSet);
            }
        }

        [Server]
        private void ServerSendAmmo()
        {
            if (!NetworkServer.active)
            {
                UnityEngine.Debug.LogWarning("[Server] function 'System.Void InventorySystem.Inventory::ServerSendAmmo()' called when server was not active");
            }
            else
            {
                if (base.isLocalPlayer)
                {
                    return;
                }
                List<byte> list = ListPool<byte>.Shared.Rent();
                List<ushort> list2 = ListPool<ushort>.Shared.Rent();
                foreach (KeyValuePair<ItemType, ushort> item in UserInventory.ReserveAmmo)
                {
                    list.Add((byte)item.Key);
                    list2.Add(item.Value);
                }
                TargetRefreshAmmo(list.ToArray(), list2.ToArray());
                ListPool<byte>.Shared.Return(list);
                ListPool<ushort>.Shared.Return(list2);
            }
        }

        [TargetRpc]
        private void TargetRefreshItems(ItemIdentifier[] ids)
        {
            Queue<ItemIdentifier> queue = new Queue<ItemIdentifier>();
            List<ushort> list = UserInventory.Items.Keys.ToList();
            int num = 0;
            for (int i = 0; i < ids.Length; i++)
            {
                ItemIdentifier item = ids[i];
                if (!UserInventory.Items.Keys.Contains(item.SerialNumber))
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
            List<ushort> list2 = ListPool<ushort>.Shared.Rent();
            while (queue.Count > 0)
            {
                ItemIdentifier itemIdentifier = queue.Dequeue();
                ItemBase itemBase = CreateItemInstance(itemIdentifier, updateViewmodel: true);
                UserInventory.Items[itemIdentifier.SerialNumber] = itemBase;
                itemBase.OnAdded(null);
                if (itemBase is IAcquisitionConfirmationTrigger)
                {
                    list2.Add(itemIdentifier.SerialNumber);
                }
                if (itemIdentifier == CurItem)
                {
                    CurInstance = itemBase;
                }
                num++;
            }
            if (list2.Count > 0)
            {
                CmdConfirmAcquisition(list2.ToArray());
            }
            ListPool<ushort>.Shared.Return(list2);
        }

        [TargetRpc]
        private void TargetRefreshAmmo(byte[] keys, ushort[] values)
        {
            if (keys.Length == values.Length)
            {
                UserInventory.ReserveAmmo.Clear();
                for (int i = 0; i < keys.Length; i++)
                {
                    UserInventory.ReserveAmmo[(ItemType)keys[i]] = values[i];
                }
                Inventory.OnAmmoModified?.Invoke(_hub);
            }
        }

        [Command]
        public void CmdSelectItem(ushort itemSerial)
        {
            if (!_hub.interCoordinator.AnyBlocker(BlockedInteraction.OpenInventory))
            {
                ServerSelectItem(itemSerial);
            }
        }

        [Command(channel = 4)]
        private void CmdConfirmAcquisition(ushort[] itemSerials)
        {
            foreach (ushort key in itemSerials)
            {
                if (UserInventory.Items.TryGetValue(key, out var value) && value is IAcquisitionConfirmationTrigger { AcquisitionAlreadyReceived: false } acquisitionConfirmationTrigger)
                {
                    acquisitionConfirmationTrigger.ServerConfirmAcqusition();
                    acquisitionConfirmationTrigger.AcquisitionAlreadyReceived = true;
                }
            }
        }

        [Command(channel = 4)]
        public void CmdDropItem(ushort itemSerial, bool tryThrow)
        {
            if (!UserInventory.Items.TryGetValue(itemSerial, out var value) || !value.CanHolster())
            {
                return;
            }
            ItemPickupBase itemPickupBase = this.ServerDropItem(itemSerial);
            SendItemsNextFrame = true;
            if (tryThrow && !(itemPickupBase == null) && itemPickupBase.TryGetComponent<Rigidbody>(out var component))
            {
                Vector3 velocity = _hub.GetVelocity();
                Vector3 velocity2 = velocity / 3f + _hub.PlayerCameraReference.forward * 6f * (Mathf.Clamp01(Mathf.InverseLerp(7f, 0.1f, component.mass)) + 0.3f);
                velocity2.x = Mathf.Max(Mathf.Abs(velocity.x), Mathf.Abs(velocity2.x)) * (float)((!(velocity2.x < 0f)) ? 1 : (-1));
                velocity2.y = Mathf.Max(Mathf.Abs(velocity.y), Mathf.Abs(velocity2.y)) * (float)((!(velocity2.y < 0f)) ? 1 : (-1));
                velocity2.z = Mathf.Max(Mathf.Abs(velocity.z), Mathf.Abs(velocity2.z)) * (float)((!(velocity2.z < 0f)) ? 1 : (-1));
                component.position = _hub.PlayerCameraReference.position;
                component.velocity = velocity2;
                component.angularVelocity = Vector3.Lerp(value.ThrowSettings.RandomTorqueA, value.ThrowSettings.RandomTorqueB, UnityEngine.Random.value);
                float magnitude = component.angularVelocity.magnitude;
                if (magnitude > component.maxAngularVelocity)
                {
                    component.maxAngularVelocity = magnitude;
                }
            }
        }

        [Command(channel = 4)]
        public void CmdDropAmmo(byte ammoType, ushort amount)
        {
            this.ServerDropAmmo((ItemType)ammoType, amount, checkMinimals: true);
        }

        public ItemBase CreateItemInstance(ItemIdentifier identifier, bool updateViewmodel)
        {
            if (!InventoryItemLoader.AvailableItems.TryGetValue(identifier.TypeId, out var value))
            {
                return null;
            }
            ItemBase itemBase = UnityEngine.Object.Instantiate(value, ItemWorkspace);
            itemBase.transform.localPosition = Vector3.zero;
            itemBase.transform.localRotation = Quaternion.identity;
            itemBase.Owner = _hub;
            itemBase.ItemSerial = identifier.SerialNumber;
            return itemBase;
        }

        public bool DestroyItemInstance(ushort targetInstance, ItemPickupBase pickup, out ItemBase foundItem)
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
            UnityEngine.Object.Destroy(foundItem.gameObject);
            return true;
        }
    }
}
