namespace Utf8Json
{
	public static class JsonSerializer
	{
		public static class NonGeneric
		{
			private delegate void SerializeJsonWriter(ref global::Utf8Json.JsonWriter writer, object value, global::Utf8Json.IJsonFormatterResolver resolver);

			private delegate object DeserializeJsonReader(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver resolver);

			private class CompiledMethods
			{
				public readonly global::System.Func<object, global::Utf8Json.IJsonFormatterResolver, byte[]> serialize1;

				public readonly global::System.Action<global::System.IO.Stream, object, global::Utf8Json.IJsonFormatterResolver> serialize2;

				public readonly global::Utf8Json.JsonSerializer.NonGeneric.SerializeJsonWriter serialize3;

				public readonly global::System.Func<object, global::Utf8Json.IJsonFormatterResolver, global::System.ArraySegment<byte>> serializeUnsafe;

				public readonly global::System.Func<object, global::Utf8Json.IJsonFormatterResolver, string> toJsonString;

				public readonly global::System.Func<string, global::Utf8Json.IJsonFormatterResolver, object> deserialize1;

				public readonly global::System.Func<byte[], int, global::Utf8Json.IJsonFormatterResolver, object> deserialize2;

				public readonly global::System.Func<global::System.IO.Stream, global::Utf8Json.IJsonFormatterResolver, object> deserialize3;

				public readonly global::Utf8Json.JsonSerializer.NonGeneric.DeserializeJsonReader deserialize4;

