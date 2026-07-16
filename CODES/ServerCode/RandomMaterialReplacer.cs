public class RandomMaterialReplacer : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Material[] mats;

	private void Start()
	{
		int num = global::UnityEngine.Random.Range(0, mats.Length);
		global::UnityEngine.MeshRenderer[] componentsInChildren = GetComponentsInChildren<global::UnityEngine.MeshRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].material = mats[num];
		}
	}
}
