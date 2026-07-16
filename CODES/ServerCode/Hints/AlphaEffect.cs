namespace Hints
{
	public class AlphaEffect : global::Hints.HintEffect
	{
		protected float Alpha { get; private set; }

		public static global::Hints.AlphaEffect FromNetwork(global::Mirror.NetworkReader reader)
		{
			global::Hints.AlphaEffect alphaEffect = new global::Hints.AlphaEffect();
			alphaEffect.Deserialize(reader);
			return alphaEffect;
		}

		private AlphaEffect()
		{
		}

		public AlphaEffect(float alpha, float startScalar = 0f, float durationScalar = 1f)
			: base(startScalar, durationScalar)
		{
			Alpha = alpha;
		}

		public override void Deserialize(global::Mirror.NetworkReader reader)
		{
			base.Deserialize(reader);
			Alpha = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
		}

		public override void Serialize(global::Mirror.NetworkWriter writer)
		{
			base.Serialize(writer);
			global::Mirror.NetworkWriterExtensions.WriteSingle(writer, Alpha);
		}
	}
}
