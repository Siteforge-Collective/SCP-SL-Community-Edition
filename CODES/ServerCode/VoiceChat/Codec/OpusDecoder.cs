namespace VoiceChat.Codec
{
	public class OpusDecoder : global::System.IDisposable
	{
		private global::System.IntPtr _handle = global::System.IntPtr.Zero;

		private bool _previousPacketInvalid;

		public OpusDecoder()
		{
			_handle = global::VoiceChat.Codec.OpusWrapper.CreateDecoder(48000, 1);
			if (_handle == global::System.IntPtr.Zero)
			{
				throw new global::VoiceChat.Codec.OpusException(global::VoiceChat.Codec.Enums.OpusStatusCode.AllocFail, "Memory was not allocated for the encoder");
			}
		}

		public int Decode(byte[] packetData, int dataLength, float[] samples)
		{
			if (global::VoiceChat.Codec.OpusWrapper.GetBandwidth(packetData) < 0)
			{
				_previousPacketInvalid = true;
				return global::VoiceChat.Codec.OpusWrapper.Decode(_handle, null, 0, samples, fec: false, 1);
			}
			_previousPacketInvalid = false;
			return global::VoiceChat.Codec.OpusWrapper.Decode(_handle, packetData, dataLength, samples, _previousPacketInvalid, 1);
		}

		public void Dispose()
		{
			if (_handle != global::System.IntPtr.Zero)
			{
				global::VoiceChat.Codec.OpusWrapper.Destroy(_handle);
				_handle = global::System.IntPtr.Zero;
			}
		}
	}
}
