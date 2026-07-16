using UnityEngine;

namespace InventorySystem.Items.Usables.Scp244.Hypothermia
{
	public class TemperatureSubEffect : HypothermiaSubEffectBase
	{
		[SerializeField]
		private float _maxExitTemp;

		[SerializeField]
		private float _temperatureDrop;

		public float CurTemperature { get; private set; }

		public override bool IsActive => false;

		internal override void UpdateEffect(float curExposure)
		{
		}

		public override void DisableEffect()
		{
		}
	}
}
