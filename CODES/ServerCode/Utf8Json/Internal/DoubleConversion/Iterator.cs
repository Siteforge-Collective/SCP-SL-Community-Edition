namespace Utf8Json.Internal.DoubleConversion
{
	internal struct Iterator
	{
		private byte[] buffer;

		private int offset;

		public byte Value => buffer[offset];

		public Iterator(byte[] buffer, int offset)
		{
			this.buffer = buffer;
			this.offset = offset;
		}

		public static global::Utf8Json.Internal.DoubleConversion.Iterator operator ++(global::Utf8Json.Internal.DoubleConversion.Iterator self)
		{
			self.offset++;
			return self;
		}

		public static global::Utf8Json.Internal.DoubleConversion.Iterator operator +(global::Utf8Json.Internal.DoubleConversion.Iterator self, int length)
		{
			return new global::Utf8Json.Internal.DoubleConversion.Iterator
			{
				buffer = self.buffer,
				offset = self.offset + length
			};
		}

		public static int operator -(global::Utf8Json.Internal.DoubleConversion.Iterator lhs, global::Utf8Json.Internal.DoubleConversion.Iterator rhs)
		{
			return lhs.offset - rhs.offset;
		}

		public static bool operator ==(global::Utf8Json.Internal.DoubleConversion.Iterator lhs, global::Utf8Json.Internal.DoubleConversion.Iterator rhs)
		{
			return lhs.offset == rhs.offset;
		}

		public static bool operator !=(global::Utf8Json.Internal.DoubleConversion.Iterator lhs, global::Utf8Json.Internal.DoubleConversion.Iterator rhs)
		{
			return lhs.offset != rhs.offset;
		}

		public static bool operator ==(global::Utf8Json.Internal.DoubleConversion.Iterator lhs, char rhs)
		{
			return lhs.buffer[lhs.offset] == (byte)rhs;
		}

		public static bool operator !=(global::Utf8Json.Internal.DoubleConversion.Iterator lhs, char rhs)
		{
			return lhs.buffer[lhs.offset] != (byte)rhs;
		}

		public static bool operator ==(global::Utf8Json.Internal.DoubleConversion.Iterator lhs, byte rhs)
		{
			return lhs.buffer[lhs.offset] == rhs;
		}

		public static bool operator !=(global::Utf8Json.Internal.DoubleConversion.Iterator lhs, byte rhs)
		{
			return lhs.buffer[lhs.offset] != rhs;
		}

		public static bool operator >=(global::Utf8Json.Internal.DoubleConversion.Iterator lhs, char rhs)
		{
			return lhs.buffer[lhs.offset] >= (byte)rhs;
		}

		public static bool operator <=(global::Utf8Json.Internal.DoubleConversion.Iterator lhs, char rhs)
		{
			return lhs.buffer[lhs.offset] <= (byte)rhs;
		}

		public static bool operator >(global::Utf8Json.Internal.DoubleConversion.Iterator lhs, char rhs)
		{
			return lhs.buffer[lhs.offset] > (byte)rhs;
		}

		public static bool operator <(global::Utf8Json.Internal.DoubleConversion.Iterator lhs, char rhs)
		{
			return lhs.buffer[lhs.offset] < (byte)rhs;
		}
	}
}
