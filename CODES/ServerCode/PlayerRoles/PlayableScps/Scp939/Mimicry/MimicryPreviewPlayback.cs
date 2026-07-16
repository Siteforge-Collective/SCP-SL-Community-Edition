namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	public class MimicryPreviewPlayback : global::VoiceChat.Playbacks.VoiceChatPlaybackBase
	{
		private global::VoiceChat.Networking.PlaybackBuffer _playback;

		private bool _playbackSet;

		private int _duration;

		public override int MaxSamples
		{
			get
			{
				if (!_playbackSet)
				{
					return 0;
				}
				return _playback.Length;
			}
		}

		public bool IsEmpty
		{
			get
			{
				if (_playbackSet)
				{
					return _playback.ReadHead == _playback.WriteHead;
				}
				return true;
			}
		}

		private string SamplesToString(float samples)
		{
			return ((float)global::UnityEngine.Mathf.CeilToInt(samples / 48000f * 10f) / 10f).ToString("0.0s");
		}

		protected override float ReadSample()
		{
			return _playback.Read();
		}

		public void StartPreview(global::VoiceChat.Networking.PlaybackBuffer pb)
		{
			if (_playbackSet)
			{
				_playback.Clear();
			}
			else
			{
				_playback = new global::VoiceChat.Networking.PlaybackBuffer(pb.Buffer.Length, endlessTapeMode: true);
				_playbackSet = true;
			}
			_duration = pb.Length;
			_playback.Write(pb.Buffer, _duration);
		}

		public void StopPreview()
		{
			if (_playbackSet)
			{
				_playback.Clear();
			}
		}

		public void UpdateProgress(global::UnityEngine.UI.Slider timeline, global::TMPro.TextMeshProUGUI elapsed, global::TMPro.TextMeshProUGUI duration)
		{
			if (_playbackSet)
			{
				int num = _duration - _playback.Length;
				timeline.minValue = 0f;
				timeline.maxValue = _duration;
				timeline.value = num;
				elapsed.text = SamplesToString(num);
				duration.text = SamplesToString(_duration);
			}
		}
	}
}