				public CompiledMethods(global::System.Type type)
				{
					global::System.Reflection.Emit.DynamicMethod dynamicMethod = new global::System.Reflection.Emit.DynamicMethod("serialize1", typeof(byte[]), new global::System.Type[2]
					{
						typeof(object),
						typeof(global::Utf8Json.IJsonFormatterResolver)
					}, type.Module, skipVisibility: true);
					global::System.Reflection.Emit.ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(iLGenerator, 0);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitUnboxOrCast(iLGenerator, type);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(iLGenerator, 1);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(iLGenerator, GetMethod(type, "Serialize", new global::System.Type[2]
					{
						null,
						typeof(global::Utf8Json.IJsonFormatterResolver)
					}));
					iLGenerator.Emit(global::System.Reflection.Emit.OpCodes.Ret);
					serialize1 = CreateDelegate<global::System.Func<object, global::Utf8Json.IJsonFormatterResolver, byte[]>>(dynamicMethod);
					global::System.Reflection.Emit.DynamicMethod dynamicMethod2 = new global::System.Reflection.Emit.DynamicMethod("serialize2", null, new global::System.Type[3]
					{
						typeof(global::System.IO.Stream),
						typeof(object),
						typeof(global::Utf8Json.IJsonFormatterResolver)
					}, type.Module, skipVisibility: true);
					global::System.Reflection.Emit.ILGenerator iLGenerator2 = dynamicMethod2.GetILGenerator();
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(iLGenerator2, 0);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(iLGenerator2, 1);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitUnboxOrCast(iLGenerator2, type);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(iLGenerator2, 2);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(iLGenerator2, GetMethod(type, "Serialize", new global::System.Type[3]
					{
						typeof(global::System.IO.Stream),
						null,
						typeof(global::Utf8Json.IJsonFormatterResolver)
					}));
					iLGenerator2.Emit(global::System.Reflection.Emit.OpCodes.Ret);
					serialize2 = CreateDelegate<global::System.Action<global::System.IO.Stream, object, global::Utf8Json.IJsonFormatterResolver>>(dynamicMethod2);
					global::System.Reflection.Emit.DynamicMethod dynamicMethod3 = new global::System.Reflection.Emit.DynamicMethod("serialize3", null, new global::System.Type[3]
					{
						typeof(global::Utf8Json.JsonWriter).MakeByRefType(),
						typeof(object),
						typeof(global::Utf8Json.IJsonFormatterResolver)
					}, type.Module, skipVisibility: true);
					global::System.Reflection.Emit.ILGenerator iLGenerator3 = dynamicMethod3.GetILGenerator();
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(iLGenerator3, 0);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(iLGenerator3, 1);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitUnboxOrCast(iLGenerator3, type);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(iLGenerator3, 2);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(iLGenerator3, GetMethod(type, "Serialize", new global::System.Type[3]
					{
						typeof(global::Utf8Json.JsonWriter).MakeByRefType(),
						null,
						typeof(global::Utf8Json.IJsonFormatterResolver)
					}));
					iLGenerator3.Emit(global::System.Reflection.Emit.OpCodes.Ret);
					serialize3 = CreateDelegate<global::Utf8Json.JsonSerializer.NonGeneric.SerializeJsonWriter>(dynamicMethod3);
					global::System.Reflection.Emit.DynamicMethod dynamicMethod4 = new global::System.Reflection.Emit.DynamicMethod("serializeUnsafe", typeof(global::System.ArraySegment<byte>), new global::System.Type[2]
					{
						typeof(object),
						typeof(global::Utf8Json.IJsonFormatterResolver)
					}, type.Module, skipVisibility: true);
					global::System.Reflection.Emit.ILGenerator iLGenerator4 = dynamicMethod4.GetILGenerator();
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(iLGenerator4, 0);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitUnboxOrCast(iLGenerator4, type);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(iLGenerator4, 1);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(iLGenerator4, GetMethod(type, "SerializeUnsafe", new global::System.Type[2]
					{
						null,
						typeof(global::Utf8Json.IJsonFormatterResolver)
					}));
					iLGenerator4.Emit(global::System.Reflection.Emit.OpCodes.Ret);
					serializeUnsafe = CreateDelegate<global::System.Func<object, global::Utf8Json.IJsonFormatterResolver, global::System.ArraySegment<byte>>>(dynamicMethod4);
					global::System.Reflection.Emit.DynamicMethod dynamicMethod5 = new global::System.Reflection.Emit.DynamicMethod("toJsonString", typeof(string), new global::System.Type[2]
					{
						typeof(object),
						typeof(global::Utf8Json.IJsonFormatterResolver)
					}, type.Module, skipVisibility: true);
					global::System.Reflection.Emit.ILGenerator iLGenerator5 = dynamicMethod5.GetILGenerator();
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(iLGenerator5, 0);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitUnboxOrCast(iLGenerator5, type);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(iLGenerator5, 1);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(iLGenerator5, GetMethod(type, "ToJsonString", new global::System.Type[2]
					{
						null,
						typeof(global::Utf8Json.IJsonFormatterResolver)
					}));
					iLGenerator5.Emit(global::System.Reflection.Emit.OpCodes.Ret);
					toJsonString = CreateDelegate<global::System.Func<object, global::Utf8Json.IJsonFormatterResolver, string>>(dynamicMethod5);
					global::System.Reflection.Emit.DynamicMethod dynamicMethod6 = new global::System.Reflection.Emit.DynamicMethod("Deserialize", typeof(object), new global::System.Type[2]
					{
						typeof(string),
						typeof(global::Utf8Json.IJsonFormatterResolver)
					}, type.Module, skipVisibility: true);
					global::System.Reflection.Emit.ILGenerator iLGenerator6 = dynamicMethod6.GetILGenerator();
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(iLGenerator6, 0);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(iLGenerator6, 1);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(iLGenerator6, GetMethod(type, "Deserialize", new global::System.Type[2]
					{
						typeof(string),
						typeof(global::Utf8Json.IJsonFormatterResolver)
					}));
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitBoxOrDoNothing(iLGenerator6, type);
					iLGenerator6.Emit(global::System.Reflection.Emit.OpCodes.Ret);
					deserialize1 = CreateDelegate<global::System.Func<string, global::Utf8Json.IJsonFormatterResolver, object>>(dynamicMethod6);
					global::System.Reflection.Emit.DynamicMethod dynamicMethod7 = new global::System.Reflection.Emit.DynamicMethod("Deserialize", typeof(object), new global::System.Type[3]
					{
						typeof(byte[]),
						typeof(int),
						typeof(global::Utf8Json.IJsonFormatterResolver)
					}, type.Module, skipVisibility: true);
					global::System.Reflection.Emit.ILGenerator iLGenerator7 = dynamicMethod7.GetILGenerator();
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(iLGenerator7, 0);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(iLGenerator7, 1);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(iLGenerator7, 2);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(iLGenerator7, GetMethod(type, "Deserialize", new global::System.Type[3]
					{
						typeof(byte[]),
						typeof(int),
						typeof(global::Utf8Json.IJsonFormatterResolver)
					}));
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitBoxOrDoNothing(iLGenerator7, type);
					iLGenerator7.Emit(global::System.Reflection.Emit.OpCodes.Ret);
					deserialize2 = CreateDelegate<global::System.Func<byte[], int, global::Utf8Json.IJsonFormatterResolver, object>>(dynamicMethod7);
					global::System.Reflection.Emit.DynamicMethod dynamicMethod8 = new global::System.Reflection.Emit.DynamicMethod("Deserialize", typeof(object), new global::System.Type[2]
					{
						typeof(global::System.IO.Stream),
						typeof(global::Utf8Json.IJsonFormatterResolver)
					}, type.Module, skipVisibility: true);
					global::System.Reflection.Emit.ILGenerator iLGenerator8 = dynamicMethod8.GetILGenerator();
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(iLGenerator8, 0);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(iLGenerator8, 1);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(iLGenerator8, GetMethod(type, "Deserialize", new global::System.Type[2]
					{
						typeof(global::System.IO.Stream),
						typeof(global::Utf8Json.IJsonFormatterResolver)
					}));
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitBoxOrDoNothing(iLGenerator8, type);
					iLGenerator8.Emit(global::System.Reflection.Emit.OpCodes.Ret);
					deserialize3 = CreateDelegate<global::System.Func<global::System.IO.Stream, global::Utf8Json.IJsonFormatterResolver, object>>(dynamicMethod8);
					global::System.Reflection.Emit.DynamicMethod dynamicMethod9 = new global::System.Reflection.Emit.DynamicMethod("Deserialize", typeof(object), new global::System.Type[2]
					{
						typeof(global::Utf8Json.JsonReader).MakeByRefType(),
						typeof(global::Utf8Json.IJsonFormatterResolver)
					}, type.Module, skipVisibility: true);
					global::System.Reflection.Emit.ILGenerator iLGenerator9 = dynamicMethod9.GetILGenerator();
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(iLGenerator9, 0);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(iLGenerator9, 1);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(iLGenerator9, GetMethod(type, "Deserialize", new global::System.Type[2]
					{
						typeof(global::Utf8Json.JsonReader).MakeByRefType(),
						typeof(global::Utf8Json.IJsonFormatterResolver)
					}));
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitBoxOrDoNothing(iLGenerator9, type);
					iLGenerator9.Emit(global::System.Reflection.Emit.OpCodes.Ret);
					deserialize4 = CreateDelegate<global::Utf8Json.JsonSerializer.NonGeneric.DeserializeJsonReader>(dynamicMethod9);
				}

