namespace Utf8Json.Internal
{
	public class AutomataDictionary : global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.KeyValuePair<string, int>>, global::System.Collections.IEnumerable
	{
		private class AutomataNode : global::System.IComparable<global::Utf8Json.Internal.AutomataDictionary.AutomataNode>
		{
			private static readonly global::Utf8Json.Internal.AutomataDictionary.AutomataNode[] emptyNodes = new global::Utf8Json.Internal.AutomataDictionary.AutomataNode[0];

			private static readonly ulong[] emptyKeys = new ulong[0];

			public ulong Key;

			public int Value;

			public string originalKey;

			private global::Utf8Json.Internal.AutomataDictionary.AutomataNode[] nexts;

			private ulong[] nextKeys;

			private int count;

			public bool HasChildren => count != 0;

			public AutomataNode(ulong key)
			{
				Key = key;
				Value = -1;
				nexts = emptyNodes;
				nextKeys = emptyKeys;
				count = 0;
				originalKey = null;
			}

			public global::Utf8Json.Internal.AutomataDictionary.AutomataNode Add(ulong key)
			{
				int num = global::System.Array.BinarySearch(nextKeys, 0, count, key);
				if (num < 0)
				{
					if (nexts.Length == count)
					{
						global::System.Array.Resize(ref nexts, (count == 0) ? 4 : (count * 2));
						global::System.Array.Resize(ref nextKeys, (count == 0) ? 4 : (count * 2));
					}
					count++;
					global::Utf8Json.Internal.AutomataDictionary.AutomataNode automataNode = new global::Utf8Json.Internal.AutomataDictionary.AutomataNode(key);
					nexts[count - 1] = automataNode;
					nextKeys[count - 1] = key;
					global::System.Array.Sort(nexts, 0, count);
					global::System.Array.Sort(nextKeys, 0, count);
					return automataNode;
				}
				return nexts[num];
			}

			public global::Utf8Json.Internal.AutomataDictionary.AutomataNode Add(ulong key, int value, string originalKey)
			{
				global::Utf8Json.Internal.AutomataDictionary.AutomataNode automataNode = Add(key);
				automataNode.Value = value;
				automataNode.originalKey = originalKey;
				return automataNode;
			}

			public unsafe global::Utf8Json.Internal.AutomataDictionary.AutomataNode SearchNext(ref byte* p, ref int rest)
			{
				ulong key = global::Utf8Json.Internal.AutomataKeyGen.GetKey(ref p, ref rest);
				if (count < 4)
				{
					for (int i = 0; i < count; i++)
					{
						if (nextKeys[i] == key)
						{
							return nexts[i];
						}
					}
				}
				else
				{
					int num = BinarySearch(nextKeys, 0, count, key);
					if (num >= 0)
					{
						return nexts[num];
					}
				}
				return null;
			}

			public global::Utf8Json.Internal.AutomataDictionary.AutomataNode SearchNextSafe(byte[] p, ref int offset, ref int rest)
			{
				ulong keySafe = global::Utf8Json.Internal.AutomataKeyGen.GetKeySafe(p, ref offset, ref rest);
				if (count < 4)
				{
					for (int i = 0; i < count; i++)
					{
						if (nextKeys[i] == keySafe)
						{
							return nexts[i];
						}
					}
				}
				else
				{
					int num = BinarySearch(nextKeys, 0, count, keySafe);
					if (num >= 0)
					{
						return nexts[num];
					}
				}
				return null;
			}

			internal static int BinarySearch(ulong[] array, int index, int length, ulong value)
			{
				int num = index;
				int num2 = index + length - 1;
				while (num <= num2)
				{
					int num3 = num + (num2 - num >> 1);
					ulong num4 = array[num3];
					int num5 = ((num4 < value) ? (-1) : ((num4 > value) ? 1 : 0));
					if (num5 == 0)
					{
						return num3;
					}
					if (num5 < 0)
					{
						num = num3 + 1;
					}
					else
					{
						num2 = num3 - 1;
					}
				}
				return ~num;
			}

			public int CompareTo(global::Utf8Json.Internal.AutomataDictionary.AutomataNode other)
			{
				return Key.CompareTo(other.Key);
			}

			public global::System.Collections.Generic.IEnumerable<global::Utf8Json.Internal.AutomataDictionary.AutomataNode> YieldChildren()
			{
				for (int i = 0; i < count; i++)
				{
					yield return nexts[i];
				}
			}

