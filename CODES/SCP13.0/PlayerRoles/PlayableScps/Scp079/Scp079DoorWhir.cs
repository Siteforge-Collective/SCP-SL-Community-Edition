using Interactables.Interobjects.DoorUtils;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079DoorWhir
	{
		private readonly Scp079DoorLockChanger _lockChanger;

		private readonly Scp079AuxManager _auxManager;

		private readonly DoorVariant _door;

		private readonly AudioSource _whirSrc;

		private bool _active;

		private bool _deactivating;

		private float _startAux;

		private const float WhirDist = 15f;

		private const float VolumeAdjustSpeed = 0.7f;

		private const float PitchLerpSpeed = 2f;

		private const float ShutdownPitch = 0.5f;

		private const float StartPitch = 0.9f;

		private const float FinalPitch = 1.8f;

		private const float MinStartAux = 5f;

		private bool Valid => false;

		public Scp079DoorWhir(Scp079Role scp079, AudioClip whirSound)
		{
		}

		private void OnUpdate()
		{
		}

		private void UpdateAudioSource()
		{
		}

		private void MuteSource()
		{
		}

		private void Destruct()
		{
		}

		~Scp079DoorWhir()
		{
		}
	}
}
