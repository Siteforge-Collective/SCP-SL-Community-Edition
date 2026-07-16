namespace Hints
{
	public static class TextHintFunctions
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::Hints.TextHint value)
		{
			value.Serialize(writer);
		}

		public static global::Hints.TextHint Deserialize(this global::Mirror.NetworkReader reader)
		{
			return global::Hints.TextHint.FromNetwork(reader);
		}
	}
}
