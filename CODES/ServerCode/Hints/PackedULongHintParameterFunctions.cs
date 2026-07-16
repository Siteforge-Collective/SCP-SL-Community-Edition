namespace Hints
{
	public static class PackedULongHintParameterFunctions
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::Hints.PackedULongHintParameter value)
		{
			value.Serialize(writer);
		}

		public static global::Hints.PackedULongHintParameter Deserialize(this global::Mirror.NetworkReader reader)
		{
			return global::Hints.PackedULongHintParameter.FromNetwork(reader);
		}
	}
}
