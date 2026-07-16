namespace Hints
{
	public static class Scp330HintParameterFunctions
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::Hints.Scp330HintParameter value)
		{
			value.Serialize(writer);
		}

		public static global::Hints.Scp330HintParameter Deserialize(this global::Mirror.NetworkReader reader)
		{
			return global::Hints.Scp330HintParameter.FromNetwork(reader);
		}
	}
}
