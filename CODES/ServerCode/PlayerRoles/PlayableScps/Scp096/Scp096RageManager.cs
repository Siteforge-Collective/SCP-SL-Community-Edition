namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096RageManager : global::PlayerRoles.PlayableScps.Subroutines.ScpStandardSubroutine<global::PlayerRoles.PlayableScps.Scp096.Scp096Role>, global::PlayerRoles.PlayableScps.HumeShield.IHumeShieldBlocker
	{
		public const float NormalHumeRegenerationRate = 15f;

		public const float MaxRageTime = 35f;

		public const float MinimumEnrageTime = 20f;

		private const float TimePerExtraTarget = 3f;

		private const float CalmingShieldMultiplier = 2f;

		private const float EnragingShieldMultiplier = 0.5f;

		public readonly global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown HudRageDuration = new global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown();

		private global::PlayerRoles.PlayableScps.HumeShield.DynamicHumeShieldController _shieldController;

		private global::PlayerRoles.PlayableScps.Scp096.Scp096TargetsTracker _targetsTracker;

		private float _enragedTimeLeft;

		public bool HumeShieldBlocked { get; set; }

		public bool IsEnragedOrDistressed
		{
			get
			{
				if (!IsEnraged)
				{
					return IsDistressed;
				}
				return true;
			}
		}

		public bool IsEnraged => base.ScpRole.IsRageState(global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Enraged);

		public bool IsDistressed => base.ScpRole.IsRageState(global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Distressed);

		public float EnragedTimeLeft
		{
			get
			{
				return _enragedTimeLeft;
			}
			set
			{
				if (value < 0f)
				{
					value = 0f;
				}
				HudRageDuration.Remaining = value;
				_enragedTimeLeft = value;
				if (global::Mirror.NetworkServer.active && _enragedTimeLeft == 0f)
				{
					ServerEndEnrage(clearTime: false);
				}
			}
		}

		public float TotalRageTime { get; private set; }

		public void ServerEnrage(float initialDuration = 20f)
		{
			if (global::Mirror.NetworkServer.active)
			{
				base.Role.TryGetOwner(out var hub);
				if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp096Enraging, hub, initialDuration))
				{
					EnragedTimeLeft = initialDuration;
					TotalRageTime = initialDuration;
					base.ScpRole.StateController.SetRageState(global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Distressed);
					ServerIncreaseDuration(global::UnityEngine.Mathf.Max((float)_targetsTracker.Targets.Count - 3f, 0f));
				}
			}
		}

		public void ServerEndEnrage(bool clearTime = true)
		{
			if (global::Mirror.NetworkServer.active)
			{
				if (clearTime)
				{
					EnragedTimeLeft = 0f;
				}
				base.ScpRole.StateController.SetRageState(global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Calming);
				ServerSendRpc(toAll: true);
			}
		}

		public void ServerIncreaseDuration(float addedDuration = 3f)
		{
			if (global::Mirror.NetworkServer.active)
			{
				addedDuration = global::UnityEngine.Mathf.Min(addedDuration, 35f - TotalRageTime);
				TotalRageTime += addedDuration;
				EnragedTimeLeft += addedDuration;
				ServerSendRpc(toAll: true);
			}
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			global::Mirror.NetworkWriterExtensions.WriteSingle(writer, EnragedTimeLeft);
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			if (!global::Mirror.NetworkServer.active)
			{
				EnragedTimeLeft = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
			}
		}

		protected override void Awake()
		{
			base.Awake();
			_shieldController = base.ScpRole.HumeShieldModule as global::PlayerRoles.PlayableScps.HumeShield.DynamicHumeShieldController;
			GetSubroutine<global::PlayerRoles.PlayableScps.Scp096.Scp096TargetsTracker>(out _targetsTracker);
			_targetsTracker.OnTargetAdded += delegate
			{
				ServerIncreaseDuration();
			};
			base.ScpRole.StateController.OnRageUpdate += OnRageUpdate;
		}

		private void OnRageUpdate(global::PlayerRoles.PlayableScps.Scp096.Scp096RageState newState)
		{
			if (newState == global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Enraged)
			{
				HudRageDuration.Trigger(EnragedTimeLeft);
			}
			if (global::Mirror.NetworkServer.active)
			{
				float num;
				switch (newState)
				{
				case global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Enraged:
					num = 0.5f;
					HumeShieldBlocked = true;
					_shieldController.AddBlocker(this);
					break;
				case global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Calming:
					num = 2f;
					TotalRageTime = 0f;
					HumeShieldBlocked = false;
					break;
				default:
					HumeShieldBlocked = false;
					return;
				}
				global::PlayerRoles.PlayableScps.HumeShield.HumeShieldModuleBase humeShieldModule = base.ScpRole.HumeShieldModule;
				humeShieldModule.HsCurrent = global::UnityEngine.Mathf.Clamp(humeShieldModule.HsCurrent * num, 0f, humeShieldModule.HsMax);
			}
		}

		private void Update()
		{
			if (global::Mirror.NetworkServer.active)
			{
				UpdateRage();
			}
		}

		private void UpdateRage()
		{
			if (IsEnraged)
			{
				EnragedTimeLeft -= global::UnityEngine.Time.deltaTime;
			}
		}

		public override void ResetObject()
		{
			base.ResetObject();
			HudRageDuration.Clear();
			_shieldController.RegenerationRate = 15f;
			HumeShieldBlocked = false;
			_enragedTimeLeft = 0f;
			TotalRageTime = 0f;
		}
	}
}
