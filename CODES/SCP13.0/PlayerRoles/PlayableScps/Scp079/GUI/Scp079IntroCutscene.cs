using System;
using System.Diagnostics;
using System.Text;
using TMPro;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079IntroCutscene : Scp079GuiElementBase
	{
		[Serializable]
		private class Entry
		{
			[SerializeField]
			[Multiline]
			private string _text;

			[SerializeField]
			private float _timeToType;

			[SerializeField]
			private float _duration;

			[SerializeField]
			private bool _clear;

			[SerializeField]
			private AudioClip _sound;

			private string _textToPrint;

			private float _elapsed;

			private float _lettersPerSecond;

			private int _lettersOffset;

			private int _totalWritten;

			private bool _isSetup;

			private bool _tagOpen;

			private const char NewLineSymbol = '$';

			private const char OpenTagSymbol = '<';

			private const char CloseTagSymbol = '>';

			public float RemainingDuration
			{
				get
				{
					return 0f;
				}
				set
				{
				}
			}

			public void AppendText(StringBuilder sb, Transform soundTr)
			{
			}

			private void Setup(StringBuilder sb, Transform t)
			{
			}

			private void AppendCharacters(StringBuilder sb)
			{
			}
		}

		private readonly Stopwatch _stopwatch;

		private readonly StringBuilder _sb;

		[SerializeField]
		private Entry[] _entries;

		[SerializeField]
		private TMP_Text _text;

		[SerializeField]
		private float _normalSpeed;

		[SerializeField]
		private float _fastIntroSpeed;

		[SerializeField]
		private float _minSkipTime;

		private int _curEntry;

		private float _speed;

		private Scp079LostSignalHandler _lostSignalHandler;

		private static KeyCode[] _allKeyCodes;

		public static bool IsPlaying { get; private set; }

		internal override void Init(Scp079Role role, ReferenceHub owner)
		{
		}

		private void Start()
		{
		}

		private void OnDestroy()
		{
		}

		private void Update()
		{
		}

		private void StopPlaying()
		{
		}

		private bool CheckSkipping()
		{
			return false;
		}

		private bool CheckKey(KeyCode kc)
		{
			return false;
		}

		[ContextMenu("Print Total Time")]
		private void PrintTotalTime()
		{
		}
	}
}
