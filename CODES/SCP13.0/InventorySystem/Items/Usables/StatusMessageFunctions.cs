using Mirror;

namespace InventorySystem.Items.Usables
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
