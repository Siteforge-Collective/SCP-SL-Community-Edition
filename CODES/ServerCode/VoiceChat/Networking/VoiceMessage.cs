namespace VoiceChat.Networking
{
	public struct VoiceMessage : global::Mirror.NetworkMessage
	{
		public ReferenceHub Speaker;

		public global::VoiceChat.VoiceChatChannel Channel;

		public int DataLength;

		public byte[] Data;

		public bool SpeakerNull;

		public VoiceMessage(ReferenceHub ply, global::VoiceChat.VoiceChatChannel channel, byte[] data, int dataLen, bool isNull)
		{
			Speaker = ply;
			Channel = channel;
			Data = data;
			DataLength = dataLen;
			SpeakerNull = isNull;
		}
	}
}
