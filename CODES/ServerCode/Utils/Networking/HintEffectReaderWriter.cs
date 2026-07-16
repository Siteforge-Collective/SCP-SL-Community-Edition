namespace Utils.Networking
{
	public static class HintEffectReaderWriter
	{
		public enum HintEffectType : byte
		{
			Alpha = 0,
			AlphaCurve = 1,
			Outline = 2
		}

		public static global::Hints.HintEffect ReadHintEffect(this global::Mirror.NetworkReader reader)
		{
			byte b = reader.ReadByte();
			global::System.Func<global::Mirror.NetworkReader, global::Hints.HintEffect> func;
			switch ((global::Utils.Networking.HintEffectReaderWriter.HintEffectType)b)
			{
			case global::Utils.Networking.HintEffectReaderWriter.HintEffectType.Alpha:
				func = global::Hints.AlphaEffect.FromNetwork;
				break;
			case global::Utils.Networking.HintEffectReaderWriter.HintEffectType.AlphaCurve:
				func = global::Hints.AlphaCurveHintEffect.FromNetwork;
				break;
			case global::Utils.Networking.HintEffectReaderWriter.HintEffectType.Outline:
				func = global::Hints.OutlineEffect.FromNetwork;
				break;
			default:
				global::UnityEngine.Debug.LogWarning($"Received malformed hint parameter (type {b}).");
				return null;
			}
			return func(reader);
		}

		public static void WriteHintEffect(this global::Mirror.NetworkWriter writer, global::Hints.HintEffect effect)
		{
			if (effect == null)
			{
				throw new global::System.ArgumentNullException("effect");
			}
			if (effect != null)
			{
				global::Utils.Networking.HintEffectReaderWriter.HintEffectType value;
				if (!(effect is global::Hints.AlphaEffect))
				{
					if (!(effect is global::Hints.AlphaCurveHintEffect))
					{
						if (!(effect is global::Hints.OutlineEffect))
						{
							goto IL_0039;
						}
						value = global::Utils.Networking.HintEffectReaderWriter.HintEffectType.Outline;
					}
					else
					{
						value = global::Utils.Networking.HintEffectReaderWriter.HintEffectType.AlphaCurve;
					}
				}
				else
				{
					value = global::Utils.Networking.HintEffectReaderWriter.HintEffectType.Alpha;
				}
				writer.WriteByte((byte)value);
				effect.Serialize(writer);
				return;
			}
			goto IL_0039;
			IL_0039:
			throw new global::System.ArgumentException("Hint effect was of an unknown type. This type should be added to the pattern switch (needed for polymorphism to work).", "effect");
		}
	}
}
