[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/Vision/Hell_Blood")]
public class CameraFilterPack_Vision_Hell_Blood : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	private float TimeX = 1f;

	private global::UnityEngine.Material SCMaterial;

	[global::UnityEngine.Range(0f, 1f)]
	public float Hole_Size = 0.57f;

	[global::UnityEngine.Range(0f, 0.5f)]
	public float Hole_Smooth = 0.362f;

	[global::UnityEngine.Range(-2f, 2f)]
	public float Hole_Speed = 0.85f;

	[global::UnityEngine.Range(-10f, 10f)]
	public float Intensity = 0.24f;

	public global::UnityEngine.Color ColorBlood = new global::UnityEngine.Color(1f, 0f, 0f, 1f);

	[global::UnityEngine.Range(-1f, 1f)]
	public float BloodAlternative1;

	[global::UnityEngine.Range(-1f, 1f)]
	public float BloodAlternative2;

	[global::UnityEngine.Range(-1f, 1f)]
	public float BloodAlternative3 = -1f;

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
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/Vision_Hell_Blood");
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
			material.SetFloat("_Value", Hole_Size);
			material.SetFloat("_Value2", Hole_Smooth);
			material.SetFloat("_Value3", Hole_Speed * 15f);
			material.SetColor("ColorBlood", ColorBlood);
			material.SetFloat("_Value4", Intensity);
			material.SetFloat("BloodAlternative1", BloodAlternative1);
			material.SetFloat("BloodAlternative2", BloodAlternative2);
			material.SetFloat("BloodAlternative3", BloodAlternative3);
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
