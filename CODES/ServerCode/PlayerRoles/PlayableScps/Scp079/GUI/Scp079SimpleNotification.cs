namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079SimpleNotification : global::PlayerRoles.PlayableScps.Scp079.GUI.IScp079Notification
	{
		private int _totalWritten;

		private int _prevPlayed;

		private int _lettersOffset;

		private readonly bool _mute;

		private readonly global::System.Text.StringBuilder _writtenText;

		private readonly string _targetContent;

		private readonly int _length;

		private readonly float _totalHeight;

		private readonly float _startTime;

		private readonly float _endTime;

		private const float InitialSize = -5f;

		private const float LettersPerSecond = 100f;

		private const float SoundRateRatio = 0.2f;

		private const float FadeInTime = 0.08f;

		private const float AbsoluteDuration = 4.2f;

		private const float PerLetterDuration = 0.05f;

		private const float HeightPerLine = 22f;

		protected const float FadeOutDuration = 0.18f;

		private float CurrentTime => global::UnityEngine.Time.timeSinceLevelLoad;

		private float Elapsed => CurrentTime - _startTime;

		protected virtual global::System.Text.StringBuilder WrittenText => _writtenText;

		public virtual float Opacity => global::UnityEngine.Mathf.Min(1f, 1f - (CurrentTime - _endTime) / 0.18f);

		public float Height
		{
			get
			{
				float t = ((Opacity > 0f) ? (Elapsed / 0.08f) : (1f + Opacity));
				return global::UnityEngine.Mathf.Lerp(-5f, _totalHeight, t);
			}
		}

		public string DisplayedText
		{
			get
			{
				WriteLetters();
				return WrittenText.ToString();
			}
		}

		public global::PlayerRoles.PlayableScps.Scp079.GUI.NotificationSound Sound
		{
			get
			{
				int num = global::UnityEngine.Mathf.CeilToInt((float)(_totalWritten - _lettersOffset) * 0.2f);
				if (num <= _prevPlayed)
				{
					return global::PlayerRoles.PlayableScps.Scp079.GUI.NotificationSound.None;
				}
				_prevPlayed = num;
				if (!_mute)
				{
					return global::PlayerRoles.PlayableScps.Scp079.GUI.NotificationSound.Standard;
				}
				return global::PlayerRoles.PlayableScps.Scp079.GUI.NotificationSound.None;
			}
		}

		public virtual bool Delete => Opacity < -1f;

		public Scp079SimpleNotification(string targetContent, bool mute = false)
		{
			_mute = mute;
			_writtenText = new global::System.Text.StringBuilder();
			_targetContent = targetContent;
			_length = _targetContent.Length;
			_totalHeight = 22f * (float)(1 + _length - _targetContent.Replace("\n", string.Empty).Length);
			_startTime = CurrentTime;
			_endTime = _startTime + 4.2f + 0.05f * (float)_length;
		}

		private void WriteLetters()
		{
			int num = global::UnityEngine.Mathf.RoundToInt((Elapsed - 0.08f) * 100f) + _lettersOffset;
			if (num <= 0)
			{
				return;
			}
			bool flag = false;
			while (_totalWritten < _length && (_totalWritten < num || flag))
			{
				char c = _targetContent[_totalWritten];
				switch (c)
				{
				case '<':
					flag = true;
					break;
				case '>':
					flag = false;
					break;
				}
				_writtenText.Append(c);
				_totalWritten++;
				if (flag)
				{
					_lettersOffset++;
				}
			}
		}
	}
}
