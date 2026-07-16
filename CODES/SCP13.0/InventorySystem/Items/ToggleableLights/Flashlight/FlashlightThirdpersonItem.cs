using InventorySystem.Items.Thirdperson;
using PlayerRoles.FirstPersonControl.Thirdperson;
using UnityEngine;

namespace InventorySystem.Items.ToggleableLights.Flashlight
{
	public class FlashlightThirdpersonItem : IdleThirdpersonItem
	{
		public const float MaxAudioDistance = 3.2f;

		[SerializeField]
		private Light _lightSource;

		[SerializeField]
		private Renderer[] _targetRenderers;

		[SerializeField]
		private Material _onMat;

		[SerializeField]
		private Material _offMat;

		private static FlashlightItem Template => null;

		internal override void Initialize(HumanCharacterModel model, ItemIdentifier id)
		{
		}

		private void OnDestroy()
		{
		}

		private void ProcesReceivedStatus(FlashlightNetworkHandler.FlashlightMessage msg)
		{
		}

		private void SetState(bool newState)
		{
		}
	}
}
