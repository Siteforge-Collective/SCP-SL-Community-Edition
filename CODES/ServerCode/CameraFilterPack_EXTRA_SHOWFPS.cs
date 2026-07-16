[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/EXTRA/SHOWFPS")]
public class CameraFilterPack_EXTRA_SHOWFPS : global::UnityEngine.MonoBehaviour
{
	private float accum;

	private int frames;

	public float frequency = 0.5f;

	public global::UnityEngine.Shader SCShader;

	private float TimeX = 1f;

	private global::UnityEngine.Material SCMaterial;

	[global::UnityEngine.Range(8f, 42f)]
	public float Size = 12f;

	[global::UnityEngine.Range(0f, 100f)]
	private int FPS = 1;

	[global::UnityEngine.Range(0f, 10f)]
	private readonly float Value3 = 1f;

	[global::UnityEngine.Range(0f, 10f)]
	private readonly float Value4 = 1f;

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
		FPS = 0;
		StartCoroutine(FPSX());
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/EXTRA_SHOWFPS");
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
			material.SetFloat("_Value", Size);
			material.SetFloat("_Value2", FPS);
			material.SetFloat("_Value3", Value3);
			material.SetFloat("_Value4", Value4);
			material.SetVector("_ScreenResolution", new global::UnityEngine.Vector4(sourceTexture.width, sourceTexture.height, 0f, 0f));
			global::UnityEngine.Graphics.Blit(sourceTexture, destTexture, material);
		}
		else
		{
			global::UnityEngine.Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private global::System.Collections.IEnumerator FPSX()
	{
		while (true)
		{
			float num = accum / (float)frames;
			FPS = (int)num;
			accum = 0f;
			frames = 0;
			yield return new global::UnityEngine.WaitForSeconds(frequency);
		}
	}

	private void Update()
	{
		accum += global::UnityEngine.Time.timeScale / global::UnityEngine.Time.deltaTime;
		frames++;
	}

	private void OnDisable()
	{
		if ((bool)SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(SCMaterial);
		}
	}
}
