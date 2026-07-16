public static class Translations
{
	private class LoadedTranslation
	{
		public readonly string Localized;

		public readonly string Fallback;

		public readonly bool HasLocal;

		public LoadedTranslation(string[] fallbacks, string[] locals, bool hasLocal, int i)
		{
			Fallback = fallbacks[i];
			if (hasLocal && i < locals.Length)
			{
				Localized = locals[i];
				HasLocal = !string.IsNullOrWhiteSpace(Localized);
			}
			else
			{
				Localized = null;
				HasLocal = false;
			}
		}
	}

	private static bool _pascalCaseGenerated;

	private static readonly global::System.Collections.Generic.Dictionary<global::System.Type, Translations.LoadedTranslation[]> AllTranslations = new global::System.Collections.Generic.Dictionary<global::System.Type, Translations.LoadedTranslation[]>();

	private static readonly global::System.Collections.Generic.Dictionary<string, string> TypeToFilename = new global::System.Collections.Generic.Dictionary<string, string>();

	private static readonly string[] Suffixes = new string[2] { "translation", "key" };

	private static void DefineKey(string key)
	{
		string formatted = key.Replace("_", string.Empty).ToLowerInvariant();
		TypeToFilename[formatted] = key;
		Suffixes.ForEach(delegate(string x)
		{
			TypeToFilename[formatted + x] = key;
		});
	}

	private static bool TryGenerate(global::System.Type enumType, out Translations.LoadedTranslation[] generated)
	{
		if (!_pascalCaseGenerated)
		{
			_pascalCaseGenerated = true;
			global::Utils.NonAllocLINQ.DictionaryExtensions.ForEachKey(TranslationReader.Fallback, DefineKey);
		}
		if (!TypeToFilename.TryGetValue(enumType.Name.ToLowerInvariant(), out var value))
		{
			generated = new Translations.LoadedTranslation[0];
			return false;
		}
		string[] array = TranslationReader.Fallback[value];
		string[] value2;
		bool hasLocal = TranslationReader.Elements.TryGetValue(value, out value2);
		int num = array.Length;
		generated = new Translations.LoadedTranslation[num];
		for (int i = 0; i < num; i++)
		{
			generated[i] = new Translations.LoadedTranslation(array, value2, hasLocal, i);
		}
		AllTranslations[enumType] = generated;
		return true;
	}

	private static bool TryGet(global::System.Type type, object enumObject, out string str)
	{
		if (!AllTranslations.TryGetValue(type, out var value) && !TryGenerate(type, out value))
		{
			str = null;
			return false;
		}
		int num = (int)enumObject;
		if (num > value.Length)
		{
			str = null;
			return false;
		}
		Translations.LoadedTranslation loadedTranslation = value[num];
		str = (loadedTranslation.HasLocal ? loadedTranslation.Localized : loadedTranslation.Fallback);
		return !string.IsNullOrWhiteSpace(str);
	}

	public static void ResetCache()
	{
		AllTranslations.Clear();
	}

	public static bool TryGet<T>(T val, out string tr) where T : global::System.Enum
	{
		global::System.Type typeFromHandle = typeof(T);
		if (TryGet(typeFromHandle, val, out tr))
		{
			return true;
		}
		tr = val.ToString();
		global::UnityEngine.Debug.LogWarning("Missing translation! " + typeFromHandle.Name + ":" + tr + ".");
		return false;
	}

	public static string Get<T>(T val) where T : global::System.Enum
	{
		TryGet(val, out var tr);
		return tr;
	}

	public static string Get<T>(T val, string fallback) where T : global::System.Enum
	{
		if (!TryGet(val, out var tr))
		{
			return fallback;
		}
		return tr;
	}
}
