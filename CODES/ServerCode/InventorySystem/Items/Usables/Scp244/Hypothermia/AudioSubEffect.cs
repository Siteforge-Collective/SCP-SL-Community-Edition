namespace InventorySystem.Items.Usables.Scp244.Hypothermia
{
	public class AudioSubEffect : global::InventorySystem.Items.Usables.Scp244.Hypothermia.HypothermiaSubEffectBase, global::CustomPlayerEffects.ISoundtrackMutingEffect
	{
		public bool MuteSoundtrack { get; private set; }

		public override bool IsActive => false;

		internal override void UpdateEffect(float curExposure)
		{
		}
	}
}
