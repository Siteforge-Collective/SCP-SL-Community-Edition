[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/Glitch/NewGlitch5")]
public class CameraFilterPack_NewGlitch5 : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	private float TimeX = 1f;

	private global::UnityEngine.Material SCMaterial;

	[global::UnityEngine.Range(0f, 1f)]
	public float __Speed = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float _Fade = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float _Parasite = 1f;

	[global::UnityEngine.Range(0f, 0f)]
	public float _ZoomX = 1f;

	[global::UnityEngine.Range(0f, 0f)]
	public float _ZoomY = 1f;

	[global::UnityEngine.Range(0f, 0f)]
	public float _PosX = 1f;

	[global::UnityEngine.Range(0f, 0f)]
	public float _PosY = 1f;

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
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/CameraFilterPack_NewGlitch5");
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
			material.SetFloat("Parasite", _Parasite);
			material.SetFloat("ZoomX", _ZoomX);
			material.SetFloat("ZoomY", _ZoomY);
			material.SetFloat("PosX", _PosX);
			material.SetFloat("PosY", _PosY);
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
