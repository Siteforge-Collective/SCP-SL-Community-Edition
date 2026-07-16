namespace Hints
{
	public static class UIntHintParameterFunctions
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::Hints.UIntHintParameter value)
		{
			value.Serialize(writer);
		}

		public static global::Hints.UIntHintParameter Deserialize(this global::Mirror.NetworkReader reader)
		{
			return global::Hints.UIntHintParameter.FromNetwork(reader);
		}
	}
}