			public void EmitSearchNext(global::System.Reflection.Emit.ILGenerator il, global::System.Reflection.Emit.LocalBuilder p, global::System.Reflection.Emit.LocalBuilder rest, global::System.Reflection.Emit.LocalBuilder key, global::System.Action<global::System.Collections.Generic.KeyValuePair<string, int>> onFound, global::System.Action onNotFound)
			{
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloca(il, p);
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloca(il, rest);
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitCall(il, global::Utf8Json.Internal.AutomataKeyGen.GetKeyMethod);
				global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitStloc(il, key);
				EmitSearchNextCore(il, p, rest, key, onFound, onNotFound, nexts, count);
			}

			private static void EmitSearchNextCore(global::System.Reflection.Emit.ILGenerator il, global::System.Reflection.Emit.LocalBuilder p, global::System.Reflection.Emit.LocalBuilder rest, global::System.Reflection.Emit.LocalBuilder key, global::System.Action<global::System.Collections.Generic.KeyValuePair<string, int>> onFound, global::System.Action onNotFound, global::Utf8Json.Internal.AutomataDictionary.AutomataNode[] nexts, int count)
			{
				if (count < 4)
				{
					global::Utf8Json.Internal.AutomataDictionary.AutomataNode[] array = global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Where(global::System.Linq.Enumerable.Take(nexts, count), (global::Utf8Json.Internal.AutomataDictionary.AutomataNode x) => x.Value != -1));
					global::Utf8Json.Internal.AutomataDictionary.AutomataNode[] array2 = global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Where(global::System.Linq.Enumerable.Take(nexts, count), (global::Utf8Json.Internal.AutomataDictionary.AutomataNode x) => x.HasChildren));
					global::System.Reflection.Emit.Label label = il.DefineLabel();
					global::System.Reflection.Emit.Label label2 = il.DefineLabel();
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloc(il, rest);
					if (array2.Length != 0 && array.Length == 0)
					{
						il.Emit(global::System.Reflection.Emit.OpCodes.Brfalse, label2);
					}
					else
					{
						il.Emit(global::System.Reflection.Emit.OpCodes.Brtrue, label);
					}
					global::System.Reflection.Emit.Label[] array3 = global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Select(global::System.Linq.Enumerable.Range(0, global::System.Math.Max(array.Length - 1, 0)), (int _) => il.DefineLabel()));
					for (int num = 0; num < array.Length; num++)
					{
						global::System.Reflection.Emit.Label label3 = il.DefineLabel();
						if (num != 0)
						{
							il.MarkLabel(array3[num - 1]);
						}
						global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloc(il, key);
						global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitULong(il, array[num].Key);
						il.Emit(global::System.Reflection.Emit.OpCodes.Bne_Un, label3);
						onFound(new global::System.Collections.Generic.KeyValuePair<string, int>(array[num].originalKey, array[num].Value));
						il.MarkLabel(label3);
						if (num != array.Length - 1)
						{
							il.Emit(global::System.Reflection.Emit.OpCodes.Br, array3[num]);
						}
						else
						{
							onNotFound();
						}
					}
					il.MarkLabel(label);
					global::System.Reflection.Emit.Label[] array4 = global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Select(global::System.Linq.Enumerable.Range(0, global::System.Math.Max(array2.Length - 1, 0)), (int _) => il.DefineLabel()));
					for (int num2 = 0; num2 < array2.Length; num2++)
					{
						global::System.Reflection.Emit.Label label4 = il.DefineLabel();
						if (num2 != 0)
						{
							il.MarkLabel(array4[num2 - 1]);
						}
						global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloc(il, key);
						global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitULong(il, array2[num2].Key);
						il.Emit(global::System.Reflection.Emit.OpCodes.Bne_Un, label4);
						array2[num2].EmitSearchNext(il, p, rest, key, onFound, onNotFound);
						il.MarkLabel(label4);
						if (num2 != array2.Length - 1)
						{
							il.Emit(global::System.Reflection.Emit.OpCodes.Br, array4[num2]);
						}
						else
						{
							onNotFound();
						}
					}
					il.MarkLabel(label2);
					onNotFound();
				}
				else
				{
					int num3 = count / 2;
					ulong key2 = nexts[num3].Key;
					global::Utf8Json.Internal.AutomataDictionary.AutomataNode[] array5 = global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Take(global::System.Linq.Enumerable.Take(nexts, count), num3));
					global::Utf8Json.Internal.AutomataDictionary.AutomataNode[] array6 = global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Skip(global::System.Linq.Enumerable.Take(nexts, count), num3));
					global::System.Reflection.Emit.Label label5 = il.DefineLabel();
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitLdloc(il, key);
					global::Utf8Json.Internal.Emit.ILGeneratorExtensions.EmitULong(il, key2);
					il.Emit(global::System.Reflection.Emit.OpCodes.Bge, label5);
					EmitSearchNextCore(il, p, rest, key, onFound, onNotFound, array5, array5.Length);
					il.MarkLabel(label5);
					EmitSearchNextCore(il, p, rest, key, onFound, onNotFound, array6, array6.Length);
				}
			}
		}

		private readonly global::Utf8Json.Internal.AutomataDictionary.AutomataNode root;

		public AutomataDictionary()
		{
			root = new global::Utf8Json.Internal.AutomataDictionary.AutomataNode(0uL);
		}

		public void Add(string str, int value)
		{
			Add(global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation(str), value);
		}

		public void Add(byte[] bytes, int value)
		{
			int offset = 0;
			global::Utf8Json.Internal.AutomataDictionary.AutomataNode automataNode = root;
			int rest = bytes.Length;
			while (rest != 0)
			{
				ulong keySafe = global::Utf8Json.Internal.AutomataKeyGen.GetKeySafe(bytes, ref offset, ref rest);
				automataNode = ((rest != 0) ? automataNode.Add(keySafe) : automataNode.Add(keySafe, value, global::System.Text.Encoding.UTF8.GetString(bytes)));
			}
		}

		public bool TryGetValueSafe(global::System.ArraySegment<byte> key, out int value)
		{
			global::Utf8Json.Internal.AutomataDictionary.AutomataNode automataNode = root;
			byte[] array = key.Array;
			int offset = key.Offset;
			int rest = key.Count;
			while (rest != 0 && automataNode != null)
			{
				automataNode = automataNode.SearchNextSafe(array, ref offset, ref rest);
			}
			if (automataNode == null)
			{
				value = -1;
				return false;
			}
			value = automataNode.Value;
			return true;
		}

		public override string ToString()
		{
			global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
			ToStringCore(root.YieldChildren(), stringBuilder, 0);
			return stringBuilder.ToString();
		}

		private static void ToStringCore(global::System.Collections.Generic.IEnumerable<global::Utf8Json.Internal.AutomataDictionary.AutomataNode> nexts, global::System.Text.StringBuilder sb, int depth)
		{
			foreach (global::Utf8Json.Internal.AutomataDictionary.AutomataNode next in nexts)
			{
				if (depth != 0)
				{
					sb.Append(' ', depth * 2);
				}
				sb.Append("[" + next.Key + "]");
				if (next.Value != -1)
				{
					sb.Append("(" + next.originalKey + ")");
					sb.Append(" = ");
					sb.Append(next.Value);
				}
				sb.AppendLine();
				ToStringCore(next.YieldChildren(), sb, depth + 1);
			}
		}

		global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public global::System.Collections.Generic.IEnumerator<global::System.Collections.Generic.KeyValuePair<string, int>> GetEnumerator()
		{
			return YieldCore(root.YieldChildren()).GetEnumerator();
		}

		private static global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.KeyValuePair<string, int>> YieldCore(global::System.Collections.Generic.IEnumerable<global::Utf8Json.Internal.AutomataDictionary.AutomataNode> nexts)
		{
			foreach (global::Utf8Json.Internal.AutomataDictionary.AutomataNode item in nexts)
			{
				if (item.Value != -1)
				{
					yield return new global::System.Collections.Generic.KeyValuePair<string, int>(item.originalKey, item.Value);
				}
				foreach (global::System.Collections.Generic.KeyValuePair<string, int> item2 in YieldCore(item.YieldChildren()))
				{
					yield return item2;
				}
			}
		}

		public void EmitMatch(global::System.Reflection.Emit.ILGenerator il, global::System.Reflection.Emit.LocalBuilder p, global::System.Reflection.Emit.LocalBuilder rest, global::System.Reflection.Emit.LocalBuilder key, global::System.Action<global::System.Collections.Generic.KeyValuePair<string, int>> onFound, global::System.Action onNotFound)
		{
			root.EmitSearchNext(il, p, rest, key, onFound, onNotFound);
		}
	}
}
