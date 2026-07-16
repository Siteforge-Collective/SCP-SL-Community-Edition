namespace InventorySystem.Items.Usables.Scp244.Hypothermia
{
	public class PostProcessSubEffect : global::InventorySystem.Items.Usables.Scp244.Hypothermia.HypothermiaSubEffectBase
	{
		[global::UnityEngine.SerializeField]
		private global::InventorySystem.Items.Usables.Scp244.Hypothermia.TemperatureSubEffect _temp;

		public override bool IsActive => _temp.IsActive;

		internal override void UpdateEffect(float curExposure)
		{
		}
	}
}
