[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/Sharpen/Sharpen")]
public class CameraFilterPack_Sharpen_Sharpen : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	[global::UnityEngine.Range(0.001f, 100f)]
	public float Value = 4f;

	[global::UnityEngine.Range(0.001f, 32f)]
	public float Value2 = 1f;

	private float TimeX = 1f;

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
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/Sharpen_Sharpen");
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
			material.SetVector("_ScreenResolution", new global::UnityEngine.Vector4(sourceTexture.width, sourceTexture.height, 0f, 0f));
			material.SetFloat("_Value", Value);
			material.SetFloat("_Value2", Value2);
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
