namespace Utf8Json.Internal.DoubleConversion
{
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Explicit, Pack = 1)]
	internal struct UnionDoubleULong
	{
		[global::System.Runtime.InteropServices.FieldOffset(0)]
		public double d;

		[global::System.Runtime.InteropServices.FieldOffset(0)]
		public ulong u64;
	}
}
