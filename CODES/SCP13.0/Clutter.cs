using System.Collections.Generic;
using UnityEngine;

public class Clutter : MonoBehaviour
{
	[Header("Prefab Data")]
	public GameObject holderObject;

	public List<GameObject> possiblePrefabs;

	public Vector3 spawnOffset;

	public Vector3 clutterScale;

	public bool spawned;

	private const float OverallScale = 0.72745f;

	public void SpawnClutter()
	{
	}
}
