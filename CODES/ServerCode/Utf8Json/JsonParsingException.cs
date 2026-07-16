namespace Utf8Json
{
	public class JsonParsingException : global::System.Exception
	{
		private global::System.WeakReference underyingBytes;

		private int limit;

		public int Offset { get; private set; }

		public string ActualChar { get; set; }

		public JsonParsingException(string message)
			: base(message)
		{
		}

		public JsonParsingException(string message, byte[] underlyingBytes, int offset, int limit, string actualChar)
			: base(message)
		{
			underyingBytes = new global::System.WeakReference(underlyingBytes);
			Offset = offset;
			ActualChar = actualChar;
			this.limit = limit;
		}

		public byte[] GetUnderlyingByteArrayUnsafe()
		{
			return underyingBytes.Target as byte[];
		}

		public string GetUnderlyingStringUnsafe()
		{
			if (underyingBytes.Target is byte[] bytes)
			{
				return global::Utf8Json.Internal.StringEncoding.UTF8.GetString(bytes, 0, limit) + "...";
			}
			return null;
		}
	}
}
