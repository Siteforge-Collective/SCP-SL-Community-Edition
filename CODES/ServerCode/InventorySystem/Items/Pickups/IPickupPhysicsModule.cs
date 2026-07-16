namespace InventorySystem.Items.Pickups
{
	public interface IPickupPhysicsModule
	{
		void DestroyModule();

		void UpdatePhysics();

		void UpdateInfo(global::InventorySystem.Items.Pickups.PickupSyncInfo psi);
	}
}
