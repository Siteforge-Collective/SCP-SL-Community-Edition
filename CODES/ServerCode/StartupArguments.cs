public static class StartupArguments
{
	public static bool IsSetShort(string param)
	{
		return global::System.Linq.Enumerable.Any(global::System.Environment.GetCommandLineArgs(), (string x) => x.StartsWith("-") && !x.StartsWith("--") && x.Contains(param));
	}

	public static bool IsSetBool(string param, string alias = "")
	{
		if (!global::System.Environment.GetCommandLineArgs().Contains<string>("--" + param))
		{
			if (!string.IsNullOrEmpty(alias))
			{
				return IsSetShort(alias);
			}
			return false;
		}
		return true;
	}

	public static string GetArgument(string param, string alias = "", string def = "")
	{
		string[] commandLineArgs = global::System.Environment.GetCommandLineArgs();
		bool flag = false;
		string[] array = commandLineArgs;
		foreach (string text in array)
		{
			if (flag && !text.StartsWith("-"))
			{
				return text;
			}
			flag = text == "--" + param || (!string.IsNullOrEmpty(alias) && text.StartsWith("-") && !text.StartsWith("--") && text.EndsWith(alias));
		}
		return def;
	}
}
