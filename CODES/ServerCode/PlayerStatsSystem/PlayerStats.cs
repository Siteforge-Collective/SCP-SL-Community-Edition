namespace PlayerStatsSystem
{
	public class PlayerStats : global::Mirror.NetworkBehaviour
	{
		public global::PlayerStatsSystem.StatBase[] StatModules = new global::PlayerStatsSystem.StatBase[5]
		{
			new global::PlayerStatsSystem.HealthStat(),
			new global::PlayerStatsSystem.AhpStat(),
			new global::PlayerStatsSystem.StaminaStat(),
			new global::PlayerStatsSystem.AdminFlagsStat(),
			new global::PlayerStatsSystem.HumeShieldStat()
		};

		private ReferenceHub _hub;

		private bool _eventAssigned;

		private readonly global::System.Collections.Generic.Dictionary<global::System.Type, global::PlayerStatsSystem.StatBase> _dictionarizedTypes = new global::System.Collections.Generic.Dictionary<global::System.Type, global::PlayerStatsSystem.StatBase>();

		public event global::System.Action<global::PlayerStatsSystem.DamageHandlerBase> OnThisPlayerDamaged = delegate
		{
		};

		public event global::System.Action<global::PlayerStatsSystem.DamageHandlerBase> OnThisPlayerDied = delegate
		{
		};

		public static event global::System.Action<ReferenceHub, global::PlayerStatsSystem.DamageHandlerBase> OnAnyPlayerDamaged;

		public static event global::System.Action<ReferenceHub, global::PlayerStatsSystem.DamageHandlerBase> OnAnyPlayerDied;

		private void Awake()
		{
			global::PlayerStatsSystem.StatBase[] statModules = StatModules;
			foreach (global::PlayerStatsSystem.StatBase statBase in statModules)
			{
				_dictionarizedTypes.Add(statBase.GetType(), statBase);
			}
			_hub = ReferenceHub.GetHub(base.gameObject);
			statModules = StatModules;
			for (int i = 0; i < statModules.Length; i++)
			{
				statModules[i].Init(_hub);
			}
		}

		private void Start()
		{
			if (_hub.isLocalPlayer)
			{
				global::PlayerRoles.PlayerRoleManager.OnRoleChanged += OnClassChanged;
				_eventAssigned = true;
			}
		}

		private void OnDestroy()
		{
			if (_eventAssigned)
			{
				global::PlayerRoles.PlayerRoleManager.OnRoleChanged -= OnClassChanged;
				_eventAssigned = false;
			}
		}

		private void Update()
		{
			global::PlayerStatsSystem.StatBase[] statModules = StatModules;
			for (int i = 0; i < statModules.Length; i++)
			{
				statModules[i].Update();
			}
		}

		public T GetModule<T>() where T : global::PlayerStatsSystem.StatBase
		{
			return _dictionarizedTypes[typeof(T)] as T;
		}

		public bool TryGetModule<T>(out T module) where T : global::PlayerStatsSystem.StatBase
		{
			if (_dictionarizedTypes.TryGetValue(typeof(T), out var value) && value is T val)
			{
				module = val;
				return true;
			}
			module = null;
			return false;
		}

		public bool DealDamage(global::PlayerStatsSystem.DamageHandlerBase handler)
		{
			if (_hub.characterClassManager.GodMode)
			{
				return false;
			}
			if (_hub.roleManager.CurrentRole is global::PlayerRoles.IDamageHandlerProcessingRole damageHandlerProcessingRole)
			{
				handler = damageHandlerProcessingRole.ProcessDamageHandler(handler);
			}
			global::PlayerStatsSystem.DamageHandlerBase damageHandlerBase = handler;
			if (damageHandlerBase != null && damageHandlerBase is global::PlayerStatsSystem.AttackerDamageHandler attackerDamageHandler)
			{
				global::PlayerStatsSystem.AttackerDamageHandler attackerDamageHandler2 = attackerDamageHandler;
				if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerDamage, _hub, attackerDamageHandler2.Attacker.Hub, handler))
				{
					return false;
				}
			}
			else if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerDamage, _hub, null, handler))
			{
				return false;
			}
			global::PlayerStatsSystem.DamageHandlerBase.HandlerOutput handlerOutput = handler.ApplyDamage(_hub);
			if (handlerOutput == global::PlayerStatsSystem.DamageHandlerBase.HandlerOutput.Nothing)
			{
				return false;
			}
			global::PlayerStatsSystem.PlayerStats.OnAnyPlayerDamaged?.Invoke(_hub, handler);
			this.OnThisPlayerDamaged?.Invoke(handler);
			if (handlerOutput == global::PlayerStatsSystem.DamageHandlerBase.HandlerOutput.Death)
			{
				damageHandlerBase = handler;
				if (damageHandlerBase != null && damageHandlerBase is global::PlayerStatsSystem.AttackerDamageHandler attackerDamageHandler3)
				{
					global::PlayerStatsSystem.AttackerDamageHandler attackerDamageHandler4 = attackerDamageHandler3;
					if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerDying, _hub, attackerDamageHandler4.Attacker.Hub, handler))
					{
						return false;
					}
				}
				else if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerDying, _hub, null, handler))
				{
					return false;
				}
				global::PlayerStatsSystem.PlayerStats.OnAnyPlayerDied?.Invoke(_hub, handler);
				this.OnThisPlayerDied?.Invoke(handler);
				KillPlayer(handler);
				damageHandlerBase = handler;
				if (damageHandlerBase != null && damageHandlerBase is global::PlayerStatsSystem.AttackerDamageHandler attackerDamageHandler5)
				{
					global::PlayerStatsSystem.AttackerDamageHandler attackerDamageHandler6 = attackerDamageHandler5;
					global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerDeath, _hub, attackerDamageHandler6.Attacker.Hub, handler);
				}
				else
				{
					global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerDeath, _hub, null, handler);
				}
			}
			return true;
		}

		private void KillPlayer(global::PlayerStatsSystem.DamageHandlerBase handler)
		{
			global::PlayerRoles.Ragdolls.RagdollManager.ServerSpawnRagdoll(_hub, handler);
			global::InventorySystem.InventoryExtensions.ServerDropEverything(_hub.inventory);
			_hub.roleManager.ServerSetRole(global::PlayerRoles.RoleTypeId.Spectator, global::PlayerRoles.RoleChangeReason.Died);
			_hub.gameConsoleTransmission.SendToClient("You died. Reason: " + handler.ServerLogsText, "yellow");
			if (_hub.roleManager.CurrentRole is global::PlayerRoles.Spectating.SpectatorRole spectatorRole)
			{
				spectatorRole.ServerSetData(handler);
			}
		}

		private void OnClassChanged(ReferenceHub userHub, global::PlayerRoles.PlayerRoleBase prevRole, global::PlayerRoles.PlayerRoleBase newRole)
		{
			global::PlayerStatsSystem.StatBase[] statModules = userHub.playerStats.StatModules;
			for (int i = 0; i < statModules.Length; i++)
			{
				statModules[i].ClassChanged();
			}
		}

		static PlayerStats()
		{
			global::PlayerStatsSystem.PlayerStats.OnAnyPlayerDamaged = delegate
			{
			};
			global::PlayerStatsSystem.PlayerStats.OnAnyPlayerDied = delegate
			{
			};
		}

		private void MirrorProcessed()
		{
		}
	}
}
