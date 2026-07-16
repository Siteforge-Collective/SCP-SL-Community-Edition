public class SeasonalMaterialReplacer : global::UnityEngine.MonoBehaviour
{
	public global::System.Collections.Generic.List<SeasonalMaterialStruct> replacers = new global::System.Collections.Generic.List<SeasonalMaterialStruct>();

	private void Start()
	{
		global::MapGeneration.SeedSynchronizer.OnMapGenerated += Festivize;
	}

	private void OnDestroy()
	{
		global::MapGeneration.SeedSynchronizer.OnMapGenerated -= Festivize;
	}

	public void Festivize()
	{
	}
}
