using InventorySystem.Items.Pickups;
using UnityEngine;

namespace InventorySystem.Items.ToggleableLights.Flashlight
{
	public class FlashlightItem : ToggleableLightItemBase
	{
		private Light _lightSource;

		private static FlashlightItem _cachedFlashlight;

		private static bool _cacheSet;

		public override float Weight => 0f;

		public static FlashlightItem Template => null;

		protected override void SetLightSourceStatus(bool value)
		{
		}

		public override void OnAdded(ItemPickupBase pickup)
		{
		}

		protected override void OnToggled()
		{
		}
	}
}
