[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/Blend 2 Camera/PhotoshopFilters")]
public class CameraFilterPack_Blend2Camera_PhotoshopFilters : global::UnityEngine.MonoBehaviour
{
	public enum filters
	{
		Darken = 0,
		Multiply = 1,
		ColorBurn = 2,
		LinearBurn = 3,
		DarkerColor = 4,
		Lighten = 5,
		Screen = 6,
		ColorDodge = 7,
		LinearDodge = 8,
		LighterColor = 9,
		Overlay = 10,
		SoftLight = 11,
		HardLight = 12,
		VividLight = 13,
		LinearLight = 14,
		PinLight = 15,
		HardMix = 16,
		Difference = 17,
		Exclusion = 18,
		Subtract = 19,
		Divide = 20,
		Hue = 21,
		Saturation = 22,
		Color = 23,
		Luminosity = 24
	}

	private string ShaderName = "CameraFilterPack/Blend2Camera_Darken";

	public global::UnityEngine.Shader SCShader;

	public global::UnityEngine.Camera Camera2;

	public CameraFilterPack_Blend2Camera_PhotoshopFilters.filters filterchoice;

	private CameraFilterPack_Blend2Camera_PhotoshopFilters.filters filterchoicememo;

	private float TimeX = 1f;

	private global::UnityEngine.Material SCMaterial;

	[global::UnityEngine.Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	[global::UnityEngine.Range(0f, 1f)]
	public float BlendFX = 0.5f;

	private global::UnityEngine.RenderTexture Camera2tex;

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

	private void ChangeFilters()
	{
		switch (filterchoice)
		{
		case CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Darken:
			ShaderName = "CameraFilterPack/Blend2Camera_Darken";
			break;
		case CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Multiply:
			ShaderName = "CameraFilterPack/Blend2Camera_Multiply";
			break;
		case CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.ColorBurn:
			ShaderName = "CameraFilterPack/Blend2Camera_ColorBurn";
			break;
		case CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.LinearBurn:
			ShaderName = "CameraFilterPack/Blend2Camera_LinearBurn";
			break;
		case CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.DarkerColor:
			ShaderName = "CameraFilterPack/Blend2Camera_DarkerColor";
			break;
		case CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Lighten:
			ShaderName = "CameraFilterPack/Blend2Camera_Lighten";
			break;
		case CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Screen:
			ShaderName = "CameraFilterPack/Blend2Camera_Screen";
			break;
		case CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.ColorDodge:
			ShaderName = "CameraFilterPack/Blend2Camera_ColorDodge";
			break;
		case CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.LinearDodge:
			ShaderName = "CameraFilterPack/Blend2Camera_LinearDodge";
			break;
		case CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.LighterColor:
			ShaderName = "CameraFilterPack/Blend2Camera_LighterColor";
			break;
		case CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Overlay:
			ShaderName = "CameraFilterPack/Blend2Camera_Overlay";
			break;
		case CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.SoftLight:
			ShaderName = "CameraFilterPack/Blend2Camera_SoftLight";
			break;
		case CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.HardLight:
			ShaderName = "CameraFilterPack/Blend2Camera_HardLight";
			break;
		case CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.VividLight:
			ShaderName = "CameraFilterPack/Blend2Camera_VividLight";
			break;
		case CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.LinearLight:
			ShaderName = "CameraFilterPack/Blend2Camera_LinearLight";
			break;
		case CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.PinLight:
			ShaderName = "CameraFilterPack/Blend2Camera_PinLight";
			break;
		case CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.HardMix:
			ShaderName = "CameraFilterPack/Blend2Camera_HardMix";
			break;
		case CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Difference:
			ShaderName = "CameraFilterPack/Blend2Camera_Difference";
			break;
		case CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Exclusion:
			ShaderName = "CameraFilterPack/Blend2Camera_Exclusion";
			break;
		case CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Subtract:
			ShaderName = "CameraFilterPack/Blend2Camera_Subtract";
			break;
		case CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Divide:
			ShaderName = "CameraFilterPack/Blend2Camera_Divide";
			break;
		case CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Hue:
			ShaderName = "CameraFilterPack/Blend2Camera_Hue";
			break;
		case CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Saturation:
			ShaderName = "CameraFilterPack/Blend2Camera_Saturation";
			break;
		case CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Color:
			ShaderName = "CameraFilterPack/Blend2Camera_Color";
			break;
		case CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Luminosity:
			ShaderName = "CameraFilterPack/Blend2Camera_Luminosity";
			break;
		}
	}

	private void Start()
	{
		filterchoicememo = filterchoice;
		if (Camera2 != null)
		{
			Camera2tex = new global::UnityEngine.RenderTexture(global::UnityEngine.Screen.width, global::UnityEngine.Screen.height, 24);
			Camera2.targetTexture = Camera2tex;
		}
		ChangeFilters();
		SCShader = global::UnityEngine.Shader.Find(ShaderName);
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
			if (Camera2 != null)
			{
				material.SetTexture("_MainTex2", Camera2tex);
			}
			material.SetFloat("_TimeX", TimeX);
			material.SetFloat("_Value", BlendFX);
			material.SetFloat("_Value2", SwitchCameraToCamera2);
			material.SetVector("_ScreenResolution", new global::UnityEngine.Vector4(sourceTexture.width, sourceTexture.height, 0f, 0f));
			global::UnityEngine.Graphics.Blit(sourceTexture, destTexture, material);
		}
		else
		{
			global::UnityEngine.Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void OnValidate()
	{
		if (filterchoice != filterchoicememo)
		{
			ChangeFilters();
			SCShader = global::UnityEngine.Shader.Find(ShaderName);
			global::UnityEngine.Object.DestroyImmediate(SCMaterial);
			if (SCMaterial == null)
			{
				SCMaterial = new global::UnityEngine.Material(SCShader);
				SCMaterial.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			}
		}
		filterchoicememo = filterchoice;
		if (Camera2 != null)
		{
			Camera2tex = new global::UnityEngine.RenderTexture(global::UnityEngine.Screen.width, global::UnityEngine.Screen.height, 24);
			Camera2.targetTexture = Camera2tex;
		}
	}

	private void Update()
	{
	}

	private void OnEnable()
	{
		if (Camera2 != null)
		{
			Camera2tex = new global::UnityEngine.RenderTexture(global::UnityEngine.Screen.width, global::UnityEngine.Screen.height, 24);
			Camera2.targetTexture = Camera2tex;
		}
	}

	private void OnDisable()
	{
		if (Camera2 != null)
		{
			Camera2.targetTexture = null;
		}
		if ((bool)SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(SCMaterial);
		}
	}
}
