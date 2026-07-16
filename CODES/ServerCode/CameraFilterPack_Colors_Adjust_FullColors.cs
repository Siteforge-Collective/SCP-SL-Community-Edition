[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/ColorsAdjust/FullColors")]
public class CameraFilterPack_Colors_Adjust_FullColors : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	private float TimeX = 1f;

	private global::UnityEngine.Material SCMaterial;

	[global::UnityEngine.Range(-200f, 200f)]
	public float Red_R = 100f;

	[global::UnityEngine.Range(-200f, 200f)]
	public float Red_G;

	[global::UnityEngine.Range(-200f, 200f)]
	public float Red_B;

	[global::UnityEngine.Range(-200f, 200f)]
	public float Red_Constant;

	[global::UnityEngine.Range(-200f, 200f)]
	public float Green_R;

	[global::UnityEngine.Range(-200f, 200f)]
	public float Green_G = 100f;

	[global::UnityEngine.Range(-200f, 200f)]
	public float Green_B;

	[global::UnityEngine.Range(-200f, 200f)]
	public float Green_Constant;

	[global::UnityEngine.Range(-200f, 200f)]
	public float Blue_R;

	[global::UnityEngine.Range(-200f, 200f)]
	public float Blue_G;

	[global::UnityEngine.Range(-200f, 200f)]
	public float Blue_B = 100f;

	[global::UnityEngine.Range(-200f, 200f)]
	public float Blue_Constant;

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
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/Colors_Adjust_FullColors");
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
			material.SetFloat("_Red_R", Red_R / 100f);
			material.SetFloat("_Red_G", Red_G / 100f);
			material.SetFloat("_Red_B", Red_B / 100f);
			material.SetFloat("_Green_R", Green_R / 100f);
			material.SetFloat("_Green_G", Green_G / 100f);
			material.SetFloat("_Green_B", Green_B / 100f);
			material.SetFloat("_Blue_R", Blue_R / 100f);
			material.SetFloat("_Blue_G", Blue_G / 100f);
			material.SetFloat("_Blue_B", Blue_B / 100f);
			material.SetFloat("_Red_C", Red_Constant / 100f);
			material.SetFloat("_Green_C", Green_Constant / 100f);
			material.SetFloat("_Blue_C", Blue_Constant / 100f);
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
		_ = global::UnityEngine.Application.isPlaying;
	}

	private void OnDisable()
	{
		if ((bool)SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(SCMaterial);
		}
	}
}
