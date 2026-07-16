namespace InventorySystem.Items.Radio
{
	public static class RadioMessages
	{
		public enum RadioCommand : byte
		{
			Enable = 0,
			Disable = 1,
			ChangeRange = 2
		}

		public enum RadioRangeLevel : sbyte
		{
			RadioDisabled = -1,
			LowRange = 0,
			MediumRange = 1,
			HighRange = 2,
			UltraRange = 3
		}

		public static readonly global::System.Collections.Generic.Dictionary<uint, global::InventorySystem.Items.Radio.RadioStatusMessage> SyncedRangeLevels = new global::System.Collections.Generic.Dictionary<uint, global::InventorySystem.Items.Radio.RadioStatusMessage>();

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			CustomNetworkManager.OnClientReady += delegate
			{
				global::Mirror.NetworkClient.ReplaceHandler<global::InventorySystem.Items.Radio.RadioStatusMessage>(ClientStatusReceived);
				global::Mirror.NetworkServer.ReplaceHandler<global::InventorySystem.Items.Radio.ClientRadioCommandMessage>(ServerCommandReceived);
				SyncedRangeLevels.Clear();
			};
			ReferenceHub.OnPlayerAdded = (global::System.Action<ReferenceHub>)global::System.Delegate.Combine(ReferenceHub.OnPlayerAdded, (global::System.Action<ReferenceHub>)delegate(ReferenceHub hub)
			{
				if (!global::Mirror.NetworkServer.active)
				{
					return;
				}
				foreach (global::System.Collections.Generic.KeyValuePair<uint, global::InventorySystem.Items.Radio.RadioStatusMessage> syncedRangeLevel in SyncedRangeLevels)
				{
					hub.connectionToClient.Send(syncedRangeLevel.Value);
				}
			});
		}

		private static void ServerCommandReceived(global::Mirror.NetworkConnection conn, global::InventorySystem.Items.Radio.ClientRadioCommandMessage msg)
		{
			if (GetRadio(ReferenceHub.GetHub(conn.identity.gameObject), out var radio))
			{
				radio.ServerProcessCmd(msg.Command);
			}
		}

		private static void ClientStatusReceived(global::InventorySystem.Items.Radio.RadioStatusMessage msg)
		{
		}

		private static bool GetRadio(ReferenceHub ply, out global::InventorySystem.Items.Radio.RadioItem radio)
		{
			radio = null;
			if (ply == null)
			{
				return false;
			}
			foreach (global::InventorySystem.Items.ItemBase value in ply.inventory.UserInventory.Items.Values)
			{
				if (value is global::InventorySystem.Items.Radio.RadioItem radioItem)
				{
					radio = radioItem;
					return true;
				}
			}
			return false;
		}

		public static void WriteRadioStatusMessage(this global::Mirror.NetworkWriter writer, global::InventorySystem.Items.Radio.RadioStatusMessage msg)
		{
			msg.Serialize(writer);
		}

		public static global::InventorySystem.Items.Radio.RadioStatusMessage ReadRadioStatusMessage(this global::Mirror.NetworkReader reader)
		{
			return new global::InventorySystem.Items.Radio.RadioStatusMessage(reader);
		}

		public static void WriteClientRadioCommandMessage(this global::Mirror.NetworkWriter writer, global::InventorySystem.Items.Radio.ClientRadioCommandMessage msg)
		{
			msg.Serialize(writer);
		}

		public static global::InventorySystem.Items.Radio.ClientRadioCommandMessage ReadClientRadioCommandMessage(this global::Mirror.NetworkReader reader)
		{
			return new global::InventorySystem.Items.Radio.ClientRadioCommandMessage(reader);
		}
	}
}
