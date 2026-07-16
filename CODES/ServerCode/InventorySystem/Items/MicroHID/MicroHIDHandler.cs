namespace InventorySystem.Items.MicroHID
{
	public static class MicroHIDHandler
	{
		public static readonly global::System.Collections.Generic.Dictionary<ushort, float> SyncEnergy = new global::System.Collections.Generic.Dictionary<ushort, float>();

		public static readonly global::System.Collections.Generic.Dictionary<ushort, global::InventorySystem.Items.MicroHID.HidStatusMessage> SyncStates = new global::System.Collections.Generic.Dictionary<ushort, global::InventorySystem.Items.MicroHID.HidStatusMessage>();

		private static readonly global::System.Collections.Generic.Dictionary<int, global::UnityEngine.AudioSource> Sources = new global::System.Collections.Generic.Dictionary<int, global::UnityEngine.AudioSource>();

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			global::InventorySystem.Inventory.OnServerStarted += RegisterServerHandlers;
			CustomNetworkManager.OnClientReady += RegisterClientHandlers;
		}

		private static void RegisterServerHandlers()
		{
			global::Mirror.NetworkServer.ReplaceHandler<global::InventorySystem.Items.MicroHID.HidStatusMessage>(ServerReceiveKey);
		}

		private static void ServerReceiveKey(global::Mirror.NetworkConnection conn, global::InventorySystem.Items.MicroHID.HidStatusMessage msg)
		{
			if (ReferenceHub.TryGetHub(conn.identity.gameObject, out var hub) && hub.inventory.CurItem.SerialNumber == msg.Serial && hub.inventory.UserInventory.Items.TryGetValue(msg.Serial, out var value) && value is global::InventorySystem.Items.MicroHID.MicroHIDItem microHIDItem && global::System.Enum.IsDefined(typeof(global::InventorySystem.Items.MicroHID.HidUserInput), msg.MessageCode))
			{
				microHIDItem.UserInput = (global::InventorySystem.Items.MicroHID.HidUserInput)msg.MessageCode;
			}
		}

		private static void RegisterClientHandlers()
		{
			global::Mirror.NetworkClient.ReplaceHandler<global::InventorySystem.Items.MicroHID.HidStatusMessage>(ClientReceiveStatus);
			Sources.Clear();
		}

		private static void ClientReceiveStatus(global::InventorySystem.Items.MicroHID.HidStatusMessage msg)
		{
		}

		private static void ClientProcessStateSync(ReferenceHub targetPlayer, global::InventorySystem.Items.MicroHID.MicroHIDItem hidReference, global::InventorySystem.Items.MicroHID.HidStatusMessage msg)
		{
		}

		private static void ClientProcessEnergySync(ushort msgSerial, float msgEnergy)
		{
		}
	}
}
