namespace InventorySystem.Items.ToggleableLights.Lantern
{
	public class LanternViewmodel : StandardAnimatedViemodel
	{
		public LanternLightManager LanternLightManager;

		public void SetLightStatus(bool lightEnabled)
		{
		}

		public override void InitSpectator(ReferenceHub ply, ItemIdentifier id, bool wasEquipped)
		{
		}

		private void OnStatusReceived(FlashlightNetworkHandler.FlashlightMessage msg)
		{
		}

		private void OnDestroy()
		{
		}
	}
}
