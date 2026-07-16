namespace Utf8Json
{
	public struct JsonWriter
	{
		private static readonly byte[] emptyBytes = new byte[0];

		private byte[] buffer;

		private int offset;

		public int CurrentOffset => offset;

		public void AdvanceOffset(int offset)
		{
			this.offset += offset;
		}

		public static byte[] GetEncodedPropertyName(string propertyName)
		{
			global::Utf8Json.JsonWriter jsonWriter = default(global::Utf8Json.JsonWriter);
			jsonWriter.WritePropertyName(propertyName);
			return jsonWriter.ToUtf8ByteArray();
		}

		public static byte[] GetEncodedPropertyNameWithPrefixValueSeparator(string propertyName)
		{
			global::Utf8Json.JsonWriter jsonWriter = default(global::Utf8Json.JsonWriter);
			jsonWriter.WriteValueSeparator();
			jsonWriter.WritePropertyName(propertyName);
			return jsonWriter.ToUtf8ByteArray();
		}

		public static byte[] GetEncodedPropertyNameWithBeginObject(string propertyName)
		{
			global::Utf8Json.JsonWriter jsonWriter = default(global::Utf8Json.JsonWriter);
			jsonWriter.WriteBeginObject();
			jsonWriter.WritePropertyName(propertyName);
			return jsonWriter.ToUtf8ByteArray();
		}

		public static byte[] GetEncodedPropertyNameWithoutQuotation(string propertyName)
		{
			global::Utf8Json.JsonWriter jsonWriter = default(global::Utf8Json.JsonWriter);
			jsonWriter.WriteString(propertyName);
			global::System.ArraySegment<byte> arraySegment = jsonWriter.GetBuffer();
			byte[] array = new byte[arraySegment.Count - 2];
			global::System.Buffer.BlockCopy(arraySegment.Array, arraySegment.Offset + 1, array, 0, array.Length);
			return array;
		}

		public JsonWriter(byte[] initialBuffer)
		{
			buffer = initialBuffer;
			offset = 0;
		}

		public global::System.ArraySegment<byte> GetBuffer()
		{
			if (buffer == null)
			{
				return new global::System.ArraySegment<byte>(emptyBytes, 0, 0);
			}
			return new global::System.ArraySegment<byte>(buffer, 0, offset);
		}

		public byte[] ToUtf8ByteArray()
		{
			if (buffer == null)
			{
				return emptyBytes;
			}
			return global::Utf8Json.Internal.BinaryUtil.FastCloneWithResize(buffer, offset);
		}

		public override string ToString()
		{
			if (buffer == null)
			{
				return null;
			}
			return global::System.Text.Encoding.UTF8.GetString(buffer, 0, offset);
		}

		public void EnsureCapacity(int appendLength)
		{
			global::Utf8Json.Internal.BinaryUtil.EnsureCapacity(ref buffer, offset, appendLength);
		}

		public void WriteRaw(byte rawValue)
		{
			global::Utf8Json.Internal.BinaryUtil.EnsureCapacity(ref buffer, offset, 1);
			buffer[offset++] = rawValue;
		}

		public void WriteRaw(byte[] rawValue)
		{
			global::Utf8Json.Internal.BinaryUtil.EnsureCapacity(ref buffer, offset, rawValue.Length);
			global::System.Buffer.BlockCopy(rawValue, 0, buffer, offset, rawValue.Length);
			offset += rawValue.Length;
		}

		public void WriteRawUnsafe(byte rawValue)
		{
			buffer[offset++] = rawValue;
		}

		public void WriteBeginArray()
		{
			global::Utf8Json.Internal.BinaryUtil.EnsureCapacity(ref buffer, offset, 1);
			buffer[offset++] = 91;
		}

		public void WriteEndArray()
		{
			global::Utf8Json.Internal.BinaryUtil.EnsureCapacity(ref buffer, offset, 1);
			buffer[offset++] = 93;
		}

		public void WriteBeginObject()
		{
			global::Utf8Json.Internal.BinaryUtil.EnsureCapacity(ref buffer, offset, 1);
			buffer[offset++] = 123;
		}

		public void WriteEndObject()
		{
			global::Utf8Json.Internal.BinaryUtil.EnsureCapacity(ref buffer, offset, 1);
			buffer[offset++] = 125;
		}

		public void WriteValueSeparator()
		{
			global::Utf8Json.Internal.BinaryUtil.EnsureCapacity(ref buffer, offset, 1);
			buffer[offset++] = 44;
		}

		public void WriteNameSeparator()
		{
			global::Utf8Json.Internal.BinaryUtil.EnsureCapacity(ref buffer, offset, 1);
			buffer[offset++] = 58;
		}

		public void WritePropertyName(string propertyName)
		{
			WriteString(propertyName);
			WriteNameSeparator();
		}

		public void WriteQuotation()
		{
			global::Utf8Json.Internal.BinaryUtil.EnsureCapacity(ref buffer, offset, 1);
			buffer[offset++] = 34;
		}

		public void WriteNull()
		{
			global::Utf8Json.Internal.BinaryUtil.EnsureCapacity(ref buffer, offset, 4);
			buffer[offset] = 110;
			buffer[offset + 1] = 117;
			buffer[offset + 2] = 108;
			buffer[offset + 3] = 108;
			offset += 4;
		}

		public void WriteBoolean(bool value)
		{
			if (value)
			{
				global::Utf8Json.Internal.BinaryUtil.EnsureCapacity(ref buffer, offset, 4);
				buffer[offset] = 116;
				buffer[offset + 1] = 114;
				buffer[offset + 2] = 117;
				buffer[offset + 3] = 101;
				offset += 4;
			}
			else
			{
				global::Utf8Json.Internal.BinaryUtil.EnsureCapacity(ref buffer, offset, 5);
				buffer[offset] = 102;
				buffer[offset + 1] = 97;
				buffer[offset + 2] = 108;
				buffer[offset + 3] = 115;
				buffer[offset + 4] = 101;
				offset += 5;
			}
		}

		public void WriteTrue()
		{
			global::Utf8Json.Internal.BinaryUtil.EnsureCapacity(ref buffer, offset, 4);
			buffer[offset] = 116;
			buffer[offset + 1] = 114;
			buffer[offset + 2] = 117;
			buffer[offset + 3] = 101;
			offset += 4;
		}

		public void WriteFalse()
		{
			global::Utf8Json.Internal.BinaryUtil.EnsureCapacity(ref buffer, offset, 5);
			buffer[offset] = 102;
			buffer[offset + 1] = 97;
			buffer[offset + 2] = 108;
			buffer[offset + 3] = 115;
			buffer[offset + 4] = 101;
			offset += 5;
		}

		public void WriteSingle(float value)
		{
			offset += global::Utf8Json.Internal.DoubleConversion.DoubleToStringConverter.GetBytes(ref buffer, offset, value);
		}

		public void WriteDouble(double value)
		{
			offset += global::Utf8Json.Internal.DoubleConversion.DoubleToStringConverter.GetBytes(ref buffer, offset, value);
		}

		public void WriteByte(byte value)
		{
			WriteUInt64(value);
		}

		public void WriteUInt16(ushort value)
		{
			WriteUInt64(value);
		}

		public void WriteUInt32(uint value)
		{
			WriteUInt64(value);
		}

		public void WriteUInt64(ulong value)
		{
			offset += global::Utf8Json.Internal.NumberConverter.WriteUInt64(ref buffer, offset, value);
		}

		public void WriteSByte(sbyte value)
		{
			WriteInt64(value);
		}

		public void WriteInt16(short value)
		{
			WriteInt64(value);
		}

		public void WriteInt32(int value)
		{
			WriteInt64(value);
		}

		public void WriteInt64(long value)
		{
			offset += global::Utf8Json.Internal.NumberConverter.WriteInt64(ref buffer, offset, value);
		}

		public void WriteString(string value)
		{
			if (value == null)
			{
				WriteNull();
				return;
			}
			int num = offset;
			int num2 = global::Utf8Json.Internal.StringEncoding.UTF8.GetMaxByteCount(value.Length) + 2;
			global::Utf8Json.Internal.BinaryUtil.EnsureCapacity(ref buffer, num, num2);
			int num3 = 0;
			_ = value.Length;
			buffer[offset++] = 34;
			for (int i = 0; i < value.Length; i++)
			{
				byte b = 0;
				switch (value[i])
				{
				case '"':
					b = 34;
					break;
				case '\\':
					b = 92;
					break;
				case '\b':
					b = 98;
					break;
				case '\f':
					b = 102;
					break;
				case '\n':
					b = 110;
					break;
				case '\r':
					b = 114;
					break;
				case '\t':
					b = 116;
					break;
				default:
					continue;
				}
				num2 += 2;
				global::Utf8Json.Internal.BinaryUtil.EnsureCapacity(ref buffer, num, num2);
				offset += global::Utf8Json.Internal.StringEncoding.UTF8.GetBytes(value, num3, i - num3, buffer, offset);
				num3 = i + 1;
				buffer[offset++] = 92;
				buffer[offset++] = b;
			}
			if (num3 != value.Length)
			{
				offset += global::Utf8Json.Internal.StringEncoding.UTF8.GetBytes(value, num3, value.Length - num3, buffer, offset);
			}
			buffer[offset++] = 34;
		}
	}
}
