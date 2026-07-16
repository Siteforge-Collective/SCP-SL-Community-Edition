using System.Diagnostics;
using CustomPlayerEffects;
using UnityEngine;

namespace InventorySystem.Items.Usables.Scp244.Hypothermia
{
	public class AudioSubEffect : HypothermiaSubEffectBase, ISoundtrackMutingEffect
	{
		[SerializeField]
		private TemperatureSubEffect _temperature;

		[SerializeField]
		private AudioSource _fogSoundtrack;

		[SerializeField]
		private float _soundtrackFadeSpeed;

		[SerializeField]
		private AudioClip _enterFogSound;

		[SerializeField]
		private AnimationCurve _shakingOverTemperature;

		[SerializeField]
		private AudioSource _shakingSoundSource;

		[SerializeField]
		private float _thirdpersonShakeVolume;

		private bool _prevExposed;

		private readonly Stopwatch _enterSfxCooldown;

		private const float SfxDistance = 10f;

		private const float SfxCooldown = 1.5f;

		public bool MuteSoundtrack { get; private set; }

		public override bool IsActive => false;

		private void UpdateShake(float curTemp)
		{
		}

		private void UpdateExposure(bool isExposed)
		{
		}

		internal override void UpdateEffect(float curExposure)
		{
		}
	}
}
