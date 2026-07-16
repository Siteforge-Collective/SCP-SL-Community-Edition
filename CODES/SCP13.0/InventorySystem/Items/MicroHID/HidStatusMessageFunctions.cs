using Mirror;

namespace InventorySystem.Items.MicroHID
{
	public static class HidStatusMessageFunctions
	{
		public static void Serialize(this NetworkWriter writer, HidStatusMessage value)
		{
		}

		public static HidStatusMessage Deserialize(this NetworkReader reader)
		{
			return default(HidStatusMessage);
		}
	}
}
