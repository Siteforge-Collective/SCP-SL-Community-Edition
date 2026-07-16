namespace Utils.NonAllocLINQ
{
	public static class DictionaryExtensions
	{
		public static void ForEach<TKey, TVal>(this global::System.Collections.Generic.Dictionary<TKey, TVal> target, global::System.Action<global::System.Collections.Generic.KeyValuePair<TKey, TVal>> action)
		{
			foreach (global::System.Collections.Generic.KeyValuePair<TKey, TVal> item in target)
			{
				action(item);
			}
		}

		public static void FromArray<TKey, TArrItem>(this global::System.Collections.Generic.Dictionary<TKey, TArrItem> target, TArrItem[] array, global::System.Func<TArrItem, TKey> selector)
		{
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				TArrItem val = array[i];
				target[selector(val)] = val;
			}
		}

		public static void ForEachKey<TKey, TVal>(this global::System.Collections.Generic.Dictionary<TKey, TVal> target, global::System.Action<TKey> action)
		{
			target.ForEach(delegate(global::System.Collections.Generic.KeyValuePair<TKey, TVal> x)
			{
				action(x.Key);
			});
		}

		public static void ForEachValue<TKey, TVal>(this global::System.Collections.Generic.Dictionary<TKey, TVal> target, global::System.Action<TVal> action)
		{
			target.ForEach(delegate(global::System.Collections.Generic.KeyValuePair<TKey, TVal> x)
			{
				action(x.Value);
			});
		}

		public static int Count<TKey, TVal>(this global::System.Collections.Generic.Dictionary<TKey, TVal> target, global::System.Func<global::System.Collections.Generic.KeyValuePair<TKey, TVal>, bool> condition)
		{
			int num = 0;
			foreach (global::System.Collections.Generic.KeyValuePair<TKey, TVal> item in target)
			{
				if (condition(item))
				{
					num++;
				}
			}
			return num;
		}

		public static bool Any<TKey, TVal>(this global::System.Collections.Generic.Dictionary<TKey, TVal> target, global::System.Func<global::System.Collections.Generic.KeyValuePair<TKey, TVal>, bool> condition)
		{
			foreach (global::System.Collections.Generic.KeyValuePair<TKey, TVal> item in target)
			{
				if (condition(item))
				{
					return true;
				}
			}
			return false;
		}

		public static bool All<TKey, TVal>(this global::System.Collections.Generic.Dictionary<TKey, TVal> target, global::System.Func<global::System.Collections.Generic.KeyValuePair<TKey, TVal>, bool> condition)
		{
			foreach (global::System.Collections.Generic.KeyValuePair<TKey, TVal> item in target)
			{
				if (!condition(item))
				{
					return false;
				}
			}
			return true;
		}
	}
}
