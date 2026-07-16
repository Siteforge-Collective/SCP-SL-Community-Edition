namespace Utf8Json.Formatters
{
	public abstract class CollectionFormatterBase<TElement, TIntermediate, TEnumerator, TCollection> : global::Utf8Json.IJsonFormatter<TCollection>, global::Utf8Json.IJsonFormatter, global::Utf8Json.IOverwriteJsonFormatter<TCollection> where TEnumerator : global::System.Collections.Generic.IEnumerator<TElement> where TCollection : class, global::System.Collections.Generic.IEnumerable<TElement>
	{
		protected virtual global::Utf8Json.CollectionDeserializeToBehaviour? SupportedOverwriteBehaviour => null;

		public void Serialize(ref global::Utf8Json.JsonWriter writer, TCollection value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteBeginArray();
			global::Utf8Json.IJsonFormatter<TElement> formatterWithVerify = formatterResolver.GetFormatterWithVerify<TElement>();
			TEnumerator sourceEnumerator = GetSourceEnumerator(value);
			try
			{
				bool flag = true;
				while (sourceEnumerator.MoveNext())
				{
					if (flag)
					{
						flag = false;
					}
					else
					{
						writer.WriteValueSeparator();
					}
					formatterWithVerify.Serialize(ref writer, sourceEnumerator.Current, formatterResolver);
				}
			}
			finally
			{
				sourceEnumerator.Dispose();
			}
			writer.WriteEndArray();
		}

		public TCollection Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			global::Utf8Json.IJsonFormatter<TElement> formatterWithVerify = formatterResolver.GetFormatterWithVerify<TElement>();
			TIntermediate collection = Create();
			int count = 0;
			reader.ReadIsBeginArrayWithVerify();
			while (!reader.ReadIsEndArrayWithSkipValueSeparator(ref count))
			{
				Add(ref collection, count - 1, formatterWithVerify.Deserialize(ref reader, formatterResolver));
			}
			return Complete(ref collection);
		}

		public void DeserializeTo(ref TCollection value, ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (!SupportedOverwriteBehaviour.HasValue)
			{
				value = Deserialize(ref reader, formatterResolver);
			}
			else if (!reader.ReadIsNull())
			{
				global::Utf8Json.IJsonFormatter<TElement> formatterWithVerify = formatterResolver.GetFormatterWithVerify<TElement>();
				ClearOnOverwriteDeserialize(ref value);
				int count = 0;
				reader.ReadIsBeginArrayWithVerify();
				while (!reader.ReadIsEndArrayWithSkipValueSeparator(ref count))
				{
					AddOnOverwriteDeserialize(ref value, count - 1, formatterWithVerify.Deserialize(ref reader, formatterResolver));
				}
			}
		}

		protected abstract TEnumerator GetSourceEnumerator(TCollection source);

		protected abstract TIntermediate Create();

		protected abstract void Add(ref TIntermediate collection, int index, TElement value);

		protected abstract TCollection Complete(ref TIntermediate intermediateCollection);

		protected virtual void ClearOnOverwriteDeserialize(ref TCollection value)
		{
		}

		protected virtual void AddOnOverwriteDeserialize(ref TCollection collection, int index, TElement value)
		{
		}
	}
	public abstract class CollectionFormatterBase<TElement, TIntermediate, TCollection> : global::Utf8Json.Formatters.CollectionFormatterBase<TElement, TIntermediate, global::System.Collections.Generic.IEnumerator<TElement>, TCollection> where TCollection : class, global::System.Collections.Generic.IEnumerable<TElement>
	{
		protected override global::System.Collections.Generic.IEnumerator<TElement> GetSourceEnumerator(TCollection source)
		{
			return source.GetEnumerator();
		}
	}
	public abstract class CollectionFormatterBase<TElement, TCollection> : global::Utf8Json.Formatters.CollectionFormatterBase<TElement, TCollection, TCollection> where TCollection : class, global::System.Collections.Generic.IEnumerable<TElement>
	{
		protected sealed override TCollection Complete(ref TCollection intermediateCollection)
		{
			return intermediateCollection;
		}
	}
}
