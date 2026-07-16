namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	public class EnvMimicrySequence : global::UnityEngine.ScriptableObject
	{
		[global::System.Serializable]
		private class Sound
		{
			public float Range;

			public float Duration;

			public global::UnityEngine.Vector2Int Repeat;

			public global::UnityEngine.AudioClip[] Clips;

			public global::AudioPooling.AudioMixerChannelType Channel;
		}

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp939.Mimicry.EnvMimicrySequence.Sound[] _sounds;

		private global::PlayerRoles.PlayableScps.Scp939.Mimicry.EnvMimicrySequence.Sound _currentlyPlayed;

		private readonly global::System.Collections.Generic.Queue<global::PlayerRoles.PlayableScps.Scp939.Mimicry.EnvMimicrySequence.Sound> _queuedSounds = new global::System.Collections.Generic.Queue<global::PlayerRoles.PlayableScps.Scp939.Mimicry.EnvMimicrySequence.Sound>();

		public void EnqueueAll()
		{
			_sounds.ForEach(EnqueueSound);
		}

		public bool UpdateSequence(global::UnityEngine.Transform mimicPoint)
		{
			if (_currentlyPlayed == null)
			{
				if (!CollectionExtensions.TryDequeue(_queuedSounds, out var element))
				{
					return false;
				}
				global::AudioPooling.AudioSourcePoolManager.PlaySound(element.Clips.RandomItem(), mimicPoint, element.Range, 1f, FalloffType.Exponential, element.Channel);
				_currentlyPlayed = element;
			}
			_currentlyPlayed.Duration -= global::UnityEngine.Time.deltaTime;
			if (_currentlyPlayed.Duration <= 0f)
			{
				_currentlyPlayed = null;
			}
			return true;
		}

		private void EnqueueSound(global::PlayerRoles.PlayableScps.Scp939.Mimicry.EnvMimicrySequence.Sound s)
		{
			int num = global::UnityEngine.Random.Range(s.Repeat.x, s.Repeat.y + 1);
			while (num-- > 0)
			{
				global::PlayerRoles.PlayableScps.Scp939.Mimicry.EnvMimicrySequence.Sound item = new global::PlayerRoles.PlayableScps.Scp939.Mimicry.EnvMimicrySequence.Sound
				{
					Clips = s.Clips,
					Channel = s.Channel,
					Duration = s.Duration,
					Range = s.Range
				};
				_queuedSounds.Enqueue(item);
			}
		}
	}
}
