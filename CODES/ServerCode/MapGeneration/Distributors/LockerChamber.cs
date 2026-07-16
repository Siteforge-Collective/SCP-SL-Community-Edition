namespace MapGeneration.Distributors
{
	public class LockerChamber : global::UnityEngine.MonoBehaviour
	{
		public ItemType[] AcceptableItems;

		public bool IsOpen;

		public global::Interactables.Interobjects.DoorUtils.KeycardPermissions RequiredPermissions;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Animator _animator;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Transform _spawnpoint;

		[global::UnityEngine.SerializeField]
		private bool _spawnOnFirstChamberOpening;

		private static readonly int DoorHash = global::UnityEngine.Animator.StringToHash("isOpen");

		private static readonly int DeniedHash = global::UnityEngine.Animator.StringToHash("accessDenied");

		private static readonly int GrantedHash = global::UnityEngine.Animator.StringToHash("accessGranted");

		private const float MinimalTimeSinceMapGeneration = 5f;

		private readonly global::System.Diagnostics.Stopwatch _stopwatch = new global::System.Diagnostics.Stopwatch();

		private readonly global::System.Collections.Generic.HashSet<global::InventorySystem.Items.Pickups.ItemPickupBase> _content = new global::System.Collections.Generic.HashSet<global::InventorySystem.Items.Pickups.ItemPickupBase>();

		private readonly global::System.Collections.Generic.HashSet<global::InventorySystem.Items.Pickups.ItemPickupBase> _toBeSpawned = new global::System.Collections.Generic.HashSet<global::InventorySystem.Items.Pickups.ItemPickupBase>();

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
			if (id == ItemType.None || !global::InventorySystem.InventoryItemLoader.AvailableItems.TryGetValue(id, out var value))
			{
				return;
			}
			for (int i = 0; i < amount; i++)
			{
				global::InventorySystem.Items.Pickups.ItemPickupBase itemPickupBase = global::UnityEngine.Object.Instantiate(value.PickupDropModel, _spawnpoint.position, _spawnpoint.rotation);
				itemPickupBase.transform.SetParent(_spawnpoint);
				itemPickupBase.Info.ItemId = id;
				itemPickupBase.Info.Weight = value.Weight;
				itemPickupBase.Info.Locked = true;
				_content.Add(itemPickupBase);
				(itemPickupBase as global::InventorySystem.Items.Pickups.IPickupDistributorTrigger)?.OnDistributed();
				if (itemPickupBase.TryGetComponent<global::UnityEngine.Rigidbody>(out var component))
				{
					component.isKinematic = true;
					component.transform.localPosition = global::UnityEngine.Vector3.zero;
					component.transform.localRotation = global::UnityEngine.Quaternion.identity;
					global::MapGeneration.Distributors.SpawnablesDistributorBase.BodiesToUnfreeze.Add(component);
				}
				if (_spawnOnFirstChamberOpening)
				{
					_toBeSpawned.Add(itemPickupBase);
				}
				else
				{
					global::MapGeneration.Distributors.ItemDistributor.SpawnPickup(itemPickupBase);
				}
			}
		}

		public void SetDoor(bool doorStatus, global::UnityEngine.AudioClip beepClip)
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
			if (!global::Mirror.NetworkServer.active || !doorStatus || _wasEverOpened)
			{
				return;
			}
			_wasEverOpened = true;
			foreach (global::InventorySystem.Items.Pickups.ItemPickupBase item in _content)
			{
				if (!(item == null))
				{
					global::InventorySystem.Items.Pickups.PickupSyncInfo info = item.Info;
					info.Locked = false;
					item.NetworkInfo = info;
				}
			}
			if (!_spawnOnFirstChamberOpening)
			{
				return;
			}
			foreach (global::InventorySystem.Items.Pickups.ItemPickupBase item2 in _toBeSpawned)
			{
				if (!(item2 == null))
				{
					global::MapGeneration.Distributors.ItemDistributor.SpawnPickup(item2);
				}
			}
		}

		public void PlayDenied(global::UnityEngine.AudioClip deniedClip)
		{
			_targetCooldown = 0.4f;
			_stopwatch.Restart();
		}
	}
}
