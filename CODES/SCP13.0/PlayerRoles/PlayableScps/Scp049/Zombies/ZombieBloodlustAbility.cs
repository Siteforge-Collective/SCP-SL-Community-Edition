using System.Diagnostics;
using GameObjectPools;
using Mirror;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp049.Zombies
{
	public class ZombieBloodlustAbility : SubroutineBase, IPoolResettable
	{
		[SerializeField]
		private float _maxViewDistance;

		private float _simulatedStareTime;

		private readonly Stopwatch _simulatedStareSw;

		public bool LookingAtTarget { get; private set; }

		public float SimulatedStare
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		private void Update()
		{
		}

		public void RefreshChaseState()
		{
		}

		private bool AnyTargets(ReferenceHub owner, Transform camera)
		{
			return false;
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		public void ResetObject()
		{
		}
	}
}
