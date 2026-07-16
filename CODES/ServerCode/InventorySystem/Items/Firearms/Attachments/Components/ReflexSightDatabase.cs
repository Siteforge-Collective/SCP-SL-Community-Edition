namespace InventorySystem.Items.Firearms.Attachments.Components
{
	public static class ReflexSightDatabase
	{
		public static global::System.Collections.Generic.Dictionary<ushort, global::System.Collections.Generic.Dictionary<int, global::InventorySystem.Items.Firearms.Attachments.Components.ReflexSightSyncMessage>> Database = new global::System.Collections.Generic.Dictionary<ushort, global::System.Collections.Generic.Dictionary<int, global::InventorySystem.Items.Firearms.Attachments.Components.ReflexSightSyncMessage>>();

		private static readonly global::System.Collections.Generic.HashSet<int> DirtyValues = new global::System.Collections.Generic.HashSet<int>();

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			StaticUnityMethods.OnUpdate += ServerUpdate;
			global::InventorySystem.Items.Firearms.BasicMessages.FirearmBasicMessagesHandler.OnStatusRequested += HandleNewClient;
			CustomNetworkManager.OnClientReady += delegate
			{
				Database.Clear();
				global::Mirror.NetworkClient.ReplaceHandler<global::InventorySystem.Items.Firearms.Attachments.Components.ReflexSightSyncMessage>(ClientReceiveMessage);
				global::Mirror.NetworkServer.ReplaceHandler<global::InventorySystem.Items.Firearms.Attachments.Components.ReflexSightSyncMessage>(ServerReceiveMessage);
			};
		}

		private static void HandleNewClient(ReferenceHub hub)
		{
			foreach (global::System.Collections.Generic.KeyValuePair<ushort, global::System.Collections.Generic.Dictionary<int, global::InventorySystem.Items.Firearms.Attachments.Components.ReflexSightSyncMessage>> item in Database)
			{
				foreach (global::System.Collections.Generic.KeyValuePair<int, global::InventorySystem.Items.Firearms.Attachments.Components.ReflexSightSyncMessage> item2 in item.Value)
				{
					hub.networkIdentity.connectionToClient.Send(item2.Value);
				}
			}
		}

		private static void ClientReceiveMessage(global::InventorySystem.Items.Firearms.Attachments.Components.ReflexSightSyncMessage msg)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				AddMessage(msg);
			}
		}

		private static void ServerReceiveMessage(global::Mirror.NetworkConnection conn, global::InventorySystem.Items.Firearms.Attachments.Components.ReflexSightSyncMessage msg)
		{
			if (ReferenceHub.TryGetHubNetID(conn.identity.netId, out var hub) && hub.inventory.UserInventory.Items.TryGetValue(msg.WeaponSerial, out var value) && value is global::InventorySystem.Items.Firearms.Firearm firearm && firearm.Attachments[global::UnityEngine.Mathf.Min(msg.AttachmentId, firearm.Attachments.Length - 1)] is global::InventorySystem.Items.Firearms.Attachments.Components.ReflexSightAttachment)
			{
				AddMessage(msg);
			}
		}

		private static void AddMessage(global::InventorySystem.Items.Firearms.Attachments.Components.ReflexSightSyncMessage msg)
		{
			if (Database.TryGetValue(msg.WeaponSerial, out var value))
			{
				value[msg.AttachmentId] = msg;
			}
			else
			{
				Database.Add(msg.WeaponSerial, new global::System.Collections.Generic.Dictionary<int, global::InventorySystem.Items.Firearms.Attachments.Components.ReflexSightSyncMessage> { [msg.AttachmentId] = msg });
			}
			if (global::Mirror.NetworkServer.active)
			{
				DirtyValues.Add((msg.WeaponSerial << 16) | msg.AttachmentId);
			}
		}

		private static void ServerUpdate()
		{
			if (!StaticUnityMethods.IsPlaying)
			{
				return;
			}
			foreach (int dirtyValue in DirtyValues)
			{
				ushort key = (ushort)(dirtyValue >> 16);
				int key2 = (ushort)(dirtyValue & 0xFFFF);
				if (Database.TryGetValue(key, out var value) && value.TryGetValue(key2, out var value2))
				{
					global::Mirror.NetworkServer.SendToAll(value2);
				}
			}
			DirtyValues.Clear();
		}
	}
}
