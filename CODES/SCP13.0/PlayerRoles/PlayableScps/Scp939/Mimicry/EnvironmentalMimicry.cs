using System;
using System.Runtime.CompilerServices;
using Mirror;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	public class EnvironmentalMimicry : StandardSubroutine<Scp939Role>
	{
		private byte _syncOption;

		private bool _hasSound;

		private EnvMimicrySequence _currentlyPlayed;

		private MimicPointController _mimicPoint;

		[SerializeField]
		private float _activationCooldown;

		public readonly AbilityCooldown Cooldown;

		[field: SerializeField]
		public EnvMimicrySequence[] Sequences { get; private set; }

		public string CooldownText => null;

		public event Action OnSoundPlayed
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		protected override void Awake()
		{
		}

		public override void ResetObject()
		{
		}

		public void ClientSelect(EnvMimicrySequence sequence)
		{
		}

		public override void ClientWriteCmd(NetworkWriter writer)
		{
		}

		public override void ServerProcessCmd(NetworkReader reader)
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		private void Update()
		{
		}
	}
}
