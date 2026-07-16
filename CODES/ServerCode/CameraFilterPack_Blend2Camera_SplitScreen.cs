[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/Split Screen/SideBySide")]
public class CameraFilterPack_Blend2Camera_SplitScreen : global::UnityEngine.MonoBehaviour
{
	private readonly string ShaderName = "CameraFilterPack/Blend2Camera_SplitScreen";

	public global::UnityEngine.Shader SCShader;

	public global::UnityEngine.Camera Camera2;

	private float TimeX = 1f;

	private global::UnityEngine.Material SCMaterial;

	[global::UnityEngine.Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	[global::UnityEngine.Range(0f, 1f)]
	public float BlendFX = 1f;

	[global::UnityEngine.Range(-3f, 3f)]
	public float SplitX = 0.5f;

	[global::UnityEngine.Range(-3f, 3f)]
	public float SplitY = 0.5f;

	[global::UnityEngine.Range(0f, 2f)]
	public float Smooth = 0.1f;

	[global::UnityEngine.Range(-3.14f, 3.14f)]
	public float Rotation = 3.14f;

	private readonly bool ForceYSwap;

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

	private void OnValidate()
	{
		ScreenSize.x = global::UnityEngine.Screen.width;
		ScreenSize.y = global::UnityEngine.Screen.height;
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
			material.SetFloat("_Value2", SwitchCameraToCamera2);
			material.SetFloat("_Value3", SplitX);
			material.SetFloat("_Value6", SplitY);
			material.SetFloat("_Value4", Smooth);
			material.SetFloat("_Value5", Rotation);
			material.SetInt("_ForceYSwap", (!ForceYSwap) ? 1 : 0);
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
	}

	private void OnEnable()
	{
		Start();
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
