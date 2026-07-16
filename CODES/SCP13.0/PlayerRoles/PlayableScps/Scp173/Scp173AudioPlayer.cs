using System;
using System.Collections.Generic;
using System.Diagnostics;
using AudioPooling;
using Mirror;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp173
{
	public class Scp173AudioPlayer : SubroutineBase
	{
		[Serializable]
		private class Scp173Sound
		{
			public Scp173SoundId Id;

			public float MaxDistance;

			[SerializeField]
			private AudioClip[] _targetClips;

			[SerializeField]
			private AudioMixerChannelType _channel;

			[SerializeField]
			private float _volume;

			[SerializeField]
			private float _rateLimit;

			private readonly Stopwatch _sw;

			private AudioClip Random => null;

			private bool CheckRateLimit()
			{
				return false;
			}

			public void PlayLocal()
			{
			}

			public void PlayThirdperson(NetworkReader reader)
			{
			}
		}

		public enum Scp173SoundId : byte
		{
			Hit = 0,
			Teleport = 1,
			Snap = 2
		}

		[SerializeField]
		private Scp173Sound[] _sounds;

		private byte _soundToSend;

		private Vector3 _lastPos;

		private static bool _soundsDictionarized;

		private static readonly Dictionary<byte, Scp173Sound> Sounds;

		protected override void Awake()
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public void ServerSendSound(Scp173SoundId soundId)
		{
		}
	}
}
