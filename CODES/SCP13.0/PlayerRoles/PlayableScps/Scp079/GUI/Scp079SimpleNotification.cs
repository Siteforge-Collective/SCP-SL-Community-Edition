using System.Text;

namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079SimpleNotification : IScp079Notification
	{
		private int _totalWritten;

		private int _prevPlayed;

		private int _lettersOffset;

		private readonly bool _mute;

		private readonly StringBuilder _writtenText;

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

		protected const float FadeOutDuration = 0.18f;

		private float CurrentTime => 0f;

		private float Elapsed => 0f;

		protected virtual StringBuilder WrittenText => null;

		public virtual float Opacity => 0f;

		public float Height => 0f;

		public string DisplayedText => null;

		public NotificationSound Sound => default(NotificationSound);

		public virtual bool Delete => false;

		public Scp079SimpleNotification(string targetContent, bool mute = false)
		{
		}

		private void WriteLetters()
		{
		}
	}
}
