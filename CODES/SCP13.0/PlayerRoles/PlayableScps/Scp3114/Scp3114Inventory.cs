using GameObjectPools;
using InventorySystem.Items;
using PlayerRoles.Subroutines;

namespace PlayerRoles.PlayableScps.Scp3114
{
	public class Scp3114Inventory : SubroutineBase, IInteractionBlocker, IPoolSpawnable
	{
		private Scp3114Role _scpRole;

		private const BlockedInteraction DisguiseBlockers = BlockedInteraction.ItemPrimaryAction;

		private const BlockedInteraction SkeletonBlockers = BlockedInteraction.OpenInventory | BlockedInteraction.GrabItems;

		public BlockedInteraction BlockedInteractions => default(BlockedInteraction);

		public bool CanBeCleared => false;

		protected override void Awake()
		{
		}

		private void OnStatusChanged()
		{
		}

		public void SpawnObject()
		{
		}
	}
}
