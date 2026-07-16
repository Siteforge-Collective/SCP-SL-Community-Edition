public class CreditsEntry
{
	public string Title;

	public string Name;

	public bool Multi;

	public global::UnityEngine.Color Color;

	public CreditsEntry()
	{
	}

	public CreditsEntry(string title, string name)
	{
		Multi = false;
		Title = title;
		Name = name;
		Color = global::UnityEngine.Color.white;
	}

	public CreditsEntry(string title, string name, global::UnityEngine.Color color)
	{
		Multi = false;
		Title = title;
		Name = name;
		Color = color;
	}

	public CreditsEntry(string name)
	{
		Multi = false;
		Title = "";
		Name = name;
	}

	public CreditsEntry(string[] names)
	{
		Multi = true;
		Title = "";
		Name = global::System.Linq.Enumerable.Aggregate(names, "", (string current, string n) => current + n + "\n");
	}
}
