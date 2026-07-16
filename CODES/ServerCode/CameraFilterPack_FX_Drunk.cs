[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/FX/Drunk")]
public class CameraFilterPack_FX_Drunk : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	private float TimeX = 1f;

	private global::UnityEngine.Material SCMaterial;

	[global::UnityEngine.HideInInspector]
	[global::UnityEngine.Range(0f, 20f)]
	public float Value = 6f;

	[global::UnityEngine.Range(0f, 10f)]
	public float Speed = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Wavy = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Distortion;

	[global::UnityEngine.Range(0f, 1f)]
	public float DistortionWave;

	[global::UnityEngine.Range(0f, 1f)]
	public float Fade = 1f;

	[global::UnityEngine.Range(-2f, 2f)]
	public float ColoredSaturate = 1f;

	[global::UnityEngine.Range(-1f, 2f)]
	public float ColoredChange;

	[global::UnityEngine.Range(-1f, 1f)]
	public float ChangeRed;

	[global::UnityEngine.Range(-1f, 1f)]
	public float ChangeGreen;

	[global::UnityEngine.Range(-1f, 1f)]
	public float ChangeBlue;

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
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/FX_Drunk");
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
			material.SetFloat("_Value", Value);
			material.SetFloat("_Speed", Speed);
			material.SetFloat("_Distortion", Distortion);
			material.SetFloat("_DistortionWave", DistortionWave);
			material.SetFloat("_Wavy", Wavy);
			material.SetFloat("_Fade", Fade);
			material.SetFloat("_ColoredChange", ColoredChange);
			material.SetFloat("_ChangeRed", ChangeRed);
			material.SetFloat("_ChangeGreen", ChangeGreen);
			material.SetFloat("_ChangeBlue", ChangeBlue);
			material.SetFloat("_Colored", ColoredSaturate);
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
