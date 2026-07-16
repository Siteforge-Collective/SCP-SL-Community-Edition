namespace InventorySystem.Items.MicroHID
{
	public struct HidStatusMessage : global::Mirror.NetworkMessage
	{
		public ushort Serial;

		public global::InventorySystem.Items.MicroHID.HidStatusMessageType MessageType;

		public byte MessageCode;

		public float Time;
	}
}
