using GameObjectPools;
using Mirror;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp173
{
	public class Scp173BlinkTimer : SubroutineBase, IPoolResettable
	{
		private const float CooldownBaseline = 3f;

		private const float CooldownPerObserver = 0f;

		private const float BreakneckCooldownMultiplier = 0.5f;

		public const float SustainTime = 2f;

		private Scp173ObserversTracker _observers;

		private Scp173MovementModule _fpcModule;

		private Scp173BreakneckSpeedsAbility _breakneckSpeedsAbility;

		private float _totalCooldown;

		private double _initialStopTime;

		private double _endSustainTime;

		private float TotalCooldown => 0f;

		private float TotalCooldownServer => 0f;

		private float RemainingSustain => 0f;

		public float RemainingBlinkCooldown => 0f;

		public float RemainingSustainPercent => 0f;

		public bool AbilityReady => false;

		protected override void Awake()
		{
		}

		private void OnObserversChanged(int prev, int current)
		{
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

		public void ServerBlink(Vector3 pos)
		{
		}
	}
}
