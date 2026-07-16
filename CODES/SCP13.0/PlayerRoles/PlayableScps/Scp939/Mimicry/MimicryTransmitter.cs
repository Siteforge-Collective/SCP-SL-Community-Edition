using Mirror;
using PlayerRoles.Subroutines;
using VoiceChat.Networking;

namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	public class MimicryTransmitter : StandardSubroutine<Scp939Role>
	{
		private PlaybackBuffer _copierPlayback;

		private PlaybackBuffer _senderPlayback;

		private int _playbackSize;

		private int _allowedSamples;

		private int _samplesPerSecond;

		private const int HeadSamples = 1920;

		public bool IsTransmitting => false;

		protected override void Awake()
		{
		}

		public void SendVoice(PlaybackBuffer pb, int startSample, int maxLength)
		{
		}

		public void StopTransmission()
		{
		}

		public override void ServerProcessCmd(NetworkReader reader)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		public override void ResetObject()
		{
		}

		private void Update()
		{
		}
	}
}
