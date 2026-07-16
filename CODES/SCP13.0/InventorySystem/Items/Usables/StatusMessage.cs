using Mirror;

namespace InventorySystem.Items.Usables
{
	public struct StatusMessage : NetworkMessage
	{
		public enum StatusType : byte
		{
			Start = 0,
			Cancel = 1
		}

		public StatusType Status;

		public ushort ItemSerial;

		public StatusMessage(StatusType status, ushort serial)
		{
			Status = default(StatusType);
			ItemSerial = 0;
		}

		public void Serialize(NetworkWriter writer)
		{
		}
	}
}
