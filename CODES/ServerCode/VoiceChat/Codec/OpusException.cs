namespace VoiceChat.Codec
{
	public class OpusException : global::System.Exception
	{
		public readonly global::VoiceChat.Codec.Enums.OpusStatusCode StatusCode;

		public OpusException(global::VoiceChat.Codec.Enums.OpusStatusCode statusCode, string message)
			: base(message)
		{
			StatusCode = statusCode;
		}
	}
}
