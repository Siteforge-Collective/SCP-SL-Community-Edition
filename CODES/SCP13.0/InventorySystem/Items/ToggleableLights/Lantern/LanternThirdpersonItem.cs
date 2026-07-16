using InventorySystem.Items.Thirdperson;
using PlayerRoles.FirstPersonControl.Thirdperson;
using UnityEngine;

namespace InventorySystem.Items.ToggleableLights.Lantern
{
	public class LanternThirdpersonItem : IdleThirdpersonItem
	{
		private static LanternItem _cachedFlashlight;

		private static bool _cacheSet;

		[SerializeField]
		private Light _lightSource;

		[SerializeField]
		private ParticleSystem _particleSystem;

		private static LanternItem Template => null;

		internal override void Initialize(HumanCharacterModel model, ItemIdentifier id)
		{
		}

		private void OnDestroy()
		{
		}

		private void ProcessReceivedStatus(FlashlightNetworkHandler.FlashlightMessage msg)
		{
		}

		private void SetState(bool newState)
		{
		}
	}
}
