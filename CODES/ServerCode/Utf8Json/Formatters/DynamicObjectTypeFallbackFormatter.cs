namespace Utf8Json.Formatters
{
	public sealed class DynamicObjectTypeFallbackFormatter : global::Utf8Json.IJsonFormatter<object>, global::Utf8Json.IJsonFormatter
	{
		private delegate void SerializeMethod(object dynamicFormatter, ref global::Utf8Json.JsonWriter writer, object value, global::Utf8Json.IJsonFormatterResolver formatterResolver);

		private readonly global::Utf8Json.Internal.ThreadsafeTypeKeyHashTable<global::System.Collections.Generic.KeyValuePair<object, global::Utf8Json.Formatters.DynamicObjectTypeFallbackFormatter.SerializeMethod>> serializers = new global::Utf8Json.Internal.ThreadsafeTypeKeyHashTable<global::System.Collections.Generic.KeyValuePair<object, global::Utf8Json.Formatters.DynamicObjectTypeFallbackFormatter.SerializeMethod>>();

		private readonly global::Utf8Json.IJsonFormatterResolver[] innerResolvers;

		public DynamicObjectTypeFallbackFormatter(params global::Utf8Json.IJsonFormatterResolver[] innerResolvers)
		{
			this.innerResolvers = innerResolvers;
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, object value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			global::System.Type type = value.GetType();
			if (type == typeof(object))
			{
				writer.WriteBeginObject();
				writer.WriteEndObject();
				return;
			}
			if (!serializers.TryGetValue(type, out var value2))
			{
				lock (serializers)
				{
					if (!serializers.TryGetValue(type, out value2))
					{
						object obj = null;
						global::Utf8Json.IJsonFormatterResolver[] array = innerResolvers;
						for (int i = 0; i < array.Length; i++)
						{
							obj = array[i].GetFormatterDynamic(type);
							if (obj != null)
							{
								break;
							}
						}
						if (obj == null)
						{
							throw new global::Utf8Json.FormatterNotRegisteredException(type.FullName + " is not registered in this resolver. resolvers:" + string.Join(", ", global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Select(innerResolvers, (global::Utf8Json.IJsonFormatterResolver x) => x.GetType().Name))));
						}
						global::System.Type type2 = type;
						global::System.Reflection.Emit.DynamicMethod dynamicMethod = new global::System.Reflection.Emit.DynamicMethod("Serialize", null, new global::System.Type[4]
						{
							typeof(object),
							typeof(global::Utf8Json.JsonWriter).MakeByRefType(),
							typeof(object),
							typeof(global::Utf8Json.IJsonFormatterResolver)
						}, type.Module, skipVisibility: true);
						global::System.Reflection.Emit.ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
						global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(iLGenerator, 0);
						iLGenerator.Emit(global::System.Reflection.Emit.OpCodes.Castclass, typeof(global::Utf8Json.IJsonFormatter<>).MakeGenericType(type2));
						global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(iLGenerator, 1);
						global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(iLGenerator, 2);
						global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitUnboxOrCast(iLGenerator, type2);
						global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(iLGenerator, 3);
						global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(iLGenerator, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.Serialize(type2));
						iLGenerator.Emit(global::System.Reflection.Emit.OpCodes.Ret);
						value2 = new global::System.Collections.Generic.KeyValuePair<object, global::Utf8Json.Formatters.DynamicObjectTypeFallbackFormatter.SerializeMethod>(obj, (global::Utf8Json.Formatters.DynamicObjectTypeFallbackFormatter.SerializeMethod)dynamicMethod.CreateDelegate(typeof(global::Utf8Json.Formatters.DynamicObjectTypeFallbackFormatter.SerializeMethod)));
						serializers.TryAdd(type2, value2);
					}
				}
			}
			value2.Value(value2.Key, ref writer, value, formatterResolver);
		}

		public object Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			return global::Utf8Json.Formatters.PrimitiveObjectFormatter.Default.Deserialize(ref reader, formatterResolver);
		}
	}
}
