using Mirror;

namespace InventorySystem.Items.Firearms.Attachments.Components
{
	public static class ReflexSightSyncMessageReaderWriter
	{
		public static void WriteReflexSightSyncMessage(this NetworkWriter writer, ReflexSightSyncMessage msg)
		{
		}

		public static ReflexSightSyncMessage ReadReflexSightSyncMessage(this NetworkReader reader)
		{
			return default(ReflexSightSyncMessage);
		}
	}
}