				private static T CreateDelegate<T>(global::System.Reflection.Emit.DynamicMethod dm)
				{
					return (T)(object)dm.CreateDelegate(typeof(T));
				}

				private static global::System.Reflection.MethodInfo GetMethod(global::System.Type type, string name, global::System.Type[] arguments)
				{
					return global::System.Linq.Enumerable.Single(global::System.Linq.Enumerable.Where(typeof(global::Utf8Json.JsonSerializer).GetMethods(global::System.Reflection.BindingFlags.Static | global::System.Reflection.BindingFlags.Public), (global::System.Reflection.MethodInfo x) => x.Name == name), delegate(global::System.Reflection.MethodInfo x)
					{
						global::System.Reflection.ParameterInfo[] parameters = x.GetParameters();
						if (parameters.Length != arguments.Length)
						{
							return false;
						}
						for (int i = 0; i < parameters.Length; i++)
						{
							if ((!(arguments[i] == null) || !parameters[i].ParameterType.IsGenericParameter) && parameters[i].ParameterType != arguments[i])
							{
								return false;
							}
						}
						return true;
					}).MakeGenericMethod(type);
				}
			}

			private static readonly global::System.Func<global::System.Type, global::Utf8Json.JsonSerializer.NonGeneric.CompiledMethods> CreateCompiledMethods;

			private static readonly global::Utf8Json.Internal.ThreadsafeTypeKeyHashTable<global::Utf8Json.JsonSerializer.NonGeneric.CompiledMethods> serializes;

			static NonGeneric()
			{
				serializes = new global::Utf8Json.Internal.ThreadsafeTypeKeyHashTable<global::Utf8Json.JsonSerializer.NonGeneric.CompiledMethods>(64);
				CreateCompiledMethods = (global::System.Type t) => new global::Utf8Json.JsonSerializer.NonGeneric.CompiledMethods(t);
			}

