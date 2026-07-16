public class MaterialLanguageReplacer : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Material englishVersion;

	private void Start()
	{
		GetComponent<global::UnityEngine.Renderer>().material = englishVersion;
	}

	private void OnDestroy()
	{
		global::UnityEngine.Object.Destroy(GetComponent<global::UnityEngine.Renderer>().material);
	}
}
