namespace Utils
{
	public static class RAUtils
	{
		private const string PlayerNameRegex = "@\"(.*?)\".|@[^\\s.]+\\.";

		public static readonly global::System.Text.RegularExpressions.Regex IsDigit = new global::System.Text.RegularExpressions.Regex("^(\\d*\\.?\\d*)$", global::System.Text.RegularExpressions.RegexOptions.None);

		public static string FormatArguments(global::System.ArraySegment<string> args, int index)
		{
			global::System.Text.StringBuilder stringBuilder = global::NorthwoodLib.Pools.StringBuilderPool.Shared.Rent();
			foreach (string item in (global::System.Collections.Generic.IEnumerable<string>)args.Segment(index)/*cast due to .constrained prefix*/)
			{
				stringBuilder.Append(item);
				stringBuilder.Append(" ");
			}
			string result = stringBuilder.ToString();
			global::NorthwoodLib.Pools.StringBuilderPool.Shared.Return(stringBuilder);
			return result;
		}

		public static global::System.Collections.Generic.List<ReferenceHub> ProcessPlayerIdOrNamesList(global::System.ArraySegment<string> args, int startindex, out string[] newargs, bool keepEmptyEntries = false)
		{
			try
			{
				string text = FormatArguments(args, startindex);
				global::System.Collections.Generic.List<ReferenceHub> list = global::NorthwoodLib.Pools.ListPool<ReferenceHub>.Shared.Rent();
				if (text.StartsWith("@", global::System.StringComparison.Ordinal))
				{
					foreach (global::System.Text.RegularExpressions.Match item in new global::System.Text.RegularExpressions.Regex("@\"(.*?)\".|@[^\\s.]+\\.").Matches(text))
					{
						text = ReplaceFirst(text, item.Value, "");
						string name = item.Value.Substring(1).Replace("\"", "").Replace(".", "");
						global::System.Collections.Generic.List<ReferenceHub> list2 = global::System.Linq.Enumerable.ToList(global::System.Linq.Enumerable.Where(ReferenceHub.AllHubs, (ReferenceHub ply) => ply.nicknameSync.MyNick.Equals(name)));
						if (list2.Count == 1 && !list.Contains(list2[0]))
						{
							list.Add(list2[0]);
						}
					}
					newargs = text.Split(new char[1] { ' ' }, (!keepEmptyEntries) ? global::System.StringSplitOptions.RemoveEmptyEntries : global::System.StringSplitOptions.None);
					return list;
				}
				if (args.At(startindex).Length > 0 && char.IsDigit(args.At(startindex)[0]))
				{
					string[] array = args.At(startindex).Split('.');
					for (int num = 0; num < array.Length; num++)
					{
						if (int.TryParse(array[num], out var result) && ReferenceHub.TryGetHub(result, out var hub) && !list.Contains(hub))
						{
							list.Add(hub);
						}
					}
				}
				newargs = ((args.Count > 1) ? FormatArguments(args, startindex + 1).Split(new char[1] { ' ' }, (!keepEmptyEntries) ? global::System.StringSplitOptions.RemoveEmptyEntries : global::System.StringSplitOptions.None) : null);
				return list;
			}
			catch (global::System.Exception exception)
			{
				global::UnityEngine.Debug.LogException(exception);
				newargs = null;
				return null;
			}
		}

		private static string ReplaceFirst(string str, string search, string replace)
		{
			int num = str.IndexOf(search, global::System.StringComparison.Ordinal);
			if (num >= 0)
			{
				return str.Substring(0, num) + replace + str.Substring(num + search.Length);
			}
			return str;
		}
	}
}
