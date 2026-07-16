[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/TV/Broken Glass2")]
public class CameraFilterPack_TV_BrokenGlass2 : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	private float TimeX = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Bullet_1;

	[global::UnityEngine.Range(0f, 1f)]
	public float Bullet_2;

	[global::UnityEngine.Range(0f, 1f)]
	public float Bullet_3;

	[global::UnityEngine.Range(0f, 1f)]
	public float Bullet_4 = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Bullet_5;

	[global::UnityEngine.Range(0f, 1f)]
	public float Bullet_6;

	[global::UnityEngine.Range(0f, 1f)]
	public float Bullet_7;

	[global::UnityEngine.Range(0f, 1f)]
	public float Bullet_8;

	[global::UnityEngine.Range(0f, 1f)]
	public float Bullet_9;

	[global::UnityEngine.Range(0f, 1f)]
	public float Bullet_10;

	[global::UnityEngine.Range(0f, 1f)]
	public float Bullet_11;

	[global::UnityEngine.Range(0f, 1f)]
	public float Bullet_12;

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
		Texture2 = global::UnityEngine.Resources.Load("CameraFilterPack_TV_BrokenGlass_2") as global::UnityEngine.Texture2D;
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/TV_BrokenGlass2");
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
			if (Bullet_1 < 0f)
			{
				Bullet_1 = 0f;
			}
			if (Bullet_2 < 0f)
			{
				Bullet_2 = 0f;
			}
			if (Bullet_3 < 0f)
			{
				Bullet_3 = 0f;
			}
			if (Bullet_4 < 0f)
			{
				Bullet_4 = 0f;
			}
			if (Bullet_5 < 0f)
			{
				Bullet_5 = 0f;
			}
			if (Bullet_6 < 0f)
			{
				Bullet_6 = 0f;
			}
			if (Bullet_7 < 0f)
			{
				Bullet_7 = 0f;
			}
			if (Bullet_8 < 0f)
			{
				Bullet_8 = 0f;
			}
			if (Bullet_9 < 0f)
			{
				Bullet_9 = 0f;
			}
			if (Bullet_10 < 0f)
			{
				Bullet_10 = 0f;
			}
			if (Bullet_11 < 0f)
			{
				Bullet_11 = 0f;
			}
			if (Bullet_12 < 0f)
			{
				Bullet_12 = 0f;
			}
			if (Bullet_1 > 1f)
			{
				Bullet_1 = 1f;
			}
			if (Bullet_2 > 1f)
			{
				Bullet_2 = 1f;
			}
			if (Bullet_3 > 1f)
			{
				Bullet_3 = 1f;
			}
			if (Bullet_4 > 1f)
			{
				Bullet_4 = 1f;
			}
			if (Bullet_5 > 1f)
			{
				Bullet_5 = 1f;
			}
			if (Bullet_6 > 1f)
			{
				Bullet_6 = 1f;
			}
			if (Bullet_7 > 1f)
			{
				Bullet_7 = 1f;
			}
			if (Bullet_8 > 1f)
			{
				Bullet_8 = 1f;
			}
			if (Bullet_9 > 1f)
			{
				Bullet_9 = 1f;
			}
			if (Bullet_10 > 1f)
			{
				Bullet_10 = 1f;
			}
			if (Bullet_11 > 1f)
			{
				Bullet_11 = 1f;
			}
			if (Bullet_12 > 1f)
			{
				Bullet_12 = 1f;
			}
			material.SetFloat("_Bullet_1", Bullet_1);
			material.SetFloat("_Bullet_2", Bullet_2);
			material.SetFloat("_Bullet_3", Bullet_3);
			material.SetFloat("_Bullet_4", Bullet_4);
			material.SetFloat("_Bullet_5", Bullet_5);
			material.SetFloat("_Bullet_6", Bullet_6);
			material.SetFloat("_Bullet_7", Bullet_7);
			material.SetFloat("_Bullet_8", Bullet_8);
			material.SetFloat("_Bullet_9", Bullet_9);
			material.SetFloat("_Bullet_10", Bullet_10);
			material.SetFloat("_Bullet_11", Bullet_11);
			material.SetFloat("_Bullet_12", Bullet_12);
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
