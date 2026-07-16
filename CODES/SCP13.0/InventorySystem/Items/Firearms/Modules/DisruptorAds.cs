namespace InventorySystem.Items.Firearms.Modules
{
	public class DisruptorAds : StandardAds
	{
		private DisruptorAction Disruptor => null;

		protected override bool ForceDisabled => false;

		protected override bool AllowChange => false;

		public DisruptorAds(Firearm selfRef, ushort serial, float defaultAdsTime, int adsLayer, byte adsInClip, byte adsOutClip)
			: base(null, 0, 0f, 0, 0, 0)
		{
		}
	}
}
