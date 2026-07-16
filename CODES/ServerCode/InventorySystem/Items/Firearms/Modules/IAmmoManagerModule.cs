namespace InventorySystem.Items.Firearms.Modules
{
	public interface IAmmoManagerModule : global::InventorySystem.Items.Firearms.Modules.IFirearmModuleBase
	{
		byte MaxAmmo { get; }

		bool ServerTryReload();

		bool ServerTryUnload();
	}
}
