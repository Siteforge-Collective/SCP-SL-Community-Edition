using System;
using System.Collections.Generic;
using RemoteAdmin.Menus;
using TMPro;
using UnityEngine;

namespace RemoteAdmin
{
	public class StaffChatMenu : MonoBehaviour
	{
		private readonly struct MessageContent
		{
			private readonly string _content;

			private readonly DateTime _timestamp;

			public readonly bool SystemMessage;

			public MessageContent(string content, DateTime timestamp, bool systemMessage = false)
			{
				_content = null;
				_timestamp = default(DateTime);
				SystemMessage = false;
			}

			public string FormatContent()
			{
				return null;
			}
		}

		private const int MaxInstantiatedMessages = 100;

		private static readonly Color SystemMessageColor;

		[SerializeField]
		private RectTransform _chatBox;

		[SerializeField]
		private StaffChatMessage _messagePrefab;

		[SerializeField]
		private RaCommandMenu _commandMenu;

		[SerializeField]
		private TMP_InputField _input;

		private static bool _isInstantiated;

		private static StaffChatMenu _singleton;

		private static readonly List<MessageContent> NotInstantiatedMessages;

		private static readonly Queue<MessageContent> MessageHistory;

		private static string _oldAddress;

		private static ushort _oldPort;

		private static StaffChatMenu Singleton
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void ClearAllMessages()
		{
		}

		private void Update()
		{
		}

		private void Start()
		{
		}

		private void OnDestroy()
		{
		}

		private void OnEnable()
		{
		}

		private void AddMessage(MessageContent messageContent)
		{
		}

		public static void TryAddMessage(uint sender, string content)
		{
		}

		private static MessageContent FormatMessage(uint sender, string content)
		{
			return default(MessageContent);
		}

		private static void SaveMessage(MessageContent messageContent)
		{
		}
	}
}
