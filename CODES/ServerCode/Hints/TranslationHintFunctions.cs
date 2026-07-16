namespace Hints
{
	public static class TranslationHintFunctions
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::Hints.TranslationHint value)
		{
			value.Serialize(writer);
		}

		public static global::Hints.TranslationHint Deserialize(this global::Mirror.NetworkReader reader)
		{
			return global::Hints.TranslationHint.FromNetwork(reader);
		}
	}
}
