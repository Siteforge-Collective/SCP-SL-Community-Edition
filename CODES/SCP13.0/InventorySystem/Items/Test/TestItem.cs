using InventorySystem.Items.Autosync;
using Mirror;

namespace InventorySystem.Items.Test
{
	public class TestItem : AutosyncItem
	{
		public override float Weight => 0f;

		public static void Log(string msg)
		{
		}

		public override void EquipUpdate()
		{
		}

		public override void ServerConfirmAcqusition()
		{
		}

		public override void ClientProcessRpcLocally(NetworkReader reader)
		{
		}

		public override void ServerProcessCmd(NetworkReader reader)
		{
		}
	}
}
