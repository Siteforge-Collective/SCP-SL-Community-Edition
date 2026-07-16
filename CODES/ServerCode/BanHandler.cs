public static class BanHandler
{
	public enum BanType
	{
		NULL = -1,
		UserId = 0,
		IP = 1
	}

	public static BanHandler.BanType GetBanType(int type)
	{
		if (type > global::System.Linq.Enumerable.Max(global::System.Linq.Enumerable.Cast<int>(global::System.Enum.GetValues(typeof(BanHandler.BanType)))) || type < global::System.Linq.Enumerable.Min(global::System.Linq.Enumerable.Cast<int>(global::System.Enum.GetValues(typeof(BanHandler.BanType)))))
		{
			return BanHandler.BanType.UserId;
		}
		return (BanHandler.BanType)type;
	}

	public static void Init()
	{
		try
		{
			if (!global::System.IO.File.Exists(GetPath(BanHandler.BanType.UserId)))
			{
				global::System.IO.File.Create(GetPath(BanHandler.BanType.UserId)).Close();
			}
			else
			{
				FileManager.RemoveEmptyLines(GetPath(BanHandler.BanType.UserId));
			}
			if (!global::System.IO.File.Exists(GetPath(BanHandler.BanType.IP)))
			{
				global::System.IO.File.Create(GetPath(BanHandler.BanType.IP)).Close();
			}
			else
			{
				FileManager.RemoveEmptyLines(GetPath(BanHandler.BanType.IP));
			}
		}
		catch
		{
			ServerConsole.AddLog("Can't create ban files!");
		}
		ValidateBans();
	}

	public static bool IssueBan(BanDetails ban, BanHandler.BanType banType, bool forced = false)
	{
		try
		{
			if (banType == BanHandler.BanType.IP && ban.Id.Equals("localClient", global::System.StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			ban.OriginalName = ban.OriginalName.Replace(";", ":");
			ban.Issuer = ban.Issuer.Replace(";", ":");
			ban.Reason = ban.Reason.Replace(";", ":");
			Misc.ReplaceUnsafeCharacters(ref ban.OriginalName);
			Misc.ReplaceUnsafeCharacters(ref ban.Issuer);
			if (!global::System.Linq.Enumerable.Any(GetBans(banType), (BanDetails b) => b.Id == ban.Id))
			{
				if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.BanIssued, ban, banType) && !forced)
				{
					return false;
				}
				FileManager.AppendFile(ban.ToString(), GetPath(banType));
				FileManager.RemoveEmptyLines(GetPath(banType));
			}
			else
			{
				if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.BanUpdated, ban, banType) && !forced)
				{
					return false;
				}
				RemoveBan(ban.Id, banType, forced: true);
				IssueBan(ban, banType, forced: true);
			}
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static void ValidateBans()
	{
		ServerConsole.AddLog("Validating bans...");
		ValidateBans(BanHandler.BanType.UserId);
		ValidateBans(BanHandler.BanType.IP);
		ServerConsole.AddLog("Bans has been validated.");
	}

	public static void ValidateBans(BanHandler.BanType banType)
	{
		global::System.Collections.Generic.List<string> list = FileManager.ReadAllLinesList(GetPath(banType));
		global::System.Collections.Generic.List<int> list2 = global::NorthwoodLib.Pools.ListPool<int>.Shared.Rent();
		for (int num = list.Count - 1; num >= 0; num--)
		{
			string ban = list[num];
			if (ProcessBanItem(ban, banType) == null || !CheckExpiration(ProcessBanItem(ban, banType), BanHandler.BanType.NULL))
			{
				list2.Add(num);
			}
		}
		global::System.Collections.Generic.List<int> list3 = global::NorthwoodLib.Pools.ListPool<int>.Shared.Rent();
		foreach (int item in list2)
		{
			if (!list3.Contains(item))
			{
				list3.Add(item);
			}
		}
		global::NorthwoodLib.Pools.ListPool<int>.Shared.Return(list2);
		foreach (int item2 in global::System.Linq.Enumerable.OrderByDescending(list3, (int index) => index))
		{
			list.RemoveAt(item2);
		}
		global::NorthwoodLib.Pools.ListPool<int>.Shared.Return(list3);
		if (FileManager.ReadAllLines(GetPath(banType)) != list.ToArray())
		{
			FileManager.WriteToFile(list.ToArray(), GetPath(banType));
		}
	}

	public static bool CheckExpiration(BanDetails ban, BanHandler.BanType banType)
	{
		if (ban == null)
		{
			return false;
		}
		if (TimeBehaviour.ValidateTimestamp(ban.Expires, TimeBehaviour.CurrentTimestamp(), 0L))
		{
			return true;
		}
		if (banType >= BanHandler.BanType.UserId)
		{
			RemoveBan(ban.Id, banType, forced: true);
		}
		return false;
	}

	public static BanDetails ReturnChecks(BanDetails ban, BanHandler.BanType banType)
	{
		if (!CheckExpiration(ban, banType))
		{
			return null;
		}
		return ban;
	}

	public static void RemoveBan(string id, BanHandler.BanType banType, bool forced = false)
	{
		if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.BanRevoked, id, banType) || forced)
		{
			id = id.Replace(";", ":").Replace(global::System.Environment.NewLine, "").Replace("\n", "");
			FileManager.WriteToFile(global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Where(FileManager.ReadAllLines(GetPath(banType)), (string l) => ProcessBanItem(l, banType) != null && ProcessBanItem(l, banType).Id != id)), GetPath(banType));
		}
	}

	public static global::System.Collections.Generic.List<BanDetails> GetBans(BanHandler.BanType banType)
	{
		return global::System.Linq.Enumerable.ToList(global::System.Linq.Enumerable.Where(global::System.Linq.Enumerable.Select(FileManager.ReadAllLines(GetPath(banType)), (string b) => ProcessBanItem(b, banType)), (BanDetails b) => b != null));
	}

	public static BanDetails GetBan(string id, BanHandler.BanType banType)
	{
		return global::System.Linq.Enumerable.FirstOrDefault(GetBans(banType), (BanDetails s) => string.Equals(s.Id, id, global::System.StringComparison.OrdinalIgnoreCase));
	}

	public static global::System.Collections.Generic.KeyValuePair<BanDetails, BanDetails> QueryBan(string userId, string ip)
	{
		string ban = null;
		string ban2 = null;
		if (!string.IsNullOrEmpty(userId))
		{
			userId = userId.Replace(";", ":").Replace(global::System.Environment.NewLine, "").Replace("\n", "");
			ban = global::System.Linq.Enumerable.FirstOrDefault(FileManager.ReadAllLines(GetPath(BanHandler.BanType.UserId)), (string b) => ProcessBanItem(b, BanHandler.BanType.UserId)?.Id == userId);
		}
		if (!string.IsNullOrEmpty(ip))
		{
			ip = ip.Replace(";", ":").Replace(global::System.Environment.NewLine, "").Replace("\n", "");
			ban2 = global::System.Linq.Enumerable.FirstOrDefault(FileManager.ReadAllLines(GetPath(BanHandler.BanType.IP)), (string b) => ProcessBanItem(b, BanHandler.BanType.IP)?.Id == ip);
		}
		return new global::System.Collections.Generic.KeyValuePair<BanDetails, BanDetails>(ReturnChecks(ProcessBanItem(ban, BanHandler.BanType.UserId), BanHandler.BanType.UserId), ReturnChecks(ProcessBanItem(ban2, BanHandler.BanType.IP), BanHandler.BanType.IP));
	}

	public static BanDetails ProcessBanItem(string ban, BanHandler.BanType banType)
	{
		if (string.IsNullOrEmpty(ban) || !ban.Contains(";"))
		{
			return null;
		}
		string[] array = ban.Split(';');
		if (array.Length != 6)
		{
			return null;
		}
		if (banType == BanHandler.BanType.UserId && !array[1].Contains("@"))
		{
			array[1] = array[1].Trim() + "@steam";
		}
		return new BanDetails
		{
			OriginalName = array[0],
			Id = array[1].Trim(),
			Expires = global::System.Convert.ToInt64(array[2].Trim()),
			Reason = array[3],
			Issuer = array[4],
			IssuanceTime = global::System.Convert.ToInt64(array[5].Trim())
		};
	}

	public static string GetPath(BanHandler.BanType banType)
	{
		if (banType != BanHandler.BanType.UserId && banType == BanHandler.BanType.IP)
		{
			return global::GameCore.ConfigSharing.Paths[0] + "IpBans.txt";
		}
		return global::GameCore.ConfigSharing.Paths[0] + "UserIdBans.txt";
	}
}
