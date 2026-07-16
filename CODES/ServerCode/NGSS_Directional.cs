[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.Light))]
[global::UnityEngine.ExecuteInEditMode]
public class NGSS_Directional : global::UnityEngine.MonoBehaviour
{
	public enum SAMPLER_COUNT
	{
		SAMPLERS_16 = 0,
		SAMPLERS_25 = 1,
		SAMPLERS_32 = 2,
		SAMPLERS_64 = 3
	}

	[global::UnityEngine.Tooltip("Optimize shadows performance by skipping fragments that are 100% lit or 100% shadowed. Some tiny noisy artefacts can be seen if shadows are too soft.")]
	public bool EARLY_BAILOUT_OPTIMIZATION = true;

	[global::UnityEngine.Tooltip("Help with bias problems but can leads to some noisy artefacts if Early Bailout Optimization is enabled.\nRequires PCSS in order to work.")]
	public bool USE_BIAS_FADE;

	[global::UnityEngine.Tooltip("Provides Area Light like soft-shadows. With shadows being harder at close ranges and softer at long ranges.\nDisable it if you are looking for uniformly simple soft-shadows.")]
	public bool PCSS_ENABLED = true;

	private bool PCSS_SWITCH = true;

	[global::UnityEngine.Tooltip("Overall softness for both PCF and PCSS shadows.\nRecommended value: 0.01.")]
	[global::UnityEngine.Range(0f, 0.02f)]
	public float PCSS_GLOBAL_SOFTNESS = 0.01f;

	[global::UnityEngine.Tooltip("PCSS softness when shadows is close to caster.\nRecommended value: 0.05.")]
	[global::UnityEngine.Range(0f, 1f)]
	public float PCSS_FILTER_DIR_MIN = 0.05f;

	[global::UnityEngine.Tooltip("PCSS softness when shadows is far from caster.\nRecommended value: 0.25.\nIf too high can lead to visible artifacts when early bailout is enabled.")]
	[global::UnityEngine.Range(0f, 0.5f)]
	public float PCSS_FILTER_DIR_MAX = 0.25f;

	[global::UnityEngine.Tooltip("Amount of banding or noise. Example: 0.0 gives 100 % Banding and 10.0 gives 100 % Noise.")]
	[global::UnityEngine.Range(0f, 10f)]
	public float BANDING_NOISE_AMOUNT = 1f;

	[global::UnityEngine.Tooltip("Recommended values: Mobile = 16, Consoles = 25, Desktop Low = 32, Desktop High = 64")]
	public NGSS_Directional.SAMPLER_COUNT SAMPLERS_COUNT = NGSS_Directional.SAMPLER_COUNT.SAMPLERS_64;

	private bool isInitialized;

	private bool isGraphicSet;

	private global::UnityEngine.Light m_Light;

	private global::UnityEngine.Rendering.CommandBuffer rawShadowDepthCB;

	private global::UnityEngine.RenderTexture m_ShadowmapCopy;

	private void OnDestroy()
	{
		if (isGraphicSet)
		{
			isGraphicSet = false;
			global::UnityEngine.Rendering.GraphicsSettings.SetCustomShader(global::UnityEngine.Rendering.BuiltinShaderType.ScreenSpaceShadows, global::UnityEngine.Shader.Find("Hidden/Internal-ScreenSpaceShadows"));
			global::UnityEngine.Rendering.GraphicsSettings.SetShaderMode(global::UnityEngine.Rendering.BuiltinShaderType.ScreenSpaceShadows, global::UnityEngine.Rendering.BuiltinShaderMode.UseBuiltin);
		}
		RemoveCommandBuffers();
	}

	private void OnDisable()
	{
		if (isGraphicSet)
		{
			isGraphicSet = false;
			global::UnityEngine.Rendering.GraphicsSettings.SetCustomShader(global::UnityEngine.Rendering.BuiltinShaderType.ScreenSpaceShadows, global::UnityEngine.Shader.Find("Hidden/Internal-ScreenSpaceShadows"));
			global::UnityEngine.Rendering.GraphicsSettings.SetShaderMode(global::UnityEngine.Rendering.BuiltinShaderType.ScreenSpaceShadows, global::UnityEngine.Rendering.BuiltinShaderMode.UseBuiltin);
		}
		RemoveCommandBuffers();
	}

	private void OnApplicationQuit()
	{
		if (isGraphicSet)
		{
			isGraphicSet = false;
			global::UnityEngine.Rendering.GraphicsSettings.SetCustomShader(global::UnityEngine.Rendering.BuiltinShaderType.ScreenSpaceShadows, global::UnityEngine.Shader.Find("Hidden/Internal-ScreenSpaceShadows"));
			global::UnityEngine.Rendering.GraphicsSettings.SetShaderMode(global::UnityEngine.Rendering.BuiltinShaderType.ScreenSpaceShadows, global::UnityEngine.Rendering.BuiltinShaderMode.UseBuiltin);
		}
		RemoveCommandBuffers();
	}

	private void RemoveCommandBuffers()
	{
		if (isInitialized)
		{
			m_Light.RemoveCommandBuffer(global::UnityEngine.Rendering.LightEvent.AfterShadowMap, rawShadowDepthCB);
			m_ShadowmapCopy = null;
			isInitialized = false;
		}
	}

	private void OnEnable()
	{
		Init();
	}

