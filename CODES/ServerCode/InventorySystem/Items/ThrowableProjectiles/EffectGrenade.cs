namespace InventorySystem.Items.ThrowableProjectiles
{
	public class EffectGrenade : global::InventorySystem.Items.ThrowableProjectiles.TimeGrenade
	{
		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (base.TargetTime != 0f)
			{
				PlayExplosionEffects();
			}
		}

		protected virtual void PlayExplosionEffects()
		{
		}

		protected override void ServerFuseEnd()
		{
			DestroySelf();
		}

		private void MirrorProcessed()
		{
		}
	}
}
