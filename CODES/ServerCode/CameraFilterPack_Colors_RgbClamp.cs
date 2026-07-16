[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/Colors/RgbClamp")]
public class CameraFilterPack_Colors_RgbClamp : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	private float TimeX = 1f;

	private global::UnityEngine.Material SCMaterial;

	[global::UnityEngine.Range(0f, 1f)]
	public float Red_Start;

	[global::UnityEngine.Range(0f, 1f)]
	public float Red_End = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Green_Start;

	[global::UnityEngine.Range(0f, 1f)]
	public float Green_End = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Blue_Start;

	[global::UnityEngine.Range(0f, 1f)]
	public float Blue_End = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float RGB_Start;

	[global::UnityEngine.Range(0f, 1f)]
	public float RGB_End = 1f;

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
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/Colors_RgbClamp");
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
			material.SetFloat("_Value", Red_Start);
			material.SetFloat("_Value2", Red_End);
			material.SetFloat("_Value3", Green_Start);
			material.SetFloat("_Value4", Green_End);
			material.SetFloat("_Value5", Blue_Start);
			material.SetFloat("_Value6", Blue_End);
			material.SetFloat("_Value7", RGB_Start);
			material.SetFloat("_Value8", RGB_End);
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
