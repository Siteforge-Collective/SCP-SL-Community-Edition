[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/Chroma Key/Color Key")]
public class CameraFilterPack_Blend2Camera_ColorKey : global::UnityEngine.MonoBehaviour
{
	private readonly string ShaderName = "CameraFilterPack/Blend2Camera_ColorKey";

	public global::UnityEngine.Shader SCShader;

	public global::UnityEngine.Camera Camera2;

	private float TimeX = 1f;

	private global::UnityEngine.Material SCMaterial;

	[global::UnityEngine.Range(0f, 1f)]
	public float BlendFX = 1f;

	public global::UnityEngine.Color ColorKey;

	[global::UnityEngine.Range(-0.2f, 0.2f)]
	public float Adjust;

	[global::UnityEngine.Range(-0.2f, 0.2f)]
	public float Precision;

	[global::UnityEngine.Range(-0.2f, 0.2f)]
	public float Luminosity;

	[global::UnityEngine.Range(-0.3f, 0.3f)]
	public float Change_Red;

	[global::UnityEngine.Range(-0.3f, 0.3f)]
	public float Change_Green;

	[global::UnityEngine.Range(-0.3f, 0.3f)]
	public float Change_Blue;

	private global::UnityEngine.RenderTexture Camera2tex;

	private global::UnityEngine.Vector2 ScreenSize;

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
		if (Camera2 != null)
		{
			Camera2tex = new global::UnityEngine.RenderTexture((int)ScreenSize.x, (int)ScreenSize.y, 24);
			Camera2.targetTexture = Camera2tex;
		}
		SCShader = global::UnityEngine.Shader.Find(ShaderName);
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
			if (Camera2 != null)
			{
				material.SetTexture("_MainTex2", Camera2tex);
			}
			material.SetFloat("_TimeX", TimeX);
			material.SetFloat("_Value", BlendFX);
			material.SetFloat("_Value2", Adjust);
			material.SetFloat("_Value3", Precision);
			material.SetFloat("_Value4", Luminosity);
			material.SetFloat("_Value5", Change_Red);
			material.SetFloat("_Value6", Change_Green);
			material.SetFloat("_Value7", Change_Blue);
			material.SetColor("_ColorKey", ColorKey);
			global::UnityEngine.Graphics.Blit(sourceTexture, destTexture, material);
		}
		else
		{
			global::UnityEngine.Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void Update()
	{
		ScreenSize.x = global::UnityEngine.Screen.width;
		ScreenSize.y = global::UnityEngine.Screen.height;
		_ = global::UnityEngine.Application.isPlaying;
	}

	private void OnEnable()
	{
		Start();
		Update();
	}

	private void OnDisable()
	{
		if (Camera2 != null && Camera2.targetTexture != null)
		{
			Camera2.targetTexture = null;
		}
		if ((bool)SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(SCMaterial);
		}
	}
}
