using Mirror;

namespace InventorySystem.Items.Firearms.Attachments
{
	public struct AttachmentsChangeRequest : NetworkMessage
	{
		public ushort WeaponSerial;

		public uint AttachmentsCode;

		public void Deserialize(NetworkReader reader)
		{
		}

		public void Serialize(NetworkWriter writer)
		{
		}
	}
}
