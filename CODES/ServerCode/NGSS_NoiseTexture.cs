[global::UnityEngine.ExecuteInEditMode]
public class NGSS_NoiseTexture : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Texture noiseTex;

	[global::UnityEngine.Range(0f, 1f)]
	public float noiseScale = 1f;

	private bool isTextureSet;

	private void Update()
	{
		global::UnityEngine.Shader.SetGlobalFloat("NGSS_NOISE_TEXTURE_SCALE", noiseScale);
		if (!isTextureSet && !(noiseTex == null))
		{
			global::UnityEngine.Shader.SetGlobalTexture("NGSS_NOISE_TEXTURE", noiseTex);
			isTextureSet = true;
		}
	}
}
