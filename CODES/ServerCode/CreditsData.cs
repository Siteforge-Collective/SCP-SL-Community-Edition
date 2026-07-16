public static class CreditsData
{
	public static bool RemoteLoaded;

	public const string Patreons = "nz\n[TFB]Bornjester\nDragon\nok yes\nwiggy";

	internal const string CurrentNicknamePlaceholder = "<current nickname>";

	private static readonly string CachePath;

	internal static CreditsCategory[] Data;

	static CreditsData()
	{
		CachePath = FileManager.GetAppFolder() + "CreditsCache.json";
		if (!global::System.IO.File.Exists(CachePath))
		{
			global::System.IO.File.Copy("CreditsCache.json", CachePath);
		}
		SetCredits(global::System.IO.File.ReadAllText(CachePath));
	}

	internal static void LoadData(string text)
	{
		SetCredits(text);
		global::System.IO.File.WriteAllText(CachePath, text);
	}

	private static void SetCredits(string text)
	{
		CreditsList creditsList = JsonSerialize.FromJson<CreditsList>(text);
		global::System.Collections.Generic.List<CreditsCategory> list = new global::System.Collections.Generic.List<CreditsCategory>();
		global::System.Collections.Generic.IEnumerable<TranslationManifest> enumerable = global::System.Linq.Enumerable.Select(global::System.IO.Directory.EnumerateFiles("Translations/", "manifest.json", global::System.IO.SearchOption.AllDirectories), (string s) => JsonSerialize.FromFile<TranslationManifest>(s));
		for (int num = 0; num < creditsList.credits.Length; num++)
		{
			CreditsListCategory creditsListCategory = creditsList.credits[num];
			global::System.Collections.Generic.List<CreditsEntry> list2 = new global::System.Collections.Generic.List<CreditsEntry>();
			for (int num2 = 0; num2 < creditsListCategory.members.Length; num2++)
			{
				list2.Add(ProcessEntry(creditsListCategory.members[num2]));
			}
			if (creditsListCategory.category.Equals("SPECIAL THANKS", global::System.StringComparison.OrdinalIgnoreCase))
			{
				list2.Add(new CreditsEntry(TranslationReader.Get("NewMainMenu", 87, "For playing our game!"), "<current nickname>", new global::UnityEngine.Color32(byte.MaxValue, 215, 0, byte.MaxValue)));
				list.Add(new CreditsCategory
				{
					Header = creditsListCategory.category,
					Records = list2.ToArray()
				});
				list2.Clear();
				foreach (TranslationManifest item in enumerable)
				{
					string text2 = item.Name;
					if (!(text2 == "English (default)"))
					{
						int num3 = item.Name.IndexOf('-');
						if (num3 > -1)
						{
							text2 = item.Name.Substring(num3 + 2);
						}
						string[] authors = item.Authors;
						foreach (string name in authors)
						{
							list2.Add(new CreditsEntry(text2, name));
						}
						list2.Add(new CreditsEntry("", ""));
					}
				}
				list.Add(new CreditsCategory
				{
					Header = "COMMUNITY TRANSLATORS",
					Records = list2.ToArray()
				});
			}
			else
			{
				list.Add(new CreditsCategory
				{
					Header = creditsListCategory.category,
					Records = list2.ToArray()
				});
			}
		}
		Data = list.ToArray();
		RemoteLoaded = true;
	}

	private static CreditsEntry ProcessEntry(CreditsListMember member)
	{
		if (string.IsNullOrEmpty(member.name))
		{
			return new CreditsEntry();
		}
		if (!string.IsNullOrEmpty(member.title))
		{
			return new CreditsEntry(member.title, member.name, HexToColor(member));
		}
		return new CreditsEntry(member.name);
	}

	private static global::UnityEngine.Color32 HexToColor(CreditsListMember member)
	{
		if (string.IsNullOrEmpty(member.color))
		{
			return global::UnityEngine.Color.white;
		}
		if (Misc.TryParseColor(member.color, out var color))
		{
			return color;
		}
		global::UnityEngine.Debug.LogError("Error during processing credits color (" + member.name + " - " + member.title + " - " + member.color + ").");
		return global::UnityEngine.Color.white;
	}
}
