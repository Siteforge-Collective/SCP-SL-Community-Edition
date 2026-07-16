using Mirror;

namespace InventorySystem.Items.Firearms.BasicMessages
{
	public static class StatusMessageFunctions
	{
		public static void Serialize(this NetworkWriter writer, StatusMessage value)
		{
		}

		public static StatusMessage Deserialize(this NetworkReader reader)
		{
			return default(StatusMessage);
		}
	}
}
