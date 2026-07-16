using Mirror;

namespace InventorySystem.Items.Firearms.BasicMessages
{
	public static class RequestMessageFunctions
	{
		public static void Serialize(this NetworkWriter writer, RequestMessage value)
		{
		}

		public static RequestMessage Deserialize(this NetworkReader reader)
		{
			return default(RequestMessage);
		}
	}
}
