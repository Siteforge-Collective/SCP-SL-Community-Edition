[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/AAA/Blood On Screen")]
public class CameraFilterPack_AAA_BloodOnScreen : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	private float TimeX = 1f;

	[global::UnityEngine.Range(0.02f, 1.6f)]
	public float Blood_On_Screen = 1f;

	public global::UnityEngine.Color Blood_Color = global::UnityEngine.Color.red;

	[global::UnityEngine.Range(0f, 2f)]
	public float Blood_Intensify = 0.7f;

	[global::UnityEngine.Range(0f, 2f)]
	public float Blood_Darkness = 0.5f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Blood_Distortion_Speed = 0.25f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Blood_Fade = 1f;

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
		Texture2 = global::UnityEngine.Resources.Load("CameraFilterPack_AAA_BloodOnScreen1") as global::UnityEngine.Texture2D;
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/AAA_BloodOnScreen");
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
			material.SetFloat("_Value", global::UnityEngine.Mathf.Clamp(Blood_On_Screen, 0.02f, 1.6f));
			material.SetFloat("_Value2", global::UnityEngine.Mathf.Clamp(Blood_Intensify, 0f, 2f));
			material.SetFloat("_Value3", global::UnityEngine.Mathf.Clamp(Blood_Darkness, 0f, 2f));
			material.SetFloat("_Value4", global::UnityEngine.Mathf.Clamp(Blood_Fade, 0f, 1f));
			material.SetFloat("_Value5", global::UnityEngine.Mathf.Clamp(Blood_Distortion_Speed, 0f, 2f));
			material.SetColor("_Color2", Blood_Color);
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
