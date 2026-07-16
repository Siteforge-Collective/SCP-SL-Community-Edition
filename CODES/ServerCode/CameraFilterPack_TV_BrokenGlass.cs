[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/TV/Broken Glass")]
public class CameraFilterPack_TV_BrokenGlass : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	private float TimeX = 1f;

	[global::UnityEngine.Range(0f, 128f)]
	public float Broken_Small;

	[global::UnityEngine.Range(0f, 128f)]
	public float Broken_Medium;

	[global::UnityEngine.Range(0f, 128f)]
	public float Broken_High;

	[global::UnityEngine.Range(0f, 128f)]
	public float Broken_Big = 1f;

	[global::UnityEngine.Range(0f, 0.004f)]
	public float LightReflect = 0.002f;

	private global::UnityEngine.Material SCMaterial;

	private global::UnityEngine.Texture2D Texture2;

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
		Texture2 = global::UnityEngine.Resources.Load("CameraFilterPack_TV_BrokenGlass1") as global::UnityEngine.Texture2D;
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/TV_BrokenGlass");
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
			material.SetFloat("_Value", LightReflect);
			material.SetFloat("_Value2", Broken_Small);
			material.SetFloat("_Value3", Broken_Medium);
			material.SetFloat("_Value4", Broken_High);
			material.SetFloat("_Value5", Broken_Big);
			material.SetTexture("_MainTex2", Texture2);
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
