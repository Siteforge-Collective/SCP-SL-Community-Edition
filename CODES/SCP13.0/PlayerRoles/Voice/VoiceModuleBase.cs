using System.Diagnostics;
using GameObjectPools;
using Mirror;
using UnityEngine;
using VoiceChat;
using VoiceChat.Codec;
using VoiceChat.Networking;

namespace PlayerRoles.Voice
{
    public abstract class VoiceModuleBase : MonoBehaviour, IPoolResettable, IPoolSpawnable
    {
        public delegate void SamplesReceived(float[] samples, int len);

        public event SamplesReceived OnSamplesReceived;

        private VoiceChatChannel _lastChannel;

        private OpusDecoder _defaultDecoder;

        private ReferenceHub _owner;

        private int _sentPackets;

        private int _prevSent;

        private const float SilenceTolerance = 0.1f;

        private const float RateLimiterTimeframe = 0.5f;

        private const int RateLimiterTolerance = 128;

        private readonly Stopwatch _rateStopwatch = Stopwatch.StartNew();

        private readonly Stopwatch _silenceStopwatch = Stopwatch.StartNew();

        private static float[] _receiveBuffer;

        private static bool _receiveBufferSet;

        protected ReferenceHub Owner => _owner;

        protected virtual OpusDecoder Decoder => _defaultDecoder;

        public PlayerRoleBase Role { get; private set; }

        public bool ServerIsSending { get; set; }

        public GroupMuteFlags ReceiveFlags { get; set; }

        public VoiceChatChannel CurrentChannel
        {
            get => _lastChannel;
            set
            {
                if (_lastChannel != value)
                {
                    _lastChannel = value;
                    OnChannelChanged();
                }
            }
        }

        public abstract bool IsSpeaking { get; }

        protected virtual void Awake()
        {
            Role = GetComponent<PlayerRoleBase>();
        }

        protected virtual void Update()
        {
            if (ServerIsSending && _silenceStopwatch.Elapsed.TotalSeconds > SilenceTolerance)
            {
                ServerIsSending = false;
                _prevSent = 1;
                _silenceStopwatch.Restart();
            }

            if (_rateStopwatch.Elapsed.TotalSeconds >= RateLimiterTimeframe)
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

        public abstract VoiceChatChannel GetUserInput();

        public virtual VoiceChatChannel ValidateSend(VoiceChatChannel channel)
        {
            return channel;
        }

        public virtual VoiceChatChannel ValidateReceive(ReferenceHub speaker, VoiceChatChannel channel)
        {
            return channel;
        }

        public virtual void ResetObject()
        {
            _lastChannel = VoiceChatChannel.None;
            if (_defaultDecoder != null)
            {
                _defaultDecoder.Dispose();
                _defaultDecoder = null;
            }
            ReceiveFlags = GroupMuteFlags.None;
        }

        public virtual void SpawnObject()
        {
            _owner = Role.LastOwner;
            if (_owner != null)
            {
                _defaultDecoder ??= new OpusDecoder();

                if (_owner.isLocalPlayer)
                {
                    VoiceChatMicCapture.StartRecording();
                }

                if (NetworkServer.active)
                {
                    if (VoiceChatReceivePrefs.RememberedFlags.TryGetValue(_owner.connectionToClient, out var flags))
                    {
                        ReceiveFlags = flags;
                    }
                    else
                    {
                        ReceiveFlags = GroupMuteFlags.None;
                    }
                }
            }
        }

        public bool CheckRateLimit()
        {
            return _sentPackets++ < RateLimiterTolerance;
        }

        public void ProcessMessage(VoiceMessage msg)
        {
            CurrentChannel = msg.Channel;

            if (!_receiveBufferSet)
            {
                _receiveBuffer = new float[24000];
                _receiveBufferSet = true;
            }

            int len = Decoder.Decode(msg.Data, msg.DataLength, _receiveBuffer);

            if (Owner.isLocalPlayer || VoiceChatMutes.GetFlags(Owner) == VcMuteFlags.None)
            {
                ProcessSamples(_receiveBuffer, len);
                OnSamplesReceived?.Invoke(_receiveBuffer, len);
            }
        }
    }
}