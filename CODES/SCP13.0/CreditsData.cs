using UnityEngine;

public static class CreditsData
{
	public static bool RemoteLoaded;

	internal const string CurrentNicknamePlaceholder = "<current nickname>";

	private static readonly string CachePath;

	internal static CreditsCategory[] Data;

	static CreditsData()
	{
	}

	internal static void LoadData(string text)
	{
	}

	private static void SetCredits(string text)
	{
	}

	private static CreditsEntry ProcessEntry(CreditsListMember member)
	{
		return null;
	}

	private static Color32 HexToColor(CreditsListMember member)
	{
		return default(Color32);
	}
}
