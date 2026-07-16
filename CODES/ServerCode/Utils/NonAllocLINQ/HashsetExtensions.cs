namespace Utils.NonAllocLINQ
{
	public static class HashsetExtensions
	{
		public static void ForEach<T>(this global::System.Collections.Generic.HashSet<T> target, global::System.Action<T> action)
		{
			foreach (T item in target)
			{
				action(item);
			}
		}

		public static bool Any<T>(this global::System.Collections.Generic.HashSet<T> target, global::System.Func<T, bool> condition)
		{
			foreach (T item in target)
			{
				if (condition(item))
				{
					return true;
				}
			}
			return false;
		}

		public static int Count<T>(this global::System.Collections.Generic.HashSet<T> target, global::System.Func<T, bool> condition)
		{
			int num = 0;
			foreach (T item in target)
			{
				if (condition(item))
				{
					num++;
				}
			}
			return num;
		}

		public static bool All<T>(this global::System.Collections.Generic.HashSet<T> target, global::System.Func<T, bool> condition, bool emptyResult = true)
		{
			foreach (T item in target)
			{
				if (!condition(item))
				{
					return false;
				}
				emptyResult = true;
			}
			return emptyResult;
		}

		public static T FirstOrDefault<T>(this global::System.Collections.Generic.HashSet<T> target, global::System.Func<T, bool> condition, T defaultRet)
		{
			foreach (T item in target)
			{
				if (condition(item))
				{
					return item;
				}
			}
			return defaultRet;
		}

		public static bool TryGetFirst<T>(this global::System.Collections.Generic.HashSet<T> target, global::System.Func<T, bool> condition, out T first)
		{
			foreach (T item in target)
			{
				if (condition(item))
				{
					first = item;
					return true;
				}
			}
			first = default(T);
			return false;
		}
	}
}
