namespace PlayerRoles.PlayableScps.Scp173
{
	public class Scp173TantrumAbility : global::PlayerRoles.PlayableScps.Subroutines.ScpKeySubroutine<global::PlayerRoles.PlayableScps.Scp173.Scp173Role>
	{
		private const float StainedKillReward = 400f;

		private const float CooldownTime = 30f;

		private const float RayMaxDistance = 3f;

		private const float TantrumHeight = 1.25f;

		private global::PlayerRoles.PlayableScps.Scp173.Scp173ObserversTracker _observersTracker;

		[global::UnityEngine.SerializeField]
		private global::Hazards.TantrumEnvironmentalHazard _tantrumPrefab;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.LayerMask _tantrumMask;

		public readonly global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown Cooldown = new global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown();

		protected override ActionName TargetKey => ActionName.ToggleFlashlight;

		protected override void OnKeyDown()
		{
			base.OnKeyDown();
			ClientSendCmd();
		}

		protected override void Awake()
		{
			base.Awake();
			GetSubroutine<global::PlayerRoles.PlayableScps.Scp173.Scp173ObserversTracker>(out _observersTracker);
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			if (!Cooldown.IsReady || _observersTracker.IsObserved || !global::UnityEngine.Physics.Raycast(base.ScpRole.FpcModule.Position, global::UnityEngine.Vector3.down, out var hitInfo, 3f, _tantrumMask) || !global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp173CreateTantrum, base.Owner))
			{
				return;
			}
			Cooldown.Trigger(30f);
			ServerSendRpc(toAll: true);
			global::Hazards.TantrumEnvironmentalHazard tantrumEnvironmentalHazard = global::UnityEngine.Object.Instantiate(_tantrumPrefab);
			global::UnityEngine.Vector3 targetPos = hitInfo.point + global::UnityEngine.Vector3.up * 1.25f;
			tantrumEnvironmentalHazard.SynchronizedPosition = new global::RelativePositioning.RelativePosition(targetPos);
			global::Mirror.NetworkServer.Spawn(tantrumEnvironmentalHazard.gameObject);
			foreach (TeslaGate teslaGate in TeslaGateController.Singleton.TeslaGates)
			{
				if (teslaGate.PlayerInIdleRange(base.Owner))
				{
					teslaGate.TantrumsToBeDestroyed.Add(tantrumEnvironmentalHazard);
				}
			}
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			Cooldown.WriteCooldown(writer);
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			Cooldown.ReadCooldown(reader);
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			global::PlayerStatsSystem.PlayerStats.OnAnyPlayerDied += CheckDeath;
		}

		public override void ResetObject()
		{
			base.ResetObject();
			Cooldown.Clear();
			global::PlayerStatsSystem.PlayerStats.OnAnyPlayerDied -= CheckDeath;
		}

		private void CheckDeath(ReferenceHub ply, global::PlayerStatsSystem.DamageHandlerBase handler)
		{
			if (global::Mirror.NetworkServer.active && handler is global::PlayerStatsSystem.ScpDamageHandler scpDamageHandler && !(scpDamageHandler.Attacker.Hub != base.Owner) && ply.playerEffectsController.TryGetEffect<global::CustomPlayerEffects.Stained>(out var playerEffect) && playerEffect.IsEnabled)
			{
				global::PlayerRoles.PlayableScps.HumeShield.HumeShieldModuleBase humeShieldModule = base.ScpRole.HumeShieldModule;
				humeShieldModule.HsCurrent = global::UnityEngine.Mathf.Min(humeShieldModule.HsMax, humeShieldModule.HsCurrent + 400f);
			}
		}
	}
}