			private static global::Utf8Json.JsonSerializer.NonGeneric.CompiledMethods GetOrAdd(global::System.Type type)
			{
				return serializes.GetOrAdd(type, CreateCompiledMethods);
			}

			public static byte[] Serialize(object value)
			{
				if (value == null)
				{
					return global::Utf8Json.JsonSerializer.Serialize(value);
				}
				return Serialize(value.GetType(), value, defaultResolver);
			}

			public static byte[] Serialize(global::System.Type type, object value)
			{
				return Serialize(type, value, defaultResolver);
			}

			public static byte[] Serialize(object value, global::Utf8Json.IJsonFormatterResolver resolver)
			{
				if (value == null)
				{
					return global::Utf8Json.JsonSerializer.Serialize(value, resolver);
				}
				return Serialize(value.GetType(), value, resolver);
			}

			public static byte[] Serialize(global::System.Type type, object value, global::Utf8Json.IJsonFormatterResolver resolver)
			{
				return GetOrAdd(type).serialize1(value, resolver);
			}

			public static void Serialize(global::System.IO.Stream stream, object value)
			{
				if (value == null)
				{
					global::Utf8Json.JsonSerializer.Serialize(stream, value);
				}
				else
				{
					Serialize(value.GetType(), stream, value, defaultResolver);
				}
			}

			public static void Serialize(global::System.Type type, global::System.IO.Stream stream, object value)
			{
				Serialize(type, stream, value, defaultResolver);
			}

			public static void Serialize(global::System.IO.Stream stream, object value, global::Utf8Json.IJsonFormatterResolver resolver)
			{
				if (value == null)
				{
					global::Utf8Json.JsonSerializer.Serialize(stream, value, resolver);
				}
				else
				{
					Serialize(value.GetType(), stream, value, resolver);
				}
			}

			public static void Serialize(global::System.Type type, global::System.IO.Stream stream, object value, global::Utf8Json.IJsonFormatterResolver resolver)
			{
				GetOrAdd(type).serialize2(stream, value, resolver);
			}

			public static void Serialize(ref global::Utf8Json.JsonWriter writer, object value, global::Utf8Json.IJsonFormatterResolver resolver)
			{
				if (value == null)
				{
					writer.WriteNull();
				}
				else
				{
					Serialize(value.GetType(), ref writer, value, resolver);
				}
			}

			public static void Serialize(global::System.Type type, ref global::Utf8Json.JsonWriter writer, object value)
			{
				Serialize(type, ref writer, value, defaultResolver);
			}

			public static void Serialize(global::System.Type type, ref global::Utf8Json.JsonWriter writer, object value, global::Utf8Json.IJsonFormatterResolver resolver)
			{
				GetOrAdd(type).serialize3(ref writer, value, resolver);
			}

			public static global::System.ArraySegment<byte> SerializeUnsafe(object value)
			{
				if (value == null)
				{
					return global::Utf8Json.JsonSerializer.SerializeUnsafe(value);
				}
				return SerializeUnsafe(value.GetType(), value);
			}

			public static global::System.ArraySegment<byte> SerializeUnsafe(global::System.Type type, object value)
			{
				return SerializeUnsafe(type, value, defaultResolver);
			}

			public static global::System.ArraySegment<byte> SerializeUnsafe(object value, global::Utf8Json.IJsonFormatterResolver resolver)
			{
				if (value == null)
				{
					return global::Utf8Json.JsonSerializer.SerializeUnsafe(value);
				}
				return SerializeUnsafe(value.GetType(), value, resolver);
			}

			public static global::System.ArraySegment<byte> SerializeUnsafe(global::System.Type type, object value, global::Utf8Json.IJsonFormatterResolver resolver)
			{
				return GetOrAdd(type).serializeUnsafe(value, resolver);
			}

			public static string ToJsonString(object value)
			{
				if (value == null)
				{
					return "null";
				}
				return ToJsonString(value.GetType(), value);
			}

			public static string ToJsonString(global::System.Type type, object value)
			{
				return ToJsonString(type, value, defaultResolver);
			}

			public static string ToJsonString(object value, global::Utf8Json.IJsonFormatterResolver resolver)
			{
				if (value == null)
				{
					return "null";
				}
				return ToJsonString(value.GetType(), value, resolver);
			}

			public static string ToJsonString(global::System.Type type, object value, global::Utf8Json.IJsonFormatterResolver resolver)
			{
				return GetOrAdd(type).toJsonString(value, resolver);
			}

