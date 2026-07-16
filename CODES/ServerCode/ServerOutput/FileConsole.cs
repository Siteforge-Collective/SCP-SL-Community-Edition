namespace ServerOutput
{
	public class FileConsole : global::ServerOutput.IServerOutput, global::System.IDisposable
	{
		private bool _disposing;

		private uint _logId;

		private readonly string _session;

		private readonly global::System.IO.FileSystemWatcher _fsw;

		private readonly global::System.Threading.Thread _queueThread;

		private readonly global::System.Collections.Concurrent.ConcurrentQueue<global::ServerOutput.IOutputEntry> _prompterQueue = new global::System.Collections.Concurrent.ConcurrentQueue<global::ServerOutput.IOutputEntry>();

		public FileConsole(string session)
		{
			_session = (string.IsNullOrEmpty(session) ? "default" : session);
			_queueThread = new global::System.Threading.Thread(Prompt)
			{
				Priority = global::System.Threading.ThreadPriority.Lowest,
				IsBackground = true,
				Name = "Dedicated server console output"
			};
			_fsw = new global::System.IO.FileSystemWatcher
			{
				Path = "SCPSL_Data/Dedicated/" + _session,
				NotifyFilter = global::System.IO.NotifyFilters.FileName
			};
		}

		public void Start()
		{
			if (global::System.IO.Directory.Exists("SCPSL_Data/Dedicated/" + _session) && global::System.Environment.GetCommandLineArgs().Contains<string>("-nodedicateddelete"))
			{
				string[] files = global::System.IO.Directory.GetFiles("SCPSL_Data/Dedicated/" + _session);
				for (int i = 0; i < files.Length; i++)
				{
					global::System.IO.File.Delete(files[i]);
				}
			}
			global::System.IO.Directory.CreateDirectory("SCPSL_Data/Dedicated/" + _session);
			_queueThread.Start();
			_fsw.Created += delegate(object sender, global::System.IO.FileSystemEventArgs args)
			{
				if (args.Name.Contains("cs") && args.Name.Contains("mapi"))
				{
					new global::System.Threading.Thread((global::System.Threading.ThreadStart)delegate
					{
						ReadLog(args.FullPath);
					}).Start();
				}
			};
			_fsw.EnableRaisingEvents = true;
		}

		public void Dispose()
		{
			_disposing = true;
			_fsw.Dispose();
			try
			{
				if (_queueThread.IsAlive)
				{
					_queueThread.Abort();
				}
			}
			catch
			{
			}
			if (!ServerStatic.KeepSession && global::System.IO.Directory.Exists("SCPSL_Data/Dedicated/" + _session))
			{
				global::System.IO.Directory.Delete("SCPSL_Data/Dedicated/" + _session, recursive: true);
			}
		}

		private void ReadLog(string path)
		{
			try
			{
				if (!global::System.IO.File.Exists(path))
				{
					return;
				}
				string text = path.Remove(0, path.IndexOf("cs", global::System.StringComparison.Ordinal));
				string empty = string.Empty;
				string text2 = string.Empty;
				global::System.ConsoleColor color = global::System.ConsoleColor.Gray;
				try
				{
					text2 = "Error while reading the file: " + text;
					using (global::System.IO.StreamReader streamReader = new global::System.IO.StreamReader("SCPSL_Data/Dedicated/" + _session + "/" + text))
					{
						string text3 = streamReader.ReadToEnd();
						text2 = "Error while dedecting 'terminator end-of-message' signal.";
						if (text3.Contains("terminator"))
						{
							text3 = text3.Remove(text3.LastIndexOf("terminator", global::System.StringComparison.Ordinal));
						}
						text2 = "Error while sending message.";
						ServerConsole.PrompterQueue.Enqueue(text3);
						global::System.IO.File.Delete("SCPSL_Data/Dedicated/" + _session + "/" + text);
						return;
					}
				}
				catch
				{
					global::UnityEngine.Debug.LogError("Error in server console: " + text2);
				}
				if (!string.IsNullOrEmpty(empty))
				{
					AddLog(empty, color);
				}
			}
			catch (global::System.Exception exception)
			{
				global::UnityEngine.Debug.LogException(exception);
			}
		}

		public void AddLog(string text, global::System.ConsoleColor color)
		{
			if (!string.IsNullOrWhiteSpace(text))
			{
				if (ServerStatic.IsDedicated)
				{
					_prompterQueue.Enqueue(new global::ServerOutput.TextOutputEntry(text, color));
				}
				else
				{
					global::GameCore.Console.AddLog(text, global::UnityEngine.Color.grey);
				}
			}
		}

		public void AddLog(string text)
		{
			AddLog(text, global::System.ConsoleColor.Gray);
		}

		public void AddOutput(global::ServerOutput.IOutputEntry entry)
		{
			if (ServerStatic.IsDedicated)
			{
				_prompterQueue.Enqueue(entry);
			}
		}

		private void Prompt()
		{
			while (!_disposing)
			{
				global::ServerOutput.IOutputEntry result;
				if (_prompterQueue.Count == 0)
				{
					global::System.Threading.Thread.Sleep(25);
				}
				else if (_prompterQueue.TryDequeue(out result))
				{
					global::System.IO.StreamWriter streamWriter = new global::System.IO.StreamWriter("SCPSL_Data/Dedicated/" + _session + "/sl" + _logId + ".mapi");
					_logId++;
					streamWriter.WriteLine(result.ToString());
					streamWriter.Close();
				}
			}
		}
	}
}
