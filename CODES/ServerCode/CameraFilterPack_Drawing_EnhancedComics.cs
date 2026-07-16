[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/Drawing/EnhancedComics")]
public class CameraFilterPack_Drawing_EnhancedComics : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	private float TimeX = 1f;

	private global::UnityEngine.Material SCMaterial;

	[global::UnityEngine.Range(0f, 1f)]
	public float DotSize = 0.15f;

	[global::UnityEngine.Range(0f, 1f)]
	public float _ColorR = 0.9f;

	[global::UnityEngine.Range(0f, 1f)]
	public float _ColorG = 0.4f;

	[global::UnityEngine.Range(0f, 1f)]
	public float _ColorB = 0.4f;

	[global::UnityEngine.Range(0f, 1f)]
	public float _Blood = 0.5f;

	[global::UnityEngine.Range(0f, 1f)]
	public float _SmoothStart = 0.02f;

	[global::UnityEngine.Range(0f, 1f)]
	public float _SmoothEnd = 0.1f;

	public global::UnityEngine.Color ColorRGB = new global::UnityEngine.Color(1f, 0f, 0f);

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
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/Drawing_EnhancedComics");
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
			material.SetFloat("_DotSize", DotSize);
			material.SetFloat("_ColorR", _ColorR);
			material.SetFloat("_ColorG", _ColorG);
			material.SetFloat("_ColorB", _ColorB);
			material.SetFloat("_Blood", _Blood);
			material.SetColor("_ColorRGB", ColorRGB);
			material.SetFloat("_SmoothStart", _SmoothStart);
			material.SetFloat("_SmoothEnd", _SmoothEnd);
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
