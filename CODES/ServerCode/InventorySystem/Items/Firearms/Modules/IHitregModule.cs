namespace InventorySystem.Items.Firearms.Modules
{
	public interface IHitregModule : global::InventorySystem.Items.Firearms.Modules.IFirearmModuleBase
	{
		bool ClientCalculateHit(out global::InventorySystem.Items.Firearms.BasicMessages.ShotMessage message);

		void ServerProcessShot(global::InventorySystem.Items.Firearms.BasicMessages.ShotMessage message);
	}
}
