[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/Broken/Simple")]
public class CameraFilterPack_Broken_Simple : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	private float TimeX = 1f;

	private global::UnityEngine.Material SCMaterial;

	[global::UnityEngine.Range(0f, 1f)]
	public float __Speed = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float _Broke1 = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float _Broke2 = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float _PosX = 0.5f;

	[global::UnityEngine.Range(0f, 1f)]
	public float _PosY = 0.5f;

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
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/CameraFilterPack_Broken_Simple");
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
			material.SetFloat("Broke1", _Broke1);
			material.SetFloat("Broke2", _Broke2);
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
