using CustomRendering;
using Mirror;
using UnityEngine;

public class BloodEffectsSystem : MonoBehaviour
{
	public static BloodEffectsSystem LocalPlayerSingleton;

	public float HealthLerpSpeed;

	public float PulseBloodDeductSpeed;

	public float ScrapeBloodDeductSpeed;

	public AnimationCurve grayscaleAnimationCurve;

	public AnimationCurve vignetteAnimationCurve;

	private BloodHit _bloodHit;

	private Grayscale _bloodGrayScale;

	private float _dyingRatio;

	private bool _instantUpdate;

	private int _fullScapeRoundRobin;

	public bool DisableHurtEffect { get; set; }

	private void Awake()
	{
	}

	[Client]
	public void AddPulseBlood(Vector3 hitDir, float opacityMultiplier = 1f)
	{
	}

	[Client]
	public void AddScrapeBlood(Vector3 hitDir, float opacityMultiplier = 1f)
	{
	}

	[Client]
	public void AddScrapes(float opacity)
	{
	}

	[Client]
	public void ResetScrapes()
	{
	}

	private void FadePulseBlood(float speed)
	{
	}

	private void FadeScrapeBlood(float speed)
	{
	}

	private void Update()
	{
	}

	private float GetDeadAmount(ReferenceHub hub)
	{
		return 0f;
	}
}
