using MapGeneration;
using System.Collections.Generic;
using UnityEngine;

public class SeasonalMaterialReplacer : MonoBehaviour
{
    public List<SeasonalMaterialStruct> replacers = new List<SeasonalMaterialStruct>();

    private void Start()
    {
        SeedSynchronizer.OnMapGenerated += Festivize;
    }

    private void OnDestroy()
    {
        SeedSynchronizer.OnMapGenerated -= Festivize;
    }


    public void Festivize()
	{
	}
}
