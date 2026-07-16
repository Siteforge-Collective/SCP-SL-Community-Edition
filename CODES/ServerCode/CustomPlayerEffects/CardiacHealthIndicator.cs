namespace CustomPlayerEffects
{
	public class CardiacHealthIndicator : global::CustomPlayerEffects.SubEffectBase
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _healthToWeight;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Rendering.PostProcessing.PostProcessVolume _ppv;

		[global::UnityEngine.SerializeField]
		private float _speedMultiplier;

		public override bool IsActive => base.MainEffect.IsEnabled;

		public override void DisableEffect()
		{
			base.DisableEffect();
		}

		internal override void UpdateEffect()
		{
			base.UpdateEffect();
		}

		internal override void Init(global::CustomPlayerEffects.StatusEffectBase mainEffect)
		{
			base.Init(mainEffect);
		}
	}
}
