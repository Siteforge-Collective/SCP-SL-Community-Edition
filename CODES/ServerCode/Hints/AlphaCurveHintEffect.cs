namespace Hints
{
	public class AlphaCurveHintEffect : global::Hints.CurveHintEffect
	{
		public static global::Hints.AlphaCurveHintEffect FromNetwork(global::Mirror.NetworkReader reader)
		{
			global::Hints.AlphaCurveHintEffect alphaCurveHintEffect = new global::Hints.AlphaCurveHintEffect();
			alphaCurveHintEffect.Deserialize(reader);
			return alphaCurveHintEffect;
		}

		private AlphaCurveHintEffect()
		{
		}

		public AlphaCurveHintEffect(global::UnityEngine.AnimationCurve curve, float startScalar = 0f, float durationScalar = 1f)
			: base(curve, startScalar, durationScalar)
		{
		}
	}
}
