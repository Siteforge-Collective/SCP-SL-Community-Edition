using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace MapGeneration.Distributors
{
    public class ItemSpawnpoint : DistributorSpawnpointBase
    {
        public static readonly HashSet<ItemSpawnpoint> AutospawnInstances = new HashSet<ItemSpawnpoint>();

        public static readonly HashSet<ItemSpawnpoint> RandomInstances = new HashSet<ItemSpawnpoint>();

        public string TriggerDoorName;

        public ItemType AutospawnItem = ItemType.None;

        [Min(1f)]
        [FormerlySerializedAs("_maxUses")]
        public int MaxUses;

        [SerializeField]
        private ItemType[] _acceptedItems;

        [SerializeField]
        private Transform[] _positionVariants;

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
            if (_uses >= MaxUses)
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

        public Transform Occupy()
        {
            _uses++;
            return _positionVariants[Random.Range(0, _positionVariants.Length)];
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
