namespace Hints
{
	public static class AmmoHintParameterFunctions
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::Hints.AmmoHintParameter value)
		{
			value.Serialize(writer);
		}

		public static global::Hints.AmmoHintParameter Deserialize(this global::Mirror.NetworkReader reader)
		{
			return global::Hints.AmmoHintParameter.FromNetwork(reader);
		}
	}
}
