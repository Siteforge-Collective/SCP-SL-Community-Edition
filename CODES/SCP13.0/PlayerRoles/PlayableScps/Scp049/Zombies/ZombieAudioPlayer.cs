using System;
using System.Collections.Generic;
using System.Diagnostics;
using AudioPooling;
using Mirror;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp049.Zombies
{
	public class ZombieAudioPlayer : SubroutineBase
	{
		[Serializable]
		private class Scp0492Sound
		{
			public Scp0492SoundId Id;

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

		public enum Scp0492SoundId : byte
		{
			Growl = 0,
			AngryGrowl = 1,
			Attack = 2
		}

		private const float GrowlMaxCooldown = 7.5f;

		private const float GrowlMinCooldown = 11.25f;

		private static bool _soundsSerialized;

		private static readonly Dictionary<byte, Scp0492Sound> Sounds;

		public readonly AbilityCooldown GrowlTimer;

		[SerializeField]
		private Scp0492Sound[] _sounds;

		private ZombieBloodlustAbility _visionTracker;

		private byte _soundToSend;

		private Vector3 _lastPos;

		public void ServerGrowl()
		{
		}

		private void Update()
		{
		}

		protected override void Awake()
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public void ServerSendSound(Scp0492SoundId soundId)
		{
		}
	}
}
