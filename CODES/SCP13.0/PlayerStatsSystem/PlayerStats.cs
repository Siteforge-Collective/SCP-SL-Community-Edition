using System;
using System.Collections.Generic;
using InventorySystem;
using Mirror;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp106;
using PlayerRoles.Ragdolls;
using PlayerRoles.Spectating;

namespace PlayerStatsSystem
{
    public class PlayerStats : NetworkBehaviour
    {
        public static readonly Type[] DefinedModules = new Type[]
        {
            typeof(HealthStat),
            typeof(AhpStat),
            typeof(StaminaStat),
            typeof(AdminFlagsStat),
            typeof(HumeShieldStat),
            typeof(VigorStat)
        };

        private ReferenceHub _hub;
        private bool _eventAssigned;
        private StatBase[] _statModules;
        private readonly Dictionary<Type, StatBase> _dictionarizedTypes = new Dictionary<Type, StatBase>();

        public static event Action<ReferenceHub, DamageHandlerBase> OnAnyPlayerDamaged;
        public static event Action<ReferenceHub, DamageHandlerBase> OnAnyPlayerDied;

        public event Action<DamageHandlerBase> OnThisPlayerDamaged;
        public event Action<DamageHandlerBase> OnThisPlayerDied;

        public StatBase[] StatModules
        {
            get
            {
                if (_statModules != null) return _statModules;
                _statModules = new StatBase[DefinedModules.Length];
                for (int i = 0; i < DefinedModules.Length; i++)
                {
                    _statModules[i] = Activator.CreateInstance(DefinedModules[i]) as StatBase;
                }
                return _statModules;
            }
        }

        private void Awake()
        {
            _hub = ReferenceHub.GetHub(gameObject);
            foreach (var stat in StatModules)
            {
                _dictionarizedTypes.Add(stat.GetType(), stat);
                stat.Init(_hub);
            }
        }

        private void Start()
        {
            if (_hub.isLocalPlayer)
            {
                PlayerRoleManager.OnRoleChanged += OnClassChanged;
                _eventAssigned = true;
            }
        }

        private void OnDestroy()
        {
            if (_eventAssigned)
            {
                PlayerRoleManager.OnRoleChanged -= OnClassChanged;
            }
        }

        public T GetModule<T>() where T : StatBase
        {
            if (_dictionarizedTypes.TryGetValue(typeof(T), out var stat))
                return (T)stat;
            throw new InvalidCastException($"Module {typeof(T).Name} not found!");
        }

        public bool TryGetModule<T>(out T module) where T : StatBase
        {
            if (_dictionarizedTypes.TryGetValue(typeof(T), out var stat))
            {
                module = (T)stat;
                return true;
            }
            module = null;
            return false;
        }

        public bool DealDamage(DamageHandlerBase handler)
        {
            if (_hub.characterClassManager.GodMode)
            {
                return false;
            }
            if (_hub.roleManager.CurrentRole is IDamageHandlerProcessingRole damageHandlerProcessingRole)
            {
                handler = damageHandlerProcessingRole.ProcessDamageHandler(handler);
            }
            DamageHandlerBase.HandlerOutput handlerOutput = handler.ApplyDamage(_hub);
            if (handlerOutput == DamageHandlerBase.HandlerOutput.Nothing)
            {
                return false;
            }
            PlayerStats.OnAnyPlayerDamaged?.Invoke(_hub, handler);
            this.OnThisPlayerDamaged?.Invoke(handler);
            if (handlerOutput == DamageHandlerBase.HandlerOutput.Death)
            {
                PlayerStats.OnAnyPlayerDied?.Invoke(_hub, handler);
                this.OnThisPlayerDied?.Invoke(handler);
                KillPlayer(handler);
            }
            return true;
        }

        private void KillPlayer(DamageHandlerBase handler)
        {
            RagdollManager.ServerSpawnRagdoll(_hub, handler);
            _hub.inventory.ServerDropEverything();

            _hub.roleManager.ServerSetRole(RoleTypeId.Spectator, RoleChangeReason.Died);

            _hub.gameConsoleTransmission.SendToClient("You died. Reason: " + handler.ServerLogsText, "yellow");

            if (_hub.roleManager.CurrentRole is SpectatorRole spectatorRole)
            {
                spectatorRole.ServerSetData(handler);
            }
        }

        private void OnClassChanged(ReferenceHub userHub, PlayerRoleBase prevRole, PlayerRoleBase newRole)
        {
            if (userHub.isLocalPlayer && UserMainInterface.Singleton != null)
            {
                UserMainInterface.Singleton.PlyStats.SetActive(true);
            }

            foreach (var stat in StatModules)
            {
                stat.ClassChanged();
            }
        }
    }
}