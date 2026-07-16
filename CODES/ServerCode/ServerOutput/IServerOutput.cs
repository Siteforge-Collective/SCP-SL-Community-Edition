namespace ServerOutput
{
	public interface IServerOutput : global::System.IDisposable
	{
		void Start();

		void AddLog(string text, global::System.ConsoleColor color);

		void AddLog(string text);

		void AddOutput(global::ServerOutput.IOutputEntry entry);
	}
}
