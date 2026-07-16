namespace MapGeneration.Distributors
{
	public class StructureSpawnpoint : global::MapGeneration.Distributors.DistributorSpawnpointBase
	{
		public static readonly global::System.Collections.Generic.HashSet<global::MapGeneration.Distributors.StructureSpawnpoint> AvailableInstances = new global::System.Collections.Generic.HashSet<global::MapGeneration.Distributors.StructureSpawnpoint>();

		public global::MapGeneration.Distributors.StructureType[] CompatibleStructures;

		public string TriggerDoorName;

		private void Start()
		{
			AvailableInstances.Add(this);
		}

		private void OnDestroy()
		{
			AvailableInstances.Remove(this);
		}
	}
}
