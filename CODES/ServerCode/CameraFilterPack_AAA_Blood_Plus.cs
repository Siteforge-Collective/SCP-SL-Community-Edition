[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/AAA/Blood_Plus")]
public class CameraFilterPack_AAA_Blood_Plus : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	private float TimeX = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Blood_1 = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Blood_2;

	[global::UnityEngine.Range(0f, 1f)]
	public float Blood_3;

	[global::UnityEngine.Range(0f, 1f)]
	public float Blood_4;

	[global::UnityEngine.Range(0f, 1f)]
	public float Blood_5;

	[global::UnityEngine.Range(0f, 1f)]
	public float Blood_6;

	[global::UnityEngine.Range(0f, 1f)]
	public float Blood_7;

	[global::UnityEngine.Range(0f, 1f)]
	public float Blood_8;

	[global::UnityEngine.Range(0f, 1f)]
	public float Blood_9;

	[global::UnityEngine.Range(0f, 1f)]
	public float Blood_10;

	[global::UnityEngine.Range(0f, 1f)]
	public float Blood_11;

	[global::UnityEngine.Range(0f, 1f)]
	public float Blood_12;

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
		Texture2 = global::UnityEngine.Resources.Load("CameraFilterPack_AAA_Blood2") as global::UnityEngine.Texture2D;
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/AAA_Blood_Plus");
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
			material.SetFloat("_Value2", global::UnityEngine.Mathf.Clamp(Blood_1, 0f, 1f));
			material.SetFloat("_Value3", global::UnityEngine.Mathf.Clamp(Blood_2, 0f, 1f));
			material.SetFloat("_Value4", global::UnityEngine.Mathf.Clamp(Blood_3, 0f, 1f));
			material.SetFloat("_Value5", global::UnityEngine.Mathf.Clamp(Blood_4, 0f, 1f));
			material.SetFloat("_Value6", global::UnityEngine.Mathf.Clamp(Blood_5, 0f, 1f));
			material.SetFloat("_Value7", global::UnityEngine.Mathf.Clamp(Blood_6, 0f, 1f));
			material.SetFloat("_Value8", global::UnityEngine.Mathf.Clamp(Blood_7, 0f, 1f));
			material.SetFloat("_Value9", global::UnityEngine.Mathf.Clamp(Blood_8, 0f, 1f));
			material.SetFloat("_Value10", global::UnityEngine.Mathf.Clamp(Blood_9, 0f, 1f));
			material.SetFloat("_Value11", global::UnityEngine.Mathf.Clamp(Blood_10, 0f, 1f));
			material.SetFloat("_Value12", global::UnityEngine.Mathf.Clamp(Blood_11, 0f, 1f));
			material.SetFloat("_Value13", global::UnityEngine.Mathf.Clamp(Blood_12, 0f, 1f));
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
