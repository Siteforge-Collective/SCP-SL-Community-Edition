namespace InventorySystem.Items.Firearms
{
	public class DisruptorViewmodel : AnimatedFirearmViewmodel
	{
		private enum DisruptorLayer
		{
			MainAnims = 0,
			Ads = 1,
			NoMag = 2,
			Trigger = 3,
			Idle = 4
		}

		private ParticleDisruptor _firearm;

		private float _triggerSmooth;

		internal override void OnEquipped()
		{
		}

		protected override void OnShot()
		{
		}

		public override void LateUpdate()
		{
		}

		private void SetLayerWeight(DisruptorLayer layer, float val)
		{
		}
	}
}
