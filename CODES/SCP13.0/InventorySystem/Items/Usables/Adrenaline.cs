namespace InventorySystem.Items.Usables
{
	public class Adrenaline : Consumable
	{
		private const float StaminaRegenerationPercent = 100f;

		private const float InvigoratedTargetDuration = 8f;

		private const bool InvigoratedDurationAdditive = true;

		private const float AhpAddition = 40f;

		protected override void OnEffectsActivated()
		{
		}
	}
}
