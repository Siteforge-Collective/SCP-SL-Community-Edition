namespace Hints
{
	public abstract class PrimitiveHintParameter<TValue> : global::Hints.HintParameter
	{
		private readonly global::System.Func<global::Mirror.NetworkReader, TValue> _deserializer;

		private readonly global::System.Action<global::Mirror.NetworkWriter, TValue> _serializer;

		private bool _stopFormatting;

		protected TValue Value { get; private set; }

		protected PrimitiveHintParameter(global::System.Func<global::Mirror.NetworkReader, TValue> deserializer, global::System.Action<global::Mirror.NetworkWriter, TValue> serializer)
		{
			_deserializer = deserializer;
			_serializer = serializer;
		}

		protected PrimitiveHintParameter(TValue value, global::System.Func<global::Mirror.NetworkReader, TValue> deserializer, global::System.Action<global::Mirror.NetworkWriter, TValue> serializer)
			: this(deserializer, serializer)
		{
			Value = value;
		}

		public override void Deserialize(global::Mirror.NetworkReader reader)
		{
			Value = _deserializer(reader);
		}

		public override void Serialize(global::Mirror.NetworkWriter writer)
		{
			_serializer(writer, Value);
		}
	}
}
