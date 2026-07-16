public static class ReservedSlot
{
	internal static readonly global::System.Collections.Generic.HashSet<string> Users;

	static ReservedSlot()
	{
		Users = new global::System.Collections.Generic.HashSet<string>();
		Reload();
	}

	public static void Reload()
	{
		string path = global::GameCore.ConfigSharing.Paths[3] + "UserIDReservedSlots.txt";
		Users.Clear();
		if (!global::System.IO.File.Exists(path))
		{
			FileManager.WriteStringToFile("#Put one UserID (eg. 76561198071934271@steam or 274613382353518592@discord) per line. Lines prefixed with \"#\" are ignored.", path);
			return;
		}
		using (global::System.IO.StreamReader streamReader = new global::System.IO.StreamReader(path))
		{
			string text;
			while ((text = streamReader.ReadLine()) != null)
			{
				if (!string.IsNullOrWhiteSpace(text) && !text.TrimStart().StartsWith("#") && text.Contains("@"))
				{
					Users.Add(text.Trim());
				}
			}
		}
		ServerConsole.AddLog("Reserved slots list has been loaded.");
	}

	public static bool HasReservedSlot(string userId, out bool bypass)
	{
		bool flag = Users.Contains(userId.Trim()) || !CharacterClassManager.OnlineMode;
		global::PluginAPI.Events.PlayerCheckReservedSlotCancellationData playerCheckReservedSlotCancellationData = global::PluginAPI.Events.EventManager.ExecuteEvent<global::PluginAPI.Events.PlayerCheckReservedSlotCancellationData>(global::PluginAPI.Enums.ServerEventType.PlayerCheckReservedSlot, new object[2] { userId, flag });
		bypass = playerCheckReservedSlotCancellationData.BypassReservedSlotsLimit;
		if (!playerCheckReservedSlotCancellationData.IsCancelled)
		{
			return flag;
		}
		return playerCheckReservedSlotCancellationData.HasReservedSlot;
	}
}
