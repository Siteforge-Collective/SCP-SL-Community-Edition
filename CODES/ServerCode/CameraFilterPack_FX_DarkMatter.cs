[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/FX/DarkMatter")]
public class CameraFilterPack_FX_DarkMatter : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	private float TimeX = 1f;

	private global::UnityEngine.Material SCMaterial;

	[global::UnityEngine.Range(-10f, 10f)]
	public float Speed = 0.8f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Intensity = 1f;

	[global::UnityEngine.Range(-1f, 2f)]
	public float PosX = 0.5f;

	[global::UnityEngine.Range(-1f, 2f)]
	public float PosY = 0.5f;

	[global::UnityEngine.Range(-2f, 2f)]
	public float Zoom = 0.33f;

	[global::UnityEngine.Range(0f, 5f)]
	public float DarkIntensity = 2f;

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
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/FX_DarkMatter");
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
			material.SetFloat("_Value", Speed);
			material.SetFloat("_Value2", Intensity);
			material.SetFloat("_Value3", PosX);
			material.SetFloat("_Value4", PosY);
			material.SetFloat("_Value5", Zoom);
			material.SetFloat("_Value6", DarkIntensity);
			material.SetVector("_ScreenResolution", new global::UnityEngine.Vector4(sourceTexture.width, sourceTexture.height, 0f, 0f));
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
