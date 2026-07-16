using GameObjectPools;
using UnityEngine;

namespace InventorySystem.Items.ToggleableLights.Lantern
{
	public class LanternLightManager : MonoBehaviour, IPoolResettable
	{
		private const float MinSquaredDistanceMovedToProduceSound = 900f;

		private static readonly int EmissionId;

		private MaterialPropertyBlock _mpb;

		public Light[] Lights;

		public Renderer MainRenderer;

		public Color EmissionColor;

		public ParticleSystem ParticleSystem;

		public float LightSpeed;

		public AudioClip ForwardSound;

		public AudioClip BackwardSound;

		public Rigidbody SoundTriggerRb;

		public float Forward;

		public float Backward;

		public float ForwardRad;

		public float BackwardRad;

		public float ForwardVolume;

		public float BackwardVolume;

		public float VolumeModifier;

		private float _previousRot;

		private AudioSource _forwardSound;

		private AudioSource _backwardSound;

		private LightBlink _blinker;

		private float[] _lightRanges;

		private float _enableRatio;

		private bool _initialized;

		private Vector3 _oldPosition;

		public bool IsEnabled { get; private set; }

		private float AngularMag => 0f;

		private static bool AudioSourceIsPlaying(AudioSource source)
		{
			return false;
		}

		private void AudioUpdate()
		{
		}

		private void Awake()
		{
		}

		public void SetLight(bool isEnabled)
		{
		}

		private void Update()
		{
		}

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		public void ResetObject()
		{
		}
	}
}
