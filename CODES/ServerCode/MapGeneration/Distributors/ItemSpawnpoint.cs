namespace MapGeneration.Distributors
{
	public class ItemSpawnpoint : global::MapGeneration.Distributors.DistributorSpawnpointBase
	{
		public static readonly global::System.Collections.Generic.HashSet<global::MapGeneration.Distributors.ItemSpawnpoint> AutospawnInstances = new global::System.Collections.Generic.HashSet<global::MapGeneration.Distributors.ItemSpawnpoint>();

		public static readonly global::System.Collections.Generic.HashSet<global::MapGeneration.Distributors.ItemSpawnpoint> RandomInstances = new global::System.Collections.Generic.HashSet<global::MapGeneration.Distributors.ItemSpawnpoint>();

		public string TriggerDoorName;

		public ItemType AutospawnItem = ItemType.None;

		[global::UnityEngine.SerializeField]
		private int _maxUses;

		[global::UnityEngine.SerializeField]
		private ItemType[] _acceptedItems;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Transform[] _positionVariants;

		private int _uses;

		public bool CanSpawn(ItemType[] items)
		{
			foreach (ItemType itemType in items)
			{
				if (itemType != ItemType.None && !CanSpawn(itemType))
				{
					return false;
				}
			}
			return true;
		}

		public bool CanSpawn(ItemType targetItem)
		{
			if (_uses >= _maxUses)
			{
				return false;
			}
			ItemType[] acceptedItems = _acceptedItems;
			for (int i = 0; i < acceptedItems.Length; i++)
			{
				if (acceptedItems[i] == targetItem)
				{
					return true;
				}
			}
			return false;
		}

		public global::UnityEngine.Transform Occupy()
		{
			_uses++;
			return _positionVariants[global::UnityEngine.Random.Range(0, _positionVariants.Length)];
		}

		private void Start()
		{
			if (AutospawnItem == ItemType.None)
			{
				RandomInstances.Add(this);
			}
			else
			{
				AutospawnInstances.Add(this);
			}
		}

		private void OnDestroy()
		{
			if (AutospawnItem == ItemType.None)
			{
				RandomInstances.Remove(this);
			}
			else
			{
				AutospawnInstances.Remove(this);
			}
		}
	}
}
