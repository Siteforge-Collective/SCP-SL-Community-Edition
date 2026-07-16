[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/Blur/Blurry")]
public class CameraFilterPack_Blur_Blurry : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	private float TimeX = 1f;

	private global::UnityEngine.Material SCMaterial;

	[global::UnityEngine.Range(0f, 20f)]
	public float Amount = 2f;

	[global::UnityEngine.Range(1f, 8f)]
	public int FastFilter = 2;

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
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/Blur_Blurry");
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
			material.SetVector("_ScreenResolution", new global::UnityEngine.Vector2(global::UnityEngine.Screen.width / fastFilter, global::UnityEngine.Screen.height / fastFilter));
			int width = sourceTexture.width / fastFilter;
			int height = sourceTexture.height / fastFilter;
			if (FastFilter > 1)
			{
				global::UnityEngine.RenderTexture temporary = global::UnityEngine.RenderTexture.GetTemporary(width, height, 0);
				temporary.filterMode = global::UnityEngine.FilterMode.Trilinear;
				global::UnityEngine.Graphics.Blit(sourceTexture, temporary, material);
				global::UnityEngine.Graphics.Blit(temporary, destTexture);
				global::UnityEngine.RenderTexture.ReleaseTemporary(temporary);
			}
			else
			{
				global::UnityEngine.Graphics.Blit(sourceTexture, destTexture, material);
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
