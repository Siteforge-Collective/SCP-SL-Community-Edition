namespace InventorySystem.Disarming
{
	public readonly struct DisarmedPlayersListMessage : global::Mirror.NetworkMessage
	{
		public readonly global::System.Collections.Generic.List<global::InventorySystem.Disarming.DisarmedPlayers.DisarmedEntry> Entries;

		public DisarmedPlayersListMessage(global::System.Collections.Generic.List<global::InventorySystem.Disarming.DisarmedPlayers.DisarmedEntry> entries)
		{
			Entries = entries;
		}
	}
}
