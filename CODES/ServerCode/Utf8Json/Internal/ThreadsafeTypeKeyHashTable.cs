namespace Utf8Json.Internal
{
	internal class ThreadsafeTypeKeyHashTable<TValue>
	{
		private class Entry
		{
			public global::System.Type Key;

			public TValue Value;

			public int Hash;

			public global::Utf8Json.Internal.ThreadsafeTypeKeyHashTable<TValue>.Entry Next;

			public override string ToString()
			{
				return string.Concat(Key, "(", Count(), ")");
			}

			private int Count()
			{
				int num = 1;
				global::Utf8Json.Internal.ThreadsafeTypeKeyHashTable<TValue>.Entry entry = this;
				while (entry.Next != null)
				{
					num++;
					entry = entry.Next;
				}
				return num;
			}
		}

		private global::Utf8Json.Internal.ThreadsafeTypeKeyHashTable<TValue>.Entry[] buckets;

		private int size;

		private readonly object writerLock = new object();

		private readonly float loadFactor;

		public ThreadsafeTypeKeyHashTable(int capacity = 4, float loadFactor = 0.75f)
		{
			int num = CalculateCapacity(capacity, loadFactor);
			buckets = new global::Utf8Json.Internal.ThreadsafeTypeKeyHashTable<TValue>.Entry[num];
			this.loadFactor = loadFactor;
		}

		public bool TryAdd(global::System.Type key, TValue value)
		{
			return TryAdd(key, (global::System.Type _) => value);
		}

		public bool TryAdd(global::System.Type key, global::System.Func<global::System.Type, TValue> valueFactory)
		{
			TValue resultingValue;
			return TryAddInternal(key, valueFactory, out resultingValue);
		}

		private bool TryAddInternal(global::System.Type key, global::System.Func<global::System.Type, TValue> valueFactory, out TValue resultingValue)
		{
			lock (writerLock)
			{
				int num = CalculateCapacity(size + 1, loadFactor);
				if (buckets.Length < num)
				{
					global::Utf8Json.Internal.ThreadsafeTypeKeyHashTable<TValue>.Entry[] value = new global::Utf8Json.Internal.ThreadsafeTypeKeyHashTable<TValue>.Entry[num];
					for (int i = 0; i < buckets.Length; i++)
					{
						for (global::Utf8Json.Internal.ThreadsafeTypeKeyHashTable<TValue>.Entry entry = buckets[i]; entry != null; entry = entry.Next)
						{
							global::Utf8Json.Internal.ThreadsafeTypeKeyHashTable<TValue>.Entry newEntryOrNull = new global::Utf8Json.Internal.ThreadsafeTypeKeyHashTable<TValue>.Entry
							{
								Key = entry.Key,
								Value = entry.Value,
								Hash = entry.Hash
							};
							AddToBuckets(value, key, newEntryOrNull, null, out resultingValue);
						}
					}
					bool num2 = AddToBuckets(value, key, null, valueFactory, out resultingValue);
					VolatileWrite(ref buckets, value);
					if (num2)
					{
						size++;
					}
					return num2;
				}
				bool num3 = AddToBuckets(buckets, key, null, valueFactory, out resultingValue);
				if (num3)
				{
					size++;
				}
				return num3;
			}
		}

		private bool AddToBuckets(global::Utf8Json.Internal.ThreadsafeTypeKeyHashTable<TValue>.Entry[] buckets, global::System.Type newKey, global::Utf8Json.Internal.ThreadsafeTypeKeyHashTable<TValue>.Entry newEntryOrNull, global::System.Func<global::System.Type, TValue> valueFactory, out TValue resultingValue)
		{
			int num = newEntryOrNull?.Hash ?? newKey.GetHashCode();
			if (buckets[num & (buckets.Length - 1)] == null)
			{
				if (newEntryOrNull != null)
				{
					resultingValue = newEntryOrNull.Value;
					VolatileWrite(ref buckets[num & (buckets.Length - 1)], newEntryOrNull);
				}
				else
				{
					resultingValue = valueFactory(newKey);
					VolatileWrite(ref buckets[num & (buckets.Length - 1)], new global::Utf8Json.Internal.ThreadsafeTypeKeyHashTable<TValue>.Entry
					{
						Key = newKey,
						Value = resultingValue,
						Hash = num
					});
				}
			}
			else
			{
				global::Utf8Json.Internal.ThreadsafeTypeKeyHashTable<TValue>.Entry entry = buckets[num & (buckets.Length - 1)];
				while (true)
				{
					if (entry.Key == newKey)
					{
						resultingValue = entry.Value;
						return false;
					}
					if (entry.Next == null)
					{
						break;
					}
					entry = entry.Next;
				}
				if (newEntryOrNull != null)
				{
					resultingValue = newEntryOrNull.Value;
					VolatileWrite(ref entry.Next, newEntryOrNull);
				}
				else
				{
					resultingValue = valueFactory(newKey);
					VolatileWrite(ref entry.Next, new global::Utf8Json.Internal.ThreadsafeTypeKeyHashTable<TValue>.Entry
					{
						Key = newKey,
						Value = resultingValue,
						Hash = num
					});
				}
			}
			return true;
		}

		public bool TryGetValue(global::System.Type key, out TValue value)
		{
			global::Utf8Json.Internal.ThreadsafeTypeKeyHashTable<TValue>.Entry[] array = buckets;
			int hashCode = key.GetHashCode();
			global::Utf8Json.Internal.ThreadsafeTypeKeyHashTable<TValue>.Entry entry = array[hashCode & (array.Length - 1)];
			if (entry != null)
			{
				if (entry.Key == key)
				{
					value = entry.Value;
					return true;
				}
				for (global::Utf8Json.Internal.ThreadsafeTypeKeyHashTable<TValue>.Entry next = entry.Next; next != null; next = next.Next)
				{
					if (next.Key == key)
					{
						value = next.Value;
						return true;
					}
				}
			}
			value = default(TValue);
			return false;
		}

		public TValue GetOrAdd(global::System.Type key, global::System.Func<global::System.Type, TValue> valueFactory)
		{
			if (TryGetValue(key, out var value))
			{
				return value;
			}
			TryAddInternal(key, valueFactory, out value);
			return value;
		}

		private static int CalculateCapacity(int collectionSize, float loadFactor)
		{
			int num = (int)((float)collectionSize / loadFactor);
			int num2;
			for (num2 = 1; num2 < num; num2 <<= 1)
			{
			}
			if (num2 < 8)
			{
				return 8;
			}
			return num2;
		}

		private static void VolatileWrite(ref global::Utf8Json.Internal.ThreadsafeTypeKeyHashTable<TValue>.Entry location, global::Utf8Json.Internal.ThreadsafeTypeKeyHashTable<TValue>.Entry value)
		{
			global::System.Threading.Thread.MemoryBarrier();
			location = value;
		}

		private static void VolatileWrite(ref global::Utf8Json.Internal.ThreadsafeTypeKeyHashTable<TValue>.Entry[] location, global::Utf8Json.Internal.ThreadsafeTypeKeyHashTable<TValue>.Entry[] value)
		{
			global::System.Threading.Thread.MemoryBarrier();
			location = value;
		}
	}
}
