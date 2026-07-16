namespace InventorySystem.Items.ThrowableProjectiles
{
	public class ThrowableItemViewmodel : StandardAnimatedViemodel
	{
		private static readonly int AimHash;

		private static readonly int ThrowWeakHash;

		private static readonly int ThrowFullHash;

		private static readonly int GrenadeModifier;

		public override void InitLocal(ItemBase ib)
		{
		}

		public override void InitSpectator(ReferenceHub ply, ItemIdentifier id, bool wasEquipped)
		{
		}

		public override void InitAny()
		{
		}

		internal override void OnEquipped()
		{
		}

		private void OnDestroy()
		{
		}

		private void ProcessAnim(ThrowableNetworkHandler.RequestType request)
		{
		}

		private void OnMsgReceived(ThrowableNetworkHandler.ThrowableItemAudioMessage msg)
		{
		}
	}
}
