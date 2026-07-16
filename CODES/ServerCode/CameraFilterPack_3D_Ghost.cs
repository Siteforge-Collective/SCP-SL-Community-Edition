[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/3D/Ghost")]
public class CameraFilterPack_3D_Ghost : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	private float TimeX = 1f;

	public bool _Visualize;

	private global::UnityEngine.Material SCMaterial;

	[global::UnityEngine.Range(0f, 100f)]
	public float _FixDistance = 5f;

	[global::UnityEngine.Range(-0.5f, 0.99f)]
	public float Ghost_Near = 0.08f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Ghost_Far = 0.55f;

	[global::UnityEngine.Range(0f, 2f)]
	public float Intensity = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float GhostWithoutObject = 1f;

	[global::UnityEngine.Range(-1f, 1f)]
	public float GhostPosX;

	[global::UnityEngine.Range(-1f, 1f)]
	public float GhostPosY;

	[global::UnityEngine.Range(0.1f, 8f)]
	public float GhostFade2 = 2f;

	[global::UnityEngine.Range(-1f, 1f)]
	public float GhostFade;

	[global::UnityEngine.Range(0.5f, 1.5f)]
	public float GhostSize = 0.9f;

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
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/3D_Ghost");
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
			material.SetFloat("_Value2", Intensity);
			material.SetFloat("GhostPosX", GhostPosX);
			material.SetFloat("GhostPosY", GhostPosY);
			material.SetFloat("GhostFade", GhostFade);
			material.SetFloat("GhostFade2", GhostFade2);
			material.SetFloat("GhostSize", GhostSize);
			material.SetFloat("_Visualize", _Visualize ? 1 : 0);
			material.SetFloat("_FixDistance", _FixDistance);
			material.SetFloat("Drop_Near", Ghost_Near);
			material.SetFloat("Drop_Far", Ghost_Far);
			material.SetFloat("Drop_With_Obj", GhostWithoutObject);
			material.SetVector("_ScreenResolution", new global::UnityEngine.Vector4(sourceTexture.width, sourceTexture.height, 0f, 0f));
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
