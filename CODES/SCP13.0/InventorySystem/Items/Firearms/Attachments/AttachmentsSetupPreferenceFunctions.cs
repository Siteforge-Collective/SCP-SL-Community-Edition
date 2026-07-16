using Mirror;

namespace InventorySystem.Items.Firearms.Attachments
{
	public static class AttachmentsSetupPreferenceFunctions
	{
		public static void Serialize(this NetworkWriter writer, AttachmentsSetupPreference value)
		{
		}

		public static AttachmentsSetupPreference Deserialize(this NetworkReader reader)
		{
			return default(AttachmentsSetupPreference);
		}
	}
}
