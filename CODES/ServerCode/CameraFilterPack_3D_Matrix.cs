[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/3D/Matrix")]
public class CameraFilterPack_3D_Matrix : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	private float TimeX = 1f;

	private global::UnityEngine.Material SCMaterial;

	public bool _Visualize;

	[global::UnityEngine.Range(0f, 100f)]
	public float _FixDistance = 1f;

	[global::UnityEngine.Range(-5f, 5f)]
	public float LightIntensity = 1f;

	[global::UnityEngine.Range(0f, 6f)]
	public float MatrixSize = 1f;

	[global::UnityEngine.Range(-4f, 4f)]
	public float MatrixSpeed = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Fade = 1f;

	public global::UnityEngine.Color _MatrixColor = new global::UnityEngine.Color(0f, 1f, 0f, 1f);

	public static global::UnityEngine.Color ChangeColorRGB;

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
		Texture2 = global::UnityEngine.Resources.Load("CameraFilterPack_3D_Matrix1") as global::UnityEngine.Texture2D;
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/3D_Matrix");
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
			material.SetFloat("_DepthLevel", Fade);
			material.SetFloat("_FixDistance", _FixDistance);
			material.SetFloat("_MatrixSize", MatrixSize);
			material.SetColor("_MatrixColor", _MatrixColor);
			material.SetFloat("_MatrixSpeed", MatrixSpeed * 2f);
			material.SetFloat("_Visualize", _Visualize ? 1 : 0);
			material.SetFloat("_LightIntensity", LightIntensity);
			material.SetTexture("_MainTex2", Texture2);
			float farClipPlane = GetComponent<global::UnityEngine.Camera>().farClipPlane;
			material.SetFloat("_FarCamera", 1000f / farClipPlane);
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
