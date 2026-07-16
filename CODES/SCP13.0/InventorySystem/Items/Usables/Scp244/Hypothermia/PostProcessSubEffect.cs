using System;
using CustomPlayerEffects;
using CustomRendering;
using UnityEngine;
using UnityEngine.Rendering;

namespace InventorySystem.Items.Usables.Scp244.Hypothermia
{
	public class PostProcessSubEffect : HypothermiaSubEffectBase
	{
		[Serializable]
		private struct IntensityOverTemp
		{
			[SerializeField]
			private AnimationCurve _effectCurve;

			[SerializeField]
			private float _scpIntensityMultiplier;

			internal float GetValue(PostProcessSubEffect fx)
			{
				return 0f;
			}
		}

		[SerializeField]
		private IntensityOverTemp _weightCurve;

		[SerializeField]
		private IntensityOverTemp _refCurve;

		[SerializeField]
		private IntensityOverTemp _intenCurve;

		[SerializeField]
		private IntensityOverTemp _frostCurve;

		private Volume _ppv;

		private VignetteRefraction _refraction;

		private bool _isAlive;

		private bool _isSCP;

		private float _temperature;

		[SerializeField]
		private TemperatureSubEffect _temp;

		public override bool IsActive => false;

		internal override void UpdateEffect(float curExposure)
		{
		}

		public override void DisableEffect()
		{
		}

		internal override void Init(StatusEffectBase mainEffect)
		{
		}
	}
}
