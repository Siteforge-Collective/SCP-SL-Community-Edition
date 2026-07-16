namespace Utf8Json.Internal
{
	internal sealed class BufferPool : global::Utf8Json.Internal.ArrayPool<byte>
	{
		public static readonly global::Utf8Json.Internal.BufferPool Default = new global::Utf8Json.Internal.BufferPool(65535);

		public BufferPool(int bufferLength)
			: base(bufferLength)
		{
		}
	}
}
