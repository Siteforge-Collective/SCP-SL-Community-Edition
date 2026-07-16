using System;
using System.Collections.Generic;
using Mirror;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp3114
{
	public class Scp3114VoiceLines : StandardSubroutine<Scp3114Role>
	{
		[Serializable]
		private class VoiceLinesDefinition
		{
			public VoiceLinesName Label;

			public AudioClip[] RandomClips;

			public float MinIdleTime;

			public float MaxDuration;

			[Range(0f, 1f)]
			public float Chance;

			private double _lastUse;

			private ushort _next;

			private List<int> _order;

			public float LastUseElapsedSeconds => 0f;

			public bool TryDrawNext(out int clipId)
			{
				clipId = default(int);
				return false;
			}

			public void Init()
			{
			}
		}

		private enum VoiceLinesName
		{
			KillSlap = 0,
			KillStrangle = 1,
			Slap = 2,
			RandomIdle = 3,
			Reveal = 4,
			EquipStart = 5,
			StartStrangle = 6
		}

		[SerializeField]
		private VoiceLinesDefinition[] _voiceLines;

		[SerializeField]
		private AudioSource _source;

		[SerializeField]
		private float _idleCycleTime;

		private float _idleRemaining;

		private byte _syncName;

		private byte _syncId;

		private bool _hasDisguise;

		protected override void Awake()
		{
		}

		private void OnStatusChanged()
		{
		}

		private void Update()
		{
		}

		private void ServerPlayConditionally(VoiceLinesName lineToPlay)
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		public override void SpawnObject()
		{
		}
	}
}
