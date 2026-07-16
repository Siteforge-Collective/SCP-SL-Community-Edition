namespace FacilitySoundtrack
{
	public class ZoneAmbientSoundtrack : global::FacilitySoundtrack.SoundtrackLayerBase
	{
		[global::System.Serializable]
		private class ZoneSoundtrack
		{
			public global::MapGeneration.FacilityZone TargetZone;

			public global::UnityEngine.AudioSource Source;

			public float VolumeScale;

			public float CrossfadeVolume { get; set; }
		}

		[global::UnityEngine.SerializeField]
		private float _fadeInSpeed;

		[global::UnityEngine.SerializeField]
		private float _fadeOutSpeed;

		[global::UnityEngine.SerializeField]
		private float _crossfadeSpeed;

		[global::UnityEngine.SerializeField]
		private global::FacilitySoundtrack.ZoneAmbientSoundtrack.ZoneSoundtrack[] _zoneSoundtracks;

		private float _weight;

		private global::MapGeneration.FacilityZone _lastZone;

		private bool IsMuted
		{
			get
			{
				if (global::PlayerRoles.Spectating.SpectatorTargetTracker.TryGetTrackedPlayer(out var hub))
				{
					return IsMutedForPlayer(hub);
				}
				if (!ReferenceHub.TryGetLocalHub(out var hub2))
				{
					return false;
				}
				return IsMutedForPlayer(hub2);
			}
		}

		public override bool Additive => true;

		public override float Weight => _weight;

		public override void UpdateVolume(float masterScale)
		{
			global::MapGeneration.RoomIdentifier roomIdentifier = global::MapGeneration.RoomIdUtils.RoomAtPosition(MainCameraController.CurrentCamera.position);
			if (roomIdentifier != null)
			{
				_lastZone = roomIdentifier.Zone;
			}
			float num = (IsMuted ? (0f - _fadeOutSpeed) : _fadeInSpeed);
			_weight = global::UnityEngine.Mathf.Clamp01(_weight + num * global::UnityEngine.Time.deltaTime);
			global::FacilitySoundtrack.ZoneAmbientSoundtrack.ZoneSoundtrack[] zoneSoundtracks = _zoneSoundtracks;
			foreach (global::FacilitySoundtrack.ZoneAmbientSoundtrack.ZoneSoundtrack zoneSoundtrack in zoneSoundtracks)
			{
				float target = ((zoneSoundtrack.TargetZone == _lastZone) ? 1 : 0);
				zoneSoundtrack.CrossfadeVolume = global::UnityEngine.Mathf.MoveTowards(zoneSoundtrack.CrossfadeVolume, target, _crossfadeSpeed * global::UnityEngine.Time.deltaTime);
				zoneSoundtrack.Source.volume = zoneSoundtrack.CrossfadeVolume * zoneSoundtrack.VolumeScale * masterScale;
			}
		}

		private bool IsMutedForPlayer(ReferenceHub hub)
		{
			global::CustomPlayerEffects.StatusEffectBase[] allEffects = hub.playerEffectsController.AllEffects;
			for (int i = 0; i < allEffects.Length; i++)
			{
				if (allEffects[i] is global::CustomPlayerEffects.ISoundtrackMutingEffect soundtrackMutingEffect && soundtrackMutingEffect.MuteSoundtrack)
				{
					return true;
				}
			}
			return false;
		}
	}
}
