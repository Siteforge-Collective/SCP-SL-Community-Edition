namespace CustomPlayerEffects
{
	public interface IWeaponModifierPlayerEffect
	{
		bool ParamsActive { get; }

		bool TryGetWeaponParam(global::InventorySystem.Items.Firearms.Attachments.AttachmentParam param, out float val);
	}
}
