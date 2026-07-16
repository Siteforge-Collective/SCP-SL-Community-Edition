namespace InventorySystem.Items.Keycards
{
	public class KeycardItem : global::InventorySystem.Items.ItemBase, global::InventorySystem.Items.IItemNametag
	{
		[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Size = 1)]
		public struct UseMessage : global::Mirror.NetworkMessage
		{
		}

		public global::Interactables.Interobjects.DoorUtils.KeycardPermissions Permissions;

		public override float Weight => 0.01f;

		public override global::InventorySystem.Items.ItemDescriptionType DescriptionType => global::InventorySystem.Items.ItemDescriptionType.Keycard;

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			CustomNetworkManager.OnClientReady += delegate
			{
				global::Mirror.NetworkServer.ReplaceHandler(delegate(global::Mirror.NetworkConnection conn, global::InventorySystem.Items.Keycards.KeycardItem.UseMessage msg)
				{
					if (ReferenceHub.TryGetHubNetID(conn.identity.netId, out var hub) && hub.inventory.CurInstance is global::InventorySystem.Items.Keycards.KeycardItem keycardItem && !(keycardItem == null))
					{
						global::PlayerRoles.Spectating.SpectatorNetworking.SendToSpectatorsOf(default(global::InventorySystem.Items.Keycards.KeycardItem.UseMessage), hub);
					}
				});
			};
		}
	}
}
