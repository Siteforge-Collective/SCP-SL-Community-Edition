namespace Hints
{
	public static class PackedLongHintParameterFunctions
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::Hints.PackedLongHintParameter value)
		{
			value.Serialize(writer);
		}

		public static global::Hints.PackedLongHintParameter Deserialize(this global::Mirror.NetworkReader reader)
		{
			return global::Hints.PackedLongHintParameter.FromNetwork(reader);
		}
	}
}
