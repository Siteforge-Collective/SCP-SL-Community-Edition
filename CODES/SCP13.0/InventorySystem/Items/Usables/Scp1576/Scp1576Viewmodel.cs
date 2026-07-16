using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace InventorySystem.Items.Usables.Scp1576
{
	public class Scp1576Viewmodel : UsableItemViewmodel
	{
		private const ItemType Scp1576Type = ItemType.SCP1576;

		private static readonly Dictionary<ushort, float> PrevWeights;

		private static readonly Dictionary<ushort, Stopwatch> TimersBySerial;

		private static float _cachedUseTime;

		private static bool _useTimeCacheSet;

		private bool _wasCranking;

		[SerializeField]
		private int _posLayer;

		[SerializeField]
		private Material _beltMaterial;

		[SerializeField]
		private Vector2 _beltSpeed;

		[SerializeField]
		private ParticleSystem _particles;

		[SerializeField]
		private AudioSource _audioLoop;

		[SerializeField]
		private AudioClip _endRecordClip;

		[SerializeField]
		private AudioClip _rewindClip;

		[SerializeField]
		private AudioClip _startRecording;

		[SerializeField]
		private Scp1576Source _playbackSource;

		private static float UseTime => 0f;

		public override void LateUpdate()
		{
		}

		private static void PlaySound(AudioClip clip)
		{
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void OnClientStatusReceived(StatusMessage msg)
		{
		}
	}
}
