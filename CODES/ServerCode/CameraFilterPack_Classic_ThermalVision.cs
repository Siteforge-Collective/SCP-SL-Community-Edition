[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/Classic/ThermalVision")]
public class CameraFilterPack_Classic_ThermalVision : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	private float TimeX = 1f;

	private global::UnityEngine.Material SCMaterial;

	[global::UnityEngine.Range(0f, 1f)]
	public float __Speed = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float _Fade = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float _Crt = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float _Curve = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float _Color1 = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float _Color2 = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float _Color3 = 1f;

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
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/CameraFilterPack_Classic_ThermalVision");
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
			material.SetFloat("_Speed", __Speed);
			material.SetFloat("Fade", _Fade);
			material.SetFloat("Crt", _Crt);
			material.SetFloat("Curve", _Curve);
			material.SetFloat("Color1", _Color1);
			material.SetFloat("Color2", _Color2);
			material.SetFloat("Color3", _Color3);
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
