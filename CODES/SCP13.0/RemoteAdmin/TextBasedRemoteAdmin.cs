using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RemoteAdmin
{
	public class TextBasedRemoteAdmin : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		private readonly struct CommandDictionaryEntry
		{
			internal readonly string Usage;

			internal readonly string Description;

			internal CommandDictionaryEntry(string usage, string description)
			{
				Usage = null;
				Description = null;
			}
		}

		private class CommandDistanceComparer : IComparer<CommandDistance>
		{
			int IComparer<CommandDistance>.Compare(CommandDistance x, CommandDistance y)
			{
				return 0;
			}
		}

		private readonly struct CommandDistance : IComparable<CommandDistance>, IEquatable<CommandDistance>
		{
			internal readonly QueryProcessor.CommandData Command;

			private readonly int _distance;

			internal CommandDistance(QueryProcessor.CommandData command, int distance)
			{
				Command = default(QueryProcessor.CommandData);
				_distance = 0;
			}

			public int CompareTo(CommandDistance other)
			{
				return 0;
			}

			public static bool operator <(CommandDistance left, CommandDistance right)
			{
				return false;
			}

			public static bool operator >(CommandDistance left, CommandDistance right)
			{
				return false;
			}

			public static bool operator <=(CommandDistance left, CommandDistance right)
			{
				return false;
			}

			public static bool operator >=(CommandDistance left, CommandDistance right)
			{
				return false;
			}

			public bool Equals(CommandDistance other)
			{
				return false;
			}

			public override bool Equals(object obj)
			{
				return false;
			}

			public override int GetHashCode()
			{
				return 0;
			}

			public static bool operator ==(CommandDistance left, CommandDistance right)
			{
				return false;
			}

			public static bool operator !=(CommandDistance left, CommandDistance right)
			{
				return false;
			}
		}

		public static TextBasedRemoteAdmin singleton;

		public TextMeshProUGUI consoleWindow;

		public TMP_InputField commandField;

		private UIController _ui;

		public GameObject commandSuggest;

		public TextMeshProUGUI commandSuggestion;

		private List<KeyValuePair<string, CommandDictionaryEntry>> _clientLastCommandSearchResults;

		private int _clientLastCommandIndex;

		private int _clientCommandPosition;

		private readonly List<string> _clientCommandLogs;

		public static readonly List<QueryProcessor.CommandData> Commands;

		private static readonly string[] AllAliases;

		private static readonly string[] SelfAliases;

		private static readonly HashSet<string> ClearCommands;

		private static readonly CommandDistanceComparer _cdComparer;

		private static readonly List<CommandDistance> _distances;

		private static readonly Dictionary<string, CommandDistance> _addedDistances;

		private readonly List<string> _logs;

		private void Start()
		{
		}

		private void Awake()
		{
		}

		public static void AddLog(string log)
		{
		}

		private void RefreshConsole()
		{
		}

		private void Update()
		{
		}

		public void SendCommand()
		{
		}

		private void CommandFieldOnValueChanged()
		{
		}

		private void SetCommandSuggest(List<KeyValuePair<string, CommandDictionaryEntry>> display, int highlightId = -1)
		{
		}

		private List<KeyValuePair<string, CommandDictionaryEntry>> GetDictionaryFromArg(QueryProcessor.CommandData command, int argId)
		{
			return null;
		}

		private static List<KeyValuePair<string, CommandDictionaryEntry>> GetDictionaryFromCommands(List<CommandDistance> commands)
		{
			return null;
		}

		private static List<KeyValuePair<string, CommandDictionaryEntry>> GetDictionaryFromCommands(List<QueryProcessor.CommandData> commands)
		{
			return null;
		}

		private static string UsageArrayToHumanReadableUsage(IEnumerable<string> usage)
		{
			return null;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
		}

		private static void FuzzySearchCommand(string partialCommand)
		{
		}

		private bool ShouldProcessAllPlayers(string args)
		{
			return false;
		}

		private static bool ShouldProcessSelf(string args)
		{
			return false;
		}

		private void LookupPlayer(string arg)
		{
		}
	}
}
