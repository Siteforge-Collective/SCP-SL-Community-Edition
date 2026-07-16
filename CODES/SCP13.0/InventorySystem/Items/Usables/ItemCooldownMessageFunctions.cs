using Mirror;

namespace InventorySystem.Items.Usables
{
	public static class ItemCooldownMessageFunctions
	{
		public static void Serialize(this NetworkWriter writer, ItemCooldownMessage value)
		{
		}

		public static ItemCooldownMessage Deserialize(this NetworkReader reader)
		{
			return default(ItemCooldownMessage);
		}
	}
}
