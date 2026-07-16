namespace VoiceChat.Networking
{
	public static class VoiceMessageReadersWriters
	{
		private static readonly byte[] ReceiveArray = new byte[512];

		public static global::VoiceChat.Networking.VoiceMessage DeserializeVoiceMessage(this global::Mirror.NetworkReader reader)
		{
			int value = reader.ReadRecyclablePlayerId().Value;
			global::VoiceChat.VoiceChatChannel channel = (global::VoiceChat.VoiceChatChannel)reader.ReadByte();
			int num = global::Mirror.NetworkReaderExtensions.ReadUInt16(reader);
			reader.ReadBytes(ReceiveArray, num);
			if (value == 0 || !ReferenceHub.TryGetHub(value, out var hub))
			{
				return new global::VoiceChat.Networking.VoiceMessage(null, channel, ReceiveArray, num, isNull: true);
			}
			return new global::VoiceChat.Networking.VoiceMessage(hub, channel, ReceiveArray, num, isNull: false);
		}

		public static void SerializeVoiceMessage(this global::Mirror.NetworkWriter writer, global::VoiceChat.Networking.VoiceMessage msg)
		{
			writer.WriteRecyclablePlayerId(new RecyclablePlayerId(msg.Speaker.PlayerId));
			writer.WriteByte((byte)msg.Channel);
			global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, (ushort)msg.DataLength);
			writer.WriteBytes(msg.Data, 0, msg.DataLength);
		}
	}
}
