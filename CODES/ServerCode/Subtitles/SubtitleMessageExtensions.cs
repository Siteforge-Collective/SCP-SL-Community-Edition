namespace Subtitles
{
	public static class SubtitleMessageExtensions
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::Subtitles.SubtitleMessage value)
		{
			global::Subtitles.SubtitlePart[] subtitleParts = value.SubtitleParts;
			if (subtitleParts == null || subtitleParts.Length == 0)
			{
				return;
			}
			writer.WriteByte((byte)subtitleParts.Length);
			for (int i = 0; i < subtitleParts.Length; i++)
			{
				global::Subtitles.SubtitlePart subtitlePart = subtitleParts[i];
				writer.WriteByte((byte)((subtitlePart.OptionalData != null) ? ((byte)subtitlePart.OptionalData.Length) : 0));
				if (subtitlePart.OptionalData != null && subtitlePart.OptionalData.Length != 0)
				{
					for (int j = 0; j < subtitlePart.OptionalData.Length; j++)
					{
						global::Mirror.NetworkWriterExtensions.WriteString(writer, subtitlePart.OptionalData[j]);
					}
				}
				writer.WriteByte((byte)subtitlePart.Subtitle);
			}
		}

		public static global::Subtitles.SubtitleMessage Deserialize(this global::Mirror.NetworkReader reader)
		{
			int num = reader.ReadByte();
			global::Subtitles.SubtitlePart[] array = new global::Subtitles.SubtitlePart[num];
			for (int i = 0; i < num; i++)
			{
				int num2 = reader.ReadByte();
				string[] array2 = new string[num2];
				for (int j = 0; j < num2; j++)
				{
					array2[j] = global::Mirror.NetworkReaderExtensions.ReadString(reader);
				}
				array[i] = new global::Subtitles.SubtitlePart((global::Subtitles.SubtitleType)reader.ReadByte(), (num2 == 0) ? null : array2);
			}
			return new global::Subtitles.SubtitleMessage
			{
				SubtitleParts = array
			};
		}

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			CustomNetworkManager.OnClientReady += RegisterHandlers;
		}

		private static void RegisterHandlers()
		{
			global::Mirror.NetworkClient.ReplaceHandler<global::Subtitles.SubtitleMessage>(ClientMessageReceived);
		}

		private static void ClientMessageReceived(global::Subtitles.SubtitleMessage msg)
		{
		}
	}
}
