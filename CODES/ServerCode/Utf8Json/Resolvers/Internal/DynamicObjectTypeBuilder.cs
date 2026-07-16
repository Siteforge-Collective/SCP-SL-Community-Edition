namespace Utf8Json.Resolvers.Internal
{
	internal static class DynamicObjectTypeBuilder
	{
		private struct DeserializeInfo
		{
			public global::Utf8Json.Internal.Emit.MetaMember MemberInfo;

			public global::System.Reflection.Emit.LocalBuilder LocalField;

			public global::System.Reflection.Emit.LocalBuilder IsDeserializedField;
		}

		internal static class EmitInfo
		{
			internal static class JsonWriter
			{
				public static readonly global::System.Reflection.MethodInfo GetEncodedPropertyNameWithBeginObject;

				public static readonly global::System.Reflection.MethodInfo GetEncodedPropertyNameWithPrefixValueSeparator;

				public static readonly global::System.Reflection.MethodInfo GetEncodedPropertyNameWithoutQuotation;

				public static readonly global::System.Reflection.MethodInfo GetEncodedPropertyName;

				public static readonly global::System.Reflection.MethodInfo WriteNull;

				public static readonly global::System.Reflection.MethodInfo WriteRaw;

				public static readonly global::System.Reflection.MethodInfo WriteBeginObject;

				public static readonly global::System.Reflection.MethodInfo WriteEndObject;

				public static readonly global::System.Reflection.MethodInfo WriteValueSeparator;

				static JsonWriter()
				{
					GetEncodedPropertyNameWithBeginObject = global::Utf8Json.Internal.Emit.ExpressionUtility.GetMethodInfo(() => global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject(null));
					GetEncodedPropertyNameWithPrefixValueSeparator = global::Utf8Json.Internal.Emit.ExpressionUtility.GetMethodInfo(() => global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator(null));
					GetEncodedPropertyNameWithoutQuotation = global::Utf8Json.Internal.Emit.ExpressionUtility.GetMethodInfo(() => global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation(null));
					GetEncodedPropertyName = global::Utf8Json.Internal.Emit.ExpressionUtility.GetMethodInfo(() => global::Utf8Json.JsonWriter.GetEncodedPropertyName(null));
					WriteNull = global::Utf8Json.Internal.Emit.ExpressionUtility.GetMethodInfo((global::Utf8Json.JsonWriter writer) => writer.WriteNull());
					WriteRaw = global::Utf8Json.Internal.Emit.ExpressionUtility.GetMethodInfo((global::Utf8Json.JsonWriter writer) => writer.WriteRaw(null));
					WriteBeginObject = global::Utf8Json.Internal.Emit.ExpressionUtility.GetMethodInfo((global::Utf8Json.JsonWriter writer) => writer.WriteBeginObject());
					WriteEndObject = global::Utf8Json.Internal.Emit.ExpressionUtility.GetMethodInfo((global::Utf8Json.JsonWriter writer) => writer.WriteEndObject());
					WriteValueSeparator = global::Utf8Json.Internal.Emit.ExpressionUtility.GetMethodInfo((global::Utf8Json.JsonWriter writer) => writer.WriteValueSeparator());
				}
			}

			internal static class JsonReader
			{
				public static readonly global::System.Reflection.MethodInfo ReadIsNull;

				public static readonly global::System.Reflection.MethodInfo ReadIsBeginObjectWithVerify;

				public static readonly global::System.Reflection.MethodInfo ReadIsEndObjectWithSkipValueSeparator;

				public static readonly global::System.Reflection.MethodInfo ReadPropertyNameSegmentUnsafe;

				public static readonly global::System.Reflection.MethodInfo ReadNextBlock;

				public static readonly global::System.Reflection.MethodInfo GetBufferUnsafe;

				public static readonly global::System.Reflection.MethodInfo GetCurrentOffsetUnsafe;

				static JsonReader()
				{
					ReadIsNull = global::Utf8Json.Internal.Emit.ExpressionUtility.GetMethodInfo((global::Utf8Json.JsonReader reader) => reader.ReadIsNull());
					ReadIsBeginObjectWithVerify = global::Utf8Json.Internal.Emit.ExpressionUtility.GetMethodInfo((global::Utf8Json.JsonReader reader) => reader.ReadIsBeginObjectWithVerify());
					ReadIsEndObjectWithSkipValueSeparator = global::Utf8Json.Internal.Emit.ExpressionUtility.GetMethodInfo((global::Utf8Json.JsonReader reader, int count) => reader.ReadIsEndObjectWithSkipValueSeparator(ref count));
					ReadPropertyNameSegmentUnsafe = global::Utf8Json.Internal.Emit.ExpressionUtility.GetMethodInfo((global::Utf8Json.JsonReader reader) => reader.ReadPropertyNameSegmentRaw());
					ReadNextBlock = global::Utf8Json.Internal.Emit.ExpressionUtility.GetMethodInfo((global::Utf8Json.JsonReader reader) => reader.ReadNextBlock());
					GetBufferUnsafe = global::Utf8Json.Internal.Emit.ExpressionUtility.GetMethodInfo((global::Utf8Json.JsonReader reader) => reader.GetBufferUnsafe());
					GetCurrentOffsetUnsafe = global::Utf8Json.Internal.Emit.ExpressionUtility.GetMethodInfo((global::Utf8Json.JsonReader reader) => reader.GetCurrentOffsetUnsafe());
				}
			}

			internal static class JsonFormatterAttr
			{
				internal static readonly global::System.Reflection.MethodInfo FormatterType = global::Utf8Json.Internal.Emit.ExpressionUtility.GetPropertyInfo((global::Utf8Json.JsonFormatterAttribute attr) => attr.FormatterType).GetGetMethod();

				internal static readonly global::System.Reflection.MethodInfo Arguments = global::Utf8Json.Internal.Emit.ExpressionUtility.GetPropertyInfo((global::Utf8Json.JsonFormatterAttribute attr) => attr.Arguments).GetGetMethod();
			}

			public static readonly global::System.Reflection.ConstructorInfo ObjectCtor = global::System.Linq.Enumerable.First(global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(object)).DeclaredConstructors, (global::System.Reflection.ConstructorInfo x) => x.GetParameters().Length == 0);

			public static readonly global::System.Reflection.MethodInfo GetFormatterWithVerify = global::System.Reflection.RuntimeReflectionExtensions.GetRuntimeMethod(typeof(global::Utf8Json.JsonFormatterResolverExtensions), "GetFormatterWithVerify", new global::System.Type[1] { typeof(global::Utf8Json.IJsonFormatterResolver) });

			public static readonly global::System.Reflection.ConstructorInfo InvalidOperationExceptionConstructor = global::System.Linq.Enumerable.First(global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::System.InvalidOperationException)).DeclaredConstructors, delegate(global::System.Reflection.ConstructorInfo x)
			{
				global::System.Reflection.ParameterInfo[] parameters = x.GetParameters();
				return parameters.Length == 1 && parameters[0].ParameterType == typeof(string);
			});

			public static readonly global::System.Reflection.MethodInfo GetTypeFromHandle = global::Utf8Json.Internal.Emit.ExpressionUtility.GetMethodInfo(() => global::System.Type.GetTypeFromHandle(default(global::System.RuntimeTypeHandle)));

			public static readonly global::System.Reflection.MethodInfo TypeGetProperty = global::Utf8Json.Internal.Emit.ExpressionUtility.GetMethodInfo((global::System.Type t) => t.GetProperty(null, global::System.Reflection.BindingFlags.Default));

			public static readonly global::System.Reflection.MethodInfo TypeGetField = global::Utf8Json.Internal.Emit.ExpressionUtility.GetMethodInfo((global::System.Type t) => t.GetField(null, global::System.Reflection.BindingFlags.Default));

			public static readonly global::System.Reflection.MethodInfo GetCustomAttributeJsonFormatterAttribute = global::Utf8Json.Internal.Emit.ExpressionUtility.GetMethodInfo(() => global::System.Reflection.CustomAttributeExtensions.GetCustomAttribute<global::Utf8Json.JsonFormatterAttribute>((global::System.Reflection.MemberInfo)null, false));

			public static readonly global::System.Reflection.MethodInfo ActivatorCreateInstance = global::Utf8Json.Internal.Emit.ExpressionUtility.GetMethodInfo(() => global::System.Activator.CreateInstance((global::System.Type)null, (object[])null));

			public static readonly global::System.Reflection.MethodInfo GetUninitializedObject = global::Utf8Json.Internal.Emit.ExpressionUtility.GetMethodInfo(() => global::System.Runtime.Serialization.FormatterServices.GetUninitializedObject(null));

			public static readonly global::System.Reflection.MethodInfo GetTypeMethod = global::Utf8Json.Internal.Emit.ExpressionUtility.GetMethodInfo((object o) => o.GetType());

			public static readonly global::System.Reflection.MethodInfo TypeEquals = global::Utf8Json.Internal.Emit.ExpressionUtility.GetMethodInfo((global::System.Type t) => t.Equals(null));

			public static readonly global::System.Reflection.MethodInfo NongenericSerialize = global::Utf8Json.Internal.Emit.ExpressionUtility.GetMethodInfo((global::Utf8Json.JsonWriter writer) => global::Utf8Json.JsonSerializer.NonGeneric.Serialize(null, ref writer, null, null));

			public static global::System.Reflection.MethodInfo Serialize(global::System.Type type)
			{
				return global::System.Reflection.RuntimeReflectionExtensions.GetRuntimeMethod(typeof(global::Utf8Json.IJsonFormatter<>).MakeGenericType(type), "Serialize", new global::System.Type[3]
				{
					typeof(global::Utf8Json.JsonWriter).MakeByRefType(),
					type,
					typeof(global::Utf8Json.IJsonFormatterResolver)
				});
			}

			public static global::System.Reflection.MethodInfo Deserialize(global::System.Type type)
			{
				return global::System.Reflection.RuntimeReflectionExtensions.GetRuntimeMethod(typeof(global::Utf8Json.IJsonFormatter<>).MakeGenericType(type), "Deserialize", new global::System.Type[2]
				{
					typeof(global::Utf8Json.JsonReader).MakeByRefType(),
					typeof(global::Utf8Json.IJsonFormatterResolver)
				});
			}

			public static global::System.Reflection.MethodInfo GetNullableHasValue(global::System.Type type)
			{
				return global::System.Reflection.RuntimeReflectionExtensions.GetRuntimeProperty(typeof(global::System.Nullable<>).MakeGenericType(type), "HasValue").GetGetMethod();
			}
		}

		internal class Utf8JsonDynamicObjectResolverException : global::System.Exception
		{
			public Utf8JsonDynamicObjectResolverException(string message)
				: base(message)
			{
			}
		}

		private static readonly global::System.Text.RegularExpressions.Regex SubtractFullNameRegex = new global::System.Text.RegularExpressions.Regex(", Version=\\d+.\\d+.\\d+.\\d+, Culture=\\w+, PublicKeyToken=\\w+");

		private static int nameSequence = 0;

		private static global::System.Collections.Generic.HashSet<global::System.Type> ignoreTypes = new global::System.Collections.Generic.HashSet<global::System.Type>
		{
			typeof(object),
			typeof(short),
			typeof(int),
			typeof(long),
			typeof(ushort),
			typeof(uint),
			typeof(ulong),
			typeof(float),
			typeof(double),
			typeof(bool),
			typeof(byte),
			typeof(sbyte),
			typeof(decimal),
			typeof(char),
			typeof(string),
			typeof(global::System.Guid),
			typeof(global::System.TimeSpan),
			typeof(global::System.DateTime),
			typeof(global::System.DateTimeOffset)
		};

		private static global::System.Collections.Generic.HashSet<global::System.Type> jsonPrimitiveTypes = new global::System.Collections.Generic.HashSet<global::System.Type>
		{
			typeof(short),
			typeof(int),
			typeof(long),
			typeof(ushort),
			typeof(uint),
			typeof(ulong),
			typeof(float),
			typeof(double),
			typeof(bool),
			typeof(byte),
			typeof(sbyte),
			typeof(string)
		};

		public static object BuildFormatterToAssembly<T>(global::Utf8Json.Internal.Emit.DynamicAssembly assembly, global::Utf8Json.IJsonFormatterResolver selfResolver, global::System.Func<string, string> nameMutator, bool excludeNull)
		{
			global::System.Reflection.TypeInfo typeInfo = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(T));
			if (global::Utf8Json.Internal.ReflectionExtensions.IsNullable(typeInfo))
			{
				typeInfo = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeInfo.GenericTypeArguments[0]);
				object formatterDynamic = selfResolver.GetFormatterDynamic(typeInfo.AsType());
				if (formatterDynamic == null)
				{
					return null;
				}
				return (global::Utf8Json.IJsonFormatter<T>)global::System.Activator.CreateInstance(typeof(global::Utf8Json.Formatters.StaticNullableFormatter<>).MakeGenericType(typeInfo.AsType()), formatterDynamic);
			}
			if (global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::System.Exception)).IsAssignableFrom(typeInfo))
			{
				return BuildAnonymousFormatter(typeof(T), nameMutator, excludeNull, allowPrivate: false, isException: true);
			}
			if (global::Utf8Json.Internal.ReflectionExtensions.IsAnonymous(typeInfo) || TryGetInterfaceEnumerableElementType(typeof(T), out var _))
			{
				return BuildAnonymousFormatter(typeof(T), nameMutator, excludeNull, allowPrivate: false, isException: false);
			}
			global::System.Reflection.TypeInfo typeInfo2 = BuildType(assembly, typeof(T), nameMutator, excludeNull);
			if (typeInfo2 == null)
			{
				return null;
			}
			return (global::Utf8Json.IJsonFormatter<T>)global::System.Activator.CreateInstance(typeInfo2.AsType());
		}

		public static object BuildFormatterToDynamicMethod<T>(global::Utf8Json.IJsonFormatterResolver selfResolver, global::System.Func<string, string> nameMutator, bool excludeNull, bool allowPrivate)
		{
			global::System.Reflection.TypeInfo typeInfo = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(T));
			if (global::Utf8Json.Internal.ReflectionExtensions.IsNullable(typeInfo))
			{
				typeInfo = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeInfo.GenericTypeArguments[0]);
				object formatterDynamic = selfResolver.GetFormatterDynamic(typeInfo.AsType());
				if (formatterDynamic == null)
				{
					return null;
				}
				return (global::Utf8Json.IJsonFormatter<T>)global::System.Activator.CreateInstance(typeof(global::Utf8Json.Formatters.StaticNullableFormatter<>).MakeGenericType(typeInfo.AsType()), formatterDynamic);
			}
			if (global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::System.Exception)).IsAssignableFrom(typeInfo))
			{
				return BuildAnonymousFormatter(typeof(T), nameMutator, excludeNull, allowPrivate: false, isException: true);
			}
			return BuildAnonymousFormatter(typeof(T), nameMutator, excludeNull, allowPrivate, isException: false);
		}

		private static global::System.Reflection.TypeInfo BuildType(global::Utf8Json.Internal.Emit.DynamicAssembly assembly, global::System.Type type, global::System.Func<string, string> nameMutator, bool excludeNull)
		{
			if (ignoreTypes.Contains(type))
			{
				return null;
			}
			global::Utf8Json.Internal.Emit.MetaType metaType = new global::Utf8Json.Internal.Emit.MetaType(type, nameMutator, allowPrivate: false);
			bool hasShouldSerialize = global::System.Linq.Enumerable.Any(metaType.Members, (global::Utf8Json.Internal.Emit.MetaMember x) => x.ShouldSerializeMethodInfo != null);
			global::System.Type type2 = typeof(global::Utf8Json.IJsonFormatter<>).MakeGenericType(type);
			global::System.Reflection.Emit.TypeBuilder typeBuilder = assembly.DefineType("Utf8Json.Formatters." + SubtractFullNameRegex.Replace(type.FullName, "").Replace(".", "_") + "Formatter" + global::System.Threading.Interlocked.Increment(ref nameSequence), global::System.Reflection.TypeAttributes.Public | global::System.Reflection.TypeAttributes.Sealed, null, new global::System.Type[1] { type2 });
			global::System.Reflection.Emit.ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(global::System.Reflection.MethodAttributes.Public, global::System.Reflection.CallingConventions.Standard, global::System.Type.EmptyTypes);
			global::System.Reflection.Emit.FieldBuilder stringByteKeysField = typeBuilder.DefineField("stringByteKeys", typeof(byte[][]), global::System.Reflection.FieldAttributes.Private | global::System.Reflection.FieldAttributes.InitOnly);
			global::System.Reflection.Emit.ILGenerator iLGenerator = constructorBuilder.GetILGenerator();
			global::System.Collections.Generic.Dictionary<global::Utf8Json.Internal.Emit.MetaMember, global::System.Reflection.FieldInfo> customFormatterLookup = BuildConstructor(typeBuilder, metaType, constructorBuilder, stringByteKeysField, iLGenerator, excludeNull, hasShouldSerialize);
			global::System.Reflection.Emit.MethodBuilder methodBuilder = typeBuilder.DefineMethod("Serialize", global::System.Reflection.MethodAttributes.Public | global::System.Reflection.MethodAttributes.Final | global::System.Reflection.MethodAttributes.Virtual, null, new global::System.Type[3]
			{
				typeof(global::Utf8Json.JsonWriter).MakeByRefType(),
				type,
				typeof(global::Utf8Json.IJsonFormatterResolver)
			});
			global::System.Reflection.Emit.ILGenerator il = methodBuilder.GetILGenerator();
			BuildSerialize(type, metaType, il, delegate
			{
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLoadThis(il);
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdfld(il, stringByteKeysField);
			}, delegate(int index, global::Utf8Json.Internal.Emit.MetaMember member)
			{
				if (!customFormatterLookup.TryGetValue(member, out var value))
				{
					return false;
				}
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLoadThis(il);
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdfld(il, value);
				return true;
			}, excludeNull, hasShouldSerialize, 1);
			global::System.Reflection.Emit.MethodBuilder methodBuilder2 = typeBuilder.DefineMethod("Deserialize", global::System.Reflection.MethodAttributes.Public | global::System.Reflection.MethodAttributes.Final | global::System.Reflection.MethodAttributes.Virtual, type, new global::System.Type[2]
			{
				typeof(global::Utf8Json.JsonReader).MakeByRefType(),
				typeof(global::Utf8Json.IJsonFormatterResolver)
			});
			global::System.Reflection.Emit.ILGenerator il2 = methodBuilder2.GetILGenerator();
			BuildDeserialize(type, metaType, il2, delegate(int index, global::Utf8Json.Internal.Emit.MetaMember member)
			{
				if (!customFormatterLookup.TryGetValue(member, out var value))
				{
					return false;
				}
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLoadThis(il2);
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdfld(il2, value);
				return true;
			}, useGetUninitializedObject: false, 1);
			return typeBuilder.CreateTypeInfo();
		}

		public static object BuildAnonymousFormatter(global::System.Type type, global::System.Func<string, string> nameMutator, bool excludeNull, bool allowPrivate, bool isException)
		{
			if (ignoreTypes.Contains(type))
			{
				return false;
			}
			global::Utf8Json.Internal.Emit.MetaType metaType;
			if (isException)
			{
				global::System.Collections.Generic.HashSet<string> ignoreSet = new global::System.Collections.Generic.HashSet<string>(global::System.Linq.Enumerable.Select(new string[6] { "HelpLink", "TargetSite", "HResult", "Data", "ClassName", "InnerException" }, (string x) => nameMutator(x)));
				metaType = new global::Utf8Json.Internal.Emit.MetaType(type, nameMutator, allowPrivate: false);
				metaType.BestmatchConstructor = null;
				metaType.ConstructorParameters = new global::Utf8Json.Internal.Emit.MetaMember[0];
				metaType.Members = global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Concat(global::System.Linq.Enumerable.Concat(new global::Utf8Json.Internal.Emit.StringConstantValueMetaMember[1]
				{
					new global::Utf8Json.Internal.Emit.StringConstantValueMetaMember(nameMutator("ClassName"), type.FullName)
				}, global::System.Linq.Enumerable.Where(metaType.Members, (global::Utf8Json.Internal.Emit.MetaMember x) => !ignoreSet.Contains(x.Name))), new global::Utf8Json.Internal.Emit.InnerExceptionMetaMember[1]
				{
					new global::Utf8Json.Internal.Emit.InnerExceptionMetaMember(nameMutator("InnerException"))
				}));
			}
			else
			{
				metaType = new global::Utf8Json.Internal.Emit.MetaType(type, nameMutator, allowPrivate);
			}
			bool flag = global::System.Linq.Enumerable.Any(metaType.Members, (global::Utf8Json.Internal.Emit.MetaMember x) => x.ShouldSerializeMethodInfo != null);
			global::System.Collections.Generic.List<byte[]> list = new global::System.Collections.Generic.List<byte[]>();
			int num = 0;
			foreach (global::Utf8Json.Internal.Emit.MetaMember item3 in global::System.Linq.Enumerable.Where(metaType.Members, (global::Utf8Json.Internal.Emit.MetaMember x) => x.IsReadable))
			{
				if (excludeNull || flag)
				{
					list.Add(global::Utf8Json.JsonWriter.GetEncodedPropertyName(item3.Name));
				}
				else if (num == 0)
				{
					list.Add(global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject(item3.Name));
				}
				else
				{
					list.Add(global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator(item3.Name));
				}
				num++;
			}
			global::System.Collections.Generic.List<object> serializeCustomFormatters = new global::System.Collections.Generic.List<object>();
			global::System.Collections.Generic.List<object> deserializeCustomFormatters = new global::System.Collections.Generic.List<object>();
			foreach (global::Utf8Json.Internal.Emit.MetaMember item4 in global::System.Linq.Enumerable.Where(metaType.Members, (global::Utf8Json.Internal.Emit.MetaMember x) => x.IsReadable))
			{
				global::Utf8Json.JsonFormatterAttribute customAttribute = item4.GetCustomAttribute<global::Utf8Json.JsonFormatterAttribute>(inherit: true);
				if (customAttribute != null)
				{
					object item = global::System.Activator.CreateInstance(customAttribute.FormatterType, customAttribute.Arguments);
					serializeCustomFormatters.Add(item);
				}
				else
				{
					serializeCustomFormatters.Add(null);
				}
			}
			global::Utf8Json.Internal.Emit.MetaMember[] members = metaType.Members;
			for (int num2 = 0; num2 < members.Length; num2++)
			{
				global::Utf8Json.JsonFormatterAttribute customAttribute2 = members[num2].GetCustomAttribute<global::Utf8Json.JsonFormatterAttribute>(inherit: true);
				if (customAttribute2 != null)
				{
					object item2 = global::System.Activator.CreateInstance(customAttribute2.FormatterType, customAttribute2.Arguments);
					deserializeCustomFormatters.Add(item2);
				}
				else
				{
					deserializeCustomFormatters.Add(null);
				}
			}
			global::System.Reflection.Emit.DynamicMethod dynamicMethod = new global::System.Reflection.Emit.DynamicMethod("Serialize", null, new global::System.Type[5]
			{
				typeof(byte[][]),
				typeof(object[]),
				typeof(global::Utf8Json.JsonWriter).MakeByRefType(),
				type,
				typeof(global::Utf8Json.IJsonFormatterResolver)
			}, type.Module, skipVisibility: true);
			global::System.Reflection.Emit.ILGenerator il = dynamicMethod.GetILGenerator();
			BuildSerialize(type, metaType, il, delegate
			{
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(il, 0);
			}, delegate(int index, global::Utf8Json.Internal.Emit.MetaMember member)
			{
				if (serializeCustomFormatters.Count == 0)
				{
					return false;
				}
				if (serializeCustomFormatters[index] == null)
				{
					return false;
				}
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(il, 1);
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdc_I4(il, index);
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldelem_Ref);
				il.Emit(global::System.Reflection.Emit.OpCodes.Castclass, serializeCustomFormatters[index].GetType());
				return true;
			}, excludeNull, flag, 2);
			global::System.Reflection.Emit.DynamicMethod dynamicMethod2 = new global::System.Reflection.Emit.DynamicMethod("Deserialize", type, new global::System.Type[3]
			{
				typeof(object[]),
				typeof(global::Utf8Json.JsonReader).MakeByRefType(),
				typeof(global::Utf8Json.IJsonFormatterResolver)
			}, type.Module, skipVisibility: true);
			global::System.Reflection.Emit.ILGenerator il2 = dynamicMethod2.GetILGenerator();
			BuildDeserialize(type, metaType, il2, delegate(int index, global::Utf8Json.Internal.Emit.MetaMember member)
			{
				if (deserializeCustomFormatters.Count == 0)
				{
					return false;
				}
				if (deserializeCustomFormatters[index] == null)
				{
					return false;
				}
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(il2, 0);
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdc_I4(il2, index);
				il2.Emit(global::System.Reflection.Emit.OpCodes.Ldelem_Ref);
				il2.Emit(global::System.Reflection.Emit.OpCodes.Castclass, deserializeCustomFormatters[index].GetType());
				return true;
			}, useGetUninitializedObject: true, 1);
			object obj = dynamicMethod.CreateDelegate(typeof(global::Utf8Json.Resolvers.Internal.AnonymousJsonSerializeAction<>).MakeGenericType(type));
			object obj2 = dynamicMethod2.CreateDelegate(typeof(global::Utf8Json.Resolvers.Internal.AnonymousJsonDeserializeFunc<>).MakeGenericType(type));
			return global::System.Activator.CreateInstance(typeof(global::Utf8Json.Resolvers.Internal.DynamicMethodAnonymousFormatter<>).MakeGenericType(type), list.ToArray(), serializeCustomFormatters.ToArray(), deserializeCustomFormatters.ToArray(), obj, obj2);
		}

		private static global::System.Collections.Generic.Dictionary<global::Utf8Json.Internal.Emit.MetaMember, global::System.Reflection.FieldInfo> BuildConstructor(global::System.Reflection.Emit.TypeBuilder builder, global::Utf8Json.Internal.Emit.MetaType info, global::System.Reflection.ConstructorInfo method, global::System.Reflection.Emit.FieldBuilder stringByteKeysField, global::System.Reflection.Emit.ILGenerator il, bool excludeNull, bool hasShouldSerialize)
		{
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(il, 0);
			il.Emit(global::System.Reflection.Emit.OpCodes.Call, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.ObjectCtor);
			int value = global::System.Linq.Enumerable.Count(info.Members, (global::Utf8Json.Internal.Emit.MetaMember x) => x.IsReadable);
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(il, 0);
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdc_I4(il, value);
			il.Emit(global::System.Reflection.Emit.OpCodes.Newarr, typeof(byte[]));
			int num = 0;
			foreach (global::Utf8Json.Internal.Emit.MetaMember item in global::System.Linq.Enumerable.Where(info.Members, (global::Utf8Json.Internal.Emit.MetaMember x) => x.IsReadable))
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Dup);
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdc_I4(il, num);
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldstr, item.Name);
				if (excludeNull || hasShouldSerialize)
				{
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.JsonWriter.GetEncodedPropertyName);
				}
				else if (num == 0)
				{
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.JsonWriter.GetEncodedPropertyNameWithBeginObject);
				}
				else
				{
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator);
				}
				il.Emit(global::System.Reflection.Emit.OpCodes.Stelem_Ref);
				num++;
			}
			il.Emit(global::System.Reflection.Emit.OpCodes.Stfld, stringByteKeysField);
			global::System.Collections.Generic.Dictionary<global::Utf8Json.Internal.Emit.MetaMember, global::System.Reflection.FieldInfo> result = BuildCustomFormatterField(builder, info, il);
			il.Emit(global::System.Reflection.Emit.OpCodes.Ret);
			return result;
		}

		private static global::System.Collections.Generic.Dictionary<global::Utf8Json.Internal.Emit.MetaMember, global::System.Reflection.FieldInfo> BuildCustomFormatterField(global::System.Reflection.Emit.TypeBuilder builder, global::Utf8Json.Internal.Emit.MetaType info, global::System.Reflection.Emit.ILGenerator il)
		{
			global::System.Collections.Generic.Dictionary<global::Utf8Json.Internal.Emit.MetaMember, global::System.Reflection.FieldInfo> dictionary = new global::System.Collections.Generic.Dictionary<global::Utf8Json.Internal.Emit.MetaMember, global::System.Reflection.FieldInfo>();
			foreach (global::Utf8Json.Internal.Emit.MetaMember item in global::System.Linq.Enumerable.Where(info.Members, (global::Utf8Json.Internal.Emit.MetaMember x) => x.IsReadable || x.IsWritable))
			{
				global::Utf8Json.JsonFormatterAttribute customAttribute = item.GetCustomAttribute<global::Utf8Json.JsonFormatterAttribute>(inherit: true);
				if (customAttribute != null)
				{
					global::System.Reflection.Emit.FieldBuilder fieldBuilder = builder.DefineField(item.Name + "_formatter", customAttribute.FormatterType, global::System.Reflection.FieldAttributes.Private | global::System.Reflection.FieldAttributes.InitOnly);
					int value = 52;
					global::System.Reflection.Emit.LocalBuilder local = il.DeclareLocal(typeof(global::Utf8Json.JsonFormatterAttribute));
					il.Emit(global::System.Reflection.Emit.OpCodes.Ldtoken, info.Type);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.GetTypeFromHandle);
					il.Emit(global::System.Reflection.Emit.OpCodes.Ldstr, item.MemberName);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdc_I4(il, value);
					if (item.IsProperty)
					{
						global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.TypeGetProperty);
					}
					else
					{
						global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.TypeGetField);
					}
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitTrue(il);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.GetCustomAttributeJsonFormatterAttribute);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitStloc(il, local);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLoadThis(il);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloc(il, local);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.JsonFormatterAttr.FormatterType);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloc(il, local);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.JsonFormatterAttr.Arguments);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.ActivatorCreateInstance);
					il.Emit(global::System.Reflection.Emit.OpCodes.Castclass, customAttribute.FormatterType);
					il.Emit(global::System.Reflection.Emit.OpCodes.Stfld, fieldBuilder);
					dictionary.Add(item, fieldBuilder);
				}
			}
			return dictionary;
		}

		private static void BuildSerialize(global::System.Type type, global::Utf8Json.Internal.Emit.MetaType info, global::System.Reflection.Emit.ILGenerator il, global::System.Action emitStringByteKeys, global::System.Func<int, global::Utf8Json.Internal.Emit.MetaMember, bool> tryEmitLoadCustomFormatter, bool excludeNull, bool hasShouldSerialize, int firstArgIndex)
		{
			global::Utf8Json.Internal.Emit.ArgumentField argumentField = new global::Utf8Json.Internal.Emit.ArgumentField(il, firstArgIndex);
			global::Utf8Json.Internal.Emit.ArgumentField argValue = new global::Utf8Json.Internal.Emit.ArgumentField(il, firstArgIndex + 1, type);
			global::Utf8Json.Internal.Emit.ArgumentField argResolver = new global::Utf8Json.Internal.Emit.ArgumentField(il, firstArgIndex + 2);
			global::System.Reflection.TypeInfo typeInfo = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(type);
			global::Utf8Json.Internal.Emit.InnerExceptionMetaMember innerExceptionMetaMember = global::System.Linq.Enumerable.FirstOrDefault(global::System.Linq.Enumerable.OfType<global::Utf8Json.Internal.Emit.InnerExceptionMetaMember>(info.Members));
			if (innerExceptionMetaMember != null)
			{
				innerExceptionMetaMember.argWriter = argumentField;
				innerExceptionMetaMember.argValue = argValue;
				innerExceptionMetaMember.argResolver = argResolver;
			}
			if (info.IsClass && info.BestmatchConstructor == null && TryGetInterfaceEnumerableElementType(type, out var elementType))
			{
				global::System.Type type2 = typeof(global::System.Collections.Generic.IEnumerable<>).MakeGenericType(elementType);
				argResolver.EmitLoad();
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.GetFormatterWithVerify.MakeGenericMethod(type2));
				argumentField.EmitLoad();
				argValue.EmitLoad();
				argResolver.EmitLoad();
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.Serialize(type2));
				il.Emit(global::System.Reflection.Emit.OpCodes.Ret);
				return;
			}
			if (info.IsClass)
			{
				global::System.Reflection.Emit.Label label = il.DefineLabel();
				argValue.EmitLoad();
				il.Emit(global::System.Reflection.Emit.OpCodes.Brtrue_S, label);
				argumentField.EmitLoad();
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.JsonWriter.WriteNull);
				il.Emit(global::System.Reflection.Emit.OpCodes.Ret);
				il.MarkLabel(label);
			}
			if (type == typeof(global::System.Exception))
			{
				global::System.Reflection.Emit.Label label2 = il.DefineLabel();
				global::System.Reflection.Emit.LocalBuilder local = il.DeclareLocal(typeof(global::System.Type));
				argValue.EmitLoad();
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.GetTypeMethod);
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitStloc(il, local);
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloc(il, local);
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldtoken, typeof(global::System.Exception));
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.GetTypeFromHandle);
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.TypeEquals);
				il.Emit(global::System.Reflection.Emit.OpCodes.Brtrue, label2);
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloc(il, local);
				argumentField.EmitLoad();
				argValue.EmitLoad();
				argResolver.EmitLoad();
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.NongenericSerialize);
				il.Emit(global::System.Reflection.Emit.OpCodes.Ret);
				il.MarkLabel(label2);
			}
			global::System.Reflection.Emit.LocalBuilder local2 = null;
			global::System.Reflection.Emit.Label label3 = il.DefineLabel();
			global::System.Reflection.Emit.Label[] array = null;
			if (excludeNull || hasShouldSerialize)
			{
				local2 = il.DeclareLocal(typeof(bool));
				argumentField.EmitLoad();
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.JsonWriter.WriteBeginObject);
				array = global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Select(global::System.Linq.Enumerable.Where(info.Members, (global::Utf8Json.Internal.Emit.MetaMember x) => x.IsReadable), (global::Utf8Json.Internal.Emit.MetaMember _) => il.DefineLabel()));
			}
			int num = 0;
			foreach (global::Utf8Json.Internal.Emit.MetaMember item in global::System.Linq.Enumerable.Where(info.Members, (global::Utf8Json.Internal.Emit.MetaMember x) => x.IsReadable))
			{
				if (excludeNull || hasShouldSerialize)
				{
					il.MarkLabel(array[num]);
					if (excludeNull)
					{
						if (global::Utf8Json.Internal.ReflectionExtensions.IsNullable(global::System.Reflection.IntrospectionExtensions.GetTypeInfo(item.Type)))
						{
							global::System.Reflection.Emit.LocalBuilder local3 = il.DeclareLocal(item.Type);
							argValue.EmitLoad();
							item.EmitLoadValue(il);
							global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitStloc(il, local3);
							global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloca(il, local3);
							global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.GetNullableHasValue(item.Type.GetGenericArguments()[0]));
							il.Emit(global::System.Reflection.Emit.OpCodes.Brfalse_S, (num < array.Length - 1) ? array[num + 1] : label3);
						}
						else if (!item.Type.IsValueType && !(item is global::Utf8Json.Internal.Emit.StringConstantValueMetaMember))
						{
							argValue.EmitLoad();
							item.EmitLoadValue(il);
							il.Emit(global::System.Reflection.Emit.OpCodes.Brfalse_S, (num < array.Length - 1) ? array[num + 1] : label3);
						}
					}
					if (hasShouldSerialize && item.ShouldSerializeMethodInfo != null)
					{
						argValue.EmitLoad();
						global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, item.ShouldSerializeMethodInfo);
						il.Emit(global::System.Reflection.Emit.OpCodes.Brfalse_S, (num < array.Length - 1) ? array[num + 1] : label3);
					}
					global::System.Reflection.Emit.Label label4 = il.DefineLabel();
					global::System.Reflection.Emit.Label label5 = il.DefineLabel();
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloc(il, local2);
					il.Emit(global::System.Reflection.Emit.OpCodes.Brtrue_S, label5);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitTrue(il);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitStloc(il, local2);
					il.Emit(global::System.Reflection.Emit.OpCodes.Br, label4);
					il.MarkLabel(label5);
					argumentField.EmitLoad();
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.JsonWriter.WriteValueSeparator);
					il.MarkLabel(label4);
				}
				argumentField.EmitLoad();
				emitStringByteKeys();
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdc_I4(il, num);
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldelem_Ref);
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.JsonWriter.WriteRaw);
				EmitSerializeValue(typeInfo, item, il, num, tryEmitLoadCustomFormatter, argumentField, argValue, argResolver);
				num++;
			}
			il.MarkLabel(label3);
			if (!excludeNull && num == 0)
			{
				argumentField.EmitLoad();
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.JsonWriter.WriteBeginObject);
			}
			argumentField.EmitLoad();
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.JsonWriter.WriteEndObject);
			il.Emit(global::System.Reflection.Emit.OpCodes.Ret);
		}

		private static void EmitSerializeValue(global::System.Reflection.TypeInfo type, global::Utf8Json.Internal.Emit.MetaMember member, global::System.Reflection.Emit.ILGenerator il, int index, global::System.Func<int, global::Utf8Json.Internal.Emit.MetaMember, bool> tryEmitLoadCustomFormatter, global::Utf8Json.Internal.Emit.ArgumentField writer, global::Utf8Json.Internal.Emit.ArgumentField argValue, global::Utf8Json.Internal.Emit.ArgumentField argResolver)
		{
			global::System.Type type2 = member.Type;
			if (member is global::Utf8Json.Internal.Emit.InnerExceptionMetaMember)
			{
				(member as global::Utf8Json.Internal.Emit.InnerExceptionMetaMember).EmitSerializeDirectly(il);
			}
			else if (tryEmitLoadCustomFormatter(index, member))
			{
				writer.EmitLoad();
				argValue.EmitLoad();
				member.EmitLoadValue(il);
				argResolver.EmitLoad();
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.Serialize(type2));
			}
			else if (jsonPrimitiveTypes.Contains(type2))
			{
				writer.EmitLoad();
				argValue.EmitLoad();
				member.EmitLoadValue(il);
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::System.Linq.Enumerable.First(global::System.Linq.Enumerable.OrderByDescending(global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Utf8Json.JsonWriter)).GetDeclaredMethods("Write" + type2.Name), (global::System.Reflection.MethodInfo x) => x.GetParameters().Length)));
			}
			else
			{
				argResolver.EmitLoad();
				il.Emit(global::System.Reflection.Emit.OpCodes.Call, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.GetFormatterWithVerify.MakeGenericMethod(type2));
				writer.EmitLoad();
				argValue.EmitLoad();
				member.EmitLoadValue(il);
				argResolver.EmitLoad();
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.Serialize(type2));
			}
		}

		private unsafe static void BuildDeserialize(global::System.Type type, global::Utf8Json.Internal.Emit.MetaType info, global::System.Reflection.Emit.ILGenerator il, global::System.Func<int, global::Utf8Json.Internal.Emit.MetaMember, bool> tryEmitLoadCustomFormatter, bool useGetUninitializedObject, int firstArgIndex)
		{
			if (info.IsClass && info.BestmatchConstructor == null && (!useGetUninitializedObject || !info.IsConcreteClass))
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldstr, "generated serializer for " + type.Name + " does not support deserialize.");
				il.Emit(global::System.Reflection.Emit.OpCodes.Newobj, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.InvalidOperationExceptionConstructor);
				il.Emit(global::System.Reflection.Emit.OpCodes.Throw);
				return;
			}
			global::Utf8Json.Internal.Emit.ArgumentField argReader = new global::Utf8Json.Internal.Emit.ArgumentField(il, firstArgIndex);
			global::Utf8Json.Internal.Emit.ArgumentField argResolver = new global::Utf8Json.Internal.Emit.ArgumentField(il, firstArgIndex + 1);
			global::System.Reflection.Emit.Label label = il.DefineLabel();
			argReader.EmitLoad();
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.JsonReader.ReadIsNull);
			il.Emit(global::System.Reflection.Emit.OpCodes.Brfalse_S, label);
			if (info.IsClass)
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldnull);
				il.Emit(global::System.Reflection.Emit.OpCodes.Ret);
			}
			else
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldstr, "json value is null, struct is not supported");
				il.Emit(global::System.Reflection.Emit.OpCodes.Newobj, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.InvalidOperationExceptionConstructor);
				il.Emit(global::System.Reflection.Emit.OpCodes.Throw);
			}
			il.MarkLabel(label);
			argReader.EmitLoad();
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.JsonReader.ReadIsBeginObjectWithVerify);
			bool isSideEffectFreeType = true;
			if (info.BestmatchConstructor != null)
			{
				isSideEffectFreeType = IsSideEffectFreeConstructorType(info.BestmatchConstructor);
				if (global::System.Linq.Enumerable.Any(info.Members, (global::Utf8Json.Internal.Emit.MetaMember x) => !x.IsReadable && x.IsWritable))
				{
					isSideEffectFreeType = false;
				}
			}
			global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.DeserializeInfo[] infoList = global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Select(info.Members, (global::Utf8Json.Internal.Emit.MetaMember item) => new global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.DeserializeInfo
			{
				MemberInfo = item,
				LocalField = il.DeclareLocal(item.Type),
				IsDeserializedField = (isSideEffectFreeType ? null : il.DeclareLocal(typeof(bool)))
			}));
			global::System.Reflection.Emit.LocalBuilder local = il.DeclareLocal(typeof(int));
			global::Utf8Json.Internal.AutomataDictionary automataDictionary = new global::Utf8Json.Internal.AutomataDictionary();
			for (int num = 0; num < info.Members.Length; num++)
			{
				automataDictionary.Add(global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation(info.Members[num].Name), num);
			}
			global::System.Reflection.Emit.LocalBuilder local2 = il.DeclareLocal(typeof(byte[]));
			global::System.Reflection.Emit.LocalBuilder local3 = il.DeclareLocal(typeof(byte).MakeByRefType(), pinned: true);
			global::System.Reflection.Emit.LocalBuilder local4 = il.DeclareLocal(typeof(global::System.ArraySegment<byte>));
			global::System.Reflection.Emit.LocalBuilder key = il.DeclareLocal(typeof(ulong));
			global::System.Reflection.Emit.LocalBuilder localBuilder = il.DeclareLocal(typeof(byte*));
			global::System.Reflection.Emit.LocalBuilder localBuilder2 = il.DeclareLocal(typeof(int));
			argReader.EmitLoad();
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.JsonReader.GetBufferUnsafe);
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitStloc(il, local2);
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloc(il, local2);
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdc_I4(il, 0);
			il.Emit(global::System.Reflection.Emit.OpCodes.Ldelema, typeof(byte));
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitStloc(il, local3);
			global::System.Reflection.Emit.Label continueWhile = il.DefineLabel();
			global::System.Reflection.Emit.Label label2 = il.DefineLabel();
			global::System.Reflection.Emit.Label readNext = il.DefineLabel();
			il.MarkLabel(continueWhile);
			argReader.EmitLoad();
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloca(il, local);
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.JsonReader.ReadIsEndObjectWithSkipValueSeparator);
			il.Emit(global::System.Reflection.Emit.OpCodes.Brtrue, label2);
			argReader.EmitLoad();
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.JsonReader.ReadPropertyNameSegmentUnsafe);
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitStloc(il, local4);
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloc(il, local3);
			il.Emit(global::System.Reflection.Emit.OpCodes.Conv_I);
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloca(il, local4);
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::System.Reflection.RuntimeReflectionExtensions.GetRuntimeProperty(typeof(global::System.ArraySegment<byte>), "Offset").GetGetMethod());
			il.Emit(global::System.Reflection.Emit.OpCodes.Add);
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitStloc(il, localBuilder);
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloca(il, local4);
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::System.Reflection.RuntimeReflectionExtensions.GetRuntimeProperty(typeof(global::System.ArraySegment<byte>), "Count").GetGetMethod());
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitStloc(il, localBuilder2);
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloc(il, localBuilder2);
			il.Emit(global::System.Reflection.Emit.OpCodes.Brfalse, readNext);
			automataDictionary.EmitMatch(il, localBuilder, localBuilder2, key, delegate(global::System.Collections.Generic.KeyValuePair<string, int> x)
			{
				int value = x.Value;
				if (infoList[value].MemberInfo != null)
				{
					EmitDeserializeValue(il, infoList[value], value, tryEmitLoadCustomFormatter, argReader, argResolver);
					if (!isSideEffectFreeType)
					{
						global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitTrue(il);
						global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitStloc(il, infoList[value].IsDeserializedField);
					}
					il.Emit(global::System.Reflection.Emit.OpCodes.Br, continueWhile);
				}
				else
				{
					il.Emit(global::System.Reflection.Emit.OpCodes.Br, readNext);
				}
			}, delegate
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Br, readNext);
			});
			il.MarkLabel(readNext);
			argReader.EmitLoad();
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.JsonReader.ReadNextBlock);
			il.Emit(global::System.Reflection.Emit.OpCodes.Br, continueWhile);
			il.MarkLabel(label2);
			il.Emit(global::System.Reflection.Emit.OpCodes.Ldc_I4_0);
			il.Emit(global::System.Reflection.Emit.OpCodes.Conv_U);
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitStloc(il, local3);
			global::System.Reflection.Emit.LocalBuilder localBuilder3 = EmitNewObject(il, type, info, infoList, isSideEffectFreeType);
			if (localBuilder3 != null)
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldloc, localBuilder3);
			}
			il.Emit(global::System.Reflection.Emit.OpCodes.Ret);
		}

		private static void EmitDeserializeValue(global::System.Reflection.Emit.ILGenerator il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.DeserializeInfo info, int index, global::System.Func<int, global::Utf8Json.Internal.Emit.MetaMember, bool> tryEmitLoadCustomFormatter, global::Utf8Json.Internal.Emit.ArgumentField reader, global::Utf8Json.Internal.Emit.ArgumentField argResolver)
		{
			global::Utf8Json.Internal.Emit.MetaMember memberInfo = info.MemberInfo;
			global::System.Type type = memberInfo.Type;
			if (tryEmitLoadCustomFormatter(index, memberInfo))
			{
				reader.EmitLoad();
				argResolver.EmitLoad();
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.Deserialize(type));
			}
			else if (jsonPrimitiveTypes.Contains(type))
			{
				reader.EmitLoad();
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::System.Linq.Enumerable.First(global::System.Linq.Enumerable.OrderByDescending(global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Utf8Json.JsonReader)).GetDeclaredMethods("Read" + type.Name), (global::System.Reflection.MethodInfo x) => x.GetParameters().Length)));
			}
			else
			{
				argResolver.EmitLoad();
				il.Emit(global::System.Reflection.Emit.OpCodes.Call, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.GetFormatterWithVerify.MakeGenericMethod(type));
				reader.EmitLoad();
				argResolver.EmitLoad();
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.Deserialize(type));
			}
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitStloc(il, info.LocalField);
		}

		private static global::System.Reflection.Emit.LocalBuilder EmitNewObject(global::System.Reflection.Emit.ILGenerator il, global::System.Type type, global::Utf8Json.Internal.Emit.MetaType info, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.DeserializeInfo[] members, bool isSideEffectFreeType)
		{
			if (info.IsClass)
			{
				global::System.Reflection.Emit.LocalBuilder localBuilder = null;
				if (!isSideEffectFreeType)
				{
					localBuilder = il.DeclareLocal(type);
				}
				if (info.BestmatchConstructor != null)
				{
					global::Utf8Json.Internal.Emit.MetaMember[] constructorParameters = info.ConstructorParameters;
					foreach (global::Utf8Json.Internal.Emit.MetaMember item in constructorParameters)
					{
						global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloc(il, global::System.Linq.Enumerable.First(members, (global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.DeserializeInfo x) => x.MemberInfo == item).LocalField);
					}
					il.Emit(global::System.Reflection.Emit.OpCodes.Newobj, info.BestmatchConstructor);
				}
				else
				{
					il.Emit(global::System.Reflection.Emit.OpCodes.Ldtoken, type);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.GetTypeFromHandle);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.EmitInfo.GetUninitializedObject);
				}
				if (!isSideEffectFreeType)
				{
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitStloc(il, localBuilder);
				}
				{
					foreach (global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.DeserializeInfo item3 in global::System.Linq.Enumerable.Where(members, (global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.DeserializeInfo x) => x.MemberInfo != null && x.MemberInfo.IsWritable))
					{
						if (isSideEffectFreeType)
						{
							il.Emit(global::System.Reflection.Emit.OpCodes.Dup);
							global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloc(il, item3.LocalField);
							item3.MemberInfo.EmitStoreValue(il);
							continue;
						}
						global::System.Reflection.Emit.Label label = il.DefineLabel();
						global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloc(il, item3.IsDeserializedField);
						il.Emit(global::System.Reflection.Emit.OpCodes.Brfalse, label);
						global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloc(il, localBuilder);
						global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloc(il, item3.LocalField);
						item3.MemberInfo.EmitStoreValue(il);
						il.MarkLabel(label);
					}
					return localBuilder;
				}
			}
			global::System.Reflection.Emit.LocalBuilder localBuilder2 = il.DeclareLocal(type);
			if (info.BestmatchConstructor == null)
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldloca, localBuilder2);
				il.Emit(global::System.Reflection.Emit.OpCodes.Initobj, type);
			}
			else
			{
				global::Utf8Json.Internal.Emit.MetaMember[] constructorParameters = info.ConstructorParameters;
				foreach (global::Utf8Json.Internal.Emit.MetaMember item2 in constructorParameters)
				{
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloc(il, global::System.Linq.Enumerable.First(members, (global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.DeserializeInfo x) => x.MemberInfo == item2).LocalField);
				}
				il.Emit(global::System.Reflection.Emit.OpCodes.Newobj, info.BestmatchConstructor);
				il.Emit(global::System.Reflection.Emit.OpCodes.Stloc, localBuilder2);
			}
			foreach (global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.DeserializeInfo item4 in global::System.Linq.Enumerable.Where(members, (global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.DeserializeInfo x) => x.MemberInfo != null && x.MemberInfo.IsWritable))
			{
				if (isSideEffectFreeType)
				{
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloca(il, localBuilder2);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloc(il, item4.LocalField);
					item4.MemberInfo.EmitStoreValue(il);
					continue;
				}
				global::System.Reflection.Emit.Label label2 = il.DefineLabel();
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloc(il, item4.IsDeserializedField);
				il.Emit(global::System.Reflection.Emit.OpCodes.Brfalse, label2);
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloca(il, localBuilder2);
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloc(il, item4.LocalField);
				item4.MemberInfo.EmitStoreValue(il);
				il.MarkLabel(label2);
			}
			return localBuilder2;
		}

		private static bool IsSideEffectFreeConstructorType(global::System.Reflection.ConstructorInfo ctorInfo)
		{
			global::System.Reflection.MethodBody methodBody = ctorInfo.GetMethodBody();
			if (methodBody == null)
			{
				return false;
			}
			byte[] iLAsByteArray = methodBody.GetILAsByteArray();
			if (iLAsByteArray == null)
			{
				return false;
			}
			global::System.Collections.Generic.List<global::System.Reflection.Emit.OpCode> list = new global::System.Collections.Generic.List<global::System.Reflection.Emit.OpCode>();
			using (global::Utf8Json.Internal.ILStreamReader iLStreamReader = new global::Utf8Json.Internal.ILStreamReader(iLAsByteArray))
			{
				while (!iLStreamReader.EndOfStream)
				{
					global::System.Reflection.Emit.OpCode opCode = iLStreamReader.ReadOpCode();
					if (opCode != global::System.Reflection.Emit.OpCodes.Nop && opCode != global::System.Reflection.Emit.OpCodes.Ldloc_0 && opCode != global::System.Reflection.Emit.OpCodes.Ldloc_S && opCode != global::System.Reflection.Emit.OpCodes.Stloc_0 && opCode != global::System.Reflection.Emit.OpCodes.Stloc_S && opCode != global::System.Reflection.Emit.OpCodes.Blt && opCode != global::System.Reflection.Emit.OpCodes.Blt_S && opCode != global::System.Reflection.Emit.OpCodes.Bgt && opCode != global::System.Reflection.Emit.OpCodes.Bgt_S)
					{
						list.Add(opCode);
						if (list.Count == 4)
						{
							break;
						}
					}
				}
			}
			if (list.Count == 3 && list[0] == global::System.Reflection.Emit.OpCodes.Ldarg_0 && list[1] == global::System.Reflection.Emit.OpCodes.Call && list[2] == global::System.Reflection.Emit.OpCodes.Ret)
			{
				if (ctorInfo.DeclaringType.BaseType == typeof(object))
				{
					return true;
				}
				global::System.Reflection.ConstructorInfo constructor = ctorInfo.DeclaringType.BaseType.GetConstructor(global::System.Type.EmptyTypes);
				if (constructor == null)
				{
					return false;
				}
				return IsSideEffectFreeConstructorType(constructor);
			}
			return false;
		}

		private static bool TryGetInterfaceEnumerableElementType(global::System.Type type, out global::System.Type elementType)
		{
			global::System.Type[] interfaces = type.GetInterfaces();
			foreach (global::System.Type type2 in interfaces)
			{
				if (type2.IsGenericType && type2.GetGenericTypeDefinition() == typeof(global::System.Collections.Generic.IEnumerable<>))
				{
					global::System.Type[] genericArguments = type2.GetGenericArguments();
					elementType = genericArguments[0];
					return true;
				}
			}
			elementType = null;
			return false;
		}
	}
}