	private void Init()
	{
		if (isInitialized)
		{
			return;
		}
		if (!isGraphicSet)
		{
			isGraphicSet = true;
			global::UnityEngine.Rendering.GraphicsSettings.SetShaderMode(global::UnityEngine.Rendering.BuiltinShaderType.ScreenSpaceShadows, global::UnityEngine.Rendering.BuiltinShaderMode.UseCustom);
			global::UnityEngine.Rendering.GraphicsSettings.SetCustomShader(global::UnityEngine.Rendering.BuiltinShaderType.ScreenSpaceShadows, global::UnityEngine.Shader.Find("Hidden/NGSS_Directional"));
		}
		if (!PCSS_ENABLED)
		{
			return;
		}
		m_Light = GetComponent<global::UnityEngine.Light>();
		int num = ((global::UnityEngine.QualitySettings.shadowResolution == global::UnityEngine.ShadowResolution.VeryHigh) ? 4096 : ((global::UnityEngine.QualitySettings.shadowResolution == global::UnityEngine.ShadowResolution.High) ? 2048 : ((global::UnityEngine.QualitySettings.shadowResolution == global::UnityEngine.ShadowResolution.Medium) ? 1024 : 512)));
		m_ShadowmapCopy = null;
		m_ShadowmapCopy = new global::UnityEngine.RenderTexture(num, num, 0, global::UnityEngine.RenderTextureFormat.RFloat);
		m_ShadowmapCopy.filterMode = global::UnityEngine.FilterMode.Bilinear;
		m_ShadowmapCopy.useMipMap = false;
		rawShadowDepthCB = new global::UnityEngine.Rendering.CommandBuffer
		{
			name = "NGSS Directional PCSS buffer"
		};
		rawShadowDepthCB.Clear();
		rawShadowDepthCB.SetShadowSamplingMode(global::UnityEngine.Rendering.BuiltinRenderTextureType.CurrentActive, global::UnityEngine.Rendering.ShadowSamplingMode.RawDepth);
		rawShadowDepthCB.Blit(global::UnityEngine.Rendering.BuiltinRenderTextureType.CurrentActive, m_ShadowmapCopy);
		rawShadowDepthCB.SetGlobalTexture("NGSS_DirectionalRawDepth", m_ShadowmapCopy);
		global::UnityEngine.Rendering.CommandBuffer[] commandBuffers = m_Light.GetCommandBuffers(global::UnityEngine.Rendering.LightEvent.AfterShadowMap);
		for (int i = 0; i < commandBuffers.Length; i++)
		{
			if (commandBuffers[i].name == rawShadowDepthCB.name)
			{
				isInitialized = true;
				return;
			}
		}
		m_Light.AddCommandBuffer(global::UnityEngine.Rendering.LightEvent.AfterShadowMap, rawShadowDepthCB);
		isInitialized = true;
	}

	private void Update()
	{
		if (PCSS_ENABLED != PCSS_SWITCH)
		{
			PCSS_SWITCH = !PCSS_SWITCH;
			if (PCSS_ENABLED)
			{
				isInitialized = false;
				Init();
			}
			else
			{
				RemoveCommandBuffers();
			}
		}
		SetGlobalSettings();
	}

	private void SetGlobalSettings()
	{
		global::UnityEngine.Shader.SetGlobalFloat("NGSS_PCSS_GLOBAL_SOFTNESS", PCSS_GLOBAL_SOFTNESS);
		global::UnityEngine.Shader.SetGlobalFloat("NGSS_PCSS_FILTER_DIR_MIN", (PCSS_FILTER_DIR_MIN > PCSS_FILTER_DIR_MAX) ? PCSS_FILTER_DIR_MAX : PCSS_FILTER_DIR_MIN);
		global::UnityEngine.Shader.SetGlobalFloat("NGSS_PCSS_FILTER_DIR_MAX", (PCSS_FILTER_DIR_MAX < PCSS_FILTER_DIR_MIN) ? PCSS_FILTER_DIR_MIN : PCSS_FILTER_DIR_MAX);
		global::UnityEngine.Shader.SetGlobalFloat("NGSS_POISSON_SAMPLING_NOISE_DIR", BANDING_NOISE_AMOUNT);
		if (PCSS_ENABLED)
		{
			global::UnityEngine.Shader.EnableKeyword("NGSS_PCSS_FILTER_DIR");
		}
		else
		{
			global::UnityEngine.Shader.DisableKeyword("NGSS_PCSS_FILTER_DIR");
		}
		if (EARLY_BAILOUT_OPTIMIZATION)
		{
			global::UnityEngine.Shader.EnableKeyword("NGSS_USE_EARLY_BAILOUT_OPTIMIZATION_DIR");
		}
		else
		{
			global::UnityEngine.Shader.DisableKeyword("NGSS_USE_EARLY_BAILOUT_OPTIMIZATION_DIR");
		}
		if (USE_BIAS_FADE)
		{
			global::UnityEngine.Shader.EnableKeyword("NGSS_USE_BIAS_FADE_DIR");
		}
		else
		{
			global::UnityEngine.Shader.DisableKeyword("NGSS_USE_BIAS_FADE_DIR");
		}
		global::UnityEngine.Shader.DisableKeyword("DIR_POISSON_64");
		global::UnityEngine.Shader.DisableKeyword("DIR_POISSON_32");
		global::UnityEngine.Shader.DisableKeyword("DIR_POISSON_25");
		global::UnityEngine.Shader.DisableKeyword("DIR_POISSON_16");
		global::UnityEngine.Shader.EnableKeyword((SAMPLERS_COUNT == NGSS_Directional.SAMPLER_COUNT.SAMPLERS_64) ? "DIR_POISSON_64" : ((SAMPLERS_COUNT == NGSS_Directional.SAMPLER_COUNT.SAMPLERS_32) ? "DIR_POISSON_32" : ((SAMPLERS_COUNT == NGSS_Directional.SAMPLER_COUNT.SAMPLERS_25) ? "DIR_POISSON_25" : "DIR_POISSON_16")));
	}
}
