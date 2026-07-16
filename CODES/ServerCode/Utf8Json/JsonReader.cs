namespace Utf8Json
{
	public struct JsonReader
	{
		internal static class StringBuilderCache
		{
			[global::System.ThreadStatic]
			private static byte[] buffer;

			[global::System.ThreadStatic]
			private static char[] codePointStringBuffer;

			public static byte[] GetBuffer()
			{
				if (buffer == null)
				{
					buffer = new byte[65535];
				}
				return buffer;
			}

			public static char[] GetCodePointStringBuffer()
			{
				if (codePointStringBuffer == null)
				{
					codePointStringBuffer = new char[65535];
				}
				return codePointStringBuffer;
			}
		}

		private static readonly global::System.ArraySegment<byte> nullTokenSegment = new global::System.ArraySegment<byte>(new byte[4] { 110, 117, 108, 108 }, 0, 4);

		private static readonly byte[] bom = global::System.Text.Encoding.UTF8.GetPreamble();

		private readonly byte[] bytes;

		private int offset;

		private bool IsInRange => offset < bytes.Length;

		public JsonReader(byte[] bytes)
			: this(bytes, 0)
		{
		}

		public JsonReader(byte[] bytes, int offset)
		{
			this.bytes = bytes;
			this.offset = offset;
			if (bytes.Length >= 3 && bytes[offset] == bom[0] && bytes[offset + 1] == bom[1] && bytes[offset + 2] == bom[2])
			{
				this.offset = (offset += 3);
			}
		}

		private global::Utf8Json.JsonParsingException CreateParsingException(string expected)
		{
			char c = (char)bytes[offset];
			string text = c.ToString();
			int num = offset;
			try
			{
				switch (GetCurrentJsonToken())
				{
				case global::Utf8Json.JsonToken.Number:
				{
					global::System.ArraySegment<byte> arraySegment = ReadNumberSegment();
					text = global::Utf8Json.Internal.StringEncoding.UTF8.GetString(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
					break;
				}
				case global::Utf8Json.JsonToken.String:
					text = "\"" + ReadString() + "\"";
					break;
				case global::Utf8Json.JsonToken.True:
					text = "true";
					break;
				case global::Utf8Json.JsonToken.False:
					text = "false";
					break;
				case global::Utf8Json.JsonToken.Null:
					text = "null";
					break;
				}
			}
			catch
			{
			}
			return new global::Utf8Json.JsonParsingException("expected:'" + expected + "', actual:'" + text + "', at offset:" + num, bytes, num, offset, text);
		}

		private global::Utf8Json.JsonParsingException CreateParsingExceptionMessage(string message)
		{
			char c = (char)bytes[offset];
			string actualChar = c.ToString();
			int limit = offset;
			return new global::Utf8Json.JsonParsingException(message, bytes, limit, limit, actualChar);
		}

		public void AdvanceOffset(int offset)
		{
			this.offset += offset;
		}

		public byte[] GetBufferUnsafe()
		{
			return bytes;
		}

		public int GetCurrentOffsetUnsafe()
		{
			return offset;
		}

		public global::Utf8Json.JsonToken GetCurrentJsonToken()
		{
			SkipWhiteSpace();
			if (offset < bytes.Length)
			{
				switch (bytes[offset])
				{
				case 123:
					return global::Utf8Json.JsonToken.BeginObject;
				case 125:
					return global::Utf8Json.JsonToken.EndObject;
				case 91:
					return global::Utf8Json.JsonToken.BeginArray;
				case 93:
					return global::Utf8Json.JsonToken.EndArray;
				case 116:
					return global::Utf8Json.JsonToken.True;
				case 102:
					return global::Utf8Json.JsonToken.False;
				case 110:
					return global::Utf8Json.JsonToken.Null;
				case 44:
					return global::Utf8Json.JsonToken.ValueSeparator;
				case 58:
					return global::Utf8Json.JsonToken.NameSeparator;
				case 45:
					return global::Utf8Json.JsonToken.Number;
				case 48:
					return global::Utf8Json.JsonToken.Number;
				case 49:
					return global::Utf8Json.JsonToken.Number;
				case 50:
					return global::Utf8Json.JsonToken.Number;
				case 51:
					return global::Utf8Json.JsonToken.Number;
				case 52:
					return global::Utf8Json.JsonToken.Number;
				case 53:
					return global::Utf8Json.JsonToken.Number;
				case 54:
					return global::Utf8Json.JsonToken.Number;
				case 55:
					return global::Utf8Json.JsonToken.Number;
				case 56:
					return global::Utf8Json.JsonToken.Number;
				case 57:
					return global::Utf8Json.JsonToken.Number;
				case 34:
					return global::Utf8Json.JsonToken.String;
				default:
					return global::Utf8Json.JsonToken.None;
				}
			}
			return global::Utf8Json.JsonToken.None;
		}

		public void SkipWhiteSpace()
		{
			for (int i = offset; i < bytes.Length; i++)
			{
				switch (bytes[i])
				{
				case 47:
					i = ReadComment(bytes, i);
					break;
				default:
					offset = i;
					return;
				case 9:
				case 10:
				case 13:
				case 32:
					break;
				}
			}
			offset = bytes.Length;
		}

		public bool ReadIsNull()
		{
			SkipWhiteSpace();
			if (IsInRange && bytes[offset] == 110)
			{
				if (bytes[offset + 1] == 117 && bytes[offset + 2] == 108 && bytes[offset + 3] == 108)
				{
					offset += 4;
					return true;
				}
				throw CreateParsingException("null");
			}
			return false;
		}

		public bool ReadIsBeginArray()
		{
			SkipWhiteSpace();
			if (IsInRange && bytes[offset] == 91)
			{
				offset++;
				return true;
			}
			return false;
		}

		public void ReadIsBeginArrayWithVerify()
		{
			if (!ReadIsBeginArray())
			{
				throw CreateParsingException("[");
			}
		}

		public bool ReadIsEndArray()
		{
			SkipWhiteSpace();
			if (IsInRange && bytes[offset] == 93)
			{
				offset++;
				return true;
			}
			return false;
		}

		public void ReadIsEndArrayWithVerify()
		{
			if (!ReadIsEndArray())
			{
				throw CreateParsingException("]");
			}
		}

		public bool ReadIsEndArrayWithSkipValueSeparator(ref int count)
		{
			SkipWhiteSpace();
			if (IsInRange && bytes[offset] == 93)
			{
				offset++;
				return true;
			}
			if (count++ != 0)
			{
				ReadIsValueSeparatorWithVerify();
			}
			return false;
		}

		public bool ReadIsInArray(ref int count)
		{
			if (count == 0)
			{
				ReadIsBeginArrayWithVerify();
				if (ReadIsEndArray())
				{
					return false;
				}
			}
			else
			{
				if (ReadIsEndArray())
				{
					return false;
				}
				ReadIsValueSeparatorWithVerify();
			}
			count++;
			return true;
		}

		public bool ReadIsBeginObject()
		{
			SkipWhiteSpace();
			if (IsInRange && bytes[offset] == 123)
			{
				offset++;
				return true;
			}
			return false;
		}

		public void ReadIsBeginObjectWithVerify()
		{
			if (!ReadIsBeginObject())
			{
				throw CreateParsingException("{");
			}
		}

		public bool ReadIsEndObject()
		{
			SkipWhiteSpace();
			if (IsInRange && bytes[offset] == 125)
			{
				offset++;
				return true;
			}
			return false;
		}

		public void ReadIsEndObjectWithVerify()
		{
			if (!ReadIsEndObject())
			{
				throw CreateParsingException("}");
			}
		}

		public bool ReadIsEndObjectWithSkipValueSeparator(ref int count)
		{
			SkipWhiteSpace();
			if (IsInRange && bytes[offset] == 125)
			{
				offset++;
				return true;
			}
			if (count++ != 0)
			{
				ReadIsValueSeparatorWithVerify();
			}
			return false;
		}

		public bool ReadIsInObject(ref int count)
		{
			if (count == 0)
			{
				ReadIsBeginObjectWithVerify();
				if (ReadIsEndObject())
				{
					return false;
				}
			}
			else
			{
				if (ReadIsEndObject())
				{
					return false;
				}
				ReadIsValueSeparatorWithVerify();
			}
			count++;
			return true;
		}

		public bool ReadIsValueSeparator()
		{
			SkipWhiteSpace();
			if (IsInRange && bytes[offset] == 44)
			{
				offset++;
				return true;
			}
			return false;
		}

		public void ReadIsValueSeparatorWithVerify()
		{
			if (!ReadIsValueSeparator())
			{
				throw CreateParsingException(",");
			}
		}

		public bool ReadIsNameSeparator()
		{
			SkipWhiteSpace();
			if (IsInRange && bytes[offset] == 58)
			{
				offset++;
				return true;
			}
			return false;
		}

		public void ReadIsNameSeparatorWithVerify()
		{
			if (!ReadIsNameSeparator())
			{
				throw CreateParsingException(":");
			}
		}

		private void ReadStringSegmentCore(out byte[] resultBytes, out int resultOffset, out int resultLength)
		{
			byte[] array = null;
			int num = 0;
			char[] array2 = null;
			int num2 = 0;
			if (bytes[offset] != 34)
			{
				throw CreateParsingException("String Begin Token");
			}
			offset++;
			int num3 = offset;
			for (int i = offset; i < bytes.Length; i++)
			{
				byte b = 0;
				switch (bytes[i])
				{
				case 92:
				{
					switch ((char)bytes[i + 1])
					{
					case '"':
					case '/':
					case '\\':
						b = bytes[i + 1];
						break;
					case 'b':
						b = 8;
						break;
					case 'f':
						b = 12;
						break;
					case 'n':
						b = 10;
						break;
					case 'r':
						b = 13;
						break;
					case 't':
						b = 9;
						break;
					case 'u':
					{
						if (array2 == null)
						{
							array2 = global::Utf8Json.JsonReader.StringBuilderCache.GetCodePointStringBuffer();
						}
						if (num2 == 0)
						{
							if (array == null)
							{
								array = global::Utf8Json.JsonReader.StringBuilderCache.GetBuffer();
							}
							int num5 = i - num3;
							global::Utf8Json.Internal.BinaryUtil.EnsureCapacity(ref array, num, num5 + 1);
							global::System.Buffer.BlockCopy(bytes, num3, array, num, num5);
							num += num5;
						}
						if (array2.Length == num2)
						{
							global::System.Array.Resize(ref array2, array2.Length * 2);
						}
						byte a = bytes[i + 2];
						char b2 = (char)bytes[i + 3];
						char c = (char)bytes[i + 4];
						char d = (char)bytes[i + 5];
						int codePoint = GetCodePoint((char)a, b2, c, d);
						array2[num2++] = (char)codePoint;
						i += 5;
						offset += 6;
						num3 = offset;
						continue;
					}
					default:
						throw CreateParsingExceptionMessage("Bad JSON escape.");
					}
					if (array == null)
					{
						array = global::Utf8Json.JsonReader.StringBuilderCache.GetBuffer();
					}
					if (num2 != 0)
					{
						global::Utf8Json.Internal.BinaryUtil.EnsureCapacity(ref array, num, global::Utf8Json.Internal.StringEncoding.UTF8.GetMaxByteCount(num2));
						num += global::Utf8Json.Internal.StringEncoding.UTF8.GetBytes(array2, 0, num2, array, num);
						num2 = 0;
					}
					int num6 = i - num3;
					global::Utf8Json.Internal.BinaryUtil.EnsureCapacity(ref array, num, num6 + 1);
					global::System.Buffer.BlockCopy(bytes, num3, array, num, num6);
					num += num6;
					array[num++] = b;
					i++;
					offset += 2;
					num3 = offset;
					continue;
				}
				case 34:
				{
					offset++;
					if (num == 0 && num2 == 0)
					{
						resultBytes = bytes;
						resultOffset = num3;
						resultLength = offset - 1 - num3;
						return;
					}
					if (array == null)
					{
						array = global::Utf8Json.JsonReader.StringBuilderCache.GetBuffer();
					}
					if (num2 != 0)
					{
						global::Utf8Json.Internal.BinaryUtil.EnsureCapacity(ref array, num, global::Utf8Json.Internal.StringEncoding.UTF8.GetMaxByteCount(num2));
						num += global::Utf8Json.Internal.StringEncoding.UTF8.GetBytes(array2, 0, num2, array, num);
						num2 = 0;
					}
					int num4 = offset - num3 - 1;
					global::Utf8Json.Internal.BinaryUtil.EnsureCapacity(ref array, num, num4);
					global::System.Buffer.BlockCopy(bytes, num3, array, num, num4);
					num += num4;
					resultBytes = array;
					resultOffset = 0;
					resultLength = num;
					return;
				}
				}
				if (num2 != 0)
				{
					if (array == null)
					{
						array = global::Utf8Json.JsonReader.StringBuilderCache.GetBuffer();
					}
					global::Utf8Json.Internal.BinaryUtil.EnsureCapacity(ref array, num, global::Utf8Json.Internal.StringEncoding.UTF8.GetMaxByteCount(num2));
					num += global::Utf8Json.Internal.StringEncoding.UTF8.GetBytes(array2, 0, num2, array, num);
					num2 = 0;
				}
				offset++;
			}
			resultLength = 0;
			resultBytes = null;
			resultOffset = 0;
			throw CreateParsingException("String End Token");
		}

		private static int GetCodePoint(char a, char b, char c, char d)
		{
			return ((ToNumber(a) * 16 + ToNumber(b)) * 16 + ToNumber(c)) * 16 + ToNumber(d);
		}

		private static int ToNumber(char x)
		{
			if ('0' <= x && x <= '9')
			{
				return x - 48;
			}
			if ('a' <= x && x <= 'f')
			{
				return x - 97 + 10;
			}
			if ('A' <= x && x <= 'F')
			{
				return x - 65 + 10;
			}
			throw new global::Utf8Json.JsonParsingException("Invalid Character" + x);
		}

		public global::System.ArraySegment<byte> ReadStringSegmentUnsafe()
		{
			if (ReadIsNull())
			{
				return nullTokenSegment;
			}
			ReadStringSegmentCore(out var resultBytes, out var resultOffset, out var resultLength);
			return new global::System.ArraySegment<byte>(resultBytes, resultOffset, resultLength);
		}

		public string ReadString()
		{
			if (ReadIsNull())
			{
				return null;
			}
			ReadStringSegmentCore(out var resultBytes, out var resultOffset, out var resultLength);
			return global::System.Text.Encoding.UTF8.GetString(resultBytes, resultOffset, resultLength);
		}

		public string ReadPropertyName()
		{
			string result = ReadString();
			ReadIsNameSeparatorWithVerify();
			return result;
		}

		public global::System.ArraySegment<byte> ReadStringSegmentRaw()
		{
			global::System.ArraySegment<byte> arraySegment = default(global::System.ArraySegment<byte>);
			if (ReadIsNull())
			{
				return nullTokenSegment;
			}
			if (bytes[offset++] != 34)
			{
				throw CreateParsingException("\"");
			}
			int num = offset;
			for (int i = offset; i < bytes.Length; i++)
			{
				if (bytes[i] == 34 && bytes[i - 1] != 92)
				{
					offset = i + 1;
					return new global::System.ArraySegment<byte>(bytes, num, offset - num - 1);
				}
			}
			throw CreateParsingExceptionMessage("not found end string.");
		}

		public global::System.ArraySegment<byte> ReadPropertyNameSegmentRaw()
		{
			global::System.ArraySegment<byte> result = ReadStringSegmentRaw();
			ReadIsNameSeparatorWithVerify();
			return result;
		}

		public bool ReadBoolean()
		{
			SkipWhiteSpace();
			if (bytes[offset] == 116)
			{
				if (bytes[offset + 1] == 114 && bytes[offset + 2] == 117 && bytes[offset + 3] == 101)
				{
					offset += 4;
					return true;
				}
				throw CreateParsingException("true");
			}
			if (bytes[offset] == 102)
			{
				if (bytes[offset + 1] == 97 && bytes[offset + 2] == 108 && bytes[offset + 3] == 115 && bytes[offset + 4] == 101)
				{
					offset += 5;
					return false;
				}
				throw CreateParsingException("false");
			}
			throw CreateParsingException("true | false");
		}

		private static bool IsWordBreak(byte c)
		{
			switch (c)
			{
			case 32:
			case 34:
			case 44:
			case 58:
			case 91:
			case 93:
			case 123:
			case 125:
				return true;
			default:
				return false;
			}
		}

		public void ReadNext()
		{
			global::Utf8Json.JsonToken currentJsonToken = GetCurrentJsonToken();
			ReadNextCore(currentJsonToken);
		}

		private void ReadNextCore(global::Utf8Json.JsonToken token)
		{
			switch (token)
			{
			case global::Utf8Json.JsonToken.BeginObject:
			case global::Utf8Json.JsonToken.EndObject:
			case global::Utf8Json.JsonToken.BeginArray:
			case global::Utf8Json.JsonToken.EndArray:
			case global::Utf8Json.JsonToken.ValueSeparator:
			case global::Utf8Json.JsonToken.NameSeparator:
				offset++;
				break;
			case global::Utf8Json.JsonToken.True:
			case global::Utf8Json.JsonToken.Null:
				offset += 4;
				break;
			case global::Utf8Json.JsonToken.False:
				offset += 5;
				break;
			case global::Utf8Json.JsonToken.String:
			{
				offset++;
				for (int j = offset; j < bytes.Length; j++)
				{
					if (bytes[j] == 34 && bytes[j - 1] != 92)
					{
						offset = j + 1;
						return;
					}
				}
				throw CreateParsingExceptionMessage("not found end string.");
			}
			case global::Utf8Json.JsonToken.Number:
			{
				for (int i = offset; i < bytes.Length; i++)
				{
					if (IsWordBreak(bytes[i]))
					{
						offset = i;
						return;
					}
				}
				offset = bytes.Length;
				break;
			}
			case global::Utf8Json.JsonToken.None:
				break;
			}
		}

		public void ReadNextBlock()
		{
			ReadNextBlockCore(0);
		}

		private void ReadNextBlockCore(int stack)
		{
			global::Utf8Json.JsonToken currentJsonToken = GetCurrentJsonToken();
			switch (currentJsonToken)
			{
			default:
				return;
			case global::Utf8Json.JsonToken.BeginObject:
			case global::Utf8Json.JsonToken.BeginArray:
				offset++;
				ReadNextBlockCore(stack + 1);
				return;
			case global::Utf8Json.JsonToken.EndObject:
			case global::Utf8Json.JsonToken.EndArray:
				offset++;
				if (stack - 1 != 0)
				{
					ReadNextBlockCore(stack - 1);
				}
				return;
			case global::Utf8Json.JsonToken.Number:
			case global::Utf8Json.JsonToken.String:
			case global::Utf8Json.JsonToken.True:
			case global::Utf8Json.JsonToken.False:
			case global::Utf8Json.JsonToken.Null:
			case global::Utf8Json.JsonToken.ValueSeparator:
			case global::Utf8Json.JsonToken.NameSeparator:
				break;
			case global::Utf8Json.JsonToken.None:
				return;
			}
			do
			{
				ReadNextCore(currentJsonToken);
				currentJsonToken = GetCurrentJsonToken();
			}
			while (stack != 0 && (int)currentJsonToken >= 5);
			if (stack != 0)
			{
				ReadNextBlockCore(stack);
			}
		}

		public global::System.ArraySegment<byte> ReadNextBlockSegment()
		{
			int num = offset;
			ReadNextBlock();
			return new global::System.ArraySegment<byte>(bytes, num, offset - num);
		}

		public sbyte ReadSByte()
		{
			return checked((sbyte)ReadInt64());
		}

		public short ReadInt16()
		{
			return checked((short)ReadInt64());
		}

		public int ReadInt32()
		{
			return checked((int)ReadInt64());
		}

		public long ReadInt64()
		{
			SkipWhiteSpace();
			int readCount;
			long result = global::Utf8Json.Internal.NumberConverter.ReadInt64(bytes, offset, out readCount);
			if (readCount == 0)
			{
				throw CreateParsingException("Number Token");
			}
			offset += readCount;
			return result;
		}

		public byte ReadByte()
		{
			return checked((byte)ReadUInt64());
		}

		public ushort ReadUInt16()
		{
			return checked((ushort)ReadUInt64());
		}

		public uint ReadUInt32()
		{
			return checked((uint)ReadUInt64());
		}

		public ulong ReadUInt64()
		{
			SkipWhiteSpace();
			int readCount;
			ulong result = global::Utf8Json.Internal.NumberConverter.ReadUInt64(bytes, offset, out readCount);
			if (readCount == 0)
			{
				throw CreateParsingException("Number Token");
			}
			offset += readCount;
			return result;
		}

		public float ReadSingle()
		{
			SkipWhiteSpace();
			int readCount;
			float result = global::Utf8Json.Internal.DoubleConversion.StringToDoubleConverter.ToSingle(bytes, offset, out readCount);
			if (readCount == 0)
			{
				throw CreateParsingException("Number Token");
			}
			offset += readCount;
			return result;
		}

		public double ReadDouble()
		{
			SkipWhiteSpace();
			int readCount;
			double result = global::Utf8Json.Internal.DoubleConversion.StringToDoubleConverter.ToDouble(bytes, offset, out readCount);
			if (readCount == 0)
			{
				throw CreateParsingException("Number Token");
			}
			offset += readCount;
			return result;
		}

		public global::System.ArraySegment<byte> ReadNumberSegment()
		{
			SkipWhiteSpace();
			int num = offset;
			int num2 = offset;
			while (true)
			{
				if (num2 < bytes.Length)
				{
					if (!global::Utf8Json.Internal.NumberConverter.IsNumberRepresentation(bytes[num2]))
					{
						offset = num2;
						break;
					}
					num2++;
					continue;
				}
				offset = bytes.Length;
				break;
			}
			return new global::System.ArraySegment<byte>(bytes, num, offset - num);
		}

		private static int ReadComment(byte[] bytes, int offset)
		{
			if (bytes[offset + 1] == 47)
			{
				offset += 2;
				for (int i = offset; i < bytes.Length; i++)
				{
					if (bytes[i] == 13 || bytes[i] == 10)
					{
						return i;
					}
				}
				throw new global::Utf8Json.JsonParsingException("Can not find end token of single line comment(\r or \n).");
			}
			if (bytes[offset + 1] == 42)
			{
				offset += 2;
				for (int j = offset; j < bytes.Length; j++)
				{
					if (bytes[j] == 42 && bytes[j + 1] == 47)
					{
						return j + 1;
					}
				}
				throw new global::Utf8Json.JsonParsingException("Can not find end token of multi line comment(*/).");
			}
			return offset;
		}
	}
}
