using System.Collections.Generic;
using System.Diagnostics;
using Interactables.Interobjects.DoorUtils;
using InventorySystem;
using InventorySystem.Items.Pickups;
using Mirror;
using UnityEngine;

namespace MapGeneration.Distributors
{
    public class LockerChamber : MonoBehaviour
    {
        public ItemType[] AcceptableItems;

        public bool IsOpen;

        public KeycardPermissions RequiredPermissions;

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private Transform _spawnpoint;

        [SerializeField]
        private bool _useMultipleSpawnpoints;

        [SerializeField]
        private Transform[] _spawnpoints;

        [SerializeField]
        private bool _spawnOnFirstChamberOpening;

        private static readonly int DoorHash = Animator.StringToHash("isOpen");

        private static readonly int DeniedHash = Animator.StringToHash("accessDenied");

        private static readonly int GrantedHash = Animator.StringToHash("accessGranted");

        private const float MinimalTimeSinceMapGeneration = 5f;

        private readonly Stopwatch _stopwatch = new Stopwatch();

        private readonly HashSet<ItemPickupBase> _content = new HashSet<ItemPickupBase>();

        private readonly HashSet<ItemPickupBase> _toBeSpawned = new HashSet<ItemPickupBase>();

        private byte _animatorStatusCode;

        private bool _prevDoor;

        private bool _wasEverOpened;

        private float _targetCooldown;

        public bool CanInteract
        {
            get
            {
                if (!AnimatorSet)
                {
                    return false;
                }
                if (!_stopwatch.IsRunning)
                {
                    return true;
                }
                if (_stopwatch.Elapsed.TotalSeconds >= (double)_targetCooldown)
                {
                    _stopwatch.Stop();
                    return true;
                }
                return false;
            }
        }

        public bool AnimatorSet
        {
            get
            {
                if (_animatorStatusCode == 0)
                {
                    _animatorStatusCode = (byte)((_animator == null) ? 1u : 2u);
                }
                return _animatorStatusCode == 2;
            }
        }

        public virtual void SpawnItem(ItemType id, int amount)
        {
            if (id == ItemType.None || !InventoryItemLoader.AvailableItems.TryGetValue(id, out var value))
            {
                return;
            }
            int num = 0;
            for (int i = 0; i < amount; i++)
            {
                Transform transform = _spawnpoint;
                if (_useMultipleSpawnpoints && _spawnpoints.Length != 0)
                {
                    if (num > _spawnpoints.Length)
                    {
                        num = 0;
                    }
                    transform = _spawnpoints[num];
                    num++;
                }
                ItemPickupBase itemPickupBase = Object.Instantiate(value.PickupDropModel, transform.position, transform.rotation);
                itemPickupBase.transform.SetParent(transform);
                itemPickupBase.Info.ItemId = id;
                itemPickupBase.Info.WeightKg = value.Weight;
                itemPickupBase.Info.Locked = true;
                _content.Add(itemPickupBase);
                (itemPickupBase as IPickupDistributorTrigger)?.OnDistributed();
                if (itemPickupBase.TryGetComponent<Rigidbody>(out var component))
                {
                    component.isKinematic = true;
                    component.transform.localPosition = Vector3.zero;
                    component.transform.localRotation = Quaternion.identity;
                    SpawnablesDistributorBase.BodiesToUnfreeze.Add(component);
                }
                if (_spawnOnFirstChamberOpening)
                {
                    _toBeSpawned.Add(itemPickupBase);
                }
                else
                {
                    ItemDistributor.SpawnPickup(itemPickupBase);
                }
            }
        }

        public void SetDoor(bool doorStatus, AudioClip beepClip)
        {
            if (doorStatus == _prevDoor)
            {
                return;
            }
            IsOpen = doorStatus;
            _prevDoor = doorStatus;
            if (AnimatorSet)
            {
                _animator.SetBool(DoorHash, doorStatus);
                _targetCooldown = 1f;
                _stopwatch.Restart();
            }
            if (!NetworkServer.active || !doorStatus || _wasEverOpened)
            {
                return;
            }
            _wasEverOpened = true;
            foreach (ItemPickupBase item in _content)
            {
                if (!(item == null))
                {
                    PickupSyncInfo info = item.Info;
                    info.Locked = false;
                    item.Info = info;
                }
            }
            if (!_spawnOnFirstChamberOpening)
            {
                return;
            }
            foreach (ItemPickupBase item2 in _toBeSpawned)
            {
                if (!(item2 == null))
                {
                    ItemDistributor.SpawnPickup(item2);
                }
            }
        }

        public void PlayDenied(AudioClip deniedClip)
        {
            _targetCooldown = 0.4f;
            _stopwatch.Restart();
        }
    }
}
