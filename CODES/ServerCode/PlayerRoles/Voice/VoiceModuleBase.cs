namespace PlayerRoles.Voice
{
	public abstract class VoiceModuleBase : global::UnityEngine.MonoBehaviour, global::GameObjectPools.IPoolResettable, global::GameObjectPools.IPoolSpawnable
	{
		public delegate void SamplesReceived(float[] samples, int len);

		private global::VoiceChat.VoiceChatChannel _lastChannel;

		private global::VoiceChat.Codec.OpusDecoder _defaultDecoder;

		private ReferenceHub _owner;

		private int _sentPackets;

		private int _prevSent;

		private const float SilenceTolerance = 0.1f;

		private const float RateLimiterTimeframe = 0.5f;

		private const int RateLimiterTolerance = 128;

		private readonly global::System.Diagnostics.Stopwatch _rateStopwatch = global::System.Diagnostics.Stopwatch.StartNew();

		private readonly global::System.Diagnostics.Stopwatch _silenceStopwatch = global::System.Diagnostics.Stopwatch.StartNew();

		private static float[] _receiveBuffer;

		private static bool _receiveBufferSet;

		protected ReferenceHub Owner => _owner;

		protected virtual global::VoiceChat.Codec.OpusDecoder Decoder => _defaultDecoder;

		public global::PlayerRoles.PlayerRoleBase Role { get; private set; }

		public bool ServerIsSending { get; private set; }

		public global::PlayerRoles.Voice.GroupMuteFlags ReceiveFlags { get; set; }

		public global::VoiceChat.VoiceChatChannel CurrentChannel
		{
			get
			{
				return _lastChannel;
			}
			internal set
			{
				if (_lastChannel != value)
				{
					_lastChannel = value;
					OnChannelChanged();
				}
			}
		}

		public abstract bool IsSpeaking { get; }

		public event global::PlayerRoles.Voice.VoiceModuleBase.SamplesReceived OnSamplesReceived;

		protected virtual void Awake()
		{
			Role = GetComponent<global::PlayerRoles.PlayerRoleBase>();
		}

		protected virtual void Update()
		{
			if (_sentPackets > _prevSent)
			{
				ServerIsSending = true;
				_silenceStopwatch.Restart();
			}
			else if (ServerIsSending && _silenceStopwatch.Elapsed.TotalSeconds > 0.10000000149011612)
			{
				ServerIsSending = false;
			}
			if (_rateStopwatch.Elapsed.TotalSeconds >= 0.5)
			{
				_sentPackets = 0;
				_rateStopwatch.Restart();
			}
			_prevSent = _sentPackets;
		}

		protected virtual void OnChannelChanged()
		{
		}

		protected abstract void ProcessSamples(float[] data, int len);

		public abstract global::VoiceChat.VoiceChatChannel GetUserInput();

		public virtual global::VoiceChat.VoiceChatChannel ValidateSend(global::VoiceChat.VoiceChatChannel channel)
		{
			return channel;
		}

		public virtual global::VoiceChat.VoiceChatChannel ValidateReceive(ReferenceHub speaker, global::VoiceChat.VoiceChatChannel channel)
		{
			return channel;
		}

		public virtual void ResetObject()
		{
			_lastChannel = global::VoiceChat.VoiceChatChannel.None;
			_defaultDecoder?.Dispose();
			ReceiveFlags = global::PlayerRoles.Voice.GroupMuteFlags.None;
		}

		public virtual void SpawnObject()
		{
			if (Role.TryGetOwner(out _owner))
			{
				_defaultDecoder = new global::VoiceChat.Codec.OpusDecoder();
				if (Owner.isLocalPlayer)
				{
					global::VoiceChat.VoiceChatMicCapture.StartRecording();
				}
				if (global::Mirror.NetworkServer.active)
				{
					ReceiveFlags = global::PlayerRoles.Voice.VoiceChatReceivePrefs.GetFlagsForUser(Owner);
				}
			}
		}

		public bool CheckRateLimit()
		{
			return _sentPackets++ < 128;
		}

		public void ProcessMessage(global::VoiceChat.Networking.VoiceMessage msg)
		{
			CurrentChannel = msg.Channel;
			if (!_receiveBufferSet)
			{
				_receiveBufferSet = true;
				_receiveBuffer = new float[24000];
			}
			int len = Decoder.Decode(msg.Data, msg.DataLength, _receiveBuffer);
			if (Owner.isLocalPlayer || global::VoiceChat.VoiceChatMutes.GetFlags(Owner) == global::VoiceChat.VcMuteFlags.None)
			{
				ProcessSamples(_receiveBuffer, len);
				this.OnSamplesReceived?.Invoke(_receiveBuffer, len);
			}
		}
	}
}
