namespace MapGeneration.Distributors
{
	[global::System.Serializable]
	public class LockerLoot
	{
		public ItemType TargetItem;

		public int RemainingUses;

		public int MaxPerChamber;

		public int ProbabilityPoints;
	}
}
