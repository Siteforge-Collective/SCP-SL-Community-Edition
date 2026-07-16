using UnityEngine;
using VoiceChat;
using VoiceChat.Codec;
using VoiceChat.Playbacks;

namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939VoiceModule : StandardScpVoiceModule
	{
		[SerializeField]
		private SingleBufferPlayback _proximityChat;

		private OpusDecoder _mimicryDecoder;

		private bool _mimicryDecoderSet;

		public const VoiceChatChannel MimicryChannel = VoiceChatChannel.Mimicry;

		protected override OpusDecoder Decoder => null;

		protected override void ProcessSamples(float[] data, int len)
		{
		}

		public override VoiceChatChannel ValidateReceive(ReferenceHub speaker, VoiceChatChannel channel)
		{
			return default(VoiceChatChannel);
		}

		public override VoiceChatChannel ValidateSend(VoiceChatChannel channel)
		{
			return default(VoiceChatChannel);
		}

		public void ClearMimicryPlayback()
		{
		}

		public override void ResetObject()
		{
		}
	}
}
