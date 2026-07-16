namespace AudioPooling
{
	public class AudioSourcePoolManager : global::UnityEngine.MonoBehaviour
	{
		[global::UnityEngine.SerializeField]
		private CurvePreset[] _curves;

		[global::UnityEngine.SerializeField]
		private global::AudioPooling.ChannelPreset[] _channels;

		private static bool _initialized;

		private static global::AudioPooling.AudioSourcePoolManager _singleton;

		private static readonly global::System.Collections.Generic.HashSet<global::UnityEngine.AudioSource> Instances = new global::System.Collections.Generic.HashSet<global::UnityEngine.AudioSource>();

		private static readonly global::System.Collections.Generic.Dictionary<FalloffType, global::UnityEngine.AnimationCurve> Curves = new global::System.Collections.Generic.Dictionary<FalloffType, global::UnityEngine.AnimationCurve>();

		private static readonly global::System.Collections.Generic.Dictionary<global::AudioPooling.AudioMixerChannelType, global::UnityEngine.Audio.AudioMixerGroup> Channels = new global::System.Collections.Generic.Dictionary<global::AudioPooling.AudioMixerChannelType, global::UnityEngine.Audio.AudioMixerGroup>();

		private void Awake()
		{
			_singleton = this;
			_initialized = true;
			Instances.Clear();
			if (Curves.Count == 0)
			{
				CurvePreset[] curves = _curves;
				foreach (CurvePreset curvePreset in curves)
				{
					Curves[curvePreset.Type] = curvePreset.FalloffCurve;
				}
				global::AudioPooling.ChannelPreset[] channels = _channels;
				for (int i = 0; i < channels.Length; i++)
				{
					global::AudioPooling.ChannelPreset channelPreset = channels[i];
					Channels[channelPreset.Type] = channelPreset.Group;
				}
			}
		}

		private void OnDestroy()
		{
			_initialized = false;
		}

		public static global::UnityEngine.AudioSource PlaySound(global::UnityEngine.AudioClip sound, global::UnityEngine.Vector3 position, float maxDistance, float volume = 1f, FalloffType falloffType = FalloffType.Exponential, global::AudioPooling.AudioMixerChannelType channel = global::AudioPooling.AudioMixerChannelType.DefaultSfx, float spatial = 1f, bool reserved = false)
		{
			global::UnityEngine.AudioSource audioSource = PreSetupSource(sound, maxDistance, falloffType, channel, volume, spatial, reserved);
			audioSource.transform.SetParent(_singleton.transform);
			audioSource.transform.position = position;
			audioSource.Play();
			return audioSource;
		}

		public static global::UnityEngine.AudioSource PlaySound(global::UnityEngine.AudioClip sound, global::RelativePositioning.RelativePosition relativePosition, float maxDistance, float volume = 1f, FalloffType falloffType = FalloffType.Exponential, global::AudioPooling.AudioMixerChannelType channel = global::AudioPooling.AudioMixerChannelType.DefaultSfx, float spatial = 1f, bool reserved = false)
		{
			global::UnityEngine.AudioSource audioSource = PlaySound(sound, relativePosition.Position, maxDistance, volume, falloffType, channel, spatial, reserved);
			if (global::RelativePositioning.WaypointBase.TryGetWaypoint(relativePosition.WaypointId, out var wp))
			{
				audioSource.transform.SetParent(wp.transform);
			}
			return audioSource;
		}

		public static global::UnityEngine.AudioSource PlaySound(global::UnityEngine.AudioClip sound, global::UnityEngine.Transform trackedObject, float maxDistance, float volume = 1f, FalloffType falloffType = FalloffType.Exponential, global::AudioPooling.AudioMixerChannelType channel = global::AudioPooling.AudioMixerChannelType.DefaultSfx, float spatial = 1f, bool reserved = false)
		{
			global::UnityEngine.AudioSource audioSource = PreSetupSource(sound, maxDistance, falloffType, channel, volume, spatial, reserved);
			audioSource.transform.SetParent(trackedObject);
			audioSource.transform.localPosition = global::UnityEngine.Vector3.zero;
			audioSource.Play();
			return audioSource;
		}

		public static global::UnityEngine.AudioSource GetFree(bool reserved)
		{
			if (!reserved)
			{
				foreach (global::UnityEngine.AudioSource instance in Instances)
				{
					if (instance != null && !instance.isPlaying)
					{
						return instance;
					}
				}
			}
			global::UnityEngine.AudioSource audioSource = new global::UnityEngine.GameObject().AddComponent<global::UnityEngine.AudioSource>();
			if (!reserved)
			{
				Instances.Add(audioSource);
			}
			return audioSource;
		}

		private static global::UnityEngine.AudioSource PreSetupSource(global::UnityEngine.AudioClip sound, float maxDistance, FalloffType falloffType, global::AudioPooling.AudioMixerChannelType channel, float volume, float spatial, bool reserved)
		{
			global::UnityEngine.AudioSource free = GetFree(reserved);
			if (!_initialized)
			{
				free.enabled = false;
				return free;
			}
			if (!Curves.TryGetValue(falloffType, out var value))
			{
				throw new global::System.Exception("Curve for falloff type \"" + falloffType.ToString() + "\" is not defined in the AudioSourcePoolManager.");
			}
			if (!Channels.TryGetValue(channel, out var value2))
			{
				throw new global::System.Exception("Channel \"" + channel.ToString() + "\" is not defined in the AudioSourcePoolManager.");
			}
			free.enabled = true;
			free.playOnAwake = false;
			free.loop = false;
			free.dopplerLevel = 0f;
			free.volume = volume;
			free.spatialBlend = spatial;
			free.rolloffMode = global::UnityEngine.AudioRolloffMode.Custom;
			free.SetCustomCurve(global::UnityEngine.AudioSourceCurveType.CustomRolloff, value);
			free.maxDistance = maxDistance;
			free.outputAudioMixerGroup = value2;
			free.clip = sound;
			free.reverbZoneMix = 1f;
			free.pitch = 1f;
			return free;
		}
	}
}
