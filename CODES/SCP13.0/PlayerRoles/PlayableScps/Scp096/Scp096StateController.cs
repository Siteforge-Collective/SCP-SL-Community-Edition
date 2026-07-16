using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Mirror;
using PlayerRoles.Subroutines;

namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096StateController : StandardSubroutine<Scp096Role>
	{
		private Scp096RageState _rageState;

		private Scp096AbilityState _abilityState;

		private readonly Stopwatch _rageChangeSw;

		private readonly Stopwatch _abilityChangeSw;

		public Scp096RageState RageState
		{
			get
			{
				return default(Scp096RageState);
			}
			set
			{
			}
		}

		public Scp096AbilityState AbilityState
		{
			get
			{
				return default(Scp096AbilityState);
			}
			set
			{
			}
		}

		public float LastRageUpdate => 0f;

		public float LastAbilityUpdate => 0f;

		public event Action<Scp096RageState> OnRageUpdate
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

		public event Action<Scp096AbilityState> OnAbilityUpdate
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

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		public override void ResetObject()
		{
		}

		public override void SpawnObject()
		{
		}

		public void SetRageState(Scp096RageState state)
		{
		}

		public void SetAbilityState(Scp096AbilityState state)
		{
		}
	}
}
