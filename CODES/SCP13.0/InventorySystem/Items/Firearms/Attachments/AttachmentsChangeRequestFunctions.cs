using Mirror;

namespace InventorySystem.Items.Firearms.Attachments
{
	public static class AttachmentsChangeRequestFunctions
	{
		public static void Serialize(this NetworkWriter writer, AttachmentsChangeRequest value)
		{
		}

		public static AttachmentsChangeRequest Deserialize(this NetworkReader reader)
		{
			return default(AttachmentsChangeRequest);
		}
	}
}
