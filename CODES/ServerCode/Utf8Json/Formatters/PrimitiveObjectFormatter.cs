namespace Utf8Json.Formatters
{
	public sealed class PrimitiveObjectFormatter : global::Utf8Json.IJsonFormatter<object>, global::Utf8Json.IJsonFormatter
	{
		public static readonly global::Utf8Json.IJsonFormatter<object> Default = new global::Utf8Json.Formatters.PrimitiveObjectFormatter();

		private static readonly global::System.Collections.Generic.Dictionary<global::System.Type, int> typeToJumpCode = new global::System.Collections.Generic.Dictionary<global::System.Type, int>
		{
			{
				typeof(bool),
				0
			},
			{
				typeof(char),
				1
			},
			{
				typeof(sbyte),
				2
			},
			{
				typeof(byte),
				3
			},
			{
				typeof(short),
				4
			},
			{
				typeof(ushort),
				5
			},
			{
				typeof(int),
				6
			},
			{
				typeof(uint),
				7
			},
			{
				typeof(long),
				8
			},
			{
				typeof(ulong),
				9
			},
			{
				typeof(float),
				10
			},
			{
				typeof(double),
				11
			},
			{
				typeof(global::System.DateTime),
				12
			},
			{
				typeof(string),
				13
			},
			{
				typeof(byte[]),
				14
			}
		};

		public void Serialize(ref global::Utf8Json.JsonWriter writer, object value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			global::System.Type type = value.GetType();
			if (typeToJumpCode.TryGetValue(type, out var value2))
			{
				switch (value2)
				{
				case 0:
					writer.WriteBoolean((bool)value);
					return;
				case 1:
					global::Utf8Json.Formatters.CharFormatter.Default.Serialize(ref writer, (char)value, formatterResolver);
					return;
				case 2:
					writer.WriteSByte((sbyte)value);
					return;
				case 3:
					writer.WriteByte((byte)value);
					return;
				case 4:
					writer.WriteInt16((short)value);
					return;
				case 5:
					writer.WriteUInt16((ushort)value);
					return;
				case 6:
					writer.WriteInt32((int)value);
					return;
				case 7:
					writer.WriteUInt32((uint)value);
					return;
				case 8:
					writer.WriteInt64((long)value);
					return;
				case 9:
					writer.WriteUInt64((ulong)value);
					return;
				case 10:
					writer.WriteSingle((float)value);
					return;
				case 11:
					writer.WriteDouble((double)value);
					return;
				case 12:
					global::Utf8Json.Formatters.ISO8601DateTimeFormatter.Default.Serialize(ref writer, (global::System.DateTime)value, formatterResolver);
					return;
				case 13:
					writer.WriteString((string)value);
					return;
				case 14:
					global::Utf8Json.Formatters.ByteArrayFormatter.Default.Serialize(ref writer, (byte[])value, formatterResolver);
					return;
				}
			}
			if (type.IsEnum)
			{
				writer.WriteString(type.ToString());
				return;
			}
			if (value is global::System.Collections.IDictionary dictionary)
			{
				int num = 0;
				writer.WriteBeginObject();
				foreach (global::System.Collections.DictionaryEntry item in dictionary)
				{
					if (num != 0)
					{
						writer.WriteValueSeparator();
					}
					writer.WritePropertyName((string)item.Key);
					Serialize(ref writer, item.Value, formatterResolver);
				}
				writer.WriteEndObject();
				return;
			}
			if (value is global::System.Collections.ICollection collection)
			{
				int num2 = 0;
				writer.WriteBeginArray();
				foreach (object item2 in collection)
				{
					if (num2 != 0)
					{
						writer.WriteValueSeparator();
					}
					Serialize(ref writer, item2, formatterResolver);
				}
				writer.WriteEndArray();
				return;
			}
			throw new global::System.InvalidOperationException("Not supported primitive object resolver. type:" + type.Name);
		}

		public object Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			global::Utf8Json.JsonToken currentJsonToken = reader.GetCurrentJsonToken();
			switch (currentJsonToken)
			{
			case global::Utf8Json.JsonToken.BeginObject:
			{
				global::System.Collections.Generic.Dictionary<string, object> dictionary = new global::System.Collections.Generic.Dictionary<string, object>();
				reader.ReadIsBeginObjectWithVerify();
				int count2 = 0;
				while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref count2))
				{
					string key = reader.ReadPropertyName();
					object value = Deserialize(ref reader, formatterResolver);
					dictionary.Add(key, value);
				}
				return dictionary;
			}
			case global::Utf8Json.JsonToken.BeginArray:
			{
				global::System.Collections.Generic.List<object> list = new global::System.Collections.Generic.List<object>(4);
				reader.ReadIsBeginArrayWithVerify();
				int count = 0;
				while (!reader.ReadIsEndArrayWithSkipValueSeparator(ref count))
				{
					list.Add(Deserialize(ref reader, formatterResolver));
				}
				return list;
			}
			case global::Utf8Json.JsonToken.Number:
				return reader.ReadDouble();
			case global::Utf8Json.JsonToken.String:
				return reader.ReadString();
			case global::Utf8Json.JsonToken.True:
				return reader.ReadBoolean();
			case global::Utf8Json.JsonToken.False:
				return reader.ReadBoolean();
			case global::Utf8Json.JsonToken.EndObject:
			case global::Utf8Json.JsonToken.EndArray:
			case global::Utf8Json.JsonToken.ValueSeparator:
			case global::Utf8Json.JsonToken.NameSeparator:
				throw new global::System.InvalidOperationException("Invalid Json Token:" + currentJsonToken);
			case global::Utf8Json.JsonToken.Null:
				reader.ReadIsNull();
				return null;
			default:
				return null;
			}
		}
	}
}
