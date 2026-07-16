namespace MapGeneration
{
	public class InitiallySpawnedItems : global::UnityEngine.MonoBehaviour
	{
		private global::System.Collections.Generic.HashSet<ushort> _initiallySpawnedItemSerials = new global::System.Collections.Generic.HashSet<ushort>();

		public static global::MapGeneration.InitiallySpawnedItems Singleton;

		private void Awake()
		{
			Singleton = this;
		}

		public bool IsInitiallySpawned(ushort _itemSerial)
		{
			return _initiallySpawnedItemSerials.Contains(_itemSerial);
		}

		public bool IsInitiallySpawned(global::InventorySystem.Items.ItemIdentifier _item)
		{
			return IsInitiallySpawned(_item.SerialNumber);
		}

		public void AddInitial(ushort _itemSerial)
		{
			_initiallySpawnedItemSerials.Add(_itemSerial);
		}

		public void AddInitial(global::InventorySystem.Items.ItemIdentifier _item)
		{
			AddInitial(_item.SerialNumber);
		}

		public void RemoveInitial(ushort _itemSerial)
		{
			_initiallySpawnedItemSerials.Remove(_itemSerial);
		}

		public void RemoveInitial(global::InventorySystem.Items.ItemIdentifier _item)
		{
			RemoveInitial(_item.SerialNumber);
		}

		public void ClearAll()
		{
			_initiallySpawnedItemSerials.Clear();
		}
	}
}
