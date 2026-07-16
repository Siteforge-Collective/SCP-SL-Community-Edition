namespace PlayerRoles.RoleAssign
{
	public static class ScpTicketsLoader
	{
		public static readonly global::System.Collections.Generic.List<global::PlayerRoles.RoleAssign.TicketEntry> AllEntries = new global::System.Collections.Generic.List<global::PlayerRoles.RoleAssign.TicketEntry>(65535);

		private const string Filename = "ScpTickets.txt";

		private const int ExpectedCapacity = 65535;

		private const int ExpirationTime = 1296000;

		private const char Separator = ';';

		private static string FilePath => global::GameCore.ConfigSharing.Paths[6] + "ScpTickets.txt";

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			global::GameCore.ConfigFile.OnConfigReloaded = (global::System.Action)global::System.Delegate.Combine(global::GameCore.ConfigFile.OnConfigReloaded, new global::System.Action(LoadEntries));
			Shutdown.OnQuit += SaveEntries;
			global::RoundRestarting.RoundRestart.OnRestartTriggered += SaveEntries;
		}

		private static void SaveEntries()
		{
			FileManager.WriteToFile(global::System.Linq.Enumerable.Select(AllEntries, SerializeEntry), FilePath);
		}

		private static void LoadEntries()
		{
			AllEntries.Clear();
			if (!global::System.IO.File.Exists(FilePath))
			{
				return;
			}
			long num = global::System.DateTimeOffset.Now.ToUnixTimeSeconds();
			using (global::System.IO.FileStream stream = new global::System.IO.FileStream(FilePath, global::System.IO.FileMode.Open, global::System.IO.FileAccess.Read, global::System.IO.FileShare.Read))
			{
				using (global::System.IO.StreamReader streamReader = new global::System.IO.StreamReader(stream))
				{
					while (true)
					{
						string text = streamReader.ReadLine();
						if (text == null)
						{
							break;
						}
						if (TryDeserialize(text, out var entry) && entry.LastUpdate + 1296000 >= num)
						{
							AllEntries.Add(entry);
						}
					}
				}
			}
		}

		private static string SerializeEntry(global::PlayerRoles.RoleAssign.TicketEntry entry)
		{
			return entry.UserId + ';' + entry.Tickets + ';' + entry.LastUpdate;
		}

		private static bool TryDeserialize(string text, out global::PlayerRoles.RoleAssign.TicketEntry entry)
		{
			entry = null;
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			string[] array = text.Split(';');
			if (array.Length != 3)
			{
				return false;
			}
			if (!int.TryParse(array[1], out var result))
			{
				return false;
			}
			if (!long.TryParse(array[2], out var result2))
			{
				return false;
			}
			entry = new global::PlayerRoles.RoleAssign.TicketEntry
			{
				UserId = array[0],
				Tickets = result,
				LastUpdate = result2
			};
			return true;
		}
	}
}
