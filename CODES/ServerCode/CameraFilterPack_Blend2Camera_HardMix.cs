[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/Blend 2 Camera/HardMix")]
public class CameraFilterPack_Blend2Camera_HardMix : global::UnityEngine.MonoBehaviour
{
	private readonly string ShaderName = "CameraFilterPack/Blend2Camera_HardMix";

	public global::UnityEngine.Shader SCShader;

	public global::UnityEngine.Camera Camera2;

	private float TimeX = 1f;

	private global::UnityEngine.Material SCMaterial;

	[global::UnityEngine.Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	[global::UnityEngine.Range(0f, 1f)]
	public float BlendFX = 0.5f;

	private global::UnityEngine.RenderTexture Camera2tex;

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
			Camera2tex = new global::UnityEngine.RenderTexture(global::UnityEngine.Screen.width, global::UnityEngine.Screen.height, 24);
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
			material.SetFloat("_Value2", SwitchCameraToCamera2);
			material.SetVector("_ScreenResolution", new global::UnityEngine.Vector4(sourceTexture.width, sourceTexture.height, 0f, 0f));
			global::UnityEngine.Graphics.Blit(sourceTexture, destTexture, material);
		}
		else
		{
			global::UnityEngine.Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void OnValidate()
	{
		if (Camera2 != null)
		{
			Camera2tex = new global::UnityEngine.RenderTexture(global::UnityEngine.Screen.width, global::UnityEngine.Screen.height, 24);
			Camera2.targetTexture = Camera2tex;
		}
	}

	private void Update()
	{
	}

	private void OnEnable()
	{
		if (Camera2 != null)
		{
			Camera2tex = new global::UnityEngine.RenderTexture(global::UnityEngine.Screen.width, global::UnityEngine.Screen.height, 24);
			Camera2.targetTexture = Camera2tex;
		}
	}

	private void OnDisable()
	{
		if (Camera2 != null)
		{
			Camera2.targetTexture = null;
		}
		if ((bool)SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(SCMaterial);
		}
	}
}
