namespace Utf8Json.Formatters
{
	public class EnumFormatter<T> : global::Utf8Json.IJsonFormatter<T>, global::Utf8Json.IJsonFormatter, global::Utf8Json.IObjectPropertyNameFormatter<T>
	{
		private static readonly global::Utf8Json.Internal.ByteArrayStringHashTable<T> nameValueMapping;

		private static readonly global::System.Collections.Generic.Dictionary<T, string> valueNameMapping;

		private static readonly global::Utf8Json.JsonSerializeAction<T> defaultSerializeByUnderlyingValue;

		private static readonly global::Utf8Json.JsonDeserializeFunc<T> defaultDeserializeByUnderlyingValue;

		private readonly bool serializeByName;

		private readonly global::Utf8Json.JsonSerializeAction<T> serializeByUnderlyingValue;

		private readonly global::Utf8Json.JsonDeserializeFunc<T> deserializeByUnderlyingValue;

		static EnumFormatter()
		{
			global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
			global::System.Collections.Generic.List<object> list2 = new global::System.Collections.Generic.List<object>();
			global::System.Type type = typeof(T);
			foreach (global::System.Reflection.FieldInfo item in global::System.Linq.Enumerable.Where(type.GetFields(), (global::System.Reflection.FieldInfo fi) => fi.FieldType == type))
			{
				object value = item.GetValue(null);
				string name = global::System.Enum.GetName(type, value);
				global::System.Runtime.Serialization.DataMemberAttribute dataMemberAttribute = global::System.Linq.Enumerable.FirstOrDefault(global::System.Linq.Enumerable.OfType<global::System.Runtime.Serialization.DataMemberAttribute>(item.GetCustomAttributes(typeof(global::System.Runtime.Serialization.DataMemberAttribute), inherit: true)));
				global::System.Runtime.Serialization.EnumMemberAttribute enumMemberAttribute = global::System.Linq.Enumerable.FirstOrDefault(global::System.Linq.Enumerable.OfType<global::System.Runtime.Serialization.EnumMemberAttribute>(item.GetCustomAttributes(typeof(global::System.Runtime.Serialization.EnumMemberAttribute), inherit: true)));
				list2.Add(value);
				list.Add((enumMemberAttribute != null && enumMemberAttribute.Value != null) ? enumMemberAttribute.Value : ((dataMemberAttribute != null && dataMemberAttribute.Name != null) ? dataMemberAttribute.Name : name));
			}
			nameValueMapping = new global::Utf8Json.Internal.ByteArrayStringHashTable<T>(list.Count);
			valueNameMapping = new global::System.Collections.Generic.Dictionary<T, string>(list.Count);
			for (int num = 0; num < list.Count; num++)
			{
				nameValueMapping.Add(global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation(list[num]), (T)list2[num]);
				valueNameMapping[(T)list2[num]] = list[num];
			}
			bool isBoxed;
			object serializeDelegate = global::Utf8Json.Formatters.EnumFormatterHelper.GetSerializeDelegate(typeof(T), out isBoxed);
			if (isBoxed)
			{
				global::Utf8Json.JsonSerializeAction<object> boxSerialize = (global::Utf8Json.JsonSerializeAction<object>)serializeDelegate;
				defaultSerializeByUnderlyingValue = delegate(ref global::Utf8Json.JsonWriter writer, T val, global::Utf8Json.IJsonFormatterResolver _)
				{
					boxSerialize(ref writer, val, _);
				};
			}
			else
			{
				defaultSerializeByUnderlyingValue = (global::Utf8Json.JsonSerializeAction<T>)serializeDelegate;
			}
			bool isBoxed2;
			object deserializeDelegate = global::Utf8Json.Formatters.EnumFormatterHelper.GetDeserializeDelegate(typeof(T), out isBoxed2);
			if (isBoxed2)
			{
				global::Utf8Json.JsonDeserializeFunc<object> boxDeserialize = (global::Utf8Json.JsonDeserializeFunc<object>)deserializeDelegate;
				defaultDeserializeByUnderlyingValue = delegate(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver _)
				{
					return (T)boxDeserialize(ref reader, _);
				};
			}
			else
			{
				defaultDeserializeByUnderlyingValue = (global::Utf8Json.JsonDeserializeFunc<T>)deserializeDelegate;
			}
		}

		public EnumFormatter(bool serializeByName)
		{
			this.serializeByName = serializeByName;
			serializeByUnderlyingValue = defaultSerializeByUnderlyingValue;
			deserializeByUnderlyingValue = defaultDeserializeByUnderlyingValue;
		}

		public EnumFormatter(global::Utf8Json.JsonSerializeAction<T> valueSerializeAction, global::Utf8Json.JsonDeserializeFunc<T> valueDeserializeAction)
		{
			serializeByName = false;
			serializeByUnderlyingValue = valueSerializeAction;
			deserializeByUnderlyingValue = valueDeserializeAction;
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, T value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (serializeByName)
			{
				if (!valueNameMapping.TryGetValue(value, out var value2))
				{
					value2 = value.ToString();
				}
				writer.WriteString(value2);
			}
			else
			{
				serializeByUnderlyingValue(ref writer, value, formatterResolver);
			}
		}

		public T Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			switch (reader.GetCurrentJsonToken())
			{
			case global::Utf8Json.JsonToken.String:
			{
				global::System.ArraySegment<byte> key = reader.ReadStringSegmentUnsafe();
				if (!nameValueMapping.TryGetValue(key, out var value))
				{
					string value2 = global::Utf8Json.Internal.StringEncoding.UTF8.GetString(key.Array, key.Offset, key.Count);
					return (T)global::System.Enum.Parse(typeof(T), value2);
				}
				return value;
			}
			case global::Utf8Json.JsonToken.Number:
				return deserializeByUnderlyingValue(ref reader, formatterResolver);
			default:
				throw new global::System.InvalidOperationException("Can't parse JSON to Enum format.");
			}
		}

		public void SerializeToPropertyName(ref global::Utf8Json.JsonWriter writer, T value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (serializeByName)
			{
				Serialize(ref writer, value, formatterResolver);
				return;
			}
			writer.WriteQuotation();
			Serialize(ref writer, value, formatterResolver);
			writer.WriteQuotation();
		}

		public T DeserializeFromPropertyName(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (serializeByName)
			{
				return Deserialize(ref reader, formatterResolver);
			}
			if (reader.GetCurrentJsonToken() != global::Utf8Json.JsonToken.String)
			{
				throw new global::System.InvalidOperationException("Can't parse JSON to Enum format.");
			}
			reader.AdvanceOffset(1);
			T result = Deserialize(ref reader, formatterResolver);
			reader.SkipWhiteSpace();
			reader.AdvanceOffset(1);
			return result;
		}
	}
}
