namespace Utf8Json.Internal.Emit
{
	internal class MetaType
	{
		public global::System.Type Type { get; private set; }

		public bool IsClass { get; private set; }

		public bool IsStruct => !IsClass;

		public bool IsConcreteClass { get; private set; }

		public global::System.Reflection.ConstructorInfo BestmatchConstructor { get; internal set; }

		public global::Utf8Json.Internal.Emit.MetaMember[] ConstructorParameters { get; internal set; }

		public global::Utf8Json.Internal.Emit.MetaMember[] Members { get; internal set; }

		public MetaType(global::System.Type type, global::System.Func<string, string> nameMutetor, bool allowPrivate)
		{
			global::System.Reflection.TypeInfo typeInfo = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(type);
			bool flag = typeInfo.IsClass || typeInfo.IsInterface || typeInfo.IsAbstract;
			Type = type;
			global::System.Collections.Generic.Dictionary<string, global::Utf8Json.Internal.Emit.MetaMember> dictionary = new global::System.Collections.Generic.Dictionary<string, global::Utf8Json.Internal.Emit.MetaMember>();
			foreach (global::System.Reflection.PropertyInfo allProperty in type.GetAllProperties())
			{
				if (allProperty.GetIndexParameters().Length != 0 || global::System.Reflection.CustomAttributeExtensions.GetCustomAttribute<global::System.Runtime.Serialization.IgnoreDataMemberAttribute>(allProperty, inherit: true) != null)
				{
					continue;
				}
				global::System.Runtime.Serialization.DataMemberAttribute customAttribute = global::System.Reflection.CustomAttributeExtensions.GetCustomAttribute<global::System.Runtime.Serialization.DataMemberAttribute>(allProperty, inherit: true);
				string name = ((customAttribute != null && customAttribute.Name != null) ? customAttribute.Name : nameMutetor(allProperty.Name));
				global::Utf8Json.Internal.Emit.MetaMember metaMember = new global::Utf8Json.Internal.Emit.MetaMember(allProperty, name, allowPrivate);
				if (metaMember.IsReadable || metaMember.IsWritable)
				{
					if (dictionary.ContainsKey(metaMember.Name))
					{
						throw new global::System.InvalidOperationException("same (custom)name is in type. Type:" + type.Name + " Name:" + metaMember.Name);
					}
					dictionary.Add(metaMember.Name, metaMember);
				}
			}
			foreach (global::System.Reflection.FieldInfo allField in type.GetAllFields())
			{
				if (global::System.Reflection.CustomAttributeExtensions.GetCustomAttribute<global::System.Runtime.Serialization.IgnoreDataMemberAttribute>(allField, inherit: true) != null || global::System.Reflection.CustomAttributeExtensions.GetCustomAttribute<global::System.Runtime.CompilerServices.CompilerGeneratedAttribute>(allField, inherit: true) != null || allField.IsStatic || allField.Name.StartsWith("<"))
				{
					continue;
				}
				global::System.Runtime.Serialization.DataMemberAttribute customAttribute2 = global::System.Reflection.CustomAttributeExtensions.GetCustomAttribute<global::System.Runtime.Serialization.DataMemberAttribute>(allField, inherit: true);
				string name2 = ((customAttribute2 != null && customAttribute2.Name != null) ? customAttribute2.Name : nameMutetor(allField.Name));
				global::Utf8Json.Internal.Emit.MetaMember metaMember2 = new global::Utf8Json.Internal.Emit.MetaMember(allField, name2, allowPrivate);
				if (metaMember2.IsReadable || metaMember2.IsWritable)
				{
					if (dictionary.ContainsKey(metaMember2.Name))
					{
						throw new global::System.InvalidOperationException("same (custom)name is in type. Type:" + type.Name + " Name:" + metaMember2.Name);
					}
					dictionary.Add(metaMember2.Name, metaMember2);
				}
			}
			global::System.Reflection.ConstructorInfo ctor = global::System.Linq.Enumerable.SingleOrDefault(global::System.Linq.Enumerable.Where(typeInfo.DeclaredConstructors, (global::System.Reflection.ConstructorInfo x) => x.IsPublic), (global::System.Reflection.ConstructorInfo x) => global::System.Reflection.CustomAttributeExtensions.GetCustomAttribute<global::Utf8Json.SerializationConstructorAttribute>(x, inherit: false) != null);
			global::System.Collections.Generic.List<global::Utf8Json.Internal.Emit.MetaMember> list = new global::System.Collections.Generic.List<global::Utf8Json.Internal.Emit.MetaMember>();
			global::System.Collections.Generic.IEnumerator<global::System.Reflection.ConstructorInfo> enumerator3 = null;
			if (ctor == null)
			{
				enumerator3 = global::System.Linq.Enumerable.OrderByDescending(global::System.Linq.Enumerable.Where(typeInfo.DeclaredConstructors, (global::System.Reflection.ConstructorInfo x) => x.IsPublic), (global::System.Reflection.ConstructorInfo x) => x.GetParameters().Length).GetEnumerator();
				if (enumerator3.MoveNext())
				{
					ctor = enumerator3.Current;
				}
			}
			if (ctor != null)
			{
				global::System.Linq.ILookup<string, global::System.Collections.Generic.KeyValuePair<string, global::Utf8Json.Internal.Emit.MetaMember>> lookup = global::System.Linq.Enumerable.ToLookup(dictionary, (global::System.Collections.Generic.KeyValuePair<string, global::Utf8Json.Internal.Emit.MetaMember> x) => x.Key, (global::System.Collections.Generic.KeyValuePair<string, global::Utf8Json.Internal.Emit.MetaMember> x) => x, global::System.StringComparer.OrdinalIgnoreCase);
				do
				{
					list.Clear();
					int num = 0;
					global::System.Reflection.ParameterInfo[] parameters = ctor.GetParameters();
					foreach (global::System.Reflection.ParameterInfo parameterInfo in parameters)
					{
						global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.KeyValuePair<string, global::Utf8Json.Internal.Emit.MetaMember>> source = lookup[parameterInfo.Name];
						switch (global::System.Linq.Enumerable.Count(source))
						{
						case 1:
						{
							global::Utf8Json.Internal.Emit.MetaMember value = global::System.Linq.Enumerable.First(source).Value;
							if (parameterInfo.ParameterType == value.Type && value.IsReadable)
							{
								list.Add(value);
								num++;
							}
							else
							{
								ctor = null;
							}
							continue;
						}
						case 0:
							ctor = null;
							continue;
						}
						if (enumerator3 != null)
						{
							ctor = null;
							continue;
						}
						throw new global::System.InvalidOperationException("duplicate matched constructor parameter name:" + type.FullName + " parameterName:" + parameterInfo.Name + " paramterType:" + parameterInfo.ParameterType.Name);
					}
				}
				while (TryGetNextConstructor(enumerator3, ref ctor));
			}
			IsClass = flag;
			IsConcreteClass = flag && !typeInfo.IsAbstract && !typeInfo.IsInterface;
			BestmatchConstructor = ctor;
			ConstructorParameters = list.ToArray();
			Members = global::System.Linq.Enumerable.ToArray(dictionary.Values);
		}

		private static bool TryGetNextConstructor(global::System.Collections.Generic.IEnumerator<global::System.Reflection.ConstructorInfo> ctorEnumerator, ref global::System.Reflection.ConstructorInfo ctor)
		{
			if (ctorEnumerator == null || ctor != null)
			{
				return false;
			}
			if (ctorEnumerator.MoveNext())
			{
				ctor = ctorEnumerator.Current;
				return true;
			}
			ctor = null;
			return false;
		}
	}
}
