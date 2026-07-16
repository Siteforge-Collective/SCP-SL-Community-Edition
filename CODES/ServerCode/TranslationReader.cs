public static class TranslationReader
{
	private static string _translationPath;

	public static readonly global::System.Collections.Generic.Dictionary<string, string[]> Elements;

	public static readonly global::System.Collections.Generic.Dictionary<string, string[]> Fallback;

	private static readonly global::System.Collections.Generic.Dictionary<string, global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.Dictionary<string, int>>> _positions;

	private static readonly global::System.Text.RegularExpressions.Regex _matchFormat;

	private static global::TMPro.TMP_FontAsset[] defaultFallbacks;

	public const string NoTranslation = "NO_TRANSLATION";

	public const string TranslationDirectory = "Translations/";

	public static string TranslationPath
	{
		get
		{
			return _translationPath;
		}
		private set
		{
			_translationPath = global::System.IO.Path.GetFullPath(value);
			TranslationDirectoryName = global::System.IO.Path.GetFileName(_translationPath);
		}
	}

	public static string TranslationDirectoryName { get; private set; }

	public static TranslationManifest TranslationManifest { get; private set; }

	public static global::System.Globalization.CultureInfo TranslationCulture { get; private set; }

	public static event global::System.Action OnTranslationsRefreshed;

	static TranslationReader()
	{
		Elements = new global::System.Collections.Generic.Dictionary<string, string[]>();
		Fallback = new global::System.Collections.Generic.Dictionary<string, string[]>();
		_positions = new global::System.Collections.Generic.Dictionary<string, global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.Dictionary<string, int>>>();
		_matchFormat = new global::System.Text.RegularExpressions.Regex("\\{.*?\\}|\\[.*?\\]");
		Refresh();
		LoadPositions();
		global::UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneWasLoaded;
	}

	private static void OnSceneWasLoaded(global::UnityEngine.SceneManagement.Scene scene, global::UnityEngine.SceneManagement.LoadSceneMode mode)
	{
		PlayerPrefsSl.Refresh();
		Refresh();
	}

	[global::UnityEngine.RuntimeInitializeOnLoadMethod]
	public static void Refresh()
	{
		TranslationPath = GetTranslationPath();
		TranslationManifest = LoadTranslation(TranslationPath, Elements);
		LoadTranslation(CheckPath("en", "English (default)"), Fallback);
		TranslationCulture = null;
		string[] array = TranslationManifest.InterfaceLocales ?? global::System.Array.Empty<string>();
		foreach (string name in array)
		{
			try
			{
				TranslationCulture = global::System.Globalization.CultureInfo.GetCultureInfo(name);
			}
			catch
			{
				TranslationCulture = null;
				continue;
			}
			break;
		}
		if (TranslationCulture == null)
		{
			TranslationCulture = global::System.Globalization.CultureInfo.CurrentCulture;
		}
		global::System.Globalization.CultureInfo.CurrentCulture = TranslationCulture;
		global::System.Globalization.CultureInfo.CurrentUICulture = TranslationCulture;
		Translations.ResetCache();
		TranslationReader.OnTranslationsRefreshed?.Invoke();
	}

	private static string GetTranslationPath()
	{
		return CheckPath("en", "English (default)") ?? throw new global::System.IO.DirectoryNotFoundException();
	}

	private static void LoadPositions()
	{
		string[] files = global::System.IO.Directory.GetFiles(CheckPath("en", "English (default)"));
		foreach (string path in files)
		{
			string[] array = global::System.IO.File.ReadAllLines(path);
			global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.Dictionary<string, int>> dictionary = new global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.Dictionary<string, int>>();
			for (int j = 0; j < array.Length; j++)
			{
				global::System.Collections.Generic.Dictionary<string, int> dictionary2 = new global::System.Collections.Generic.Dictionary<string, int>();
				global::System.Text.RegularExpressions.MatchCollection matchCollection = _matchFormat.Matches(array[j]);
				for (int k = 0; k < matchCollection.Count; k++)
				{
					if (!dictionary2.ContainsKey(matchCollection[k].Value))
					{
						dictionary2.Add(matchCollection[k].Value, k);
					}
				}
				dictionary.Add(j, dictionary2);
			}
			_positions.Add(global::System.IO.Path.GetFileNameWithoutExtension(path), dictionary);
		}
	}

	private static TranslationManifest LoadTranslation(string translationPath, global::System.Collections.Generic.Dictionary<string, string[]> dictionary)
	{
		dictionary.Clear();
		if (global::System.IO.File.Exists(translationPath + "Legacy_Interfaces.txt") && global::System.IO.File.Exists(translationPath + "Legancy_Interfaces.txt"))
		{
			global::System.IO.File.Delete(translationPath + "Legancy_Interfaces.txt");
		}
		foreach (string item in global::System.IO.Directory.EnumerateFiles(translationPath, "*.txt"))
		{
			string[] array = FileManager.ReadAllLines(item);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = array[i].Replace("\\n", global::System.Environment.NewLine);
				foreach (global::System.Text.RegularExpressions.Match item2 in _matchFormat.Matches(array[i]))
				{
					if (_positions.TryGetValue(global::System.IO.Path.GetFileNameWithoutExtension(item), out var value) && value.TryGetValue(i, out var value2) && value2.TryGetValue(item2.Value, out var value3))
					{
						array[i] = array[i].Replace(item2.Value, "{" + value3 + "}");
					}
				}
			}
			string fileNameWithoutExtension = global::System.IO.Path.GetFileNameWithoutExtension(item);
			dictionary[(fileNameWithoutExtension == "Legancy_Interfaces") ? "Legacy_Interfaces" : fileNameWithoutExtension] = array;
		}
		try
		{
			return JsonSerialize.FromFile<TranslationManifest>(global::System.IO.Path.Combine(translationPath, "manifest.json"));
		}
		catch (global::System.IO.FileNotFoundException)
		{
			return new TranslationManifest(global::System.IO.Path.GetFileName(translationPath), global::System.Array.Empty<string>(), global::System.Array.Empty<string>(), global::System.Array.Empty<string>(), global::System.Array.Empty<string>());
		}
	}

	private static string CheckPath(params string[] suffixes)
	{
		foreach (string text in suffixes)
		{
			if (!string.IsNullOrWhiteSpace(text))
			{
				string text2 = "Translations/" + text;
				if (global::System.IO.Directory.Exists(text2))
				{
					return text2;
				}
			}
		}
		return null;
	}

	public static string[] GetKeys(string keyName)
	{
		if (keyName == "Legancy_Interfaces")
		{
			keyName = "Legacy_Interfaces";
		}
		if (Fallback.TryGetValue(keyName, out var value))
		{
			return value;
		}
		global::UnityEngine.Debug.LogWarning("Tried to get **FALLBACK** translation from nonexistent file " + keyName);
		return null;
	}

	public static string[] GetFallbackKeys(string keyName)
	{
		if (keyName == "Legancy_Interfaces")
		{
			keyName = "Legacy_Interfaces";
		}
		if (!Fallback.TryGetValue(keyName, out var value))
		{
			return null;
		}
		return value;
	}

	public static string Get(string keyName, int index, string defaultValue = "NO_TRANSLATION")
	{
		if (global::GameCore.Console.TranslationDebugMode)
		{
			return $"{keyName}:{index}";
		}
		if (keyName == "Legancy_Interfaces")
		{
			keyName = "Legacy_Interfaces";
		}
		return GetFallback(keyName, index, defaultValue) ?? defaultValue;
	}

	public static bool TryGet(string keyName, int index, out string val)
	{
		if (Fallback.TryGetValue(keyName, out var value) && value.TryGet(index, out var element) && !string.IsNullOrWhiteSpace(element))
		{
			val = element;
			return true;
		}
		val = GetFallback(keyName, index, "NO_TRANSLATION");
		return false;
	}

	private static string GetFallback(string keyName, int index, string defaultvalue)
	{
		if (Fallback.TryGetValue(keyName, out var value) && value.TryGet(index, out var element) && !string.IsNullOrWhiteSpace(element))
		{
			return element;
		}
		global::UnityEngine.Debug.LogWarning(string.Format("Missing **FALLBACK** translation! {0}:{1}. Default value: {2}", keyName, index, defaultvalue.Replace("<", "(<)")));
		return null;
	}

	public static string GetFormatted(string keyName, int index, string defaultvalue, object obj1)
	{
		return string.Format(Get(keyName, index, defaultvalue), obj1);
	}

	public static string GetFormatted(string keyName, int index, string defaultvalue, object obj1, object obj2)
	{
		return string.Format(Get(keyName, index, defaultvalue), obj1, obj2);
	}

	public static string GetFormatted(string keyName, int index, string defaultvalue, object obj1, object obj2, object obj3)
	{
		return string.Format(Get(keyName, index, defaultvalue), obj1, obj2, obj3);
	}

	public static string GetFormatted(string keyName, int index, string defaultvalue, params object[] format)
	{
		return string.Format(Get(keyName, index, defaultvalue), format);
	}
}
