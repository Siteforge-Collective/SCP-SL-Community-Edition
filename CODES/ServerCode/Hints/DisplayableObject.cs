namespace Hints
{
	public abstract class DisplayableObject<TData> : global::Hints.NetworkObject<TData>
	{
		public float DurationScalar { get; private set; }

		protected DisplayableObject(float durationScalar = 1f)
		{
			DurationScalar = durationScalar;
		}

		public override void Deserialize(global::Mirror.NetworkReader reader)
		{
			DurationScalar = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
		}

		public override void Serialize(global::Mirror.NetworkWriter writer)
		{
			global::Mirror.NetworkWriterExtensions.WriteSingle(writer, DurationScalar);
		}
	}
}
