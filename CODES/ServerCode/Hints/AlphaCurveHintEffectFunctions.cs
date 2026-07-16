namespace Hints
{
	public static class AlphaCurveHintEffectFunctions
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::Hints.AlphaCurveHintEffect value)
		{
			value.Serialize(writer);
		}

		public static global::Hints.AlphaCurveHintEffect Deserialize(this global::Mirror.NetworkReader reader)
		{
			return global::Hints.AlphaCurveHintEffect.FromNetwork(reader);
		}
	}
}
