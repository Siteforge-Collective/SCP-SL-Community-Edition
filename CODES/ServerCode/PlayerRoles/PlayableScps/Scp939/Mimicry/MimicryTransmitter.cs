namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	public class MimicryTransmitter : global::PlayerRoles.PlayableScps.Subroutines.ScpStandardSubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939Role>
	{
		private global::VoiceChat.Networking.PlaybackBuffer _copierPlayback;

		private global::VoiceChat.Networking.PlaybackBuffer _senderPlayback;

		private int _playbackSize;

		private int _allowedSamples;

		private int _samplesPerSecond;

		private const int HeadSamples = 1920;

		protected override void Awake()
		{
			base.Awake();
			_samplesPerSecond = 48000;
		}

		public void SendVoice(global::VoiceChat.Networking.PlaybackBuffer pb)
		{
			pb.Reorganize();
			int num = pb.Buffer.Length;
			if (_playbackSize < num)
			{
				_copierPlayback = new global::VoiceChat.Networking.PlaybackBuffer(num);
				_senderPlayback = new global::VoiceChat.Networking.PlaybackBuffer(num);
				_playbackSize = num;
			}
			else
			{
				_copierPlayback.Clear();
				_senderPlayback.Clear();
			}
			_copierPlayback.Write(pb.Buffer, pb.Length);
			_allowedSamples = 1920;
		}

		public override void ResetObject()
		{
			base.ResetObject();
			_copierPlayback?.Clear();
			_senderPlayback?.Clear();
		}

		private void Update()
		{
			if (base.Owner.isLocalPlayer && _playbackSize != 0)
			{
				_allowedSamples += global::UnityEngine.Mathf.CeilToInt(global::UnityEngine.Time.deltaTime * (float)_samplesPerSecond);
				int num = global::UnityEngine.Mathf.Min(_allowedSamples, _copierPlayback.Length);
				if (num > 0)
				{
					_copierPlayback.ReadTo(_senderPlayback.Buffer, num, _senderPlayback.WriteHead);
					_senderPlayback.WriteHead += num;
				}
				_allowedSamples = 0;
				global::VoiceChat.Networking.VoiceTransceiver.ClientSendData(_senderPlayback, global::VoiceChat.VoiceChatChannel.Mimicry, 1);
			}
		}
	}
}
