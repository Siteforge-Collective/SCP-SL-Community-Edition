public class Clutter : global::UnityEngine.MonoBehaviour
{
	[global::UnityEngine.Header("Prefab Data")]
	public global::UnityEngine.GameObject holderObject;

	public global::System.Collections.Generic.List<global::UnityEngine.GameObject> possiblePrefabs = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();

	public global::UnityEngine.Vector3 spawnOffset;

	public global::UnityEngine.Vector3 clutterScale = global::UnityEngine.Vector3.zero;

	public bool spawned;

	private const float OverallScale = 0.72745f;

	public void SpawnClutter()
	{
		global::GameCore.Console.AddDebugLog("MGCLTR", "Spawning clutter component on object of name \"" + base.gameObject.name + "\"", MessageImportance.LeastImportant, nospace: true);
		spawned = true;
		if (!holderObject)
		{
			holderObject = base.gameObject;
		}
		global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate((possiblePrefabs.Count > 0) ? possiblePrefabs[global::UnityEngine.Random.Range(0, possiblePrefabs.Count)] : base.gameObject, holderObject.transform.position + spawnOffset * 0.72745f, holderObject.transform.rotation.normalized, holderObject.transform.parent);
		if (clutterScale != global::UnityEngine.Vector3.zero)
		{
			gameObject.transform.localScale = clutterScale;
		}
		else
		{
			gameObject.transform.localScale = holderObject.transform.localScale;
		}
		gameObject.SetActive(value: true);
		if (gameObject.TryGetComponent<Clutter>(out var component))
		{
			global::UnityEngine.Object.Destroy(component);
		}
		global::UnityEngine.Object.Destroy(holderObject);
	}
}