			public static object Deserialize(global::System.Type type, string json)
			{
				return Deserialize(type, json, defaultResolver);
			}

			public static object Deserialize(global::System.Type type, string json, global::Utf8Json.IJsonFormatterResolver resolver)
			{
				return GetOrAdd(type).deserialize1(json, resolver);
			}

			public static object Deserialize(global::System.Type type, byte[] bytes)
			{
				return Deserialize(type, bytes, defaultResolver);
			}

			public static object Deserialize(global::System.Type type, byte[] bytes, global::Utf8Json.IJsonFormatterResolver resolver)
			{
				return Deserialize(type, bytes, 0, defaultResolver);
			}

			public static object Deserialize(global::System.Type type, byte[] bytes, int offset)
			{
				return Deserialize(type, bytes, offset, defaultResolver);
			}

			public static object Deserialize(global::System.Type type, byte[] bytes, int offset, global::Utf8Json.IJsonFormatterResolver resolver)
			{
				return GetOrAdd(type).deserialize2(bytes, offset, resolver);
			}

			public static object Deserialize(global::System.Type type, global::System.IO.Stream stream)
			{
				return Deserialize(type, stream, defaultResolver);
			}

			public static object Deserialize(global::System.Type type, global::System.IO.Stream stream, global::Utf8Json.IJsonFormatterResolver resolver)
			{
				return GetOrAdd(type).deserialize3(stream, resolver);
			}

			public static object Deserialize(global::System.Type type, ref global::Utf8Json.JsonReader reader)
			{
				return Deserialize(type, ref reader, defaultResolver);
			}

			public static object Deserialize(global::System.Type type, ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver resolver)
			{
				return GetOrAdd(type).deserialize4(ref reader, resolver);
			}
		}

		private static class MemoryPool
		{
			[global::System.ThreadStatic]
			private static byte[] buffer;

			public static byte[] GetBuffer()
			{
				if (buffer == null)
				{
					buffer = new byte[65536];
				}
				return buffer;
			}
		}

		private static global::Utf8Json.IJsonFormatterResolver defaultResolver;

		private static readonly byte[][] indent = global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Select(global::System.Linq.Enumerable.Range(0, 100), (int x) => global::System.Text.Encoding.UTF8.GetBytes(new string(' ', x * 2))));

		private static readonly byte[] newLine = global::System.Text.Encoding.UTF8.GetBytes(global::System.Environment.NewLine);

		public static global::Utf8Json.IJsonFormatterResolver DefaultResolver
		{
			get
			{
				if (defaultResolver == null)
				{
					defaultResolver = global::Utf8Json.Resolvers.StandardResolver.Default;
				}
				return defaultResolver;
			}
		}

		public static bool IsInitialized => defaultResolver != null;

		public static void SetDefaultResolver(global::Utf8Json.IJsonFormatterResolver resolver)
		{
			defaultResolver = resolver;
		}

		public static byte[] Serialize<T>(T obj)
		{
			return Serialize(obj, defaultResolver);
		}

		public static byte[] Serialize<T>(T value, global::Utf8Json.IJsonFormatterResolver resolver)
		{
			if (resolver == null)
			{
				resolver = DefaultResolver;
			}
			global::Utf8Json.JsonWriter writer = new global::Utf8Json.JsonWriter(global::Utf8Json.JsonSerializer.MemoryPool.GetBuffer());
			resolver.GetFormatterWithVerify<T>().Serialize(ref writer, value, resolver);
			return writer.ToUtf8ByteArray();
		}

		public static void Serialize<T>(ref global::Utf8Json.JsonWriter writer, T value)
		{
			Serialize(ref writer, value, defaultResolver);
		}

		public static void Serialize<T>(ref global::Utf8Json.JsonWriter writer, T value, global::Utf8Json.IJsonFormatterResolver resolver)
		{
			if (resolver == null)
			{
				resolver = DefaultResolver;
			}
			resolver.GetFormatterWithVerify<T>().Serialize(ref writer, value, resolver);
		}

		public static void Serialize<T>(global::System.IO.Stream stream, T value)
		{
			Serialize(stream, value, defaultResolver);
		}

