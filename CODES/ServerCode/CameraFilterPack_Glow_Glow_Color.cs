[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/Glow/Glow_Color")]
public class CameraFilterPack_Glow_Glow_Color : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	private float TimeX = 1f;

	private global::UnityEngine.Material SCMaterial;

	[global::UnityEngine.Range(0f, 20f)]
	public float Amount = 4f;

	[global::UnityEngine.Range(2f, 16f)]
	public int FastFilter = 4;

	[global::UnityEngine.Range(0f, 1f)]
	public float Threshold = 0.5f;

	[global::UnityEngine.Range(0f, 3f)]
	public float Intensity = 2.25f;

	[global::UnityEngine.Range(-1f, 1f)]
	public float Precision = 0.56f;

	public global::UnityEngine.Color GlowColor = new global::UnityEngine.Color(0f, 0.7f, 1f, 1f);

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
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/Glow_Glow_Color");
		if (!global::UnityEngine.SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
		}
	}

	private void OnRenderImage(global::UnityEngine.RenderTexture sourceTexture, global::UnityEngine.RenderTexture destTexture)
	{
		if (SCShader != null)
		{
			int fastFilter = FastFilter;
			TimeX += global::UnityEngine.Time.deltaTime;
			if (TimeX > 100f)
			{
				TimeX = 0f;
			}
			material.SetFloat("_TimeX", TimeX);
			material.SetFloat("_Amount", Amount);
			material.SetFloat("_Value1", Threshold);
			material.SetFloat("_Value2", Intensity);
			material.SetFloat("_Value3", Precision);
			material.SetColor("_GlowColor", GlowColor);
			material.SetVector("_ScreenResolution", new global::UnityEngine.Vector2(global::UnityEngine.Screen.width / fastFilter, global::UnityEngine.Screen.height / fastFilter));
			int width = sourceTexture.width / fastFilter;
			int height = sourceTexture.height / fastFilter;
			if (FastFilter > 1)
			{
				global::UnityEngine.RenderTexture temporary = global::UnityEngine.RenderTexture.GetTemporary(width, height, 0);
				global::UnityEngine.RenderTexture temporary2 = global::UnityEngine.RenderTexture.GetTemporary(width, height, 0);
				temporary.filterMode = global::UnityEngine.FilterMode.Trilinear;
				global::UnityEngine.Graphics.Blit(sourceTexture, temporary, material, 3);
				global::UnityEngine.Graphics.Blit(temporary, temporary2, material, 2);
				global::UnityEngine.Graphics.Blit(temporary2, temporary, material, 0);
				material.SetFloat("_Amount", Amount * 2f);
				global::UnityEngine.Graphics.Blit(temporary, temporary2, material, 2);
				global::UnityEngine.Graphics.Blit(temporary2, temporary, material, 0);
				material.SetTexture("_MainTex2", temporary);
				global::UnityEngine.RenderTexture.ReleaseTemporary(temporary);
				global::UnityEngine.RenderTexture.ReleaseTemporary(temporary2);
				global::UnityEngine.Graphics.Blit(sourceTexture, destTexture, material, 1);
			}
			else
			{
				global::UnityEngine.Graphics.Blit(sourceTexture, destTexture, material, 0);
			}
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
