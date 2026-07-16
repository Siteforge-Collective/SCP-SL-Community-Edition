namespace InventorySystem.Items.Usables.Scp330
{
	public struct SelectScp330Message : global::Mirror.NetworkMessage
	{
		public ushort Serial;

		public int CandyID;

		public bool Drop;
	}
}
