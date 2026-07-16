using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CmdBinding : MonoBehaviour
{
	public class Bind
	{
		public string command;

		public KeyCode key;
	}

	public static readonly List<Bind> Bindings;

    static CmdBinding()
    {
        Bindings = new List<Bind>();
        Load();
    }

    private void Update()
	{
	}

    public static void KeyBind(KeyCode code, string cmd)
    {
        foreach (Bind binding in Bindings)
        {
            if (binding.key == code)
            {
                binding.command = cmd;
                Save();
                return;
            }
        }
        Bindings.Add(new Bind
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
            string text2 = text;
            int key = (int)Bindings[i].key;
            text = text2 + key + ":" + Bindings[i].command;
            if (i != Bindings.Count - 1)
            {
                text += Environment.NewLine;
            }
        }
        StreamWriter streamWriter = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/SCP Secret Laboratory/cmdbinding.txt");
        streamWriter.WriteLine(text);
        streamWriter.Close();
    }

    public static void Load()
    {
        GameCore.Console.AddLog("Loading cmd bindings...", Color.grey);
        try
        {
            Bindings.Clear();
            if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/SCP Secret Laboratory/cmdbinding.txt"))
            {
                Revent();
            }
            StreamReader streamReader = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/SCP Secret Laboratory/cmdbinding.txt");
            string text;
            while ((text = streamReader.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(text) && text.Contains(":"))
                {
                    Bindings.Add(new Bind
                    {
                        command = text.Split(':')[1],
                        key = (KeyCode)int.Parse(text.Split(':')[0])
                    });
                }
            }
            streamReader.Close();
        }
        catch (Exception ex)
        {
            Debug.Log("REVENT: " + ex.StackTrace + " - " + ex.Message);
            Revent();
        }
    }

    private static void Revent()
    {
        Debug.Log("Reventing!");
        new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/SCP Secret Laboratory/cmdbinding.txt").Close();
    }

    public static void ChangeKeybinding(KeyCode code, string cmd)
	{
	}
}
