using UnityEngine;

[RequireComponent(typeof(AudioLowPassFilter))]
[RequireComponent(typeof(AudioReverbFilter))]
[RequireComponent(typeof(AudioSource))]
public class AudioProximityEffects : MonoBehaviour
{
	[SerializeField]
	private AnimationCurve _reverbSizeOverDistance;

	[SerializeField]
	private AnimationCurve _reverbDryOverDistance;

	[SerializeField]
	private AnimationCurve _lowpassOverDistance;

	private AudioSource _audioSource;

	private AudioReverbFilter _reverbFilter;

	private AudioLowPassFilter _lowPassFilter;

	private float ProximityLevel => 0f;

	private void Awake()
	{
	}

	private void Update()
	{
	}
}
