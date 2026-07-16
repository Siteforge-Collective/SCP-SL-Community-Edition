[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/Drawing/Paper2")]
public class CameraFilterPack_Drawing_Paper2 : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	private float TimeX = 1f;

	public global::UnityEngine.Color Pencil_Color = new global::UnityEngine.Color(0f, 0.371f, 0.78f, 1f);

	[global::UnityEngine.Range(0.0001f, 0.0022f)]
	public float Pencil_Size = 0.0008f;

	[global::UnityEngine.Range(0f, 2f)]
	public float Pencil_Correction = 0.76f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Intensity = 1f;

	[global::UnityEngine.Range(0f, 2f)]
	public float Speed_Animation = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Corner_Lose = 0.85f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Fade_Paper_to_BackColor;

	[global::UnityEngine.Range(0f, 1f)]
	public float Fade_With_Original = 1f;

	public global::UnityEngine.Color Back_Color = new global::UnityEngine.Color(1f, 1f, 1f, 1f);

	private global::UnityEngine.Material SCMaterial;

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
		Texture2 = global::UnityEngine.Resources.Load("CameraFilterPack_Paper3") as global::UnityEngine.Texture2D;
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/Drawing_Paper2");
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
			material.SetColor("_PColor", Pencil_Color);
			material.SetFloat("_Value1", Pencil_Size);
			material.SetFloat("_Value2", Pencil_Correction);
			material.SetFloat("_Value3", Intensity);
			material.SetFloat("_Value4", Speed_Animation);
			material.SetFloat("_Value5", Corner_Lose);
			material.SetFloat("_Value6", Fade_Paper_to_BackColor);
			material.SetFloat("_Value7", Fade_With_Original);
			material.SetColor("_PColor2", Back_Color);
			material.SetTexture("_MainTex2", Texture2);
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
