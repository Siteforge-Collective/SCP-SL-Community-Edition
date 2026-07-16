[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/3D/Anomaly")]
public class CameraFilterPack_3D_Anomaly : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	public bool _Visualize;

	private float TimeX = 1f;

	private global::UnityEngine.Material SCMaterial;

	[global::UnityEngine.Range(0f, 100f)]
	public float _FixDistance = 23f;

	[global::UnityEngine.Range(-0.5f, 0.99f)]
	public float Anomaly_Near = 0.045f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Anomaly_Far = 0.11f;

	[global::UnityEngine.Range(0f, 2f)]
	public float Intensity = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float AnomalyWithoutObject = 1f;

	[global::UnityEngine.Range(0.1f, 1f)]
	public float Anomaly_Distortion = 0.25f;

	[global::UnityEngine.Range(4f, 64f)]
	public float Anomaly_Distortion_Size = 12f;

	[global::UnityEngine.Range(-4f, 8f)]
	public float Anomaly_Intensity = 2f;

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
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/3D_Anomaly");
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
			material.SetFloat("Anomaly_Distortion", Anomaly_Distortion);
			material.SetFloat("Anomaly_Distortion_Size", Anomaly_Distortion_Size);
			material.SetFloat("Anomaly_Intensity", Anomaly_Intensity);
			material.SetFloat("_Visualize", _Visualize ? 1 : 0);
			material.SetFloat("_FixDistance", _FixDistance);
			material.SetFloat("Anomaly_Near", Anomaly_Near);
			material.SetFloat("Anomaly_Far", Anomaly_Far);
			material.SetFloat("Anomaly_With_Obj", AnomalyWithoutObject);
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
