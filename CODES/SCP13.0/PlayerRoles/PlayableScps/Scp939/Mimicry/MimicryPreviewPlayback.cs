using VoiceChat.Networking;
using VoiceChat.Playbacks;

namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	public class MimicryPreviewPlayback : VoiceChatPlaybackBase
	{
		private PlaybackBuffer _playback;

		private bool _playbackSet;

		public override int MaxSamples => 0;

		public bool IsEmpty => false;

		protected override float ReadSample()
		{
			return 0f;
		}

		public void StartPreview(PlaybackBuffer pb, int startIndex, int length)
		{
		}

		public void StopPreview()
		{
		}
	}
}
