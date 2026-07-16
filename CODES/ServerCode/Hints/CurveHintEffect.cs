namespace Hints
{
	public abstract class CurveHintEffect : global::Hints.HintEffect
	{
		private global::UnityEngine.AnimationCurve _curve;

		protected global::UnityEngine.AnimationCurve Curve
		{
			get
			{
				return _curve;
			}
			set
			{
				if (value != null && value.length > 0)
				{
					IterationScalar = value[value.length - 1].time - value[0].time;
					IterationScalar *= IterationScalar;
				}
				else
				{
					IterationScalar = 0f;
				}
				_curve = value;
			}
		}

		protected float IterationScalar { get; private set; }

		protected CurveHintEffect(float startScalar = 0f, float durationScalar = 1f)
			: base(startScalar, durationScalar)
		{
		}

		protected CurveHintEffect(global::UnityEngine.AnimationCurve curve, float startScalar = 0f, float durationScalar = 1f)
			: this(startScalar, durationScalar)
		{
			Curve = curve ?? throw new global::System.ArgumentNullException("curve");
		}

		public override void Deserialize(global::Mirror.NetworkReader reader)
		{
			base.Deserialize(reader);
			Curve = global::Utils.Networking.AnimationCurveReaderWriter.ReadAnimationCurve(reader);
		}

		public override void Serialize(global::Mirror.NetworkWriter writer)
		{
			base.Serialize(writer);
			global::Utils.Networking.AnimationCurveReaderWriter.WriteAnimationCurve(writer, Curve);
		}
	}
}
