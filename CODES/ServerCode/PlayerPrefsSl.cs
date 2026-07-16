public static class PlayerPrefsSl
{
	public enum DataType : byte
	{
		Bool = 0,
		Byte = 1,
		Sbyte = 2,
		Char = 3,
		Decimal = 4,
		Double = 5,
		Float = 6,
		Int = 7,
		Uint = 8,
		Long = 9,
		Ulong = 10,
		Short = 11,
		Ushort = 12,
		String = 13,
		BoolArray = 14,
		ByteArray = 15,
		SbyteArray = 16,
		CharArray = 17,
		DecimalArray = 18,
		DoubleArray = 19,
		FloatArray = 20,
		IntArray = 21,
		UintArray = 22,
		LongArray = 23,
		UlongArray = 24,
		ShortArray = 25,
		UshortArray = 26,
		StringArray = 27
	}

	private const string ArraySeparator = ";;`'.+=;;";

	private const string KeySeparator = "::-%(|::";

	private static readonly global::System.Collections.Generic.Dictionary<string, string> _registry;

	private static readonly string _path;

	private static readonly global::System.Text.UTF8Encoding Encoding;

	public static event global::System.Action<string, string> SettingChanged;

	public static event global::System.Action<string> SettingRemoved;

	public static event global::System.Action SettingsRefreshed;

	static PlayerPrefsSl()
	{
		_registry = new global::System.Collections.Generic.Dictionary<string, string>();
		Encoding = new global::System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
		_path = FileManager.GetAppFolder() + "registry.txt";
		Refresh();
	}

	private static string Prefix(string key, PlayerPrefsSl.DataType type)
	{
		byte b = (byte)type;
		return b.ToString("00") + key;
	}

	public static void Refresh()
	{
		_registry.Clear();
		if (!global::System.IO.File.Exists(_path))
		{
			global::System.IO.File.Create(_path).Close();
			return;
		}
		using (global::System.IO.StreamReader streamReader = new global::System.IO.StreamReader(_path))
		{
			string text;
			while ((text = streamReader.ReadLine()) != null)
			{
				if (text.Contains("::-%(|::"))
				{
					int num = text.IndexOf("::-%(|::", global::System.StringComparison.Ordinal);
					_registry.Add(text.Substring(0, num), text.Substring(num + "::-%(|::".Length));
				}
			}
		}
		PlayerPrefsSl.SettingsRefreshed?.Invoke();
	}

	private static string Serialize()
	{
		global::System.Text.StringBuilder stringBuilder = global::NorthwoodLib.Pools.StringBuilderPool.Shared.Rent();
		foreach (global::System.Collections.Generic.KeyValuePair<string, string> item in _registry)
		{
			stringBuilder.Append(item.Key);
			stringBuilder.Append("::-%(|::");
			stringBuilder.Append(item.Value);
			stringBuilder.AppendLine();
		}
		string result = stringBuilder.ToString();
		global::NorthwoodLib.Pools.StringBuilderPool.Shared.Return(stringBuilder);
		return result;
	}

	private static void Save()
	{
		global::System.IO.File.WriteAllText(_path, Serialize(), Encoding);
	}

	public static bool HasKey(string key, PlayerPrefsSl.DataType type)
	{
		return _registry.ContainsKey(Prefix(key, type));
	}

	public static void DeleteKey(string key, PlayerPrefsSl.DataType type)
	{
		_registry.Remove(Prefix(key, type));
		Save();
		PlayerPrefsSl.SettingRemoved?.Invoke(key);
	}

	public static bool TryGetKey(string key, PlayerPrefsSl.DataType type, out string value)
	{
		return _registry.TryGetValue(Prefix(key, type), out value);
	}

	public static void SetKey(string key, PlayerPrefsSl.DataType type, string value)
	{
		WriteString(Prefix(key, type), value);
	}

	public static void DeleteAll()
	{
		_registry.Clear();
		global::System.IO.File.WriteAllText(_path, "", Encoding);
	}

	private static void WriteString(string key, string value)
	{
		_registry[key] = value;
		Save();
		PlayerPrefsSl.SettingChanged?.Invoke(key, value);
	}

	public static void Set(string key, bool value)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.Bool), value ? "true" : "false");
	}

	public static void Set(string key, byte value)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.Byte), value.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
	}

	public static void Set(string key, sbyte value)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.Sbyte), value.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
	}

	public static void Set(string key, char value)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.Char), value.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
	}

	public static void Set(string key, decimal value)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.Decimal), value.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
	}

	public static void Set(string key, double value)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.Double), value.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
	}

	public static void Set(string key, float value)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.Float), value.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
	}

	public static void Set(string key, int value)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.Int), value.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
	}

	public static void Set(string key, uint value)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.Uint), value.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
	}

	public static void Set(string key, long value)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.Long), value.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
	}

	public static void Set(string key, ulong value)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.Ulong), value.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
	}

	public static void Set(string key, short value)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.Short), value.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
	}

	public static void Set(string key, ushort value)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.Ushort), value.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
	}

	public static void Set(string key, string value)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.String), value);
	}

	public static void Set(string key, bool[] array)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.BoolArray), string.Join(";;`'.+=;;", array));
	}

	public static void Set(string key, global::System.Collections.Generic.IEnumerable<bool> ienumerable)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.BoolArray), string.Join(";;`'.+=;;", ienumerable));
	}

	public static void Set(string key, byte[] array)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.ByteArray), string.Join(";;`'.+=;;", array));
	}

	public static void Set(string key, global::System.Collections.Generic.IEnumerable<byte> ienumerable)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.ByteArray), string.Join(";;`'.+=;;", ienumerable));
	}

	public static void Set(string key, sbyte[] array)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.SbyteArray), string.Join(";;`'.+=;;", array));
	}

	public static void Set(string key, global::System.Collections.Generic.IEnumerable<sbyte> ienumerable)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.SbyteArray), string.Join(";;`'.+=;;", ienumerable));
	}

	public static void Set(string key, char[] array)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.CharArray), string.Join(";;`'.+=;;", array));
	}

	public static void Set(string key, global::System.Collections.Generic.IEnumerable<char> ienumerable)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.CharArray), string.Join(";;`'.+=;;", ienumerable));
	}

	public static void Set(string key, decimal[] array)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.DecimalArray), string.Join(";;`'.+=;;", array));
	}

	public static void Set(string key, global::System.Collections.Generic.IEnumerable<decimal> ienumerable)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.DecimalArray), string.Join(";;`'.+=;;", ienumerable));
	}

	public static void Set(string key, double[] array)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.DoubleArray), string.Join(";;`'.+=;;", array));
	}

	public static void Set(string key, global::System.Collections.Generic.IEnumerable<double> ienumerable)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.DoubleArray), string.Join(";;`'.+=;;", ienumerable));
	}

	public static void Set(string key, float[] array)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.FloatArray), string.Join(";;`'.+=;;", array));
	}

	public static void Set(string key, global::System.Collections.Generic.IEnumerable<float> ienumerable)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.FloatArray), string.Join(";;`'.+=;;", ienumerable));
	}

	public static void Set(string key, int[] array)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.IntArray), string.Join(";;`'.+=;;", array));
	}

	public static void Set(string key, global::System.Collections.Generic.IEnumerable<int> ienumerable)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.IntArray), string.Join(";;`'.+=;;", ienumerable));
	}

	public static void Set(string key, uint[] array)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.UintArray), string.Join(";;`'.+=;;", array));
	}

	public static void Set(string key, global::System.Collections.Generic.IEnumerable<uint> ienumerable)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.UintArray), string.Join(";;`'.+=;;", ienumerable));
	}

	public static void Set(string key, long[] array)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.LongArray), string.Join(";;`'.+=;;", array));
	}

	public static void Set(string key, global::System.Collections.Generic.IEnumerable<long> ienumerable)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.LongArray), string.Join(";;`'.+=;;", ienumerable));
	}

	public static void Set(string key, ulong[] array)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.UlongArray), string.Join(";;`'.+=;;", array));
	}

	public static void Set(string key, global::System.Collections.Generic.IEnumerable<ulong> ienumerable)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.UlongArray), string.Join(";;`'.+=;;", ienumerable));
	}

	public static void Set(string key, short[] array)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.ShortArray), string.Join(";;`'.+=;;", array));
	}

	public static void Set(string key, global::System.Collections.Generic.IEnumerable<short> ienumerable)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.ShortArray), string.Join(";;`'.+=;;", ienumerable));
	}

	public static void Set(string key, ushort[] array)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.UshortArray), string.Join(";;`'.+=;;", array));
	}

	public static void Set(string key, global::System.Collections.Generic.IEnumerable<ushort> ienumerable)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.UshortArray), string.Join(";;`'.+=;;", ienumerable));
	}

	public static void Set(string key, string[] array)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.StringArray), string.Join(";;`'.+=;;", array));
	}

	public static void Set(string key, global::System.Collections.Generic.IEnumerable<string> ienumerable)
	{
		WriteString(Prefix(key, PlayerPrefsSl.DataType.StringArray), string.Join(";;`'.+=;;", ienumerable));
	}

	public static bool Get(string key, bool defaultValue)
	{
		if (!_registry.TryGetValue(Prefix(key, PlayerPrefsSl.DataType.Bool), out var value) || !bool.TryParse(value, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public static byte Get(string key, byte defaultValue)
	{
		if (!_registry.TryGetValue(Prefix(key, PlayerPrefsSl.DataType.Byte), out var value) || !byte.TryParse(value, global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public static sbyte Get(string key, sbyte defaultValue)
	{
		if (!_registry.TryGetValue(Prefix(key, PlayerPrefsSl.DataType.Sbyte), out var value) || !sbyte.TryParse(value, global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public static char Get(string key, char defaultValue)
	{
		if (!_registry.TryGetValue(Prefix(key, PlayerPrefsSl.DataType.Char), out var value) || !char.TryParse(value, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public static decimal Get(string key, decimal defaultValue)
	{
		if (!_registry.TryGetValue(Prefix(key, PlayerPrefsSl.DataType.Decimal), out var value) || !decimal.TryParse(value, global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public static double Get(string key, double defaultValue)
	{
		if (!_registry.TryGetValue(Prefix(key, PlayerPrefsSl.DataType.Double), out var value) || !double.TryParse(value, global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public static float Get(string key, float defaultValue)
	{
		if (!_registry.TryGetValue(Prefix(key, PlayerPrefsSl.DataType.Float), out var value) || !float.TryParse(value, global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public static int Get(string key, int defaultValue)
	{
		if (!_registry.TryGetValue(Prefix(key, PlayerPrefsSl.DataType.Int), out var value) || !int.TryParse(value, global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public static uint Get(string key, uint defaultValue)
	{
		if (!_registry.TryGetValue(Prefix(key, PlayerPrefsSl.DataType.Uint), out var value) || !uint.TryParse(value, global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public static long Get(string key, long defaultValue)
	{
		if (!_registry.TryGetValue(Prefix(key, PlayerPrefsSl.DataType.Long), out var value) || !long.TryParse(value, global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public static ulong Get(string key, ulong defaultValue)
	{
		if (!_registry.TryGetValue(Prefix(key, PlayerPrefsSl.DataType.Ulong), out var value) || !ulong.TryParse(value, global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public static short Get(string key, short defaultValue)
	{
		if (!_registry.TryGetValue(Prefix(key, PlayerPrefsSl.DataType.Short), out var value) || !short.TryParse(value, global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public static ushort Get(string key, ushort defaultValue)
	{
		if (!_registry.TryGetValue(Prefix(key, PlayerPrefsSl.DataType.Int), out var value) || !ushort.TryParse(value, global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public static string Get(string key, string defaultValue)
	{
		if (!_registry.TryGetValue(Prefix(key, PlayerPrefsSl.DataType.String), out var value))
		{
			return defaultValue;
		}
		return value;
	}

	public static bool[] Get(string key, bool[] defaultValue)
	{
		if (_registry.TryGetValue(Prefix(key, PlayerPrefsSl.DataType.BoolArray), out var value))
		{
			string[] array = value.Split(new string[1] { ";;`'.+=;;" }, global::System.StringSplitOptions.None);
			bool[] array2 = new bool[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (!bool.TryParse(array[i], out array2[i]))
				{
					return defaultValue;
				}
			}
			return array2;
		}
		return defaultValue;
	}

	public static byte[] Get(string key, byte[] defaultValue)
	{
		if (_registry.TryGetValue(Prefix(key, PlayerPrefsSl.DataType.ByteArray), out var value))
		{
			string[] array = value.Split(new string[1] { ";;`'.+=;;" }, global::System.StringSplitOptions.None);
			byte[] array2 = new byte[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (!byte.TryParse(array[i], global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture, out array2[i]))
				{
					return defaultValue;
				}
			}
			return array2;
		}
		return defaultValue;
	}

	public static sbyte[] Get(string key, sbyte[] defaultValue)
	{
		if (_registry.TryGetValue(Prefix(key, PlayerPrefsSl.DataType.SbyteArray), out var value))
		{
			string[] array = value.Split(new string[1] { ";;`'.+=;;" }, global::System.StringSplitOptions.None);
			sbyte[] array2 = new sbyte[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (!sbyte.TryParse(array[i], global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture, out array2[i]))
				{
					return defaultValue;
				}
			}
			return array2;
		}
		return defaultValue;
	}

	public static char[] Get(string key, char[] defaultValue)
	{
		if (_registry.TryGetValue(Prefix(key, PlayerPrefsSl.DataType.CharArray), out var value))
		{
			string[] array = value.Split(new string[1] { ";;`'.+=;;" }, global::System.StringSplitOptions.None);
			char[] array2 = new char[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (!char.TryParse(array[i], out array2[i]))
				{
					return defaultValue;
				}
			}
			return array2;
		}
		return defaultValue;
	}

	public static decimal[] Get(string key, decimal[] defaultValue)
	{
		if (_registry.TryGetValue(Prefix(key, PlayerPrefsSl.DataType.DecimalArray), out var value))
		{
			string[] array = value.Split(new string[1] { ";;`'.+=;;" }, global::System.StringSplitOptions.None);
			decimal[] array2 = new decimal[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (!decimal.TryParse(array[i], global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture, out array2[i]))
				{
					return defaultValue;
				}
			}
			return array2;
		}
		return defaultValue;
	}

	public static double[] Get(string key, double[] defaultValue)
	{
		if (_registry.TryGetValue(Prefix(key, PlayerPrefsSl.DataType.DoubleArray), out var value))
		{
			string[] array = value.Split(new string[1] { ";;`'.+=;;" }, global::System.StringSplitOptions.None);
			double[] array2 = new double[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (!double.TryParse(array[i], global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture, out array2[i]))
				{
					return defaultValue;
				}
			}
			return array2;
		}
		return defaultValue;
	}

	public static float[] Get(string key, float[] defaultValue)
	{
		if (_registry.TryGetValue(Prefix(key, PlayerPrefsSl.DataType.FloatArray), out var value))
		{
			string[] array = value.Split(new string[1] { ";;`'.+=;;" }, global::System.StringSplitOptions.None);
			float[] array2 = new float[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (!float.TryParse(array[i], global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture, out array2[i]))
				{
					return defaultValue;
				}
			}
			return array2;
		}
		return defaultValue;
	}

	public static int[] Get(string key, int[] defaultValue)
	{
		if (_registry.TryGetValue(Prefix(key, PlayerPrefsSl.DataType.IntArray), out var value))
		{
			string[] array = value.Split(new string[1] { ";;`'.+=;;" }, global::System.StringSplitOptions.None);
			int[] array2 = new int[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (!int.TryParse(array[i], global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture, out array2[i]))
				{
					return defaultValue;
				}
			}
			return array2;
		}
		return defaultValue;
	}

	public static uint[] Get(string key, uint[] defaultValue)
	{
		if (_registry.TryGetValue(Prefix(key, PlayerPrefsSl.DataType.UintArray), out var value))
		{
			string[] array = value.Split(new string[1] { ";;`'.+=;;" }, global::System.StringSplitOptions.None);
			uint[] array2 = new uint[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (!uint.TryParse(array[i], global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture, out array2[i]))
				{
					return defaultValue;
				}
			}
			return array2;
		}
		return defaultValue;
	}

	public static long[] Get(string key, long[] defaultValue)
	{
		if (_registry.TryGetValue(Prefix(key, PlayerPrefsSl.DataType.LongArray), out var value))
		{
			string[] array = value.Split(new string[1] { ";;`'.+=;;" }, global::System.StringSplitOptions.None);
			long[] array2 = new long[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (!long.TryParse(array[i], global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture, out array2[i]))
				{
					return defaultValue;
				}
			}
			return array2;
		}
		return defaultValue;
	}

	public static ulong[] Get(string key, ulong[] defaultValue)
	{
		if (_registry.TryGetValue(Prefix(key, PlayerPrefsSl.DataType.UlongArray), out var value))
		{
			string[] array = value.Split(new string[1] { ";;`'.+=;;" }, global::System.StringSplitOptions.None);
			ulong[] array2 = new ulong[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (!ulong.TryParse(array[i], global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture, out array2[i]))
				{
					return defaultValue;
				}
			}
			return array2;
		}
		return defaultValue;
	}

	public static short[] Get(string key, short[] defaultValue)
	{
		if (_registry.TryGetValue(Prefix(key, PlayerPrefsSl.DataType.ShortArray), out var value))
		{
			string[] array = value.Split(new string[1] { ";;`'.+=;;" }, global::System.StringSplitOptions.None);
			short[] array2 = new short[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (!short.TryParse(array[i], global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture, out array2[i]))
				{
					return defaultValue;
				}
			}
			return array2;
		}
		return defaultValue;
	}

	public static ushort[] Get(string key, ushort[] defaultValue)
	{
		if (_registry.TryGetValue(Prefix(key, PlayerPrefsSl.DataType.UshortArray), out var value))
		{
			string[] array = value.Split(new string[1] { ";;`'.+=;;" }, global::System.StringSplitOptions.None);
			ushort[] array2 = new ushort[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (!ushort.TryParse(array[i], global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture, out array2[i]))
				{
					return defaultValue;
				}
			}
			return array2;
		}
		return defaultValue;
	}

	public static string[] Get(string key, string[] defaultValue)
	{
		if (!_registry.TryGetValue(Prefix(key, PlayerPrefsSl.DataType.StringArray), out var value))
		{
			return defaultValue;
		}
		return value.Split(new string[1] { ";;`'.+=;;" }, global::System.StringSplitOptions.None);
	}
}
