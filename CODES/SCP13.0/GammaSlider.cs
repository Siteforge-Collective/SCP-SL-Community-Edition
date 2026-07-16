using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class GammaSlider : MonoBehaviour
{
	public const string GammaConfigKey = "UVBrightness2";

	public const float DefaultBrightness = 0f;

	public const float MinBrightness = -0.7f;

	public const float MaxBrightness = 0f;

	public Slider slider;

	public Text warningText;

	public Text indicatorText;

	public static event Action<float> OnGammaChanged
	{
		[CompilerGenerated]
		add
		{
		}
		[CompilerGenerated]
		remove
		{
		}
	}

	public static float SavedToGui(float intensity)
	{
		return 0f;
	}

	public static float GuiToSaved(float setting)
	{
		return 0f;
	}

	private void Start()
	{
	}

	public void SetValue(float f)
	{
	}
}
