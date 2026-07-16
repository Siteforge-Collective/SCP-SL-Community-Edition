[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/Glasses/Night Glasses")]
public class CameraFilterPack_Glasses_On_3 : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	private float TimeX = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Fade = 0.3f;

	[global::UnityEngine.Range(0f, 0.1f)]
	public float VisionBlur = 0.005f;

	public global::UnityEngine.Color GlassesColor = new global::UnityEngine.Color(0.7f, 0.7f, 0.7f, 1f);

	public global::UnityEngine.Color GlassesColor2 = new global::UnityEngine.Color(1f, 1f, 1f, 1f);

	[global::UnityEngine.Range(0f, 1f)]
	public float GlassDistortion = 0.6f;

	[global::UnityEngine.Range(0f, 1f)]
	public float GlassAberration = 0.3f;

	[global::UnityEngine.Range(0f, 1f)]
	public float UseFinalGlassColor;

	[global::UnityEngine.Range(0f, 1f)]
	public float UseScanLine = 0.4f;

	[global::UnityEngine.Range(1f, 512f)]
	public float UseScanLineSize = 358f;

	public global::UnityEngine.Color GlassColor = new global::UnityEngine.Color(0f, 0.5f, 0f, 1f);

	private global::UnityEngine.Material SCMaterial;

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
		Texture2 = global::UnityEngine.Resources.Load("CameraFilterPack_Glasses_On4") as global::UnityEngine.Texture2D;
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/Glasses_On");
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
			material.SetFloat("UseFinalGlassColor", UseFinalGlassColor);
			material.SetFloat("Fade", Fade);
			material.SetFloat("VisionBlur", VisionBlur);
			material.SetFloat("GlassDistortion", GlassDistortion);
			material.SetFloat("GlassAberration", GlassAberration);
			material.SetColor("GlassesColor", GlassesColor);
			material.SetColor("GlassesColor2", GlassesColor2);
			material.SetColor("GlassColor", GlassColor);
			material.SetFloat("UseScanLineSize", UseScanLineSize);
			material.SetFloat("UseScanLine", UseScanLine);
			material.SetTexture("_MainTex2", Texture2);
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
