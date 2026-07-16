public static class Misc
{
	public enum IPAddressType
	{
		Unknown = 0,
		IPV4 = 1,
		IPV6 = 2,
		Localhost = 3,
		Hostname = 4
	}

	public enum PlayerInfoColorTypes
	{
		Pink = 0,
		Red = 1,
		Brown = 2,
		Silver = 3,
		LightGreen = 4,
		Crimson = 5,
		Cyan = 6,
		Aqua = 7,
		DeepPink = 8,
		Tomato = 9,
		Yellow = 10,
		Magenta = 11,
		BlueGreen = 12,
		Orange = 13,
		Lime = 14,
		Green = 15,
		Emerald = 16,
		Carmine = 17,
		Nickel = 18,
		Mint = 19,
		ArmyGreen = 20,
		Pumpkin = 21,
		Black = 22,
		White = 23
	}

	public enum CommandOperationMode
	{
		Disable = 0,
		Enable = 1,
		Toggle = 2
	}

	private static readonly global::UnityEngine.Color _raOrange;

	private static readonly global::UnityEngine.Color _darkGreen;

	private static readonly global::System.Collections.Generic.Dictionary<global::System.ConsoleColor, global::UnityEngine.Color> ConsoleColors;

	private static readonly global::System.Text.RegularExpressions.Regex _pbRgx;

	private static readonly global::System.Text.RegularExpressions.Regex _ipV4Rgx;

	private static readonly global::System.Text.RegularExpressions.Regex _ipV6Rgx;

	private static readonly global::System.Text.RegularExpressions.Regex _hostNameRgx;

	public static readonly global::System.Collections.Generic.Dictionary<Misc.PlayerInfoColorTypes, string> AllowedColors;

	private static readonly global::UnityEngine.Color32 _defaultColor;

	static Misc()
	{
		_raOrange = new global::UnityEngine.Color32(byte.MaxValue, 180, 0, byte.MaxValue);
		_darkGreen = new global::UnityEngine.Color32(80, 150, 80, byte.MaxValue);
		ConsoleColors = new global::System.Collections.Generic.Dictionary<global::System.ConsoleColor, global::UnityEngine.Color>();
		_pbRgx = new global::System.Text.RegularExpressions.Regex("^[a-zA-Z0-9]{8}$", global::System.Text.RegularExpressions.RegexOptions.IgnoreCase | global::System.Text.RegularExpressions.RegexOptions.Compiled);
		_ipV4Rgx = new global::System.Text.RegularExpressions.Regex("^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$", global::System.Text.RegularExpressions.RegexOptions.IgnoreCase | global::System.Text.RegularExpressions.RegexOptions.Compiled);
		_ipV6Rgx = new global::System.Text.RegularExpressions.Regex("^((([0-9a-f]{1,4}:){7}([0-9a-f]{1,4}|:))|(([0-9a-f]{1,4}:){6}(:[0-9a-f]{1,4}|((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3})|:))|(([0-9a-f]{1,4}:){5}(((:[0-9a-f]{1,4}){1,2})|:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3})|:))|(([0-9a-f]{1,4}:){4}(((:[0-9a-f]{1,4}){1,3})|((:[0-9a-f]{1,4})?:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:))|(([0-9a-f]{1,4}:){3}(((:[0-9a-f]{1,4}){1,4})|((:[0-9a-f]{1,4}){0,2}:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:))|(([0-9a-f]{1,4}:){2}(((:[0-9a-f]{1,4}){1,5})|((:[0-9a-f]{1,4}){0,3}:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:))|(([0-9a-f]{1,4}:){1}(((:[0-9a-f]{1,4}){1,6})|((:[0-9a-f]{1,4}){0,4}:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:))|(:(((:[0-9a-f]{1,4}){1,7})|((:[0-9a-f]{1,4}){0,5}:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:)))$", global::System.Text.RegularExpressions.RegexOptions.IgnoreCase | global::System.Text.RegularExpressions.RegexOptions.Compiled);
		_hostNameRgx = new global::System.Text.RegularExpressions.Regex("^(([a-z0-9]|[a-z0-9][a-z0-9\\-]*[a-z0-9])\\.)*([a-z0-9]|[a-z0-9][a-z0-9\\-]*[a-z0-9])$", global::System.Text.RegularExpressions.RegexOptions.IgnoreCase | global::System.Text.RegularExpressions.RegexOptions.Compiled);
		AllowedColors = new global::System.Collections.Generic.Dictionary<Misc.PlayerInfoColorTypes, string>
		{
			{
				Misc.PlayerInfoColorTypes.Pink,
				"#FF96DE"
			},
			{
				Misc.PlayerInfoColorTypes.Red,
				"#C50000"
			},
			{
				Misc.PlayerInfoColorTypes.Brown,
				"#944710"
			},
			{
				Misc.PlayerInfoColorTypes.Silver,
				"#A0A0A0"
			},
			{
				Misc.PlayerInfoColorTypes.LightGreen,
				"#32CD32"
			},
			{
				Misc.PlayerInfoColorTypes.Crimson,
				"#DC143C"
			},
			{
				Misc.PlayerInfoColorTypes.Cyan,
				"#00B7EB"
			},
			{
				Misc.PlayerInfoColorTypes.Aqua,
				"#00FFFF"
			},
			{
				Misc.PlayerInfoColorTypes.DeepPink,
				"#FF1493"
			},
			{
				Misc.PlayerInfoColorTypes.Tomato,
				"#FF6448"
			},
			{
				Misc.PlayerInfoColorTypes.Yellow,
				"#FAFF86"
			},
			{
				Misc.PlayerInfoColorTypes.Magenta,
				"#FF0090"
			},
			{
				Misc.PlayerInfoColorTypes.BlueGreen,
				"#4DFFB8"
			},
			{
				Misc.PlayerInfoColorTypes.Orange,
				"#FF9966"
			},
			{
				Misc.PlayerInfoColorTypes.Lime,
				"#BFFF00"
			},
			{
				Misc.PlayerInfoColorTypes.Green,
				"#228B22"
			},
			{
				Misc.PlayerInfoColorTypes.Emerald,
				"#50C878"
			},
			{
				Misc.PlayerInfoColorTypes.Carmine,
				"#960018"
			},
			{
				Misc.PlayerInfoColorTypes.Nickel,
				"#727472"
			},
			{
				Misc.PlayerInfoColorTypes.Mint,
				"#98FB98"
			},
			{
				Misc.PlayerInfoColorTypes.ArmyGreen,
				"#4B5320"
			},
			{
				Misc.PlayerInfoColorTypes.Pumpkin,
				"#EE7600"
			},
			{
				Misc.PlayerInfoColorTypes.Black,
				"#000000"
			},
			{
				Misc.PlayerInfoColorTypes.White,
				"#FFFFFF"
			}
		};
		_defaultColor = global::UnityEngine.Color.white;
		foreach (global::System.ConsoleColor value in global::System.Enum.GetValues(typeof(global::System.ConsoleColor)))
		{
			ConsoleColors.Add(value, ServerConsole.ConsoleColorToColor(value));
		}
	}

	public static bool CheckPermission(this global::CommandSystem.ICommandSender sender, PlayerPermissions[] perms)
	{
		if (sender is CommandSender commandSender)
		{
			return PermissionsHandler.IsPermitted(commandSender.Permissions, perms);
		}
		return false;
	}

	public static bool CheckPermission(this global::CommandSystem.ICommandSender sender, PlayerPermissions perm)
	{
		if (sender is CommandSender commandSender)
		{
			if (!commandSender.FullPermissions)
			{
				return PermissionsHandler.IsPermitted(commandSender.Permissions, perm);
			}
			return true;
		}
		return false;
	}

	public static bool CheckPermission(this global::CommandSystem.ICommandSender sender, PlayerPermissions perm, out string response)
	{
		if (sender.CheckPermission(perm))
		{
			response = null;
			return true;
		}
		response = "You don't have permissions to execute this command.\nRequired permission: " + perm;
		return false;
	}

	public static bool CheckPermission(this global::CommandSystem.ICommandSender sender, PlayerPermissions[] perms, out string response)
	{
		if (sender.CheckPermission(perms))
		{
			response = null;
			return true;
		}
		response = "You don't have permissions to execute this command.\nYou need at least one of following permissions: " + string.Join(", ", perms);
		return false;
	}

	public static string LeadingZeroes(int integer, uint len, bool plusSign = false)
	{
		bool flag = integer < 0;
		if (flag)
		{
			integer *= -1;
		}
		string text = integer.ToString();
		while (text.Length < len)
		{
			text = "0" + text;
		}
		return (flag ? "-" : (plusSign ? "+" : "")) + text;
	}

	public static string LoggedNameFromRefHub(this ReferenceHub me)
	{
		return me.nicknameSync.CombinedName + " (" + me.characterClassManager.UserId + ")";
	}

	public static int LevenshteinDistance(string s, string t)
	{
		int length = s.Length;
		int length2 = t.Length;
		int[,] array = new int[length + 1, length2 + 1];
		if (length == 0)
		{
			return length2;
		}
		if (length2 == 0)
		{
			return length;
		}
		int num = 0;
		while (num <= length)
		{
			array[num, 0] = num++;
		}
		int num2 = 0;
		while (num2 <= length2)
		{
			array[0, num2] = num2++;
		}
		for (int i = 1; i <= length; i++)
		{
			for (int j = 1; j <= length2; j++)
			{
				int num3 = ((t[j - 1] != s[i - 1]) ? 1 : 0);
				array[i, j] = global::System.Math.Min(global::System.Math.Min(array[i - 1, j] + 1, array[i, j - 1] + 1), array[i - 1, j - 1] + num3);
			}
		}
		return array[length, length2];
	}

	public static string LongestCommonSubstring(string a, string b)
	{
		if (a == null || b == null)
		{
			return string.Empty;
		}
		int[,] array = new int[a.Length, b.Length];
		int num = 0;
		string result = "";
		for (int i = 0; i < a.Length; i++)
		{
			for (int j = 0; j < b.Length; j++)
			{
				if (a[i] == b[j])
				{
					array[i, j] = ((i == 0 || j == 0) ? 1 : (array[i - 1, j - 1] + 1));
					if (array[i, j] > num)
					{
						num = array[i, j];
						result = a.Substring(i - num + 1, num);
					}
				}
				else
				{
					array[i, j] = 0;
				}
			}
		}
		return result;
	}

	private static string LongestCommonSubstringOfAInB(string a, string b)
	{
		if (b.Length < a.Length)
		{
			string text = b;
			string text2 = a;
			a = text;
			b = text2;
		}
		for (int num = a.Length; num > 0; num--)
		{
			for (int i = a.Length - num; i <= a.Length - num; i++)
			{
				string text3 = a.Substring(i, num);
				if (b.Contains(text3))
				{
					return text3;
				}
			}
		}
		return string.Empty;
	}

	public static bool ValidatePastebin(string text)
	{
		return _pbRgx.IsMatch(text);
	}

	public static bool ValidateIpOrHostname(string ipOrHost, out Misc.IPAddressType type, bool allowHostname = true, bool allowLocalhost = true)
	{
		if (ipOrHost == "localhost")
		{
			type = Misc.IPAddressType.Localhost;
			return allowLocalhost;
		}
		if (_ipV4Rgx.IsMatch(ipOrHost))
		{
			type = Misc.IPAddressType.IPV4;
			return true;
		}
		if (_ipV6Rgx.IsMatch(ipOrHost))
		{
			type = Misc.IPAddressType.IPV6;
			return true;
		}
		if (_hostNameRgx.IsMatch(ipOrHost))
		{
			type = Misc.IPAddressType.Hostname;
			return allowHostname;
		}
		type = Misc.IPAddressType.Unknown;
		return false;
	}

	public static long RelativeTimeToSeconds(string time, int defaultFactor = 1)
	{
		if (long.TryParse(time, out var result))
		{
			return result * defaultFactor;
		}
		if (time.Length < 2)
		{
			throw new global::System.Exception($"{result} is not a valid time.");
		}
		if (!long.TryParse(time.Substring(0, time.Length - 1), out result))
		{
			throw new global::System.Exception($"{result} is not a valid time.");
		}
		switch (time[time.Length - 1])
		{
		case 'S':
		case 's':
			return result;
		case 'm':
			return result * 60;
		case 'H':
		case 'h':
			return result * 3600;
		case 'D':
		case 'd':
			return result * 86400;
		case 'M':
			return result * 2592000;
		case 'Y':
		case 'y':
			return result * 31536000;
		default:
			throw new global::System.Exception($"{result} is not a valid time.");
		}
	}

	public static global::System.Collections.Generic.List<int> ProcessRaPlayersList(string playerIds)
	{
		try
		{
			global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
			string[] source = playerIds.Split('.');
			list.AddRange(global::System.Linq.Enumerable.Select(global::System.Linq.Enumerable.Where(source, (string item) => !string.IsNullOrEmpty(item)), int.Parse));
			return list;
		}
		catch (global::System.Exception exception)
		{
			global::UnityEngine.Debug.LogException(exception);
			return null;
		}
	}

	public static string GetRuntimeVersion()
	{
		try
		{
			return global::System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
		}
		catch
		{
			return "Not supported!";
		}
	}

	public static global::UnityEngine.AudioType GetAudioType(string path)
	{
		switch (global::System.IO.Path.GetExtension(path))
		{
		case ".ogg":
			return global::UnityEngine.AudioType.OGGVORBIS;
		case ".wav":
			return global::UnityEngine.AudioType.WAV;
		case ".aac":
			return global::UnityEngine.AudioType.ACC;
		case ".aiff":
			return global::UnityEngine.AudioType.AIFF;
		case ".mod":
			return global::UnityEngine.AudioType.MOD;
		case ".mp3":
		case ".mp2":
		case ".mpeg":
			return global::UnityEngine.AudioType.MPEG;
		default:
			return global::UnityEngine.AudioType.UNKNOWN;
		}
	}

	public static bool CultureInfoTryParse(string name, out global::System.Globalization.CultureInfo info)
	{
		try
		{
			info = global::System.Globalization.CultureInfo.GetCultureInfo(name);
			return true;
		}
		catch
		{
			info = null;
			return false;
		}
	}

	public static string ToHex(this global::UnityEngine.Color color)
	{
		return ((global::UnityEngine.Color32)color).ToHex();
	}

	public static string ToHex(this global::UnityEngine.Color32 color)
	{
		return "#" + color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2") + color.a.ToString("X2");
	}

	public static bool TryParseColor(string input, out global::UnityEngine.Color32 color)
	{
		if (string.IsNullOrEmpty(input))
		{
			color = _defaultColor;
			return false;
		}
		switch (input)
		{
		case "red":
			color = new global::UnityEngine.Color32(byte.MaxValue, 0, 0, byte.MaxValue);
			return true;
		case "cyan":
			color = new global::UnityEngine.Color32(0, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			return true;
		case "blue":
			color = new global::UnityEngine.Color32(0, 0, byte.MaxValue, byte.MaxValue);
			return true;
		case "darkblue":
			color = new global::UnityEngine.Color32(0, 0, 139, byte.MaxValue);
			return true;
		case "lightblue":
			color = new global::UnityEngine.Color32(173, 216, 230, byte.MaxValue);
			return true;
		case "purple":
			color = new global::UnityEngine.Color32(128, 0, 128, byte.MaxValue);
			return true;
		case "yellow":
			color = new global::UnityEngine.Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
			return true;
		case "lime":
			color = new global::UnityEngine.Color32(0, byte.MaxValue, 0, byte.MaxValue);
			return true;
		case "fuchsia":
			color = new global::UnityEngine.Color32(byte.MaxValue, 0, byte.MaxValue, byte.MaxValue);
			return true;
		case "white":
			color = new global::UnityEngine.Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			return true;
		case "silver":
			color = new global::UnityEngine.Color32(192, 192, 192, byte.MaxValue);
			return true;
		case "gray":
		case "grey":
			color = new global::UnityEngine.Color32(128, 128, 128, byte.MaxValue);
			return true;
		case "black":
			color = new global::UnityEngine.Color32(0, 0, 0, byte.MaxValue);
			return true;
		case "orange":
			color = new global::UnityEngine.Color32(byte.MaxValue, 165, 0, byte.MaxValue);
			return true;
		case "brown":
			color = new global::UnityEngine.Color32(165, 42, 42, byte.MaxValue);
			return true;
		case "maroon":
			color = new global::UnityEngine.Color32(128, 0, 0, byte.MaxValue);
			return true;
		case "green":
			color = new global::UnityEngine.Color32(0, 128, 0, byte.MaxValue);
			return true;
		case "olive":
			color = new global::UnityEngine.Color32(128, 128, 0, byte.MaxValue);
			return true;
		case "navy":
			color = new global::UnityEngine.Color32(0, 0, 128, byte.MaxValue);
			return true;
		case "teal":
			color = new global::UnityEngine.Color32(0, 128, 128, byte.MaxValue);
			return true;
		case "aqua":
			color = new global::UnityEngine.Color32(0, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			return true;
		case "magenta":
			color = new global::UnityEngine.Color32(byte.MaxValue, 0, byte.MaxValue, byte.MaxValue);
			return true;
		default:
			if (input.StartsWith("#"))
			{
				input = input.Substring(1);
			}
			if (input.Length >= 6)
			{
				if (!byte.TryParse(input.Substring(0, 2), global::System.Globalization.NumberStyles.HexNumber, null, out var result))
				{
					color = _defaultColor;
					return false;
				}
				if (!byte.TryParse(input.Substring(2, 2), global::System.Globalization.NumberStyles.HexNumber, null, out var result2))
				{
					color = _defaultColor;
					return false;
				}
				if (!byte.TryParse(input.Substring(4, 2), global::System.Globalization.NumberStyles.HexNumber, null, out var result3))
				{
					color = _defaultColor;
					return false;
				}
				byte a = byte.MaxValue;
				if (input.Length >= 8)
				{
					if (!byte.TryParse(input.Substring(6, 2), global::System.Globalization.NumberStyles.HexNumber, null, out var result4))
					{
						color = _defaultColor;
						return false;
					}
					a = result4;
				}
				color = new global::UnityEngine.Color32(result, result2, result3, a);
				return true;
			}
			color = _defaultColor;
			return false;
		}
	}

	public static global::System.ConsoleColor ClosestConsoleColor(global::UnityEngine.Color color, bool excludeDark = true)
	{
		if (color == global::UnityEngine.Color.green)
		{
			return global::System.ConsoleColor.Green;
		}
		if (color == global::UnityEngine.Color.red)
		{
			return global::System.ConsoleColor.Red;
		}
		if (color == global::UnityEngine.Color.gray || color == global::UnityEngine.Color.white || color == global::UnityEngine.Color.black)
		{
			return global::System.ConsoleColor.White;
		}
		if (color == global::UnityEngine.Color.magenta)
		{
			return global::System.ConsoleColor.Magenta;
		}
		if (color == global::UnityEngine.Color.yellow)
		{
			return global::System.ConsoleColor.Yellow;
		}
		if (color == _raOrange)
		{
			return global::System.ConsoleColor.DarkYellow;
		}
		if (color == _darkGreen)
		{
			return global::System.ConsoleColor.DarkGreen;
		}
		global::System.ConsoleColor consoleColor = global::System.ConsoleColor.White;
		double num = color.r;
		double num2 = color.g;
		double num3 = color.b;
		double num4 = double.MaxValue;
		foreach (global::System.Collections.Generic.KeyValuePair<global::System.ConsoleColor, global::UnityEngine.Color> consoleColor2 in ConsoleColors)
		{
			global::UnityEngine.Color value = consoleColor2.Value;
			double num5 = global::System.Math.Pow((double)value.r - num, 2.0) + global::System.Math.Pow((double)value.g - num2, 2.0) + global::System.Math.Pow((double)value.b - num3, 2.0);
			if (num5 == 0.0)
			{
				return consoleColor2.Key;
			}
			if (num5 < num4)
			{
				num4 = num5;
				consoleColor = consoleColor2.Key;
			}
		}
		if ((consoleColor == global::System.ConsoleColor.Black || consoleColor == global::System.ConsoleColor.Gray || consoleColor == global::System.ConsoleColor.DarkGray) && excludeDark)
		{
			return global::System.ConsoleColor.White;
		}
		return consoleColor;
	}

	public static void WriteBoolByte(this global::Mirror.NetworkWriter writer, bool bool1 = false, bool bool2 = false, bool bool3 = false, bool bool4 = false, bool bool5 = false, bool bool6 = false, bool bool7 = false, bool bool8 = false)
	{
		byte b = 0;
		if (bool1)
		{
			b |= 1;
		}
		if (bool2)
		{
			b |= 2;
		}
		if (bool3)
		{
			b |= 4;
		}
		if (bool4)
		{
			b |= 8;
		}
		if (bool5)
		{
			b |= 0x10;
		}
		if (bool6)
		{
			b |= 0x20;
		}
		if (bool7)
		{
			b |= 0x40;
		}
		if (bool8)
		{
			b |= 0x80;
		}
		writer.WriteByte(b);
	}

	public static void ReadBoolByte(this global::Mirror.NetworkReader reader, out bool bool1, out bool bool2, out bool bool3, out bool bool4, out bool bool5, out bool bool6, out bool bool7, out bool bool8)
	{
		byte b = reader.ReadByte();
		bool1 = (b & 1) == 1;
		bool2 = (b & 2) == 2;
		bool3 = (b & 4) == 4;
		bool4 = (b & 8) == 8;
		bool5 = (b & 0x10) == 16;
		bool6 = (b & 0x20) == 32;
		bool7 = (b & 0x40) == 64;
		bool8 = (b & 0x80) == 128;
	}

	public static byte BoolsToByte(bool bool1 = false, bool bool2 = false, bool bool3 = false, bool bool4 = false, bool bool5 = false, bool bool6 = false, bool bool7 = false, bool bool8 = false)
	{
		byte b = 0;
		if (bool1)
		{
			b |= 1;
		}
		if (bool2)
		{
			b |= 2;
		}
		if (bool3)
		{
			b |= 4;
		}
		if (bool4)
		{
			b |= 8;
		}
		if (bool5)
		{
			b |= 0x10;
		}
		if (bool6)
		{
			b |= 0x20;
		}
		if (bool7)
		{
			b |= 0x40;
		}
		if (bool8)
		{
			b |= 0x80;
		}
		return b;
	}

	public static void ByteToBools(byte b, out bool bool1, out bool bool2, out bool bool3, out bool bool4, out bool bool5, out bool bool6, out bool bool7, out bool bool8)
	{
		bool1 = (b & 1) == 1;
		bool2 = (b & 2) == 2;
		bool3 = (b & 4) == 4;
		bool4 = (b & 8) == 8;
		bool5 = (b & 0x10) == 16;
		bool6 = (b & 0x20) == 32;
		bool7 = (b & 0x40) == 64;
		bool8 = (b & 0x80) == 128;
	}

	public unsafe static int GetBytes(this global::System.Text.Encoding encoding, string text, global::NorthwoodLib.NativeMemory memory)
	{
		fixed (char* chars = text)
		{
			return encoding.GetBytes(chars, text.Length, memory.ToPointer<byte>(), memory.Length);
		}
	}

	public static bool IsSafeCharacter(char c)
	{
		if (c > '\u001f')
		{
			return c < '\u007f';
		}
		return false;
	}

	public static void ReplaceUnsafeCharacters(ref string text, char replaceCharacter = '?')
	{
		if (text == null)
		{
			text = string.Empty;
			return;
		}
		global::System.Text.StringBuilder stringBuilder = global::NorthwoodLib.Pools.StringBuilderPool.Shared.Rent(text.Length);
		for (int i = 0; i < text.Length; i++)
		{
			stringBuilder.Append(IsSafeCharacter(text[i]) ? text[i] : replaceCharacter);
		}
		text = stringBuilder.ToString();
		global::NorthwoodLib.Pools.StringBuilderPool.Shared.Return(stringBuilder);
	}

	public static global::System.Random CreateRandom()
	{
		byte[] array = new byte[4];
		using (global::System.Security.Cryptography.RNGCryptoServiceProvider rNGCryptoServiceProvider = new global::System.Security.Cryptography.RNGCryptoServiceProvider())
		{
			rNGCryptoServiceProvider.GetBytes(array);
		}
		return new global::System.Random(global::System.BitConverter.ToInt32(array, 0));
	}

	public static global::UnityEngine.Vector3 NormalizeIgnoreY(this global::UnityEngine.Vector3 v)
	{
		v.y = 0f;
		return v.normalized;
	}

	public static global::UnityEngine.Vector3 Abs(this global::UnityEngine.Vector3 v)
	{
		return new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Abs(v.x), global::UnityEngine.Mathf.Abs(v.y), global::UnityEngine.Mathf.Abs(v.z));
	}

	public static float SqrMagnitudeIgnoreY(this global::UnityEngine.Vector3 v)
	{
		return (float)((double)v.x * (double)v.x + (double)v.z * (double)v.z);
	}

	public static float MagnitudeIgnoreY(this global::UnityEngine.Vector3 v)
	{
		return (float)global::System.Math.Sqrt((double)v.x * (double)v.x + (double)v.z * (double)v.z);
	}

	public static float MagnitudeOnlyY(this global::UnityEngine.Vector3 v)
	{
		return global::System.Math.Abs(v.y);
	}

	public static string ToPreciseString(this global::UnityEngine.Vector2 v)
	{
		return $"[{v.x:F3}, {v.y:F3}]";
	}

	public static string ToPreciseString(this global::UnityEngine.Vector3 v)
	{
		return $"[{v.x:F3}, {v.y:F3}, {v.z:F3}]";
	}

	public static float AngleIgnoreY(global::UnityEngine.Vector3 from, global::UnityEngine.Vector3 to)
	{
		to.y = from.y;
		float num = (float)global::System.Math.Sqrt((double)from.SqrMagnitudeIgnoreY() * (double)to.SqrMagnitudeIgnoreY());
		if (!((double)num < 1.00000000362749E-15))
		{
			return (float)global::System.Math.Acos(global::UnityEngine.Mathf.Clamp(global::UnityEngine.Vector3.Dot(from, to) / num, -1f, 1f)) * 57.29578f;
		}
		return 0f;
	}

	public static bool TryCommandModeFromArgs(ref string[] newargs, out Misc.CommandOperationMode mode)
	{
		if (newargs != null && newargs.Length != 0)
		{
			switch (newargs[0].ToLowerInvariant())
			{
			case "1":
			case "true":
			case "enable":
			case "on":
				mode = Misc.CommandOperationMode.Enable;
				return true;
			case "0":
			case "false":
			case "disable":
			case "off":
				mode = Misc.CommandOperationMode.Disable;
				return true;
			default:
				mode = Misc.CommandOperationMode.Toggle;
				return false;
			}
		}
		mode = Misc.CommandOperationMode.Toggle;
		return true;
	}

	public static string RemoveStacktraceZeroes(string stacktrace)
	{
		return stacktrace.Replace(" [0x00000] in <00000000000000000000000000000000>:0", "");
	}
}
