[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/AAA/Blood_Hit")]
public class CameraFilterPack_AAA_Blood_Hit : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	private float TimeX = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Hit_Left = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Hit_Up;

	[global::UnityEngine.Range(0f, 1f)]
	public float Hit_Right;

	[global::UnityEngine.Range(0f, 1f)]
	public float Hit_Down;

	[global::UnityEngine.Range(0f, 1f)]
	public float Blood_Hit_Left;

	[global::UnityEngine.Range(0f, 1f)]
	public float Blood_Hit_Up;

	[global::UnityEngine.Range(0f, 1f)]
	public float Blood_Hit_Right;

	[global::UnityEngine.Range(0f, 1f)]
	public float Blood_Hit_Down;

	[global::UnityEngine.Range(0f, 1f)]
	public float Hit_Full;

	[global::UnityEngine.Range(0f, 1f)]
	public float Blood_Hit_Full_1;

	[global::UnityEngine.Range(0f, 1f)]
	public float Blood_Hit_Full_2;

	[global::UnityEngine.Range(0f, 1f)]
	public float Blood_Hit_Full_3;

	[global::UnityEngine.Range(0f, 1f)]
	public float LightReflect = 0.5f;

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
		Texture2 = global::UnityEngine.Resources.Load("CameraFilterPack_AAA_Blood_Hit1") as global::UnityEngine.Texture2D;
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/AAA_Blood_Hit");
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
			material.SetFloat("_Value", LightReflect);
			material.SetFloat("_Value2", global::UnityEngine.Mathf.Clamp(Hit_Left, 0f, 1f));
			material.SetFloat("_Value3", global::UnityEngine.Mathf.Clamp(Hit_Up, 0f, 1f));
			material.SetFloat("_Value4", global::UnityEngine.Mathf.Clamp(Hit_Right, 0f, 1f));
			material.SetFloat("_Value5", global::UnityEngine.Mathf.Clamp(Hit_Down, 0f, 1f));
			material.SetFloat("_Value6", global::UnityEngine.Mathf.Clamp(Blood_Hit_Left, 0f, 1f));
			material.SetFloat("_Value7", global::UnityEngine.Mathf.Clamp(Blood_Hit_Up, 0f, 1f));
			material.SetFloat("_Value8", global::UnityEngine.Mathf.Clamp(Blood_Hit_Right, 0f, 1f));
			material.SetFloat("_Value9", global::UnityEngine.Mathf.Clamp(Blood_Hit_Down, 0f, 1f));
			material.SetFloat("_Value10", global::UnityEngine.Mathf.Clamp(Hit_Full, 0f, 1f));
			material.SetFloat("_Value11", global::UnityEngine.Mathf.Clamp(Blood_Hit_Full_1, 0f, 1f));
			material.SetFloat("_Value12", global::UnityEngine.Mathf.Clamp(Blood_Hit_Full_2, 0f, 1f));
			material.SetFloat("_Value13", global::UnityEngine.Mathf.Clamp(Blood_Hit_Full_3, 0f, 1f));
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
