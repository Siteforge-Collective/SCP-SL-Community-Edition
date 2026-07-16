namespace MapGeneration.Distributors
{
	[global::System.Serializable]
	public struct SpawnableItem
	{
		public float MinimalAmount;

		public float MaxAmount;

		public bool MultiplyBySpawnpointsNumber;

		public ItemType[] PossibleSpawns;

		public global::MapGeneration.RoomName[] RoomNames;
	}
}
