namespace VoiceChat.Playbacks
{
	public class IntercomPlayback : global::VoiceChat.Playbacks.SingleBufferPlayback, global::VoiceChat.Playbacks.IGlobalPlayback
	{
		private bool _isTemplate;

		private ReferenceHub _lastSpeaker;

		private static bool _templateSet;

		private static global::VoiceChat.Playbacks.IntercomPlayback _template;

		private static int _instancesCnt;

		private static readonly global::System.Collections.Generic.List<global::VoiceChat.Playbacks.IntercomPlayback> Instances = new global::System.Collections.Generic.List<global::VoiceChat.Playbacks.IntercomPlayback>();

		public bool GlobalChatActive => MaxSamples > 0;

		public global::UnityEngine.Color GlobalChatColor { get; private set; }

		public string GlobalChatName { get; private set; }

		public float GlobalChatLoudness => base.Loudness;

		public global::VoiceChat.Playbacks.GlobalChatIconType GlobalChatIcon => global::VoiceChat.Playbacks.GlobalChatIconType.Intercom;

		protected override void Awake()
		{
			base.Awake();
			_instancesCnt++;
			Instances.Add(this);
			global::VoiceChat.GlobalChatIndicatorManager.Subscribe(this, null);
			if (!_templateSet)
			{
				_template = this;
				_isTemplate = true;
				_templateSet = true;
			}
		}

		private void OnDestroy()
		{
			global::VoiceChat.GlobalChatIndicatorManager.Unsubscribe(this);
			if (_isTemplate)
			{
				_templateSet = false;
				_instancesCnt = 0;
				Instances.Clear();
			}
		}

		private void SetSpeaker(ReferenceHub speaker)
		{
			_lastSpeaker = speaker;
			GlobalChatName = speaker.nicknameSync.DisplayName;
			GlobalChatColor = speaker.serverRoles.GetVoiceColor();
		}

		public static void ProcessSamples(ReferenceHub ply, float[] samples, int len)
		{
			if (!_templateSet)
			{
				return;
			}
			bool flag = false;
			global::VoiceChat.Playbacks.IntercomPlayback intercomPlayback = null;
			for (int i = 0; i < _instancesCnt; i++)
			{
				global::VoiceChat.Playbacks.IntercomPlayback intercomPlayback2 = Instances[i];
				if (intercomPlayback2._lastSpeaker == ply)
				{
					intercomPlayback2.Buffer.Write(samples, len);
					return;
				}
				if (!flag && intercomPlayback2.MaxSamples == 0)
				{
					intercomPlayback = intercomPlayback2;
					flag = true;
				}
			}
			if (!flag)
			{
				intercomPlayback = global::UnityEngine.Object.Instantiate(_template);
				intercomPlayback.Buffer.Clear();
			}
			intercomPlayback.SetSpeaker(ply);
			intercomPlayback.Buffer.Write(samples, len);
		}
	}
}
