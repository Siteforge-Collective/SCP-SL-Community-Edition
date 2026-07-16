using System;
using System.Collections.Generic;
using Mirror;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096AudioPlayer : StandardSubroutine<Scp096Role>
	{
		[Serializable]
		public class Scp096StateAudio
		{
			public AudioClip Audio;

			public Scp096RageState State;

			public FalloffType Falloff;

			public float MaxDistance;
		}

		[SerializeField]
		private AudioSource _rageStatesSource;

		[SerializeField]
		private AudioSource _tryNotToCrySource;

		[SerializeField]
		private float _volumeAdjustLerp;

		[SerializeField]
		private CurvePreset[] _curves;

		[SerializeField]
		private Scp096StateAudio[] _rageStatesAudioClips;

		[SerializeField]
		private AudioClip[] _lethalClips;

		[SerializeField]
		private AudioClip[] _nonLethalClips;

		[SerializeField]
		private float _lethalDistance;

		[SerializeField]
		private float _nonLethalDistance;

		[SerializeField]
		private float _pitchRandomization;

		private static bool _soundsDictionarized;

		private Scp096HitResult _syncHitSound;

		private static readonly Dictionary<Scp096RageState, Scp096StateAudio> AudioStates;

		private static readonly Dictionary<FalloffType, CurvePreset> Curves;

		private void Update()
		{
		}

		protected override void Awake()
		{
		}

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		public void Play(AudioClip clip, FalloffType falloff = FalloffType.Linear, float maxDistance = -1f)
		{
		}

		public void SetAudioState(Scp096RageState state)
		{
		}

		public void Stop()
		{
		}

		public void ServerPlayAttack(Scp096HitResult hitRes)
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		public static bool TryGetAudioForState(Scp096RageState state, out Scp096StateAudio stateAudio)
		{
			stateAudio = null;
			return false;
		}
	}
}
