namespace MapGeneration.Distributors
{
	public abstract class SpawnablesDistributorBase : global::UnityEngine.MonoBehaviour
	{
		public static readonly global::System.Collections.Generic.HashSet<global::UnityEngine.Rigidbody> BodiesToUnfreeze = new global::System.Collections.Generic.HashSet<global::UnityEngine.Rigidbody>();

		private readonly global::System.Collections.Generic.Dictionary<global::Interactables.Interobjects.DoorUtils.DoorVariant, global::System.Collections.Generic.HashSet<global::UnityEngine.GameObject>> _unspawnedObjects = new global::System.Collections.Generic.Dictionary<global::Interactables.Interobjects.DoorUtils.DoorVariant, global::System.Collections.Generic.HashSet<global::UnityEngine.GameObject>>();

		[global::UnityEngine.SerializeField]
		protected global::MapGeneration.Distributors.SpawnablesDistributorSettings Settings;

		private bool _eventsRegistered;

		private bool _placed;

		private bool _unfrozen;

		private readonly global::System.Diagnostics.Stopwatch _stopwatch = new global::System.Diagnostics.Stopwatch();

		private void Start()
		{
			if (global::Mirror.NetworkServer.active)
			{
				_eventsRegistered = true;
				global::MapGeneration.SeedSynchronizer.OnMapGenerated += _stopwatch.Restart;
				global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera.OnAnyCameraStateChanged += On079CamChanged;
				global::Interactables.Interobjects.DoorUtils.DoorEvents.OnDoorAction += OnDoorAction;
				Register();
			}
		}

		private void OnDestroy()
		{
			if (_eventsRegistered)
			{
				global::MapGeneration.SeedSynchronizer.OnMapGenerated -= _stopwatch.Restart;
				global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera.OnAnyCameraStateChanged -= On079CamChanged;
				global::Interactables.Interobjects.DoorUtils.DoorEvents.OnDoorAction -= OnDoorAction;
				Unregister();
			}
		}

		private void Update()
		{
			if (!global::Mirror.NetworkServer.active || !_stopwatch.IsRunning)
			{
				return;
			}
			float num = (float)_stopwatch.Elapsed.TotalSeconds;
			if (!_placed && num > Settings.SpawnerDelay)
			{
				PlaceSpawnables();
				_placed = true;
			}
			else if (!_unfrozen && num > Settings.UnfreezeDelay)
			{
				foreach (global::UnityEngine.Rigidbody item in BodiesToUnfreeze)
				{
					if (item != null)
					{
						item.isKinematic = false;
					}
				}
				BodiesToUnfreeze.Clear();
				_unfrozen = true;
			}
			if (_placed && _unfrozen)
			{
				_stopwatch.Stop();
			}
		}

		private void OnDoorAction(global::Interactables.Interobjects.DoorUtils.DoorVariant door, global::Interactables.Interobjects.DoorUtils.DoorAction action, ReferenceHub hub)
		{
			if (action == global::Interactables.Interobjects.DoorUtils.DoorAction.Opened || action == global::Interactables.Interobjects.DoorUtils.DoorAction.Destroyed)
			{
				SpawnForDoor(door);
			}
		}

		private void On079CamChanged(global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera cam)
		{
			if (!cam.IsActive || !global::Interactables.Interobjects.DoorUtils.DoorVariant.DoorsByRoom.TryGetValue(cam.Room, out var value))
			{
				return;
			}
			foreach (global::Interactables.Interobjects.DoorUtils.DoorVariant item in value)
			{
				SpawnForDoor(item);
			}
		}

		protected abstract void PlaceSpawnables();

		protected virtual void Register()
		{
		}

		protected virtual void Unregister()
		{
		}

		protected void RegisterUnspawnedObject(global::Interactables.Interobjects.DoorUtils.DoorVariant door, global::UnityEngine.GameObject unspawnedObject)
		{
			if (_unspawnedObjects.TryGetValue(door, out var value) && value != null)
			{
				value.Add(unspawnedObject);
				return;
			}
			_unspawnedObjects[door] = new global::System.Collections.Generic.HashSet<global::UnityEngine.GameObject> { unspawnedObject };
		}

		protected virtual void SpawnObject(global::UnityEngine.GameObject objectToSpawn)
		{
			if (objectToSpawn != null)
			{
				global::Mirror.NetworkServer.Spawn(objectToSpawn);
			}
		}

		public void SpawnForDoor(global::Interactables.Interobjects.DoorUtils.DoorVariant door)
		{
			if (!_unspawnedObjects.TryGetValue(door, out var value) || value == null)
			{
				return;
			}
			foreach (global::UnityEngine.GameObject item in value)
			{
				SpawnObject(item);
			}
			_unspawnedObjects.Remove(door);
		}
	}
}
