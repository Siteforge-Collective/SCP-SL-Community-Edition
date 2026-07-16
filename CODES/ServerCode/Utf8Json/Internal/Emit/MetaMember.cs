namespace Utf8Json.Internal.Emit
{
	internal class MetaMember
	{
		private global::System.Reflection.MethodInfo getMethod;

		private global::System.Reflection.MethodInfo setMethod;

		public string Name { get; private set; }

		public string MemberName { get; private set; }

		public bool IsProperty => PropertyInfo != null;

		public bool IsField => FieldInfo != null;

		public bool IsWritable { get; private set; }

		public bool IsReadable { get; private set; }

		public global::System.Type Type { get; private set; }

		public global::System.Reflection.FieldInfo FieldInfo { get; private set; }

		public global::System.Reflection.PropertyInfo PropertyInfo { get; private set; }

		public global::System.Reflection.MethodInfo ShouldSerializeMethodInfo { get; private set; }

		protected MetaMember(global::System.Type type, string name, string memberName, bool isWritable, bool isReadable)
		{
			Name = name;
			MemberName = memberName;
			Type = type;
			IsWritable = isWritable;
			IsReadable = isReadable;
		}

		public MetaMember(global::System.Reflection.FieldInfo info, string name, bool allowPrivate)
		{
			Name = name;
			MemberName = info.Name;
			FieldInfo = info;
			Type = info.FieldType;
			IsReadable = allowPrivate || info.IsPublic;
			IsWritable = allowPrivate || (info.IsPublic && !info.IsInitOnly);
			ShouldSerializeMethodInfo = GetShouldSerialize(info);
		}

		public MetaMember(global::System.Reflection.PropertyInfo info, string name, bool allowPrivate)
		{
			getMethod = info.GetGetMethod(nonPublic: true);
			setMethod = info.GetSetMethod(nonPublic: true);
			Name = name;
			MemberName = info.Name;
			PropertyInfo = info;
			Type = info.PropertyType;
			IsReadable = getMethod != null && (allowPrivate || getMethod.IsPublic) && !getMethod.IsStatic;
			IsWritable = setMethod != null && (allowPrivate || setMethod.IsPublic) && !setMethod.IsStatic;
			ShouldSerializeMethodInfo = GetShouldSerialize(info);
		}

		private static global::System.Reflection.MethodInfo GetShouldSerialize(global::System.Reflection.MemberInfo info)
		{
			string shouldSerialize = "ShouldSerialize" + info.Name;
			return global::System.Linq.Enumerable.FirstOrDefault(global::System.Linq.Enumerable.Where(info.DeclaringType.GetMethods(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public), (global::System.Reflection.MethodInfo x) => x.Name == shouldSerialize && x.ReturnType == typeof(bool) && x.GetParameters().Length == 0));
		}

		public T GetCustomAttribute<T>(bool inherit) where T : global::System.Attribute
		{
			if (IsProperty)
			{
				return global::System.Reflection.CustomAttributeExtensions.GetCustomAttribute<T>(PropertyInfo, inherit);
			}
			if (FieldInfo != null)
			{
				return global::System.Reflection.CustomAttributeExtensions.GetCustomAttribute<T>(FieldInfo, inherit);
			}
			return null;
		}

		public virtual void EmitLoadValue(global::System.Reflection.Emit.ILGenerator il)
		{
			if (IsProperty)
			{
				il.EmitCall(getMethod);
			}
			else
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldfld, FieldInfo);
			}
		}

		public virtual void EmitStoreValue(global::System.Reflection.Emit.ILGenerator il)
		{
			if (IsProperty)
			{
				il.EmitCall(setMethod);
			}
			else
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Stfld, FieldInfo);
			}
		}
	}
}
