[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/Lut/Lut 2 Lut")]
public class CameraFilterPack_Lut_2_Lut : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	private float TimeX = 1f;

	private global::UnityEngine.Vector4 ScreenResolution;

	private global::UnityEngine.Material SCMaterial;

	public global::UnityEngine.Texture2D LutTexture;

	public global::UnityEngine.Texture2D LutTexture2;

	private global::UnityEngine.Texture3D converted3DLut;

	private global::UnityEngine.Texture3D converted3DLut2;

	[global::UnityEngine.Range(0f, 1f)]
	public float Blend = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Fade = 1f;

	private string MemoPath;

	private string MemoPath2;

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
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/Lut_2_lut");
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
		if ((bool)converted3DLut2)
		{
			global::UnityEngine.Object.DestroyImmediate(converted3DLut2);
		}
		converted3DLut2 = new global::UnityEngine.Texture3D(num, num, num, global::UnityEngine.TextureFormat.ARGB32, mipChain: false);
		converted3DLut2.SetPixels(array);
		converted3DLut2.Apply();
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

	public global::UnityEngine.Texture3D Convert(global::UnityEngine.Texture2D temp2DTex, global::UnityEngine.Texture3D cv3D)
	{
		int num = 4096;
		if ((bool)temp2DTex)
		{
			num = temp2DTex.width * temp2DTex.height;
			num = temp2DTex.height;
			if (!ValidDimensions(temp2DTex))
			{
				global::UnityEngine.Debug.LogWarning("The given 2D texture " + temp2DTex.name + " cannot be used as a 3D LUT.");
				return cv3D;
			}
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
		if ((bool)cv3D)
		{
			global::UnityEngine.Object.DestroyImmediate(cv3D);
		}
		cv3D = new global::UnityEngine.Texture3D(num, num, num, global::UnityEngine.TextureFormat.ARGB32, mipChain: false);
		cv3D.SetPixels(array);
		cv3D.Apply();
		return cv3D;
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
				if (!LutTexture)
				{
					SetIdentityLut();
				}
				if ((bool)LutTexture)
				{
					converted3DLut = Convert(LutTexture, converted3DLut);
				}
			}
			if (converted3DLut2 == null)
			{
				if (!LutTexture2)
				{
					SetIdentityLut();
				}
				if ((bool)LutTexture2)
				{
					converted3DLut2 = Convert(LutTexture2, converted3DLut2);
				}
			}
			if ((bool)LutTexture)
			{
				converted3DLut.wrapMode = global::UnityEngine.TextureWrapMode.Clamp;
			}
			if ((bool)LutTexture2)
			{
				converted3DLut2.wrapMode = global::UnityEngine.TextureWrapMode.Clamp;
			}
			material.SetFloat("_Blend", Blend);
			material.SetFloat("_Fade", Fade);
			material.SetTexture("_LutTex", converted3DLut);
			material.SetTexture("_LutTex2", converted3DLut2);
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
