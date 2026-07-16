namespace MapGeneration.Distributors
{
	[global::UnityEngine.RequireComponent(typeof(global::MapGeneration.Distributors.StructurePositionSync))]
	public class SpawnableStructure : global::Mirror.NetworkBehaviour
	{
		public global::MapGeneration.Distributors.StructureType StructureType;

		public int MinAmount;

		public int MaxAmount;

		private void MirrorProcessed()
		{
		}
	}
}
