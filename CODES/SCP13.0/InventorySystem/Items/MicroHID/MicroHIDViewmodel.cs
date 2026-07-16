using InventorySystem.Items.SwayControllers;
using UnityEngine;

namespace InventorySystem.Items.MicroHID
{
	public class MicroHIDViewmodel : AnimatedViewmodelBase
	{
		[SerializeField]
		private Transform _swayPivot;

		[SerializeField]
		private Transform _energyGauge;

		[SerializeField]
		private float _viewmodelFov;

		[SerializeField]
		private float _firstTimeInspectMaxTime;

		private GoopSway _goopSway;

		private float _pickupTime;

		private const float MaxSkipTime = 11f;

		private static readonly int StateHash;

		private static readonly int FirstTimePickupHash;

		public override IItemSwayController SwayController => null;

		public override float ViewmodelCameraFOV => 0f;

		public HidState State => default(HidState);

		public float Energy => 0f;

		public override void InitLocal(ItemBase parent)
		{
		}

		public override void InitAny()
		{
		}

		public override void InitSpectator(ReferenceHub ply, ItemIdentifier id, bool wasEquipped)
		{
		}

		internal override void OnEquipped()
		{
		}

		private void Update()
		{
		}

		public static void LerpGauge(Transform gauge, float energy, float l)
		{
		}
	}
}
