public class CmdBinding : global::UnityEngine.MonoBehaviour
{
	public class Bind
	{
		public string command;

		public global::UnityEngine.KeyCode key;
	}

	public static readonly global::System.Collections.Generic.List<CmdBinding.Bind> Bindings;

	static CmdBinding()
	{
		Bindings = new global::System.Collections.Generic.List<CmdBinding.Bind>();
		Load();
	}

	private void Update()
	{
	}

	public static void KeyBind(global::UnityEngine.KeyCode code, string cmd)
	{
		foreach (CmdBinding.Bind binding in Bindings)
		{
			if (binding.key == code)
			{
				binding.command = cmd;
				Save();
				return;
			}
		}
		Bindings.Add(new CmdBinding.Bind
		{
			command = cmd,
			key = code
		});
	}

	public static void Save()
	{
		string text = "";
		for (int i = 0; i < Bindings.Count; i++)
		{
			text = text + (int)Bindings[i].key + ":" + Bindings[i].command;
			if (i != Bindings.Count - 1)
			{
				text += global::System.Environment.NewLine;
			}
		}
		global::System.IO.StreamWriter streamWriter = new global::System.IO.StreamWriter(global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder.ApplicationData) + "/SCP Secret Laboratory/cmdbinding.txt");
		streamWriter.WriteLine(text);
		streamWriter.Close();
	}

	public static void Load()
	{
		global::GameCore.Console.AddLog("Loading cmd bindings...", global::UnityEngine.Color.grey);
		try
		{
			Bindings.Clear();
			if (!global::System.IO.File.Exists(global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder.ApplicationData) + "/SCP Secret Laboratory/cmdbinding.txt"))
			{
				Revent();
			}
			global::System.IO.StreamReader streamReader = new global::System.IO.StreamReader(global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder.ApplicationData) + "/SCP Secret Laboratory/cmdbinding.txt");
			string text;
			while ((text = streamReader.ReadLine()) != null)
			{
				if (!string.IsNullOrEmpty(text) && text.Contains(":"))
				{
					Bindings.Add(new CmdBinding.Bind
					{
						command = text.Split(':')[1],
						key = (global::UnityEngine.KeyCode)int.Parse(text.Split(':')[0])
					});
				}
			}
			streamReader.Close();
		}
		catch (global::System.Exception ex)
		{
			global::UnityEngine.Debug.Log("REVENT: " + ex.StackTrace + " - " + ex.Message);
			Revent();
		}
	}

	private static void Revent()
	{
		global::UnityEngine.Debug.Log("Reventing!");
		new global::System.IO.StreamWriter(global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder.ApplicationData) + "/SCP Secret Laboratory/cmdbinding.txt").Close();
	}
}