		public static void Serialize<T>(global::System.IO.Stream stream, T value, global::Utf8Json.IJsonFormatterResolver resolver)
		{
			if (resolver == null)
			{
				resolver = DefaultResolver;
			}
			global::System.ArraySegment<byte> arraySegment = SerializeUnsafe(value, resolver);
			stream.Write(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
		}

		public static global::System.ArraySegment<byte> SerializeUnsafe<T>(T obj)
		{
			return SerializeUnsafe(obj, defaultResolver);
		}

		public static global::System.ArraySegment<byte> SerializeUnsafe<T>(T value, global::Utf8Json.IJsonFormatterResolver resolver)
		{
			if (resolver == null)
			{
				resolver = DefaultResolver;
			}
			global::Utf8Json.JsonWriter writer = new global::Utf8Json.JsonWriter(global::Utf8Json.JsonSerializer.MemoryPool.GetBuffer());
			resolver.GetFormatterWithVerify<T>().Serialize(ref writer, value, resolver);
			return writer.GetBuffer();
		}

		public static string ToJsonString<T>(T value)
		{
			return ToJsonString(value, defaultResolver);
		}

		public static string ToJsonString<T>(T value, global::Utf8Json.IJsonFormatterResolver resolver)
		{
			if (resolver == null)
			{
				resolver = DefaultResolver;
			}
			global::Utf8Json.JsonWriter writer = new global::Utf8Json.JsonWriter(global::Utf8Json.JsonSerializer.MemoryPool.GetBuffer());
			resolver.GetFormatterWithVerify<T>().Serialize(ref writer, value, resolver);
			return writer.ToString();
		}

		public static T Deserialize<T>(string json)
		{
			return Deserialize<T>(json, defaultResolver);
		}

		public static T Deserialize<T>(string json, global::Utf8Json.IJsonFormatterResolver resolver)
		{
			return Deserialize<T>(global::Utf8Json.Internal.StringEncoding.UTF8.GetBytes(json), resolver);
		}

		public static T Deserialize<T>(byte[] bytes)
		{
			return Deserialize<T>(bytes, defaultResolver);
		}

		public static T Deserialize<T>(byte[] bytes, global::Utf8Json.IJsonFormatterResolver resolver)
		{
			return Deserialize<T>(bytes, 0, resolver);
		}

		public static T Deserialize<T>(byte[] bytes, int offset)
		{
			return Deserialize<T>(bytes, offset, defaultResolver);
		}

		public static T Deserialize<T>(byte[] bytes, int offset, global::Utf8Json.IJsonFormatterResolver resolver)
		{
			if (resolver == null)
			{
				resolver = DefaultResolver;
			}
			global::Utf8Json.JsonReader reader = new global::Utf8Json.JsonReader(bytes, offset);
			return resolver.GetFormatterWithVerify<T>().Deserialize(ref reader, resolver);
		}

		public static T Deserialize<T>(ref global::Utf8Json.JsonReader reader)
		{
			return Deserialize<T>(ref reader, defaultResolver);
		}

		public static T Deserialize<T>(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver resolver)
		{
			if (resolver == null)
			{
				resolver = DefaultResolver;
			}
			return resolver.GetFormatterWithVerify<T>().Deserialize(ref reader, resolver);
		}

		public static T Deserialize<T>(global::System.IO.Stream stream)
		{
			return Deserialize<T>(stream, defaultResolver);
		}

		public static T Deserialize<T>(global::System.IO.Stream stream, global::Utf8Json.IJsonFormatterResolver resolver)
		{
			if (resolver == null)
			{
				resolver = DefaultResolver;
			}
			byte[] buffer = global::Utf8Json.JsonSerializer.MemoryPool.GetBuffer();
			int newSize = FillFromStream(stream, ref buffer);
			if (new global::Utf8Json.JsonReader(buffer).GetCurrentJsonToken() == global::Utf8Json.JsonToken.Number)
			{
				buffer = global::Utf8Json.Internal.BinaryUtil.FastCloneWithResize(buffer, newSize);
			}
			return Deserialize<T>(buffer, resolver);
		}

		public static string PrettyPrint(byte[] json)
		{
			return PrettyPrint(json, 0);
		}

		public static string PrettyPrint(byte[] json, int offset)
		{
			global::Utf8Json.JsonReader reader = new global::Utf8Json.JsonReader(json, offset);
			global::Utf8Json.JsonWriter writer = new global::Utf8Json.JsonWriter(global::Utf8Json.JsonSerializer.MemoryPool.GetBuffer());
			WritePrittyPrint(ref reader, ref writer, 0);
			return writer.ToString();
		}

		public static string PrettyPrint(string json)
		{
			global::Utf8Json.JsonReader reader = new global::Utf8Json.JsonReader(global::System.Text.Encoding.UTF8.GetBytes(json));
			global::Utf8Json.JsonWriter writer = new global::Utf8Json.JsonWriter(global::Utf8Json.JsonSerializer.MemoryPool.GetBuffer());
			WritePrittyPrint(ref reader, ref writer, 0);
			return writer.ToString();
		}

		public static byte[] PrettyPrintByteArray(byte[] json)
		{
			return PrettyPrintByteArray(json, 0);
		}

		public static byte[] PrettyPrintByteArray(byte[] json, int offset)
		{
			global::Utf8Json.JsonReader reader = new global::Utf8Json.JsonReader(json, offset);
			global::Utf8Json.JsonWriter writer = new global::Utf8Json.JsonWriter(global::Utf8Json.JsonSerializer.MemoryPool.GetBuffer());
			WritePrittyPrint(ref reader, ref writer, 0);
			return writer.ToUtf8ByteArray();
		}

		public static byte[] PrettyPrintByteArray(string json)
		{
			global::Utf8Json.JsonReader reader = new global::Utf8Json.JsonReader(global::System.Text.Encoding.UTF8.GetBytes(json));
			global::Utf8Json.JsonWriter writer = new global::Utf8Json.JsonWriter(global::Utf8Json.JsonSerializer.MemoryPool.GetBuffer());
			WritePrittyPrint(ref reader, ref writer, 0);
			return writer.ToUtf8ByteArray();
		}

		private static void WritePrittyPrint(ref global::Utf8Json.JsonReader reader, ref global::Utf8Json.JsonWriter writer, int depth)
		{
			switch (reader.GetCurrentJsonToken())
			{
			case global::Utf8Json.JsonToken.BeginObject:
			{
				writer.WriteBeginObject();
				writer.WriteRaw(newLine);
				int count2 = 0;
				while (reader.ReadIsInObject(ref count2))
				{
					if (count2 != 1)
					{
						writer.WriteRaw(44);
						writer.WriteRaw(newLine);
					}
					writer.WriteRaw(indent[depth + 1]);
					writer.WritePropertyName(reader.ReadPropertyName());
					writer.WriteRaw(32);
					WritePrittyPrint(ref reader, ref writer, depth + 1);
				}
				writer.WriteRaw(newLine);
				writer.WriteRaw(indent[depth]);
				writer.WriteEndObject();
				break;
			}
			case global::Utf8Json.JsonToken.BeginArray:
			{
				writer.WriteBeginArray();
				writer.WriteRaw(newLine);
				int count = 0;
				while (reader.ReadIsInArray(ref count))
				{
					if (count != 1)
					{
						writer.WriteRaw(44);
						writer.WriteRaw(newLine);
					}
					writer.WriteRaw(indent[depth + 1]);
					WritePrittyPrint(ref reader, ref writer, depth + 1);
				}
				writer.WriteRaw(newLine);
				writer.WriteRaw(indent[depth]);
				writer.WriteEndArray();
				break;
			}
			case global::Utf8Json.JsonToken.Number:
			{
				double value3 = reader.ReadDouble();
				writer.WriteDouble(value3);
				break;
			}
			case global::Utf8Json.JsonToken.String:
			{
				string value2 = reader.ReadString();
				writer.WriteString(value2);
				break;
			}
			case global::Utf8Json.JsonToken.True:
			case global::Utf8Json.JsonToken.False:
			{
				bool value = reader.ReadBoolean();
				writer.WriteBoolean(value);
				break;
			}
			case global::Utf8Json.JsonToken.Null:
				reader.ReadIsNull();
				writer.WriteNull();
				break;
			case global::Utf8Json.JsonToken.EndObject:
			case global::Utf8Json.JsonToken.EndArray:
				break;
			}
		}

		private static int FillFromStream(global::System.IO.Stream input, ref byte[] buffer)
		{
			int num = 0;
			int num2;
			while ((num2 = input.Read(buffer, num, buffer.Length - num)) > 0)
			{
				num += num2;
				if (num == buffer.Length)
				{
					global::Utf8Json.Internal.BinaryUtil.FastResize(ref buffer, num * 2);
				}
			}
			return num;
		}
	}
}
