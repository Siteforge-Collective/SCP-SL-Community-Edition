using Mirror;

namespace InventorySystem.Items.Firearms.BasicMessages
{
	public static class ShotMessageFunctions
	{
		public static void Serialize(this NetworkWriter writer, ShotMessage value)
		{
		}

		public static ShotMessage Deserialize(this NetworkReader reader)
		{
			return default(ShotMessage);
		}
	}
}
