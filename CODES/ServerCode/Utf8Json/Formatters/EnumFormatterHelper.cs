namespace Utf8Json.Formatters
{
	public static class EnumFormatterHelper
	{
		public static object GetSerializeDelegate(global::System.Type type, out bool isBoxed)
		{
			global::System.Type underlyingType = global::System.Enum.GetUnderlyingType(type);
			isBoxed = true;
			if (underlyingType == typeof(byte))
			{
				return (global::Utf8Json.JsonSerializeAction<object>)delegate(ref global::Utf8Json.JsonWriter writer, object value, global::Utf8Json.IJsonFormatterResolver _)
				{
					writer.WriteByte((byte)value);
				};
			}
			if (underlyingType == typeof(sbyte))
			{
				return (global::Utf8Json.JsonSerializeAction<object>)delegate(ref global::Utf8Json.JsonWriter writer, object value, global::Utf8Json.IJsonFormatterResolver _)
				{
					writer.WriteSByte((sbyte)value);
				};
			}
			if (underlyingType == typeof(short))
			{
				return (global::Utf8Json.JsonSerializeAction<object>)delegate(ref global::Utf8Json.JsonWriter writer, object value, global::Utf8Json.IJsonFormatterResolver _)
				{
					writer.WriteInt16((short)value);
				};
			}
			if (underlyingType == typeof(ushort))
			{
				return (global::Utf8Json.JsonSerializeAction<object>)delegate(ref global::Utf8Json.JsonWriter writer, object value, global::Utf8Json.IJsonFormatterResolver _)
				{
					writer.WriteUInt16((ushort)value);
				};
			}
			if (underlyingType == typeof(int))
			{
				return (global::Utf8Json.JsonSerializeAction<object>)delegate(ref global::Utf8Json.JsonWriter writer, object value, global::Utf8Json.IJsonFormatterResolver _)
				{
					writer.WriteInt32((int)value);
				};
			}
			if (underlyingType == typeof(uint))
			{
				return (global::Utf8Json.JsonSerializeAction<object>)delegate(ref global::Utf8Json.JsonWriter writer, object value, global::Utf8Json.IJsonFormatterResolver _)
				{
					writer.WriteUInt32((uint)value);
				};
			}
			if (underlyingType == typeof(long))
			{
				return (global::Utf8Json.JsonSerializeAction<object>)delegate(ref global::Utf8Json.JsonWriter writer, object value, global::Utf8Json.IJsonFormatterResolver _)
				{
					writer.WriteInt64((long)value);
				};
			}
			if (underlyingType == typeof(ulong))
			{
				return (global::Utf8Json.JsonSerializeAction<object>)delegate(ref global::Utf8Json.JsonWriter writer, object value, global::Utf8Json.IJsonFormatterResolver _)
				{
					writer.WriteUInt64((ulong)value);
				};
			}
			throw new global::System.InvalidOperationException("Type is not Enum. Type:" + type);
		}

		public static object GetDeserializeDelegate(global::System.Type type, out bool isBoxed)
		{
			global::System.Type underlyingType = global::System.Enum.GetUnderlyingType(type);
			isBoxed = true;
			if (underlyingType == typeof(byte))
			{
				return (global::Utf8Json.JsonDeserializeFunc<object>)delegate(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver _)
				{
					return reader.ReadByte();
				};
			}
			if (underlyingType == typeof(sbyte))
			{
				return (global::Utf8Json.JsonDeserializeFunc<object>)delegate(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver _)
				{
					return reader.ReadSByte();
				};
			}
			if (underlyingType == typeof(short))
			{
				return (global::Utf8Json.JsonDeserializeFunc<object>)delegate(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver _)
				{
					return reader.ReadInt16();
				};
			}
			if (underlyingType == typeof(ushort))
			{
				return (global::Utf8Json.JsonDeserializeFunc<object>)delegate(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver _)
				{
					return reader.ReadUInt16();
				};
			}
			if (underlyingType == typeof(int))
			{
				return (global::Utf8Json.JsonDeserializeFunc<object>)delegate(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver _)
				{
					return reader.ReadInt32();
				};
			}
			if (underlyingType == typeof(uint))
			{
				return (global::Utf8Json.JsonDeserializeFunc<object>)delegate(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver _)
				{
					return reader.ReadUInt32();
				};
			}
			if (underlyingType == typeof(long))
			{
				return (global::Utf8Json.JsonDeserializeFunc<object>)delegate(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver _)
				{
					return reader.ReadInt64();
				};
			}
			if (underlyingType == typeof(ulong))
			{
				return (global::Utf8Json.JsonDeserializeFunc<object>)delegate(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver _)
				{
					return reader.ReadUInt64();
				};
			}
			throw new global::System.InvalidOperationException("Type is not Enum. Type:" + type);
		}
	}
}
