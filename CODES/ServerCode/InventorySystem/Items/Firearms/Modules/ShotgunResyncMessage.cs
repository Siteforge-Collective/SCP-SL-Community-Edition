namespace InventorySystem.Items.Firearms.Modules
{
	public struct ShotgunResyncMessage : global::Mirror.NetworkMessage
	{
		public ushort Serial;

		public int ChamberedRounds;

		public int CockedHammers;

		public ShotgunResyncMessage(ushort serial, int chamberedBullets, int cockedHammers)
		{
			Serial = serial;
			ChamberedRounds = chamberedBullets;
			CockedHammers = cockedHammers;
		}
	}
}
