namespace VoiceChat.Codec
{
	public class OpusEncoder : global::System.IDisposable
	{
		private global::System.IntPtr _handle = global::System.IntPtr.Zero;

		public OpusEncoder(global::VoiceChat.Codec.Enums.OpusApplicationType preset)
		{
			_handle = global::VoiceChat.Codec.OpusWrapper.CreateEncoder(48000, 1, preset);
			global::VoiceChat.Codec.OpusWrapper.SetEncoderSetting(_handle, global::VoiceChat.Codec.Enums.OpusCtlSetRequest.Bitrate, 120000);
		}

		public int Encode(float[] pcmSamples, byte[] encoded, int frameSize = 480)
		{
			return global::VoiceChat.Codec.OpusWrapper.Encode(_handle, pcmSamples, frameSize, encoded);
		}

		public void Dispose()
		{
			if (!(_handle == global::System.IntPtr.Zero))
			{
				global::VoiceChat.Codec.OpusWrapper.Destroy(_handle);
				_handle = global::System.IntPtr.Zero;
			}
		}
	}
}
