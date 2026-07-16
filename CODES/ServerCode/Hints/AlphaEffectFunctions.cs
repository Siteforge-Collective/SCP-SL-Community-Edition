namespace Hints
{
	public static class AlphaEffectFunctions
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::Hints.AlphaEffect value)
		{
			value.Serialize(writer);
		}

		public static global::Hints.AlphaEffect Deserialize(this global::Mirror.NetworkReader reader)
		{
			return global::Hints.AlphaEffect.FromNetwork(reader);
		}
	}
}
