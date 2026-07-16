using UnityEngine;

namespace InventorySystem.Items.Usables.Scp244.Hypothermia
{
	public class DamageSubEffect : HypothermiaSubEffectBase
	{
		private float _damageCounter;

		[SerializeField]
		private TemperatureSubEffect _temperature;

		[SerializeField]
		private AnimationCurve _damageOverTemperature;

		public override bool IsActive => false;

		internal override void UpdateEffect(float curExposure)
		{
		}

		private void DealDamage(float curTemp)
		{
		}
	}
}
