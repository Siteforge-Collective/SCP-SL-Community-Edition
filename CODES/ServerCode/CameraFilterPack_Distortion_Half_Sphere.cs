[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/Distortion/Half_Sphere")]
public class CameraFilterPack_Distortion_Half_Sphere : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	private float TimeX = 1f;

	[global::UnityEngine.Range(1f, 6f)]
	private global::UnityEngine.Material SCMaterial;

	public float SphereSize = 2.5f;

	[global::UnityEngine.Range(-1f, 1f)]
	public float SpherePositionX;

	[global::UnityEngine.Range(-1f, 1f)]
	public float SpherePositionY;

	[global::UnityEngine.Range(1f, 10f)]
	public float Strength = 5f;

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
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/Distortion_Half_Sphere");
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
			material.SetFloat("_SphereSize", SphereSize);
			material.SetFloat("_SpherePositionX", SpherePositionX);
			material.SetFloat("_SpherePositionY", SpherePositionY);
			material.SetFloat("_Strength", Strength);
			material.SetVector("_ScreenResolution", new global::UnityEngine.Vector2(global::UnityEngine.Screen.width, global::UnityEngine.Screen.height));
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
