[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/Convert/NormalMap")]
public class CameraFilterPack_Convert_Normal : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	[global::UnityEngine.Range(0f, 0.5f)]
	public float _Heigh = 0.0125f;

	[global::UnityEngine.Range(0f, 0.25f)]
	public float _Intervale = 0.0025f;

	private global::UnityEngine.Material SCMaterial;

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
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/Color_Convert_Normal");
		if (!global::UnityEngine.SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
		}
	}

	private void OnRenderImage(global::UnityEngine.RenderTexture sourceTexture, global::UnityEngine.RenderTexture destTexture)
	{
		if (SCShader != null)
		{
			material.SetFloat("_Heigh", _Heigh);
			material.SetFloat("_Intervale", _Intervale);
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
