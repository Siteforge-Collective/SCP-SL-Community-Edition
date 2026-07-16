namespace Hints
{
	public static class HintMessageParameterFunctions
	{
		private enum HintMessageTypes : byte
		{
			Unknown = 0,
			TextHint = 1,
			TranslationHint = 2
		}

		public static void Serialize(this global::Mirror.NetworkWriter writer, global::Hints.HintMessage value)
		{
			global::Hints.Hint content = value.Content;
			if (content != null)
			{
				if (!(content is global::Hints.TextHint))
				{
					if (!(content is global::Hints.TranslationHint))
					{
						goto IL_002e;
					}
					writer.WriteByte(2);
				}
				else
				{
					writer.WriteByte(1);
				}
				value.Content.Serialize(writer);
				return;
			}
			goto IL_002e;
			IL_002e:
			global::UnityEngine.Debug.LogError("Attempted to serialize an unknown type of HintMessage!");
			writer.WriteByte(0);
		}

		public static global::Hints.HintMessage Deserialize(this global::Mirror.NetworkReader reader)
		{
			global::Hints.Hint content;
			switch ((global::Hints.HintMessageParameterFunctions.HintMessageTypes)reader.ReadByte())
			{
			case global::Hints.HintMessageParameterFunctions.HintMessageTypes.Unknown:
				global::UnityEngine.Debug.LogError("Unknown type of HintMessage has been received!");
				return default(global::Hints.HintMessage);
			case global::Hints.HintMessageParameterFunctions.HintMessageTypes.TextHint:
				content = global::Hints.TextHint.FromNetwork(reader);
				break;
			case global::Hints.HintMessageParameterFunctions.HintMessageTypes.TranslationHint:
				content = global::Hints.TranslationHint.FromNetwork(reader);
				break;
			default:
				global::UnityEngine.Debug.LogError("Invalid type of HintMessage has been received!");
				return default(global::Hints.HintMessage);
			}
			return new global::Hints.HintMessage(content);
		}
	}
}
