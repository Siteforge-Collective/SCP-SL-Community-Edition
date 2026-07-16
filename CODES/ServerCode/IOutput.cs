public interface IOutput
{
	void Print(string text);

	void Print(string text, global::System.ConsoleColor c);

	void Print(string text, global::System.ConsoleColor c, global::UnityEngine.Color rgbColor);
}
