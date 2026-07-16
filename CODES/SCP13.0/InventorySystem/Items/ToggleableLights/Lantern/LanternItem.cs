using InventorySystem.Items.Pickups;

namespace InventorySystem.Items.ToggleableLights.Lantern
{
	public class LanternItem : ToggleableLightItemBase
	{
		private const float LanternCooldownTime = 0.25f;

		private LanternViewmodel _lanternViewmodel;

		public override float Weight => 0f;

		protected override void OnToggled()
		{
		}

		protected override void SetLightSourceStatus(bool value)
		{
		}

		public override void OnAdded(ItemPickupBase pickup)
		{
		}
	}
}
