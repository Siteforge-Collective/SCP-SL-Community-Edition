using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class LightBlink : MonoBehaviour
{
	public float noshadowIntensMultiplier;

	public float minFlickerTimeRange;

	public float maxFlickerTimeRange;

	public float turnOnSpeed;

	public float maxIntensityDecreaseMultiplier;

	[Header("Used to flicker multiple lights at the same time.")]
	[Header("Leave empty to just use the attached light.")]
	public HDAdditionalLightData[] lightGroup;

	private float[] _startLightIntensity;

	private HDAdditionalLightData _defaultLight;

	private float _frequency;

	private RoomLightController _controller;

	private bool _hasParentController;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
