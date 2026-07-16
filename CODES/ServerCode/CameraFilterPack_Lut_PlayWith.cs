[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/Lut/PlayWith")]
public class CameraFilterPack_Lut_PlayWith : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	private float TimeX = 1f;

	private global::UnityEngine.Material SCMaterial;

	public global::UnityEngine.Texture2D LutTexture;

	private global::UnityEngine.Texture3D converted3DLut;

	[global::UnityEngine.Range(0f, 1f)]
	public float Blend = 1f;

	[global::UnityEngine.Range(0f, 3f)]
	public float OriginalIntensity = 1f;

	[global::UnityEngine.Range(-1f, 1f)]
	public float ResultIntensity;

	[global::UnityEngine.Range(-1f, 1f)]
	public float FinalIntensity;

	private string MemoPath;

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
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/Lut_PlayWith");
		if (!global::UnityEngine.SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
		}
	}

	public void SetIdentityLut()
	{
		int num = 16;
		global::UnityEngine.Color[] array = new global::UnityEngine.Color[num * num * num];
		float num2 = 1f / (1f * (float)num - 1f);
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num; j++)
			{
				for (int k = 0; k < num; k++)
				{
					array[i + j * num + k * num * num] = new global::UnityEngine.Color((float)i * 1f * num2, (float)j * 1f * num2, (float)k * 1f * num2, 1f);
				}
			}
		}
		if ((bool)converted3DLut)
		{
			global::UnityEngine.Object.DestroyImmediate(converted3DLut);
		}
		converted3DLut = new global::UnityEngine.Texture3D(num, num, num, global::UnityEngine.TextureFormat.ARGB32, mipChain: false);
		converted3DLut.SetPixels(array);
		converted3DLut.Apply();
	}

	public bool ValidDimensions(global::UnityEngine.Texture2D tex2d)
	{
		if (!tex2d)
		{
			return false;
		}
		if (tex2d.height != global::UnityEngine.Mathf.FloorToInt(global::UnityEngine.Mathf.Sqrt(tex2d.width)))
		{
			return false;
		}
		return true;
	}

	public void Convert(global::UnityEngine.Texture2D temp2DTex)
	{
		if ((bool)temp2DTex)
		{
			int num = temp2DTex.width * temp2DTex.height;
			num = temp2DTex.height;
			if (!ValidDimensions(temp2DTex))
			{
				global::UnityEngine.Debug.LogWarning("The given 2D texture " + temp2DTex.name + " cannot be used as a 3D LUT.");
				return;
			}
			global::UnityEngine.Color[] pixels = temp2DTex.GetPixels();
			global::UnityEngine.Color[] array = new global::UnityEngine.Color[pixels.Length];
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num; j++)
				{
					for (int k = 0; k < num; k++)
					{
						int num2 = num - j - 1;
						array[i + j * num + k * num * num] = pixels[k * num + i + num2 * num * num];
					}
				}
			}
			if ((bool)converted3DLut)
			{
				global::UnityEngine.Object.DestroyImmediate(converted3DLut);
			}
			converted3DLut = new global::UnityEngine.Texture3D(num, num, num, global::UnityEngine.TextureFormat.ARGB32, mipChain: false);
			converted3DLut.SetPixels(array);
			converted3DLut.Apply();
		}
		else
		{
			SetIdentityLut();
		}
	}

	private void OnRenderImage(global::UnityEngine.RenderTexture sourceTexture, global::UnityEngine.RenderTexture destTexture)
	{
		if (SCShader != null || !global::UnityEngine.SystemInfo.supports3DTextures)
		{
			TimeX += global::UnityEngine.Time.deltaTime;
			if (TimeX > 100f)
			{
				TimeX = 0f;
			}
			if (converted3DLut == null)
			{
				Convert(LutTexture);
			}
			converted3DLut.wrapMode = global::UnityEngine.TextureWrapMode.Clamp;
			material.SetTexture("_LutTex", converted3DLut);
			material.SetFloat("_Blend", Blend);
			material.SetFloat("_Intensity", OriginalIntensity);
			material.SetFloat("_Extra", ResultIntensity);
			material.SetFloat("_Extra2", FinalIntensity);
			global::UnityEngine.Graphics.Blit(sourceTexture, destTexture, material, (global::UnityEngine.QualitySettings.activeColorSpace == global::UnityEngine.ColorSpace.Linear) ? 1 : 0);
		}
		else
		{
			global::UnityEngine.Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void OnValidate()
	{
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
