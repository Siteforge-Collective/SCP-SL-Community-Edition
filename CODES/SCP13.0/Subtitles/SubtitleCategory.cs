using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Subtitles
{
	public class SubtitleCategory : MonoBehaviour
	{
		private class Message
		{
			public string Text { get; }

			public float Duration { get; }

			public Message(string text, float duration)
			{
			}
		}

		private const string SpeakerName = "C.A.S.S.I.E :";

		private static readonly string[] SplitSeperator;

		[SerializeField]
		private string speakerNameColor;

		public TextMeshProUGUI CategoryText;

		private float _timer;

		private Queue<Message> _messages;

		private bool _isPlayingMessage;

		private Message _currentMessage;

		private int _currentMessageSize;

		private int _speakerStartStringSize;

		public void AddSubtitle(string message, float duration, float delay)
		{
		}

		public void ClearSubtitles()
		{
		}

		private void Awake()
		{
		}

		private void CheckForMessageStart()
		{
		}

		private void CheckForMessageEnd()
		{
		}

		private void FixedUpdate()
		{
		}
	}
}
