namespace Hints
{
	public abstract class HintEffect : global::Hints.DisplayableObject<global::Hints.SharedHintData>
	{
		public float StartScalar { get; private set; }

		protected HintEffect(float startScalar = 0f, float durationScalar = 1f)
			: base(durationScalar)
		{
			StartScalar = startScalar;
		}

		public override void Deserialize(global::Mirror.NetworkReader reader)
		{
			base.Deserialize(reader);
			StartScalar = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
		}

		public override void Serialize(global::Mirror.NetworkWriter writer)
		{
			base.Serialize(writer);
			global::Mirror.NetworkWriterExtensions.WriteSingle(writer, StartScalar);
		}
	}
}
