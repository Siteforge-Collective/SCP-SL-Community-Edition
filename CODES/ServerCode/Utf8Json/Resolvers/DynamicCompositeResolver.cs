namespace Utf8Json.Resolvers
{
	public abstract class DynamicCompositeResolver : global::Utf8Json.IJsonFormatterResolver
	{
		private const string ModuleName = "Utf8Json.Resolvers.DynamicCompositeResolver";

		private static readonly global::Utf8Json.Internal.Emit.DynamicAssembly assembly;

		public readonly global::Utf8Json.IJsonFormatter[] formatters;

		public readonly global::Utf8Json.IJsonFormatterResolver[] resolvers;

		static DynamicCompositeResolver()
		{
			assembly = new global::Utf8Json.Internal.Emit.DynamicAssembly("Utf8Json.Resolvers.DynamicCompositeResolver");
		}

		public static global::Utf8Json.IJsonFormatterResolver Create(global::Utf8Json.IJsonFormatter[] formatters, global::Utf8Json.IJsonFormatterResolver[] resolvers)
		{
			string text = global::System.Guid.NewGuid().ToString().Replace("-", "");
			global::System.Reflection.Emit.TypeBuilder typeBuilder = assembly.DefineType("DynamicCompositeResolver_" + text, global::System.Reflection.TypeAttributes.Public | global::System.Reflection.TypeAttributes.Sealed, typeof(global::Utf8Json.Resolvers.DynamicCompositeResolver));
			global::System.Reflection.Emit.TypeBuilder typeBuilder2 = assembly.DefineType("DynamicCompositeResolverCache_" + text, global::System.Reflection.TypeAttributes.Public | global::System.Reflection.TypeAttributes.Sealed, null);
			global::System.Reflection.Emit.GenericTypeParameterBuilder genericTypeParameterBuilder = typeBuilder2.DefineGenericParameters("T")[0];
			global::System.Reflection.Emit.FieldBuilder fieldInfo = typeBuilder.DefineField("instance", typeBuilder, global::System.Reflection.FieldAttributes.Public | global::System.Reflection.FieldAttributes.Static);
			global::System.Reflection.Emit.FieldBuilder field = typeBuilder2.DefineField("formatter", typeof(global::Utf8Json.IJsonFormatter<>).MakeGenericType(genericTypeParameterBuilder), global::System.Reflection.FieldAttributes.Public | global::System.Reflection.FieldAttributes.Static);
			global::System.Reflection.Emit.ILGenerator iLGenerator = typeBuilder2.DefineConstructor(global::System.Reflection.MethodAttributes.Static, global::System.Reflection.CallingConventions.Standard, global::System.Type.EmptyTypes).GetILGenerator();
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdsfld(iLGenerator, fieldInfo);
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(iLGenerator, typeof(global::Utf8Json.Resolvers.DynamicCompositeResolver).GetMethod("GetFormatterLoop").MakeGenericMethod(genericTypeParameterBuilder));
			iLGenerator.Emit(global::System.Reflection.Emit.OpCodes.Stsfld, field);
			iLGenerator.Emit(global::System.Reflection.Emit.OpCodes.Ret);
			global::System.Type type = typeBuilder2.CreateTypeInfo().AsType();
			global::System.Reflection.Emit.ILGenerator iLGenerator2 = typeBuilder.DefineConstructor(global::System.Reflection.MethodAttributes.Public, global::System.Reflection.CallingConventions.Standard, new global::System.Type[2]
			{
				typeof(global::Utf8Json.IJsonFormatter[]),
				typeof(global::Utf8Json.IJsonFormatterResolver[])
			}).GetILGenerator();
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(iLGenerator2, 0);
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(iLGenerator2, 1);
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdarg(iLGenerator2, 2);
			iLGenerator2.Emit(global::System.Reflection.Emit.OpCodes.Call, typeof(global::Utf8Json.Resolvers.DynamicCompositeResolver).GetConstructors()[0]);
			iLGenerator2.Emit(global::System.Reflection.Emit.OpCodes.Ret);
			global::System.Reflection.Emit.MethodBuilder methodBuilder = typeBuilder.DefineMethod("GetFormatter", global::System.Reflection.MethodAttributes.Public | global::System.Reflection.MethodAttributes.Virtual);
			global::System.Reflection.Emit.GenericTypeParameterBuilder genericTypeParameterBuilder2 = methodBuilder.DefineGenericParameters("T")[0];
			methodBuilder.SetReturnType(typeof(global::Utf8Json.IJsonFormatter<>).MakeGenericType(genericTypeParameterBuilder2));
			global::System.Reflection.Emit.ILGenerator iLGenerator3 = methodBuilder.GetILGenerator();
			global::System.Reflection.FieldInfo field2 = global::System.Reflection.Emit.TypeBuilder.GetField(type.MakeGenericType(genericTypeParameterBuilder2), type.GetField("formatter", global::System.Reflection.BindingFlags.Static | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.GetField));
			global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdsfld(iLGenerator3, field2);
			iLGenerator3.Emit(global::System.Reflection.Emit.OpCodes.Ret);
			object obj = global::System.Activator.CreateInstance(typeBuilder.CreateTypeInfo().AsType(), new object[2] { formatters, resolvers });
			obj.GetType().GetField("instance", global::System.Reflection.BindingFlags.Static | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic).SetValue(null, obj);
			return (global::Utf8Json.IJsonFormatterResolver)obj;
		}

		public DynamicCompositeResolver(global::Utf8Json.IJsonFormatter[] formatters, global::Utf8Json.IJsonFormatterResolver[] resolvers)
		{
			this.formatters = formatters;
			this.resolvers = resolvers;
		}

		public global::Utf8Json.IJsonFormatter<T> GetFormatterLoop<T>()
		{
			global::Utf8Json.IJsonFormatter[] array = formatters;
			foreach (global::Utf8Json.IJsonFormatter jsonFormatter in array)
			{
				foreach (global::System.Type implementedInterface in global::System.Reflection.IntrospectionExtensions.GetTypeInfo(jsonFormatter.GetType()).ImplementedInterfaces)
				{
					global::System.Reflection.TypeInfo typeInfo = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(implementedInterface);
					if (typeInfo.IsGenericType && typeInfo.GenericTypeArguments[0] == typeof(T))
					{
						return (global::Utf8Json.IJsonFormatter<T>)jsonFormatter;
					}
				}
			}
			global::Utf8Json.IJsonFormatterResolver[] array2 = resolvers;
			for (int i = 0; i < array2.Length; i++)
			{
				global::Utf8Json.IJsonFormatter<T> formatter = array2[i].GetFormatter<T>();
				if (formatter != null)
				{
					return formatter;
				}
			}
			return null;
		}

		public abstract global::Utf8Json.IJsonFormatter<T> GetFormatter<T>();
	}
}
