namespace InventorySystem.Items.Flashlight
{
	public static class FlashlightNetworkHandler
	{
		public readonly struct FlashlightMessage : global::Mirror.NetworkMessage
		{
			public readonly ushort Serial;

			public readonly bool NewState;

			public FlashlightMessage(ushort flashlightSerial, bool newState)
			{
				Serial = flashlightSerial;
				NewState = newState;
			}
		}

		private static readonly global::System.Collections.Generic.HashSet<uint> AlreadyRequestedFirstimeSync = new global::System.Collections.Generic.HashSet<uint>();

		public static readonly global::System.Collections.Generic.Dictionary<ushort, bool> ReceivedStatuses = new global::System.Collections.Generic.Dictionary<ushort, bool>();

		public static event global::System.Action<global::InventorySystem.Items.Flashlight.FlashlightNetworkHandler.FlashlightMessage> OnStatusReceived;

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			CustomNetworkManager.OnClientReady += RegisterHandlers;
			global::InventorySystem.Inventory.OnLocalClientStarted += delegate
			{
				global::Mirror.NetworkClient.Send(new global::InventorySystem.Items.Flashlight.FlashlightNetworkHandler.FlashlightMessage(0, newState: false));
			};
		}

		private static void RegisterHandlers()
		{
			AlreadyRequestedFirstimeSync.Clear();
			ReceivedStatuses.Clear();
			global::Mirror.NetworkClient.ReplaceHandler<global::InventorySystem.Items.Flashlight.FlashlightNetworkHandler.FlashlightMessage>(ClientProcessMessage);
			global::Mirror.NetworkServer.ReplaceHandler<global::InventorySystem.Items.Flashlight.FlashlightNetworkHandler.FlashlightMessage>(ServerProcessMessage);
		}

		private static void ClientProcessMessage(global::InventorySystem.Items.Flashlight.FlashlightNetworkHandler.FlashlightMessage msg)
		{
		}

		private static void ServerProcessMessage(global::Mirror.NetworkConnection conn, global::InventorySystem.Items.Flashlight.FlashlightNetworkHandler.FlashlightMessage msg)
		{
			if (ReferenceHub.TryGetHubNetID(conn.identity.netId, out var hub))
			{
				if (msg.Serial == 0)
				{
					ServerProcessFirstTimeRequest(conn);
				}
				if (hub.inventory.CurItem.SerialNumber == msg.Serial && hub.inventory.CurInstance is global::InventorySystem.Items.Flashlight.FlashlightItem flashlightItem && global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerToggleFlashlight, hub, hub.inventory.CurInstance, !flashlightItem.IsEmittingLight))
				{
					flashlightItem.IsEmittingLight = msg.NewState;
					global::Utils.Networking.NetworkUtils.SendToAuthenticated(new global::InventorySystem.Items.Flashlight.FlashlightNetworkHandler.FlashlightMessage(msg.Serial, msg.NewState));
				}
			}
		}

		private static void ServerProcessFirstTimeRequest(global::Mirror.NetworkConnection conn)
		{
			if (!AlreadyRequestedFirstimeSync.Add(conn.identity.netId))
			{
				return;
			}
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (allHub.inventory.CurInstance is global::InventorySystem.Items.Flashlight.FlashlightItem flashlightItem && !(flashlightItem == null) && !flashlightItem.IsEmittingLight && global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerToggleFlashlight, allHub, flashlightItem, !flashlightItem.IsEmittingLight))
				{
					conn.Send(new global::InventorySystem.Items.Flashlight.FlashlightNetworkHandler.FlashlightMessage(allHub.inventory.CurItem.SerialNumber, newState: false));
				}
			}
		}

		public static void Serialize(this global::Mirror.NetworkWriter writer, global::InventorySystem.Items.Flashlight.FlashlightNetworkHandler.FlashlightMessage value)
		{
			global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, value.Serial);
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, value.NewState);
		}

		public static global::InventorySystem.Items.Flashlight.FlashlightNetworkHandler.FlashlightMessage Deserialize(this global::Mirror.NetworkReader reader)
		{
			return new global::InventorySystem.Items.Flashlight.FlashlightNetworkHandler.FlashlightMessage(global::Mirror.NetworkReaderExtensions.ReadUInt16(reader), global::Mirror.NetworkReaderExtensions.ReadBoolean(reader));
		}
	}
}
