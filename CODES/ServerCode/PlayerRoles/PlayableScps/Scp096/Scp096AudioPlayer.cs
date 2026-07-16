namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096AudioPlayer : global::PlayerRoles.PlayableScps.Subroutines.ScpStandardSubroutine<global::PlayerRoles.PlayableScps.Scp096.Scp096Role>
	{
		[global::System.Serializable]
		public class Scp096StateAudio
		{
			public global::UnityEngine.AudioClip Audio;

			public global::PlayerRoles.PlayableScps.Scp096.Scp096RageState State;

			public FalloffType Falloff;

			public float MaxDistance;
		}

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioSource _rageStatesSource;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioSource _tryNotToCrySource;

		[global::UnityEngine.SerializeField]
		private float _volumeAdjustLerp;

		[global::UnityEngine.SerializeField]
		private CurvePreset[] _curves;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp096.Scp096AudioPlayer.Scp096StateAudio[] _rageStatesAudioClips;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip[] _lethalClips;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip[] _nonLethalClips;

		[global::UnityEngine.SerializeField]
		private float _lethalDistance;

		[global::UnityEngine.SerializeField]
		private float _nonLethalDistance;

		[global::UnityEngine.SerializeField]
		private float _pitchRandomization;

		private static bool _soundsDictionarized = false;

		private global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult _syncHitSound;

		private static readonly global::System.Collections.Generic.Dictionary<global::PlayerRoles.PlayableScps.Scp096.Scp096RageState, global::PlayerRoles.PlayableScps.Scp096.Scp096AudioPlayer.Scp096StateAudio> AudioStates = new global::System.Collections.Generic.Dictionary<global::PlayerRoles.PlayableScps.Scp096.Scp096RageState, global::PlayerRoles.PlayableScps.Scp096.Scp096AudioPlayer.Scp096StateAudio>();

		private static readonly global::System.Collections.Generic.Dictionary<FalloffType, CurvePreset> Curves = new global::System.Collections.Generic.Dictionary<FalloffType, CurvePreset>();

		private void Update()
		{
			bool flag = base.ScpRole.IsAbilityState(global::PlayerRoles.PlayableScps.Scp096.Scp096AbilityState.TryingNotToCry);
			float num = global::UnityEngine.Mathf.Lerp(_tryNotToCrySource.volume, flag ? 1 : 0, global::UnityEngine.Time.deltaTime * _volumeAdjustLerp);
			_tryNotToCrySource.volume = num;
			_rageStatesSource.volume = 1f - num;
		}

		protected override void Awake()
		{
			base.Awake();
			base.ScpRole.StateController.OnRageUpdate += SetAudioState;
			if (!_soundsDictionarized)
			{
				global::Utils.NonAllocLINQ.DictionaryExtensions.FromArray(Curves, _curves, (CurvePreset x) => x.Type);
				global::Utils.NonAllocLINQ.DictionaryExtensions.FromArray(AudioStates, _rageStatesAudioClips, (global::PlayerRoles.PlayableScps.Scp096.Scp096AudioPlayer.Scp096StateAudio x) => x.State);
				_soundsDictionarized = true;
			}
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			SetAudioState(base.ScpRole.StateController.RageState);
		}

		public override void ResetObject()
		{
			base.ResetObject();
			_tryNotToCrySource.volume = 0f;
			_rageStatesSource.volume = 0f;
		}

		public void Play(global::UnityEngine.AudioClip clip, FalloffType falloff = FalloffType.Linear, float maxDistance = -1f)
		{
			if (Curves.TryGetValue(falloff, out var value))
			{
				_rageStatesSource.SetCustomCurve(global::UnityEngine.AudioSourceCurveType.CustomRolloff, value.FalloffCurve);
				if (maxDistance > 0f)
				{
					_rageStatesSource.maxDistance = maxDistance;
				}
				_rageStatesSource.clip = clip;
				_rageStatesSource.Play();
			}
		}

		public void SetAudioState(global::PlayerRoles.PlayableScps.Scp096.Scp096RageState state)
		{
			if (AudioStates.TryGetValue(state, out var value) && !(_rageStatesSource.clip == value.Audio))
			{
				Play(value.Audio, value.Falloff, value.MaxDistance);
			}
		}

		public void Stop()
		{
			if (_rageStatesSource.isPlaying)
			{
				_rageStatesSource.Stop();
			}
			_rageStatesSource.clip = null;
		}

		public void ServerPlayAttack(global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult hitRes)
		{
			_syncHitSound = hitRes;
			ServerSendRpc(toAll: true);
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			writer.WriteByte((byte)_syncHitSound);
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult scp096HitResult = (global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult)reader.ReadByte();
			if ((scp096HitResult & global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult.Human) != global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult.None)
			{
				bool num = (scp096HitResult & global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult.Lethal) == global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult.Lethal;
				float maxDistance = (num ? _lethalDistance : _nonLethalDistance);
				global::UnityEngine.AudioClip[] array = (num ? _lethalClips : _nonLethalClips);
				global::AudioPooling.AudioMixerChannelType channel = (num ? global::AudioPooling.AudioMixerChannelType.NoDucking : global::AudioPooling.AudioMixerChannelType.DefaultSfx);
				float pitch = global::UnityEngine.Random.Range(1f - _pitchRandomization, 1f + _pitchRandomization);
				float spatial = ((!base.Owner.isLocalPlayer) ? 1 : 0);
				global::AudioPooling.AudioSourcePoolManager.PlaySound(array.RandomItem(), base.transform, maxDistance, 1f, FalloffType.Exponential, channel, spatial).pitch = pitch;
			}
		}
	}
}
