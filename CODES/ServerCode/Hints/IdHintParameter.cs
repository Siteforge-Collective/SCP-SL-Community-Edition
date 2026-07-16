namespace Hints
{
	public abstract class IdHintParameter : global::Hints.HintParameter
	{
		protected int Id { get; private set; }

		protected IdHintParameter()
		{
		}

		protected IdHintParameter(byte id)
		{
			Id = id;
		}

		public override void Deserialize(global::Mirror.NetworkReader reader)
		{
			Id = global::Mirror.NetworkReaderExtensions.ReadInt32(reader);
		}

		public override void Serialize(global::Mirror.NetworkWriter writer)
		{
			global::Mirror.NetworkWriterExtensions.WriteInt32(writer, Id);
		}
	}
}
