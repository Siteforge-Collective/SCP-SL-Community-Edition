namespace InventorySystem.Items.Firearms.Modules
{
	public interface IActionModule : global::InventorySystem.Items.Firearms.Modules.IFirearmModuleBase
	{
		float CyclicRate { get; }

		bool IsTriggerHeld { get; }

		global::InventorySystem.Items.Firearms.FirearmStatus PredictedStatus { get; }

		global::InventorySystem.Items.Firearms.Modules.ActionModuleResponse DoClientsideAction(bool isTriggerPressed);

		bool ServerAuthorizeShot();

		bool ServerAuthorizeDryFire();
	}
}
