using Hazards;
using Mirror;
using PlayerRoles.Subroutines;
using PlayerStatsSystem;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp173
{
	public class Scp173TantrumAbility : KeySubroutine<Scp173Role>
	{
		private const float StainedKillReward = 400f;

		private const float CooldownTime = 30f;

		private const float RayMaxDistance = 3f;

		private const float TantrumHeight = 1.25f;

		public readonly DynamicAbilityCooldown Cooldown;

		[SerializeField]
		private TantrumEnvironmentalHazard _tantrumPrefab;

		[SerializeField]
		private LayerMask _tantrumMask;

		private Scp173ObserversTracker _observersTracker;

		private Scp173BlinkTimer _blinkTimer;

		protected override ActionName TargetKey => default(ActionName);

		protected override void OnKeyDown()
		{
		}

		protected override void Awake()
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

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		private void CheckDeath(ReferenceHub ply, DamageHandlerBase handler)
		{
		}
	}
}
