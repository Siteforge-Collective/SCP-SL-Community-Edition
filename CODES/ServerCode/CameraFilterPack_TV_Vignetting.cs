[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/TV/Vignetting")]
public class CameraFilterPack_TV_Vignetting : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	private global::UnityEngine.Material SCMaterial;

	private global::UnityEngine.Texture2D Vignette;

	[global::UnityEngine.Range(0f, 1f)]
	public float Vignetting = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float VignettingFull;

	[global::UnityEngine.Range(0f, 1f)]
	public float VignettingDirt;

	public global::UnityEngine.Color VignettingColor = new global::UnityEngine.Color(0f, 0f, 0f, 1f);

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
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/TV_Vignetting");
		Vignette = global::UnityEngine.Resources.Load("CameraFilterPack_TV_Vignetting1") as global::UnityEngine.Texture2D;
		if (!global::UnityEngine.SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
		}
	}

	private void OnRenderImage(global::UnityEngine.RenderTexture sourceTexture, global::UnityEngine.RenderTexture destTexture)
	{
		if (SCShader != null)
		{
			material.SetTexture("Vignette", Vignette);
			material.SetFloat("_Vignetting", Vignetting);
			material.SetFloat("_Vignetting2", VignettingFull);
			material.SetColor("_VignettingColor", VignettingColor);
			material.SetFloat("_VignettingDirt", VignettingDirt);
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
