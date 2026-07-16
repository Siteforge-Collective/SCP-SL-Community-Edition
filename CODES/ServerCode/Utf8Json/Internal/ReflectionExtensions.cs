namespace Utf8Json.Internal
{
	internal static class ReflectionExtensions
	{
		public static bool IsNullable(this global::System.Reflection.TypeInfo type)
		{
			if (type.IsGenericType)
			{
				return type.GetGenericTypeDefinition() == typeof(global::System.Nullable<>);
			}
			return false;
		}

		public static bool IsPublic(this global::System.Reflection.TypeInfo type)
		{
			return type.IsPublic;
		}

		public static bool IsAnonymous(this global::System.Reflection.TypeInfo type)
		{
			if (global::System.Reflection.CustomAttributeExtensions.GetCustomAttribute<global::System.Runtime.CompilerServices.CompilerGeneratedAttribute>(type) != null && type.Name.Contains("AnonymousType") && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$")))
			{
				return (type.Attributes & global::System.Reflection.TypeAttributes.NotPublic) == 0;
			}
			return false;
		}

		public static global::System.Collections.Generic.IEnumerable<global::System.Reflection.PropertyInfo> GetAllProperties(this global::System.Type type)
		{
			return GetAllPropertiesCore(type, new global::System.Collections.Generic.HashSet<string>());
		}

		private static global::System.Collections.Generic.IEnumerable<global::System.Reflection.PropertyInfo> GetAllPropertiesCore(global::System.Type type, global::System.Collections.Generic.HashSet<string> nameCheck)
		{
			foreach (global::System.Reflection.PropertyInfo runtimeProperty in global::System.Reflection.RuntimeReflectionExtensions.GetRuntimeProperties(type))
			{
				if (nameCheck.Add(runtimeProperty.Name))
				{
					yield return runtimeProperty;
				}
			}
			if (!(type.BaseType != null))
			{
				yield break;
			}
			foreach (global::System.Reflection.PropertyInfo item in GetAllPropertiesCore(type.BaseType, nameCheck))
			{
				yield return item;
			}
		}

		public static global::System.Collections.Generic.IEnumerable<global::System.Reflection.FieldInfo> GetAllFields(this global::System.Type type)
		{
			return GetAllFieldsCore(type, new global::System.Collections.Generic.HashSet<string>());
		}

		private static global::System.Collections.Generic.IEnumerable<global::System.Reflection.FieldInfo> GetAllFieldsCore(global::System.Type type, global::System.Collections.Generic.HashSet<string> nameCheck)
		{
			foreach (global::System.Reflection.FieldInfo runtimeField in global::System.Reflection.RuntimeReflectionExtensions.GetRuntimeFields(type))
			{
				if (nameCheck.Add(runtimeField.Name))
				{
					yield return runtimeField;
				}
			}
			if (!(type.BaseType != null))
			{
				yield break;
			}
			foreach (global::System.Reflection.FieldInfo item in GetAllFieldsCore(type.BaseType, nameCheck))
			{
				yield return item;
			}
		}
	}
}
