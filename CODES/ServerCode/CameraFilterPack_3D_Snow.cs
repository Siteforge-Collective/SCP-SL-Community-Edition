[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/3D/Snow")]
public class CameraFilterPack_3D_Snow : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	public bool _Visualize;

	private float TimeX = 1f;

	private global::UnityEngine.Material SCMaterial;

	[global::UnityEngine.Range(0f, 100f)]
	public float _FixDistance = 5f;

	[global::UnityEngine.Range(-0.5f, 0.99f)]
	public float Snow_Near = 0.08f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Snow_Far = 0.55f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Fade = 1f;

	[global::UnityEngine.Range(0f, 2f)]
	public float Intensity = 1f;

	[global::UnityEngine.Range(0.4f, 2f)]
	public float Size = 1f;

	[global::UnityEngine.Range(0f, 0.5f)]
	public float Speed = 0.275f;

	[global::UnityEngine.Range(0f, 1f)]
	public float SnowWithoutObject = 1f;

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
		Texture2 = global::UnityEngine.Resources.Load("CameraFilterPack_Blizzard1") as global::UnityEngine.Texture2D;
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/3D_Snow");
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
			material.SetFloat("_Value", Fade);
			material.SetFloat("_Value2", Intensity);
			material.SetFloat("_Value4", Speed * 6f);
			material.SetFloat("_Value5", Size);
			material.SetFloat("_Visualize", _Visualize ? 1 : 0);
			material.SetFloat("_FixDistance", _FixDistance);
			material.SetFloat("Drop_Near", Snow_Near);
			material.SetFloat("Drop_Far", Snow_Far);
			material.SetFloat("Drop_With_Obj", SnowWithoutObject);
			material.SetVector("_ScreenResolution", new global::UnityEngine.Vector4(sourceTexture.width, sourceTexture.height, 0f, 0f));
			material.SetTexture("Texture2", Texture2);
			GetComponent<global::UnityEngine.Camera>().depthTextureMode = global::UnityEngine.DepthTextureMode.Depth;
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
