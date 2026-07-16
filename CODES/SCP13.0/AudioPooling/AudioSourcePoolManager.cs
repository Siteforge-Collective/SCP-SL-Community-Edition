using System;
using System.Collections.Generic;
using RelativePositioning;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioPooling
{
    public class AudioSourcePoolManager : MonoBehaviour
    {
        [SerializeField]
        private CurvePreset[] _curves;

        [SerializeField]
        private ChannelPreset[] _channels;

        private static bool _initialized;

        private static AudioSourcePoolManager _singleton;

        private static readonly HashSet<AudioSource> Instances = new HashSet<AudioSource>();

        private static readonly Dictionary<FalloffType, AnimationCurve> Curves = new Dictionary<FalloffType, AnimationCurve>();

        private static readonly Dictionary<AudioMixerChannelType, AudioMixerGroup> Channels = new Dictionary<AudioMixerChannelType, AudioMixerGroup>();

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
                ChannelPreset[] channels = _channels;
                for (int i = 0; i < channels.Length; i++)
                {
                    ChannelPreset channelPreset = channels[i];
                    Channels[channelPreset.Type] = channelPreset.Group;
                }
            }
        }

        private void OnDestroy()
        {
            _initialized = false;
        }

        public static AudioSource PlaySound(AudioClip sound, Vector3 position, float maxDistance, float volume = 1f, FalloffType falloffType = FalloffType.Exponential, AudioMixerChannelType channel = AudioMixerChannelType.DefaultSfx, float spatial = 1f, bool reserved = false)
        {
            AudioSource audioSource = PreSetupSource(sound, maxDistance, falloffType, channel, volume, spatial, reserved);
            audioSource.transform.SetParent(_singleton.transform);
            audioSource.transform.position = position;
            if (audioSource.gameObject.activeInHierarchy)
            {
                audioSource.Play();
            }
            return audioSource;
        }

        public static AudioSource PlaySound(AudioClip sound, RelativePosition relativePosition, float maxDistance, float volume = 1f, FalloffType falloffType = FalloffType.Exponential, AudioMixerChannelType channel = AudioMixerChannelType.DefaultSfx, float spatial = 1f, bool reserved = false)
        {
            if (!WaypointBase.TryGetWaypoint(relativePosition.WaypointId, out var wp))
            {
                return PlaySound(sound, relativePosition.Relative, maxDistance, volume, falloffType, channel, spatial, reserved);
            }
            AudioSource audioSource = PlaySound(sound, wp.transform, maxDistance, volume, falloffType, channel, spatial, reserved);
            audioSource.transform.position = wp.GetWorldspacePosition(relativePosition.Relative);
            return audioSource;
        }

        public static AudioSource PlaySound(AudioClip sound, Transform trackedObject, float maxDistance, float volume = 1f, FalloffType falloffType = FalloffType.Exponential, AudioMixerChannelType channel = AudioMixerChannelType.DefaultSfx, float spatial = 1f, bool reserved = false)
        {
            AudioSource audioSource = PreSetupSource(sound, maxDistance, falloffType, channel, volume, spatial, reserved);
            audioSource.transform.SetParent(trackedObject);
            audioSource.transform.localPosition = Vector3.zero;
            if (audioSource.gameObject.activeInHierarchy)
            {
                audioSource.Play();
            }
            return audioSource;
        }

        public static AudioSource GetFree(bool reserved)
        {
            if (!reserved)
            {
                foreach (AudioSource instance in Instances)
                {
                    if (instance != null && !instance.isPlaying)
                    {
                        return instance;
                    }
                }
            }
            AudioSource audioSource = new GameObject().AddComponent<AudioSource>();
            if (!reserved)
            {
                Instances.Add(audioSource);
            }
            return audioSource;
        }

        public static AudioSource PreSetupSource(AudioClip sound, float maxDistance, FalloffType falloffType, AudioMixerChannelType channel, float volume, float spatial, bool reserved)
        {
            AudioSource free = GetFree(reserved);
            if (!_initialized)
            {
                free.enabled = false;
                return free;
            }
            if (!Curves.TryGetValue(falloffType, out var value))
            {
                throw new Exception("Curve for falloff type \"" + falloffType.ToString() + "\" is not defined in the AudioSourcePoolManager.");
            }
            if (!Channels.TryGetValue(channel, out var value2))
            {
                throw new Exception("Channel \"" + channel.ToString() + "\" is not defined in the AudioSourcePoolManager.");
            }
            free.enabled = true;
            free.playOnAwake = false;
            free.loop = false;
            free.dopplerLevel = 0f;
            free.volume = volume;
            free.spatialBlend = spatial;
            free.rolloffMode = AudioRolloffMode.Custom;
            free.SetCustomCurve(AudioSourceCurveType.CustomRolloff, value);
            free.maxDistance = maxDistance;
            free.outputAudioMixerGroup = value2;
            free.clip = sound;
            free.reverbZoneMix = 1f;
            free.pitch = 1f;
            free.mute = false;
            return free;
        }
    }
}
