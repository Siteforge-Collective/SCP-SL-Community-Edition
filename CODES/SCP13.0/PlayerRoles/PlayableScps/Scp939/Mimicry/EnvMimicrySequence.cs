using System;
using System.Collections.Generic;
using AudioPooling;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	public class EnvMimicrySequence : ScriptableObject
	{
		[Serializable]
		private class Sound
		{
			public float Range;

			public float Duration;

			public Vector2Int Repeat;

			public AudioClip[] Clips;

			public AudioMixerChannelType Channel;
		}

		[SerializeField]
		private Sound[] _sounds;

		private Sound _currentlyPlayed;

		private readonly Queue<Sound> _queuedSounds;

		public void EnqueueAll(int randomSeed)
		{
		}

		public bool UpdateSequence(Transform mimicPoint)
		{
			return false;
		}

		private void EnqueueSound(Sound s)
		{
		}
	}
}
