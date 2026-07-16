namespace InventorySystem.Items.Usables.Scp330
{
	public struct SyncScp330Message : global::Mirror.NetworkMessage
	{
		public ushort Serial;

		public global::System.Collections.Generic.List<global::InventorySystem.Items.Usables.Scp330.CandyKindID> Candies;
	}
}
