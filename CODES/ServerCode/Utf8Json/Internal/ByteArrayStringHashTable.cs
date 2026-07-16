namespace Utf8Json.Internal
{
	internal class ByteArrayStringHashTable<T> : global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.KeyValuePair<string, T>>, global::System.Collections.IEnumerable
	{
		private struct Entry
		{
			public byte[] Key;

			public T Value;

			public override string ToString()
			{
				return string.Concat("(", global::System.Text.Encoding.UTF8.GetString(Key), ", ", Value, ")");
			}
		}

		private readonly global::Utf8Json.Internal.ByteArrayStringHashTable<T>.Entry[][] buckets;

		private readonly ulong indexFor;

		public ByteArrayStringHashTable(int capacity)
			: this(capacity, 0.42f)
		{
		}

		public ByteArrayStringHashTable(int capacity, float loadFactor)
		{
			int num = CalculateCapacity(capacity, loadFactor);
			buckets = new global::Utf8Json.Internal.ByteArrayStringHashTable<T>.Entry[num][];
			indexFor = (ulong)buckets.Length - 1uL;
		}

		public void Add(string key, T value)
		{
			if (!TryAddInternal(global::System.Text.Encoding.UTF8.GetBytes(key), value))
			{
				throw new global::System.ArgumentException("Key was already exists. Key:" + key);
			}
		}

		public void Add(byte[] key, T value)
		{
			if (!TryAddInternal(key, value))
			{
				throw new global::System.ArgumentException("Key was already exists. Key:" + key);
			}
		}

		private bool TryAddInternal(byte[] key, T value)
		{
			ulong num = ByteArrayGetHashCode(key, 0, key.Length);
			global::Utf8Json.Internal.ByteArrayStringHashTable<T>.Entry entry = new global::Utf8Json.Internal.ByteArrayStringHashTable<T>.Entry
			{
				Key = key,
				Value = value
			};
			global::Utf8Json.Internal.ByteArrayStringHashTable<T>.Entry[] array = buckets[num & indexFor];
			if (array == null)
			{
				buckets[num & indexFor] = new global::Utf8Json.Internal.ByteArrayStringHashTable<T>.Entry[1] { entry };
			}
			else
			{
				for (int i = 0; i < array.Length; i++)
				{
					byte[] key2 = array[i].Key;
					if (global::Utf8Json.Internal.ByteArrayComparer.Equals(key, 0, key.Length, key2))
					{
						return false;
					}
				}
				global::Utf8Json.Internal.ByteArrayStringHashTable<T>.Entry[] array2 = new global::Utf8Json.Internal.ByteArrayStringHashTable<T>.Entry[array.Length + 1];
				global::System.Array.Copy(array, array2, array.Length);
				array = array2;
				array[array.Length - 1] = entry;
				buckets[num & indexFor] = array;
			}
			return true;
		}

		public bool TryGetValue(global::System.ArraySegment<byte> key, out T value)
		{
			global::Utf8Json.Internal.ByteArrayStringHashTable<T>.Entry[][] array = buckets;
			ulong num = ByteArrayGetHashCode(key.Array, key.Offset, key.Count);
			global::Utf8Json.Internal.ByteArrayStringHashTable<T>.Entry[] array2 = array[num & indexFor];
			if (array2 != null)
			{
				global::Utf8Json.Internal.ByteArrayStringHashTable<T>.Entry entry = array2[0];
				if (global::Utf8Json.Internal.ByteArrayComparer.Equals(key.Array, key.Offset, key.Count, entry.Key))
				{
					value = entry.Value;
					return true;
				}
				for (int i = 1; i < array2.Length; i++)
				{
					global::Utf8Json.Internal.ByteArrayStringHashTable<T>.Entry entry2 = array2[i];
					if (global::Utf8Json.Internal.ByteArrayComparer.Equals(key.Array, key.Offset, key.Count, entry2.Key))
					{
						value = entry2.Value;
						return true;
					}
				}
			}
			value = default(T);
			return false;
		}

		private static ulong ByteArrayGetHashCode(byte[] x, int offset, int count)
		{
			uint num = 0u;
			if (x != null)
			{
				int num2 = offset + count;
				num = 2166136261u;
				for (int i = offset; i < num2; i++)
				{
					num = (x[i] ^ num) * 16777619;
				}
			}
			return num;
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

		public global::System.Collections.Generic.IEnumerator<global::System.Collections.Generic.KeyValuePair<string, T>> GetEnumerator()
		{
			global::Utf8Json.Internal.ByteArrayStringHashTable<T>.Entry[][] array = buckets;
			global::Utf8Json.Internal.ByteArrayStringHashTable<T>.Entry[][] array2 = array;
			foreach (global::Utf8Json.Internal.ByteArrayStringHashTable<T>.Entry[] array3 in array2)
			{
				if (array3 != null)
				{
					global::Utf8Json.Internal.ByteArrayStringHashTable<T>.Entry[] array4 = array3;
					for (int j = 0; j < array4.Length; j++)
					{
						global::Utf8Json.Internal.ByteArrayStringHashTable<T>.Entry entry = array4[j];
						yield return new global::System.Collections.Generic.KeyValuePair<string, T>(global::System.Text.Encoding.UTF8.GetString(entry.Key), entry.Value);
					}
				}
			}
		}

		global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
