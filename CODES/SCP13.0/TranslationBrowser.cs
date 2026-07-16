using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class TranslationBrowser : MonoBehaviour
{
	public Text instancePrefab;

	public Transform parent;

	private readonly List<GameObject> spawns;

	public static readonly List<(string, bool)> Translations;

	public static readonly Dictionary<string, string> Languages;

	public static readonly Dictionary<string, string> Names;

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	public static int GetTranslationList()
	{
		return 0;
	}

	public static string NameToDirectory(string name)
	{
		return null;
	}

	private static string GetFreeName(string name)
	{
		return null;
	}

	private static string GetTranslationName(CultureInfo c)
	{
		return null;
	}

	private static string FirstToUpper(string s, CultureInfo c)
	{
		return null;
	}
}
