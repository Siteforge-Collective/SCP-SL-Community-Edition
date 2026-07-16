namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939VoiceModule : global::PlayerRoles.PlayableScps.StandardScpVoiceModule
	{
		[global::UnityEngine.SerializeField]
		private global::VoiceChat.Playbacks.SingleBufferPlayback _proximityChat;

		private global::VoiceChat.Codec.OpusDecoder _mimicryDecoder;

		private bool _mimicryDecoderSet;

		public const global::VoiceChat.VoiceChatChannel MimicryChannel = global::VoiceChat.VoiceChatChannel.Mimicry;

		protected override global::VoiceChat.Codec.OpusDecoder Decoder
		{
			get
			{
				if (base.CurrentChannel != global::VoiceChat.VoiceChatChannel.Mimicry)
				{
					return base.Decoder;
				}
				if (!_mimicryDecoderSet)
				{
					_mimicryDecoder = new global::VoiceChat.Codec.OpusDecoder();
					_mimicryDecoderSet = true;
				}
				return _mimicryDecoder;
			}
		}

		protected override void ProcessSamples(float[] data, int len)
		{
			if (base.CurrentChannel == global::VoiceChat.VoiceChatChannel.Mimicry)
			{
				_proximityChat.Buffer.Write(data, len);
			}
			else
			{
				base.ProcessSamples(data, len);
			}
		}

		public override global::VoiceChat.VoiceChatChannel ValidateReceive(ReferenceHub speaker, global::VoiceChat.VoiceChatChannel channel)
		{
			if (channel != global::VoiceChat.VoiceChatChannel.Mimicry)
			{
				return base.ValidateReceive(speaker, channel);
			}
			return channel;
		}

		public override global::VoiceChat.VoiceChatChannel ValidateSend(global::VoiceChat.VoiceChatChannel channel)
		{
			if (channel != global::VoiceChat.VoiceChatChannel.Mimicry)
			{
				return base.ValidateSend(channel);
			}
			return channel;
		}

		public override void ResetObject()
		{
			base.ResetObject();
			if (_mimicryDecoderSet)
			{
				_mimicryDecoder?.Dispose();
				_mimicryDecoderSet = false;
			}
		}
	}
}
