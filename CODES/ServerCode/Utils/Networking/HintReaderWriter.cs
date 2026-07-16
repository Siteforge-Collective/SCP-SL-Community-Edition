namespace Utils.Networking
{
	public static class HintReaderWriter
	{
		public enum HintType : byte
		{
			Translation = 0,
			Text = 1
		}

		public static global::Hints.Hint ReadHint(this global::Mirror.NetworkReader reader)
		{
			byte b = reader.ReadByte();
			global::System.Func<global::Mirror.NetworkReader, global::Hints.Hint> func;
			switch ((global::Utils.Networking.HintReaderWriter.HintType)b)
			{
			case global::Utils.Networking.HintReaderWriter.HintType.Text:
				func = global::Hints.TextHint.FromNetwork;
				break;
			case global::Utils.Networking.HintReaderWriter.HintType.Translation:
				func = global::Hints.TranslationHint.FromNetwork;
				break;
			default:
				global::UnityEngine.Debug.LogWarning($"Received malformed hint (type {b}).");
				return null;
			}
			return func(reader);
		}

		public static void WriteHint(this global::Mirror.NetworkWriter writer, global::Hints.Hint hint)
		{
			if (hint == null)
			{
				throw new global::System.ArgumentNullException("hint");
			}
			if (hint != null)
			{
				global::Utils.Networking.HintReaderWriter.HintType value;
				if (!(hint is global::Hints.TranslationHint))
				{
					if (!(hint is global::Hints.TextHint))
					{
						goto IL_002d;
					}
					value = global::Utils.Networking.HintReaderWriter.HintType.Text;
				}
				else
				{
					value = global::Utils.Networking.HintReaderWriter.HintType.Translation;
				}
				writer.WriteByte((byte)value);
				hint.Serialize(writer);
				return;
			}
			goto IL_002d;
			IL_002d:
			throw new global::System.ArgumentException("Hint was of an unknown type. This type should be added to the pattern switch (needed for polymorphism to work).", "hint");
		}
	}
}
