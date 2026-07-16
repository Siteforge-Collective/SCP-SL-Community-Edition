public class TextureMaterialLanguage : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Texture englishVersion;

	public global::UnityEngine.Material mat;

	private void Start()
	{
		mat.mainTexture = englishVersion;
	}
}
