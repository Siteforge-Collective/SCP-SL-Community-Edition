namespace Utf8Json.Resolvers.Internal
{
	internal static class DynamicGenericResolverGetFormatterHelper
	{
		private static readonly global::System.Collections.Generic.Dictionary<global::System.Type, global::System.Type> formatterMap = new global::System.Collections.Generic.Dictionary<global::System.Type, global::System.Type>
		{
			{
				typeof(global::System.Collections.Generic.List<>),
				typeof(global::Utf8Json.Formatters.ListFormatter<>)
			},
			{
				typeof(global::System.Collections.Generic.LinkedList<>),
				typeof(global::Utf8Json.Formatters.LinkedListFormatter<>)
			},
			{
				typeof(global::System.Collections.Generic.Queue<>),
				typeof(global::Utf8Json.Formatters.QeueueFormatter<>)
			},
			{
				typeof(global::System.Collections.Generic.Stack<>),
				typeof(global::Utf8Json.Formatters.StackFormatter<>)
			},
			{
				typeof(global::System.Collections.Generic.HashSet<>),
				typeof(global::Utf8Json.Formatters.HashSetFormatter<>)
			},
			{
				typeof(global::System.Collections.ObjectModel.ReadOnlyCollection<>),
				typeof(global::Utf8Json.Formatters.ReadOnlyCollectionFormatter<>)
			},
			{
				typeof(global::System.Collections.Generic.IList<>),
				typeof(global::Utf8Json.Formatters.InterfaceListFormatter<>)
			},
			{
				typeof(global::System.Collections.Generic.ICollection<>),
				typeof(global::Utf8Json.Formatters.InterfaceCollectionFormatter<>)
			},
			{
				typeof(global::System.Collections.Generic.IEnumerable<>),
				typeof(global::Utf8Json.Formatters.InterfaceEnumerableFormatter<>)
			},
			{
				typeof(global::System.Collections.Generic.Dictionary<, >),
				typeof(global::Utf8Json.Formatters.DictionaryFormatter<, >)
			},
			{
				typeof(global::System.Collections.Generic.IDictionary<, >),
				typeof(global::Utf8Json.Formatters.InterfaceDictionaryFormatter<, >)
			},
			{
				typeof(global::System.Collections.Generic.SortedDictionary<, >),
				typeof(global::Utf8Json.Formatters.SortedDictionaryFormatter<, >)
			},
			{
				typeof(global::System.Collections.Generic.SortedList<, >),
				typeof(global::Utf8Json.Formatters.SortedListFormatter<, >)
			},
			{
				typeof(global::System.Linq.ILookup<, >),
				typeof(global::Utf8Json.Formatters.InterfaceLookupFormatter<, >)
			},
			{
				typeof(global::System.Linq.IGrouping<, >),
				typeof(global::Utf8Json.Formatters.InterfaceGroupingFormatter<, >)
			}
		};

		internal static object GetFormatter(global::System.Type t)
		{
			global::System.Reflection.TypeInfo typeInfo = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(t);
			if (t.IsArray)
			{
				switch (t.GetArrayRank())
				{
				case 1:
					if (t.GetElementType() == typeof(byte))
					{
						return global::Utf8Json.Formatters.ByteArrayFormatter.Default;
					}
					return global::System.Activator.CreateInstance(typeof(global::Utf8Json.Formatters.ArrayFormatter<>).MakeGenericType(t.GetElementType()));
				case 2:
					return global::System.Activator.CreateInstance(typeof(global::Utf8Json.Formatters.TwoDimentionalArrayFormatter<>).MakeGenericType(t.GetElementType()));
				case 3:
					return global::System.Activator.CreateInstance(typeof(global::Utf8Json.Formatters.ThreeDimentionalArrayFormatter<>).MakeGenericType(t.GetElementType()));
				case 4:
					return global::System.Activator.CreateInstance(typeof(global::Utf8Json.Formatters.FourDimentionalArrayFormatter<>).MakeGenericType(t.GetElementType()));
				default:
					return null;
				}
			}
			if (typeInfo.IsGenericType)
			{
				global::System.Type genericTypeDefinition = typeInfo.GetGenericTypeDefinition();
				bool flag = global::Utf8Json.Internal.ReflectionExtensions.IsNullable(global::System.Reflection.IntrospectionExtensions.GetTypeInfo(genericTypeDefinition));
				global::System.Type type = (flag ? typeInfo.GenericTypeArguments[0] : null);
				if (genericTypeDefinition == typeof(global::System.Collections.Generic.KeyValuePair<, >))
				{
					return CreateInstance(typeof(global::Utf8Json.Formatters.KeyValuePairFormatter<, >), typeInfo.GenericTypeArguments);
				}
				if (flag && global::System.Reflection.ReflectionExtensions.IsConstructedGenericType(global::System.Reflection.IntrospectionExtensions.GetTypeInfo(type)) && type.GetGenericTypeDefinition() == typeof(global::System.Collections.Generic.KeyValuePair<, >))
				{
					return CreateInstance(typeof(global::Utf8Json.Formatters.NullableFormatter<>), new global::System.Type[1] { type });
				}
				if (genericTypeDefinition == typeof(global::System.ArraySegment<>))
				{
					if (typeInfo.GenericTypeArguments[0] == typeof(byte))
					{
						return global::Utf8Json.Formatters.ByteArraySegmentFormatter.Default;
					}
					return CreateInstance(typeof(global::Utf8Json.Formatters.ArraySegmentFormatter<>), typeInfo.GenericTypeArguments);
				}
				if (flag && global::System.Reflection.ReflectionExtensions.IsConstructedGenericType(global::System.Reflection.IntrospectionExtensions.GetTypeInfo(type)) && type.GetGenericTypeDefinition() == typeof(global::System.ArraySegment<>))
				{
					if (type == typeof(global::System.ArraySegment<byte>))
					{
						return new global::Utf8Json.Formatters.StaticNullableFormatter<global::System.ArraySegment<byte>>(global::Utf8Json.Formatters.ByteArraySegmentFormatter.Default);
					}
					return CreateInstance(typeof(global::Utf8Json.Formatters.NullableFormatter<>), new global::System.Type[1] { type });
				}
				if (formatterMap.TryGetValue(genericTypeDefinition, out var value))
				{
					return CreateInstance(value, typeInfo.GenericTypeArguments);
				}
				if (typeInfo.GenericTypeArguments.Length == 1 && global::System.Linq.Enumerable.Any(typeInfo.ImplementedInterfaces, (global::System.Type x) => global::System.Reflection.ReflectionExtensions.IsConstructedGenericType(global::System.Reflection.IntrospectionExtensions.GetTypeInfo(x)) && x.GetGenericTypeDefinition() == typeof(global::System.Collections.Generic.ICollection<>)) && global::System.Linq.Enumerable.Any(typeInfo.DeclaredConstructors, (global::System.Reflection.ConstructorInfo x) => x.GetParameters().Length == 0))
				{
					global::System.Type type2 = typeInfo.GenericTypeArguments[0];
					return CreateInstance(typeof(global::Utf8Json.Formatters.GenericCollectionFormatter<, >), new global::System.Type[2] { type2, t });
				}
				if (typeInfo.GenericTypeArguments.Length == 2 && global::System.Linq.Enumerable.Any(typeInfo.ImplementedInterfaces, (global::System.Type x) => global::System.Reflection.ReflectionExtensions.IsConstructedGenericType(global::System.Reflection.IntrospectionExtensions.GetTypeInfo(x)) && x.GetGenericTypeDefinition() == typeof(global::System.Collections.Generic.IDictionary<, >)) && global::System.Linq.Enumerable.Any(typeInfo.DeclaredConstructors, (global::System.Reflection.ConstructorInfo x) => x.GetParameters().Length == 0))
				{
					global::System.Type type3 = typeInfo.GenericTypeArguments[0];
					global::System.Type type4 = typeInfo.GenericTypeArguments[1];
					return CreateInstance(typeof(global::Utf8Json.Formatters.GenericDictionaryFormatter<, , >), new global::System.Type[3] { type3, type4, t });
				}
			}
			else
			{
				if (t == typeof(global::System.Collections.IEnumerable))
				{
					return global::Utf8Json.Formatters.NonGenericInterfaceEnumerableFormatter.Default;
				}
				if (t == typeof(global::System.Collections.ICollection))
				{
					return global::Utf8Json.Formatters.NonGenericInterfaceCollectionFormatter.Default;
				}
				if (t == typeof(global::System.Collections.IList))
				{
					return global::Utf8Json.Formatters.NonGenericInterfaceListFormatter.Default;
				}
				if (t == typeof(global::System.Collections.IDictionary))
				{
					return global::Utf8Json.Formatters.NonGenericInterfaceDictionaryFormatter.Default;
				}
				if (global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::System.Collections.IList)).IsAssignableFrom(typeInfo) && global::System.Linq.Enumerable.Any(typeInfo.DeclaredConstructors, (global::System.Reflection.ConstructorInfo x) => x.GetParameters().Length == 0))
				{
					return global::System.Activator.CreateInstance(typeof(global::Utf8Json.Formatters.NonGenericListFormatter<>).MakeGenericType(t));
				}
				if (global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::System.Collections.IDictionary)).IsAssignableFrom(typeInfo) && global::System.Linq.Enumerable.Any(typeInfo.DeclaredConstructors, (global::System.Reflection.ConstructorInfo x) => x.GetParameters().Length == 0))
				{
					return global::System.Activator.CreateInstance(typeof(global::Utf8Json.Formatters.NonGenericDictionaryFormatter<>).MakeGenericType(t));
				}
			}
			return null;
		}

		private static object CreateInstance(global::System.Type genericType, global::System.Type[] genericTypeArguments, params object[] arguments)
		{
			return global::System.Activator.CreateInstance(genericType.MakeGenericType(genericTypeArguments), arguments);
		}
	}
}
