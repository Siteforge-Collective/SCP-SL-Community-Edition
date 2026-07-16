using UnityEngine;

namespace InventorySystem.Items.ToggleableLights.Flashlight
{
	public class FlashlightViewmodel : StandardAnimatedViemodel
	{
		private static readonly int ToggleHash;

		private Light _light;

		public void PlayAnimation()
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
