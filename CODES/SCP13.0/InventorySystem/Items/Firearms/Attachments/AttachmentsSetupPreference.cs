using Mirror;

namespace InventorySystem.Items.Firearms.Attachments
{
	public struct AttachmentsSetupPreference : NetworkMessage
	{
		public ItemType Weapon;

		public uint AttachmentsCode;

		public void Deserialize(NetworkReader reader)
		{
		}

		public void Serialize(NetworkWriter writer)
		{
		}
	}
}
