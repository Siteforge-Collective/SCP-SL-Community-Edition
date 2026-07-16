public static class FileManager
{
	private static string _appfolder = "";

	private static string _configfolder = "";

	public static global::System.Text.Encoding Utf8Encoding = new global::System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

	public static void RefreshAppFolder()
	{
		_appfolder = ((ServerStatic.IsDedicated && global::GameCore.ConfigFile.HosterPolicy != null && global::GameCore.ConfigFile.HosterPolicy.GetBool("gamedir_for_configs")) ? "AppData" : (global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder.ApplicationData) + GetPathSeparator() + "SCP Secret Laboratory"));
	}

	public static string GetAppFolder(bool addSeparator = true, bool serverConfig = false, string centralConfig = "")
	{
		if (string.IsNullOrEmpty(_appfolder))
		{
			RefreshAppFolder();
		}
		if (serverConfig && !string.IsNullOrEmpty(_configfolder) && string.IsNullOrEmpty(centralConfig))
		{
			return _configfolder + (addSeparator ? GetPathSeparator().ToString() : "");
		}
		return _appfolder + ((addSeparator || serverConfig) ? GetPathSeparator().ToString() : "") + (serverConfig ? ("config/" + ((!string.IsNullOrEmpty(centralConfig)) ? centralConfig : (ServerStatic.IsDedicated ? ServerStatic.ServerPort.ToString() : "nondedicated")) + (addSeparator ? GetPathSeparator().ToString() : "")) : "");
	}

	public static string StripPath(string path)
	{
		path = path.Replace("\"", "").Trim();
		while (path.EndsWith("\\") || path.EndsWith("/") || path.EndsWith(GetPathSeparator().ToString()))
		{
			path = path.Substring(0, path.Length - 1);
		}
		return path;
	}

	public static void SetAppFolder(string path)
	{
		path = StripPath(path);
		if (!global::System.IO.Directory.Exists(path))
		{
			_appfolder = "";
		}
		else
		{
			_appfolder = path;
		}
	}

	public static void SetConfigFolder(string path)
	{
		path = StripPath(path);
		if (!global::System.IO.Directory.Exists(path))
		{
			_configfolder = "";
		}
		else
		{
			_configfolder = path;
		}
	}

	public static string ReplacePathSeparators(string path)
	{
		return path.Replace('/', GetPathSeparator()).Replace('\\', GetPathSeparator());
	}

	public static char GetPathSeparator()
	{
		return global::System.IO.Path.DirectorySeparatorChar;
	}

	public static bool FileExists(string path)
	{
		return global::System.IO.File.Exists(path);
	}

	public static bool DictionaryExists(string path)
	{
		return global::System.IO.Directory.Exists(path);
	}

	public static void FileCreate(string path)
	{
		global::System.IO.File.Create(path).Dispose();
	}

	public static global::System.IO.FileStream FileStreamCreate(string path)
	{
		return global::System.IO.File.Create(path);
	}

	public static string[] ReadAllLines(string path)
	{
		global::System.Collections.Generic.List<string> list = global::NorthwoodLib.Pools.ListPool<string>.Shared.Rent();
		using (global::System.IO.StreamReader streamReader = new global::System.IO.StreamReader(path))
		{
			string item;
			while ((item = streamReader.ReadLine()) != null)
			{
				list.Add(item);
			}
		}
		string[] result = list.ToArray();
		global::NorthwoodLib.Pools.ListPool<string>.Shared.Return(list);
		return result;
	}

	public static global::System.Collections.Generic.List<string> ReadAllLinesList(string path)
	{
		global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
		using (global::System.IO.StreamReader streamReader = new global::System.IO.StreamReader(path))
		{
			string item;
			while ((item = streamReader.ReadLine()) != null)
			{
				list.Add(item);
			}
			return list;
		}
	}

	public static void ReadAllLinesList(string path, global::System.Collections.Generic.List<string> list)
	{
		list.Clear();
		using (global::System.IO.StreamReader streamReader = new global::System.IO.StreamReader(path))
		{
			string item;
			while ((item = streamReader.ReadLine()) != null)
			{
				list.Add(item);
			}
		}
	}

	public static string[] ReadAllLinesSafe(string path)
	{
		global::System.Collections.Generic.List<string> list = global::NorthwoodLib.Pools.ListPool<string>.Shared.Rent();
		using (global::System.IO.FileStream stream = new global::System.IO.FileStream(path, global::System.IO.FileMode.Open, global::System.IO.FileAccess.Read, global::System.IO.FileShare.ReadWrite))
		{
			using (global::System.IO.StreamReader streamReader = new global::System.IO.StreamReader(stream))
			{
				string item;
				while ((item = streamReader.ReadLine()) != null)
				{
					list.Add(item);
				}
			}
		}
		string[] result = list.ToArray();
		global::NorthwoodLib.Pools.ListPool<string>.Shared.Return(list);
		return result;
	}

	public static global::System.Collections.Generic.List<string> ReadAllLinesSafeList(string path)
	{
		global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
		using (global::System.IO.FileStream stream = new global::System.IO.FileStream(path, global::System.IO.FileMode.Open, global::System.IO.FileAccess.Read, global::System.IO.FileShare.ReadWrite))
		{
			using (global::System.IO.StreamReader streamReader = new global::System.IO.StreamReader(stream))
			{
				string item;
				while ((item = streamReader.ReadLine()) != null)
				{
					list.Add(item);
				}
				return list;
			}
		}
	}

	public static void ReadAllLinesSafeList(string path, global::System.Collections.Generic.List<string> list)
	{
		list.Clear();
		using (global::System.IO.FileStream stream = new global::System.IO.FileStream(path, global::System.IO.FileMode.Open, global::System.IO.FileAccess.Read, global::System.IO.FileShare.ReadWrite))
		{
			using (global::System.IO.StreamReader streamReader = new global::System.IO.StreamReader(stream))
			{
				string item;
				while ((item = streamReader.ReadLine()) != null)
				{
					list.Add(item);
				}
			}
		}
	}

	public static string ReadAllText(string path)
	{
		return global::System.IO.File.ReadAllText(path);
	}

	public static string ReadAllTextSafe(string path)
	{
		using (global::System.IO.FileStream stream = new global::System.IO.FileStream(path, global::System.IO.FileMode.Open, global::System.IO.FileAccess.Read, global::System.IO.FileShare.ReadWrite))
		{
			using (global::System.IO.StreamReader streamReader = new global::System.IO.StreamReader(stream))
			{
				return streamReader.ReadToEnd();
			}
		}
	}

	public static void WriteToFile(global::System.Collections.Generic.IEnumerable<string> data, string path, bool removeempty = false)
	{
		global::System.IO.File.WriteAllLines(path, removeempty ? global::System.Linq.Enumerable.Where(data, (string line) => !string.IsNullOrWhiteSpace(line.Replace(global::System.Environment.NewLine, "").Replace("\r\n", "").Replace("\n", "")
			.Replace(" ", ""))) : data, Utf8Encoding);
	}

	public static void WriteStringToFile(string data, string path)
	{
		global::System.IO.File.WriteAllText(path, data, Utf8Encoding);
	}

	public static void WriteToFileSafe(global::System.Collections.Generic.IEnumerable<string> data, string path, bool removeempty = false)
	{
		using (global::System.IO.FileStream stream = new global::System.IO.FileStream(path, global::System.IO.FileMode.OpenOrCreate, global::System.IO.FileAccess.Write, global::System.IO.FileShare.ReadWrite))
		{
			using (global::System.IO.StreamWriter streamWriter = new global::System.IO.StreamWriter(stream, Utf8Encoding))
			{
				streamWriter.Write(string.Join("\r\n", data));
			}
		}
	}

	public static void WriteStringToFileSafe(string data, string path)
	{
		using (global::System.IO.FileStream stream = new global::System.IO.FileStream(path, global::System.IO.FileMode.OpenOrCreate, global::System.IO.FileAccess.Write, global::System.IO.FileShare.ReadWrite))
		{
			using (global::System.IO.StreamWriter streamWriter = new global::System.IO.StreamWriter(stream, Utf8Encoding))
			{
				streamWriter.Write(data);
			}
		}
	}

	public static void AppendFile(string data, string path, bool newLine = true)
	{
		string[] array = ReadAllLines(path);
		if (!newLine || array.Length == 0 || array[array.Length - 1].EndsWith(global::System.Environment.NewLine) || array[array.Length - 1].EndsWith("\n"))
		{
			global::System.IO.File.AppendAllText(path, data, Utf8Encoding);
		}
		else
		{
			global::System.IO.File.AppendAllText(path, global::System.Environment.NewLine + data, Utf8Encoding);
		}
	}

	public static void AppendFileSafe(string data, string path, bool newLine = true)
	{
		using (global::System.IO.FileStream stream = new global::System.IO.FileStream(path, global::System.IO.FileMode.Append, global::System.IO.FileAccess.Write, global::System.IO.FileShare.ReadWrite))
		{
			using (global::System.IO.StreamWriter streamWriter = new global::System.IO.StreamWriter(stream, Utf8Encoding))
			{
				streamWriter.Write(newLine ? ("\r\n" + data) : data);
			}
		}
	}

	public static void RenameFile(string path, string newpath)
	{
		global::System.IO.File.Move(path, newpath);
	}

	public static void DeleteFile(string path)
	{
		global::System.IO.File.Delete(path);
	}

	public static void ReplaceLine(int line, string text, string path)
	{
		string[] array = ReadAllLines(path);
		array[line] = text;
		WriteToFile(array, path);
	}

	public static void RemoveEmptyLines(string path)
	{
		string[] array = ReadAllLines(path);
		string[] array2 = global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Where(array, (string s) => !string.IsNullOrWhiteSpace(s.Replace(global::System.Environment.NewLine, "").Replace("\r\n", "").Replace("\n", "")
			.Replace(" ", ""))));
		if (array != array2)
		{
			WriteToFile(array2, path);
		}
	}

	private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs = true, bool overwrite = true)
	{
		global::System.IO.DirectoryInfo directoryInfo = new global::System.IO.DirectoryInfo(sourceDirName);
		if (!directoryInfo.Exists)
		{
			throw new global::System.IO.DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
		}
		global::System.IO.DirectoryInfo[] directories = directoryInfo.GetDirectories();
		if (global::System.IO.Directory.Exists(destDirName))
		{
			global::System.IO.Directory.Delete(destDirName, recursive: true);
		}
		global::System.IO.Directory.CreateDirectory(destDirName);
		global::System.IO.FileInfo[] files = directoryInfo.GetFiles();
		foreach (global::System.IO.FileInfo fileInfo in files)
		{
			string destFileName = global::System.IO.Path.Combine(destDirName, fileInfo.Name);
			fileInfo.CopyTo(destFileName, overwrite);
		}
		if (copySubDirs)
		{
			global::System.IO.DirectoryInfo[] array = directories;
			foreach (global::System.IO.DirectoryInfo directoryInfo2 in array)
			{
				string destDirName2 = global::System.IO.Path.Combine(destDirName, directoryInfo2.Name);
				DirectoryCopy(directoryInfo2.FullName, destDirName2, overwrite);
			}
		}
	}
}
