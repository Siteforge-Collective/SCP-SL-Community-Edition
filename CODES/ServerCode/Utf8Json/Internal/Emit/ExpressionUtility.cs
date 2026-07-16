namespace Utf8Json.Internal.Emit
{
	internal static class ExpressionUtility
	{
		private static global::System.Reflection.MethodInfo GetMethodInfoCore(global::System.Linq.Expressions.LambdaExpression expression)
		{
			if (expression == null)
			{
				throw new global::System.ArgumentNullException("expression");
			}
			return (expression.Body as global::System.Linq.Expressions.MethodCallExpression).Method;
		}

		public static global::System.Reflection.MethodInfo GetMethodInfo<T>(global::System.Linq.Expressions.Expression<global::System.Func<T>> expression)
		{
			return GetMethodInfoCore(expression);
		}

		public static global::System.Reflection.MethodInfo GetMethodInfo(global::System.Linq.Expressions.Expression<global::System.Action> expression)
		{
			return GetMethodInfoCore(expression);
		}

		public static global::System.Reflection.MethodInfo GetMethodInfo<T, TR>(global::System.Linq.Expressions.Expression<global::System.Func<T, TR>> expression)
		{
			return GetMethodInfoCore(expression);
		}

		public static global::System.Reflection.MethodInfo GetMethodInfo<T>(global::System.Linq.Expressions.Expression<global::System.Action<T>> expression)
		{
			return GetMethodInfoCore(expression);
		}

		public static global::System.Reflection.MethodInfo GetMethodInfo<TArg1, TArg2>(global::System.Linq.Expressions.Expression<global::System.Action<TArg1, TArg2>> expression)
		{
			return GetMethodInfoCore(expression);
		}

		public static global::System.Reflection.MethodInfo GetMethodInfo<T, TArg1, TR>(global::System.Linq.Expressions.Expression<global::System.Func<T, TArg1, TR>> expression)
		{
			return GetMethodInfoCore(expression);
		}

		private static global::System.Reflection.MemberInfo GetMemberInfoCore<T>(global::System.Linq.Expressions.Expression<T> source)
		{
			if (source == null)
			{
				throw new global::System.ArgumentNullException("source");
			}
			return (source.Body as global::System.Linq.Expressions.MemberExpression).Member;
		}

		public static global::System.Reflection.PropertyInfo GetPropertyInfo<T, TR>(global::System.Linq.Expressions.Expression<global::System.Func<T, TR>> expression)
		{
			return GetMemberInfoCore(expression) as global::System.Reflection.PropertyInfo;
		}

		public static global::System.Reflection.FieldInfo GetFieldInfo<T, TR>(global::System.Linq.Expressions.Expression<global::System.Func<T, TR>> expression)
		{
			return GetMemberInfoCore(expression) as global::System.Reflection.FieldInfo;
		}
	}
}
