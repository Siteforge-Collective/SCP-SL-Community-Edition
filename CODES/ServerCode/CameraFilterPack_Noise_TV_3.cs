[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/Noise/TV_3")]
public class CameraFilterPack_Noise_TV_3 : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	private float TimeX = 1f;

	private global::UnityEngine.Material SCMaterial;

	[global::UnityEngine.Range(0f, 1f)]
	public float Fade = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Fade_Additive;

	[global::UnityEngine.Range(0f, 1f)]
	public float Fade_Distortion;

	[global::UnityEngine.Range(0f, 10f)]
	private readonly float Value4 = 1f;

	private global::UnityEngine.Texture2D Texture2;

	private global::UnityEngine.Material material
	{
		get
		{
			if (SCMaterial == null)
			{
				SCMaterial = new global::UnityEngine.Material(SCShader);
				SCMaterial.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			}
			return SCMaterial;
		}
	}

	private void Start()
	{
		Texture2 = global::UnityEngine.Resources.Load("CameraFilterPack_TV_Noise3") as global::UnityEngine.Texture2D;
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/Noise_TV_3");
		if (!global::UnityEngine.SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
		}
	}

	private void OnRenderImage(global::UnityEngine.RenderTexture sourceTexture, global::UnityEngine.RenderTexture destTexture)
	{
		if (SCShader != null)
		{
			TimeX += global::UnityEngine.Time.deltaTime;
			if (TimeX > 100f)
			{
				TimeX = 0f;
			}
			material.SetFloat("_TimeX", TimeX);
			material.SetFloat("_Value", Fade);
			material.SetFloat("_Value2", Fade_Additive);
			material.SetFloat("_Value3", Fade_Distortion);
			material.SetFloat("_Value4", Value4);
			material.SetVector("_ScreenResolution", new global::UnityEngine.Vector4(sourceTexture.width, sourceTexture.height, 0f, 0f));
			material.SetTexture("Texture2", Texture2);
			global::UnityEngine.Graphics.Blit(sourceTexture, destTexture, material);
		}
		else
		{
			global::UnityEngine.Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void Update()
	{
	}

	private void OnDisable()
	{
		if ((bool)SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(SCMaterial);
		}
	}
}
