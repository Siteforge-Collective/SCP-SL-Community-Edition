[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/3D/Shield")]
public class CameraFilterPack_3D_Shield : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	public bool _Visualize;

	private float TimeX = 1f;

	private global::UnityEngine.Material SCMaterial;

	[global::UnityEngine.Range(0f, 100f)]
	public float _FixDistance = 1.5f;

	[global::UnityEngine.Range(-0.99f, 0.99f)]
	public float _Distance = 0.4f;

	[global::UnityEngine.Range(0f, 0.5f)]
	public float _Size = 0.5f;

	[global::UnityEngine.Range(0f, 1f)]
	public float _FadeShield = 0.75f;

	[global::UnityEngine.Range(-0.2f, 0.2f)]
	public float LightIntensity = 0.025f;

	public bool AutoAnimatedNear;

	[global::UnityEngine.Range(-5f, 5f)]
	public float AutoAnimatedNearSpeed = 0.5f;

	[global::UnityEngine.Range(0f, 10f)]
	public float Speed = 0.2f;

	[global::UnityEngine.Range(0f, 10f)]
	public float Speed_X = 0.2f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Speed_Y = 0.3f;

	[global::UnityEngine.Range(0f, 10f)]
	public float Intensity = 2.4f;

	public static global::UnityEngine.Color ChangeColorRGB;

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
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/3D_Shield");
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
			if (AutoAnimatedNear)
			{
				_Distance += global::UnityEngine.Time.deltaTime * AutoAnimatedNearSpeed;
				if (_Distance > 1f)
				{
					_Distance = -1f;
				}
				if (_Distance < -1f)
				{
					_Distance = 1f;
				}
				material.SetFloat("_Near", _Distance);
			}
			else
			{
				material.SetFloat("_Near", _Distance);
			}
			material.SetFloat("_Far", _Size);
			material.SetFloat("_FixDistance", _FixDistance);
			material.SetFloat("_LightIntensity", LightIntensity * 64f);
			material.SetFloat("_Visualize", _Visualize ? 1 : 0);
			material.SetFloat("_FadeShield", _FadeShield);
			material.SetFloat("_Value", Speed);
			material.SetFloat("_Value2", Speed_X);
			material.SetFloat("_Value3", Speed_Y);
			material.SetFloat("_Value4", Intensity);
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
