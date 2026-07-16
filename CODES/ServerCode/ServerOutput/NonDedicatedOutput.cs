namespace ServerOutput
{
	public class NonDedicatedOutput : global::ServerOutput.IServerOutput, global::System.IDisposable
	{
		public void Start()
		{
		}

		public void AddLog(string text, global::System.ConsoleColor color)
		{
			global::GameCore.Console.AddLog(ServerConsole.ColorText("[SRV] " + text, color), global::UnityEngine.Color.gray);
		}

		public void AddLog(string text)
		{
			AddLog(text, global::System.ConsoleColor.Gray);
		}

		public void AddOutput(global::ServerOutput.IOutputEntry entry)
		{
			if (entry is global::ServerOutput.TextOutputEntry textOutputEntry)
			{
				AddLog(textOutputEntry.Text, (global::System.ConsoleColor)textOutputEntry.Color);
			}
			else
			{
				global::GameCore.Console.AddLog("[Control Message] " + entry.GetString(), global::UnityEngine.Color.gray);
			}
		}

		public void Dispose()
		{
		}
	}
}
