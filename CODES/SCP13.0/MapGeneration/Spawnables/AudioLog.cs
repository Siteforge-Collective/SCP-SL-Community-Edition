using Interactables;
using Interactables.Verification;
using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration.Spawnables
{
	[RequireComponent(typeof(AudioSource))]
	public class AudioLog : NetworkBehaviour, IServerInteractable, IInteractable
	{
		private const double BaseCooldown = 3.0;

		public static readonly List<AudioLog> Instances = new();

		[SerializeField]
		[Min(0f)]
		private float _clipSkipSeconds;

		[SerializeField]
		[Min(0f)]
		private double _audioDuration;

		[SerializeField]
		private AudioSource _audioSource;

		private Transform _audioTransform;

		private double _triggerTimestamp;

        public IVerificationRule VerificationRule => StandardDistanceVerification.Default;

        public bool IsPlaying
        {
            get
            {
                if (!_audioSource.isPlaying)
                {
                    return _triggerTimestamp + BaseCooldown > NetworkTime.time;
                }
                return true;
            }
        }

        public float MaxHearingRange => _audioSource.maxDistance;

        public float ClipDuration => (float)_audioDuration - _clipSkipSeconds;

        public Vector3 PlayingLocation => _audioTransform.position;

        public void ServerInteract(ReferenceHub ply, byte colliderId)
        {
            if (!IsPlaying)
            {
                RpcPlayLog(_clipSkipSeconds);
                _triggerTimestamp = NetworkTime.time + (double)ClipDuration;
            }
        }

        [ClientRpc]
		private void RpcPlayLog(float playTime)
		{
            ClientPlayAudio(playTime);
		}


        private void ClientPlayAudio(float playTime = 0f)
        {
            if (playTime > 0f)
            {
                _audioSource.time = playTime;
            }
            _audioSource.Play();
        }

        private void ServerSyncClip(ReferenceHub hub)
        {
            if (IsPlaying)
            {
                double num = _triggerTimestamp - (double)ClipDuration;
                float num2 = (float)(NetworkTime.time - num);
                if (!(num2 >= ClipDuration))
                {
                    RpcPlayLog(num2);
                }
            }
        }

        private void Awake()
        {
            if (gameObject == null)
                return;

            _audioTransform = _audioSource.transform;

            Instances.Add(this);
            if (NetworkServer.active)
            {
                ReferenceHub.OnPlayerAdded = (Action<ReferenceHub>)Delegate.Combine(ReferenceHub.OnPlayerAdded, new Action<ReferenceHub>(ServerSyncClip));
            }
        }

        private void OnDestroy()
        {
            if (gameObject == null)
                return;

            Instances.Remove(this);
            if (NetworkServer.active)
            {
                ReferenceHub.OnPlayerAdded = (Action<ReferenceHub>)Delegate.Remove(ReferenceHub.OnPlayerAdded, new Action<ReferenceHub>(ServerSyncClip));
            }
        }
	}
}
