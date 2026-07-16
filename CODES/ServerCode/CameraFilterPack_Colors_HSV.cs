[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/Colors/HSV")]
public class CameraFilterPack_Colors_HSV : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	[global::UnityEngine.Range(0f, 360f)]
	public float _HueShift = 180f;

	[global::UnityEngine.Range(-32f, 32f)]
	public float _Saturation = 1f;

	[global::UnityEngine.Range(-32f, 32f)]
	public float _ValueBrightness = 1f;

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
		if (!global::UnityEngine.SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
		}
	}

	private void OnRenderImage(global::UnityEngine.RenderTexture sourceTexture, global::UnityEngine.RenderTexture destTexture)
	{
		if (SCShader != null)
		{
			material.SetFloat("_HueShift", _HueShift);
			material.SetFloat("_Sat", _Saturation);
			material.SetFloat("_Val", _ValueBrightness);
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
