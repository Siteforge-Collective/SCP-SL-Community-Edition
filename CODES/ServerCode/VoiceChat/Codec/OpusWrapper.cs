namespace VoiceChat.Codec
{
	internal class OpusWrapper
	{
		private const string DllName = "libopus-0";

		[global::System.Runtime.InteropServices.DllImport("libopus-0", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, CharSet = global::System.Runtime.InteropServices.CharSet.Ansi)]
		private static extern int opus_encoder_get_size(int channels);

		[global::System.Runtime.InteropServices.DllImport("libopus-0", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, CharSet = global::System.Runtime.InteropServices.CharSet.Ansi)]
		private static extern global::VoiceChat.Codec.Enums.OpusStatusCode opus_encoder_init(global::System.IntPtr st, int fs, int channels, global::VoiceChat.Codec.Enums.OpusApplicationType application);

		[global::System.Runtime.InteropServices.DllImport("libopus-0", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, CharSet = global::System.Runtime.InteropServices.CharSet.Ansi)]
		private static extern string opus_get_version_string();

		[global::System.Runtime.InteropServices.DllImport("libopus-0", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, CharSet = global::System.Runtime.InteropServices.CharSet.Ansi)]
		private static extern int opus_encode_float(global::System.IntPtr st, float[] pcm, int frame_size, byte[] data, int max_data_bytes);

		[global::System.Runtime.InteropServices.DllImport("libopus-0", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, CharSet = global::System.Runtime.InteropServices.CharSet.Ansi)]
		private static extern int opus_encoder_ctl(global::System.IntPtr st, global::VoiceChat.Codec.Enums.OpusCtlSetRequest request, int value);

		[global::System.Runtime.InteropServices.DllImport("libopus-0", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, CharSet = global::System.Runtime.InteropServices.CharSet.Ansi)]
		private static extern int opus_encoder_ctl(global::System.IntPtr st, global::VoiceChat.Codec.Enums.OpusCtlGetRequest request, ref int value);

		[global::System.Runtime.InteropServices.DllImport("libopus-0", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, CharSet = global::System.Runtime.InteropServices.CharSet.Ansi)]
		private static extern int opus_encode(global::System.IntPtr st, short[] pcm, int frame_size, byte[] data, int max_data_bytes);

		[global::System.Runtime.InteropServices.DllImport("libopus-0", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, CharSet = global::System.Runtime.InteropServices.CharSet.Ansi)]
		private static extern int opus_decoder_get_size(int channels);

		[global::System.Runtime.InteropServices.DllImport("libopus-0", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, CharSet = global::System.Runtime.InteropServices.CharSet.Ansi)]
		private static extern global::VoiceChat.Codec.Enums.OpusStatusCode opus_decoder_init(global::System.IntPtr st, int fr, int channels);

		[global::System.Runtime.InteropServices.DllImport("libopus-0", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, CharSet = global::System.Runtime.InteropServices.CharSet.Ansi)]
		private static extern int opus_decode(global::System.IntPtr st, byte[] data, int len, short[] pcm, int frame_size, int decode_fec);

		[global::System.Runtime.InteropServices.DllImport("libopus-0", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, CharSet = global::System.Runtime.InteropServices.CharSet.Ansi)]
		private static extern int opus_decode_float(global::System.IntPtr st, byte[] data, int len, float[] pcm, int frame_size, int decode_fec);

		[global::System.Runtime.InteropServices.DllImport("libopus-0", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, CharSet = global::System.Runtime.InteropServices.CharSet.Ansi)]
		private static extern int opus_decode(global::System.IntPtr st, global::System.IntPtr data, int len, short[] pcm, int frame_size, int decode_fec);

		[global::System.Runtime.InteropServices.DllImport("libopus-0", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, CharSet = global::System.Runtime.InteropServices.CharSet.Ansi)]
		private static extern int opus_decode_float(global::System.IntPtr st, global::System.IntPtr data, int len, float[] pcm, int frame_size, int decode_fec);

		[global::System.Runtime.InteropServices.DllImport("libopus-0", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, CharSet = global::System.Runtime.InteropServices.CharSet.Ansi)]
		private static extern int opus_packet_get_bandwidth(byte[] data);

		[global::System.Runtime.InteropServices.DllImport("libopus-0", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, CharSet = global::System.Runtime.InteropServices.CharSet.Ansi)]
		private static extern int opus_packet_get_nb_channels(byte[] data);

		[global::System.Runtime.InteropServices.DllImport("libopus-0", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, CharSet = global::System.Runtime.InteropServices.CharSet.Ansi)]
		private static extern string opus_strerror(global::VoiceChat.Codec.Enums.OpusStatusCode error);

		public static global::System.IntPtr CreateEncoder(int samplingRate, int channels, global::VoiceChat.Codec.Enums.OpusApplicationType application)
		{
			global::System.IntPtr intPtr = global::System.Runtime.InteropServices.Marshal.AllocHGlobal(opus_encoder_get_size(channels));
			global::VoiceChat.Codec.Enums.OpusStatusCode statusCode = opus_encoder_init(intPtr, samplingRate, channels, application);
			try
			{
				HandleStatusCode(statusCode);
				return intPtr;
			}
			catch (global::System.Exception ex)
			{
				if (intPtr != global::System.IntPtr.Zero)
				{
					Destroy(intPtr);
				}
				throw ex;
			}
		}

		public static int Encode(global::System.IntPtr st, float[] pcm, int frameSize, byte[] data)
		{
			if (st == global::System.IntPtr.Zero)
			{
				throw new global::System.ObjectDisposedException("Encoder is already disposed!");
			}
			int num = opus_encode_float(st, pcm, frameSize, data, data.Length);
			if (num <= 0)
			{
				HandleStatusCode((global::VoiceChat.Codec.Enums.OpusStatusCode)num);
			}
			return num;
		}

		public static int GetEncoderSetting(global::System.IntPtr st, global::VoiceChat.Codec.Enums.OpusCtlGetRequest request)
		{
			if (st == global::System.IntPtr.Zero)
			{
				throw new global::System.ObjectDisposedException("Encoder is already disposed!");
			}
			int value = 0;
			HandleStatusCode((global::VoiceChat.Codec.Enums.OpusStatusCode)opus_encoder_ctl(st, request, ref value));
			return value;
		}

		public static void SetEncoderSetting(global::System.IntPtr st, global::VoiceChat.Codec.Enums.OpusCtlSetRequest request, int value)
		{
			if (st == global::System.IntPtr.Zero)
			{
				throw new global::System.ObjectDisposedException("Encoder is already disposed!");
			}
			HandleStatusCode((global::VoiceChat.Codec.Enums.OpusStatusCode)opus_encoder_ctl(st, request, value));
		}

		public static global::System.IntPtr CreateDecoder(int samplingRate, int channels)
		{
			global::System.IntPtr intPtr = global::System.Runtime.InteropServices.Marshal.AllocHGlobal(opus_decoder_get_size(channels));
			global::VoiceChat.Codec.Enums.OpusStatusCode statusCode = opus_decoder_init(intPtr, samplingRate, channels);
			try
			{
				HandleStatusCode(statusCode);
				return intPtr;
			}
			catch (global::System.Exception ex)
			{
				if (intPtr != global::System.IntPtr.Zero)
				{
					Destroy(intPtr);
				}
				throw ex;
			}
		}

		public static int Decode(global::System.IntPtr st, byte[] data, int dataLength, float[] pcm, bool fec, int channels)
		{
			if (st == global::System.IntPtr.Zero)
			{
				throw new global::System.ObjectDisposedException("OpusDecoder is already disposed!");
			}
			int decode_fec = (fec ? 1 : 0);
			int frame_size = pcm.Length / channels;
			int num = ((data != null) ? opus_decode_float(st, data, dataLength, pcm, frame_size, decode_fec) : opus_decode_float(st, global::System.IntPtr.Zero, 0, pcm, frame_size, decode_fec));
			if (num == -4)
			{
				return 0;
			}
			if (num <= 0)
			{
				HandleStatusCode((global::VoiceChat.Codec.Enums.OpusStatusCode)num);
			}
			return num;
		}

		public static int GetBandwidth(byte[] data)
		{
			return opus_packet_get_bandwidth(data);
		}

		public static void HandleStatusCode(global::VoiceChat.Codec.Enums.OpusStatusCode statusCode)
		{
			if (statusCode == global::VoiceChat.Codec.Enums.OpusStatusCode.OK)
			{
				return;
			}
			throw new global::VoiceChat.Codec.OpusException(statusCode, opus_strerror(statusCode));
		}

		public static void Destroy(global::System.IntPtr st)
		{
			global::System.Runtime.InteropServices.Marshal.FreeHGlobal(st);
		}
	}
}
