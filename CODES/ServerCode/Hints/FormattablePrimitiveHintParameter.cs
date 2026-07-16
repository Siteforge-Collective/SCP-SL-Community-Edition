namespace Hints
{
	public abstract class FormattablePrimitiveHintParameter<TValue> : global::Hints.PrimitiveHintParameter<TValue>
	{
		protected string Format { get; private set; }

		protected FormattablePrimitiveHintParameter(global::System.Func<global::Mirror.NetworkReader, TValue> deserializer, global::System.Action<global::Mirror.NetworkWriter, TValue> serializer)
			: base(deserializer, serializer)
		{
		}

		protected FormattablePrimitiveHintParameter(TValue value, string format, global::System.Func<global::Mirror.NetworkReader, TValue> deserializer, global::System.Action<global::Mirror.NetworkWriter, TValue> serializer)
			: base(value, deserializer, serializer)
		{
			Format = format;
		}

		public override void Deserialize(global::Mirror.NetworkReader reader)
		{
			base.Deserialize(reader);
			Format = global::Mirror.NetworkReaderExtensions.ReadString(reader);
		}

		public override void Serialize(global::Mirror.NetworkWriter writer)
		{
			base.Serialize(writer);
			global::Mirror.NetworkWriterExtensions.WriteString(writer, Format);
		}
	}
}
