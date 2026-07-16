public static class ConsoleDebugMode
{
	public struct DebugChannel
	{
		public DebugLevel Level;

		public string Description;

		public global::UnityEngine.Color32 Color;

		public DebugChannel(DebugLevel lvl, string dsc, global::UnityEngine.Color32 col)
		{
			Level = lvl;
			Description = dsc;
			Color = col;
		}
	}

	public static readonly global::UnityEngine.Color32 defaultColor = new global::UnityEngine.Color32(85, 181, 125, byte.MaxValue);

	private static readonly global::System.Collections.Generic.Dictionary<string, ConsoleDebugMode.DebugChannel> debugChannels = new global::System.Collections.Generic.Dictionary<string, ConsoleDebugMode.DebugChannel>
	{
		["SCP079"] = new ConsoleDebugMode.DebugChannel((DebugLevel)global::UnityEngine.Mathf.Clamp(PlayerPrefsSl.Get("DEBUG_SCP079", 2), 0, 4), "for SCP-079 client- and server-side logging", defaultColor),
		["MAPGEN"] = new ConsoleDebugMode.DebugChannel((DebugLevel)global::UnityEngine.Mathf.Clamp(PlayerPrefsSl.Get("DEBUG_MAPGEN", 2), 0, 4), "for the Map Generator", defaultColor),
		["MISCEL"] = new ConsoleDebugMode.DebugChannel((DebugLevel)global::UnityEngine.Mathf.Clamp(PlayerPrefsSl.Get("DEBUG_MISCEL", 2), 0, 4), "for miscellaneous small sub-systems", defaultColor),
		["VC"] = new ConsoleDebugMode.DebugChannel((DebugLevel)global::UnityEngine.Mathf.Clamp(PlayerPrefsSl.Get("DEBUG_VC", 2), 0, 4), "for Voice Chat logging", defaultColor),
		["PLIST"] = new ConsoleDebugMode.DebugChannel((DebugLevel)global::UnityEngine.Mathf.Clamp(PlayerPrefsSl.Get("DEBUG_PLIST", 2), 0, 4), "for Player List", defaultColor),
		["MGCLTR"] = new ConsoleDebugMode.DebugChannel((DebugLevel)global::UnityEngine.Mathf.Clamp(PlayerPrefsSl.Get("DEBUG_MGCLTR", 2), 0, 4), "for Map Generator Clutter System", new global::UnityEngine.Color32(110, 160, 110, byte.MaxValue)),
		["SDAUTH"] = new ConsoleDebugMode.DebugChannel((DebugLevel)global::UnityEngine.Mathf.Clamp(PlayerPrefsSl.Get("DEBUG_SDAUTH", 2), 0, 4), "for Steam and Discord authenticator", new global::UnityEngine.Color32(130, 130, 130, byte.MaxValue)),
		["SCPCTRL"] = new ConsoleDebugMode.DebugChannel((DebugLevel)global::UnityEngine.Mathf.Clamp(PlayerPrefsSl.Get("DEBUG_SCPCTRL", 2), 0, 4), "for playeable SCP controller", new global::UnityEngine.Color32(130, 130, 130, byte.MaxValue)),
		["SCP096"] = new ConsoleDebugMode.DebugChannel((DebugLevel)global::UnityEngine.Mathf.Clamp(PlayerPrefsSl.Get("DEBUG_SCP096", 2), 0, 4), "for SCP-096 client and server-side logging", defaultColor),
		["SCP173"] = new ConsoleDebugMode.DebugChannel((DebugLevel)global::UnityEngine.Mathf.Clamp(PlayerPrefsSl.Get("DEBUG_SCP173", 2), 0, 4), "for SCP-173 client and server-side logging", defaultColor),
		["SEARCH"] = new ConsoleDebugMode.DebugChannel((DebugLevel)global::UnityEngine.Mathf.Clamp(PlayerPrefsSl.Get("DEBUG_SEARCH", 2), 0, 4), "for Search System logging", defaultColor)
	};

	public static bool CheckImportance(string key, MessageImportance importance)
	{
		if (debugChannels.TryGetValue(key, out var value))
		{
			return (int)value.Level >= (int)importance;
		}
		return true;
	}

	public static bool CheckImportance(string key, MessageImportance importance, out global::UnityEngine.Color32 color)
	{
		if (debugChannels.TryGetValue(key, out var value))
		{
			color = value.Color;
			return (int)value.Level >= (int)importance;
		}
		color = defaultColor;
		return true;
	}

	public static void GetList(out string[] keys, out string[] descriptions)
	{
		keys = global::System.Linq.Enumerable.ToArray(debugChannels.Keys);
		descriptions = new string[debugChannels.Keys.Count];
		for (int i = 0; i < descriptions.Length; i++)
		{
			descriptions[i] = debugChannels[keys[i]].Description;
		}
	}

	public static bool ChangeImportance(string key, int newLevel)
	{
		if (!debugChannels.ContainsKey(key))
		{
			return false;
		}
		debugChannels[key] = new ConsoleDebugMode.DebugChannel((DebugLevel)newLevel, debugChannels[key].Description, debugChannels[key].Color);
		PlayerPrefsSl.Set("DEBUG_" + key, newLevel);
		return true;
	}

	public static string ConsoleGetLevel(string key)
	{
		key = key.ToUpper();
		if (debugChannels.TryGetValue(key, out var value))
		{
			return "The " + key + " is currently on the <i>" + value.Level.ToString().ToLower() + "</i> debug level.";
		}
		return "Module '" + key + "' could not be found.";
	}
}
