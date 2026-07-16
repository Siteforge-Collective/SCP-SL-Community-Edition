namespace ServerOutput
{
	public class StandardOutput : global::ServerOutput.IServerOutput, global::System.IDisposable
	{
		public void Start()
		{
		}

		public void AddLog(string text, global::System.ConsoleColor color)
		{
			global::System.Console.ForegroundColor = color;
			global::System.Console.WriteLine(text);
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
				AddLog("[Control Message] " + entry.GetString(), global::System.ConsoleColor.Gray);
			}
		}

		public void Dispose()
		{
		}
	}
}
