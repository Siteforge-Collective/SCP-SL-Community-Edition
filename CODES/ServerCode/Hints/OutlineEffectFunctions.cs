namespace Hints
{
	public static class OutlineEffectFunctions
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::Hints.OutlineEffect value)
		{
			value.Serialize(writer);
		}

		public static global::Hints.OutlineEffect Deserialize(this global::Mirror.NetworkReader reader)
		{
			return global::Hints.OutlineEffect.FromNetwork(reader);
		}
	}
}
