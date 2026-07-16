namespace Scp914.Processors
{
	public abstract class Scp914ItemProcessor : global::UnityEngine.MonoBehaviour
	{
		public abstract global::InventorySystem.Items.ItemBase OnInventoryItemUpgraded(global::Scp914.Scp914KnobSetting setting, ReferenceHub hub, ushort serial);

		public abstract global::InventorySystem.Items.Pickups.ItemPickupBase OnPickupUpgraded(global::Scp914.Scp914KnobSetting setting, global::InventorySystem.Items.Pickups.ItemPickupBase ipb, global::UnityEngine.Vector3 newPosition);
	}
}
