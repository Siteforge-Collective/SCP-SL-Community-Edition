using System.Collections.Generic;
using PlayerRoles.FirstPersonControl.Thirdperson;
using UnityEngine;

namespace InventorySystem.Items.MarshmallowMan
{
	public class MarshmallowAudio : MonoBehaviour
	{
		[SerializeField]
		private AudioClip _holsterClip;

		[SerializeField]
		private AudioClip[] _hitClips;

		[SerializeField]
		private AudioClip[] _swingClips;

		[SerializeField]
		private AudioClip[] _footstepsClips;

		private ushort _trackedSerial;

		private Transform _audioParent;

		private static readonly List<MarshmallowAudio> Instances;

		private static bool _eventsSet;

		private const float StandardRange = 15f;

		public void Setup(ushort serial, Transform audioParent)
		{
		}

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		private void PlaySound(AudioClip clip, float range)
		{
		}

		private static void SetupEvents()
		{
		}

		private static void OnSwing(ushort serial)
		{
		}

		private static void OnHolsterRequested(ushort serial)
		{
		}

		private static void OnHit(ushort serial)
		{
		}

		private static void OnFootstepPlayed(AnimatedCharacterModel model, float loudness)
		{
		}

		private static bool TryGetInstance(ushort serial, out MarshmallowAudio inst)
		{
			inst = null;
			return false;
		}
	}
}
