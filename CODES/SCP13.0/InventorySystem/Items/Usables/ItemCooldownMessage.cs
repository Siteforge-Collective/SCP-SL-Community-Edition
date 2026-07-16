using Mirror;

namespace InventorySystem.Items.Usables
{
	public struct ItemCooldownMessage : NetworkMessage
	{
		public ushort ItemSerial;

		public float RemainingTime;

		public ItemCooldownMessage(ushort serial, float remainingTime)
		{
			ItemSerial = 0;
			RemainingTime = 0f;
		}

		public void Serialize(NetworkWriter writer)
		{
		}
	}
}
