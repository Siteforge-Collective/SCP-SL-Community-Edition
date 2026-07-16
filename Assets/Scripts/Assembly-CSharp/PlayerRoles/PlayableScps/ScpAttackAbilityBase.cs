using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using AudioPooling;
using Mirror;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.PlayableScps.Subroutines;
using PlayerRoles.Spectating;
using PlayerStatsSystem;
using RelativePositioning;
using UnityEngine;
using Utils.Networking;

namespace PlayerRoles.PlayableScps
{
    public abstract class ScpAttackAbilityBase<T> : ScpKeySubroutine<T> where T : PlayerRoleBase, IFpcRole
    {
        [SerializeField]
        private float _detectionRadius;

        [SerializeField]
        private float _detectionOffset;

        [SerializeField]
        private AudioClip _killSound;

        [SerializeField]
        private AudioClip[] _hitClipsHuman;

        [SerializeField]
        private AudioClip[] _hitClipsObjects;

        private bool _attackTriggered;

        private AttackResult _syncAttack;

        private readonly Stopwatch _delaySw = new Stopwatch();

        private readonly TolerantAbilityCooldown _clientCooldown = new TolerantAbilityCooldown(0.2f);

        private readonly TolerantAbilityCooldown _serverCooldown = new TolerantAbilityCooldown(0.2f);

        private static readonly HashSet<ReferenceHub> TargettedPlayers = new HashSet<ReferenceHub>();

        private static readonly HashSet<FpcBacktracker> BacktrackedPlayers = new HashSet<FpcBacktracker>();

        private static readonly Collider[] DetectionsNonAlloc = new Collider[128];

        private static readonly CachedLayerMask DetectionMask = new CachedLayerMask("Hitbox", "Glass");

        private static readonly CachedLayerMask BlockerMask = new CachedLayerMask("Locker", "Default", "Door");

        private const int DetectionsNumber = 128;

        public TolerantAbilityCooldown Cooldown
        {
            get
            {
                if (!base.Owner.isLocalPlayer && NetworkServer.active)
                {
                    return _serverCooldown;
                }
                return _clientCooldown;
            }
        }

        public abstract float DamageAmount { get; }

        protected abstract DamageHandlerBase DamageHandler { get; }

        protected virtual float SoundRange => 13f;

        protected virtual float AttackDelay => 0f;

        protected virtual float BaseCooldown => 1f;

        protected virtual bool SelfRepeating => true;

        protected virtual bool CanTriggerAbility => _clientCooldown.IsReady;

        protected override ActionName TargetKey => ActionName.Shoot;

        protected override bool KeyPressable
        {
            get
            {
                if (!base.KeyPressable)
                    return false;
                return InventorySystem.GUI.InventoryGuiController.ItemsSafeForInteraction;
            }
        }

        private Transform PlyCam => base.Owner.PlayerCameraReference;

        private Vector3 OverlapSphereOrigin => PlyCam.position + PlyCam.forward * _detectionOffset;

        public event Action<AttackResult> OnAttacked
        {
            [CompilerGenerated]
            add
            {
                Action<AttackResult> action = _onAttacked;
                Action<AttackResult> action2;
                do
                {
                    action2 = action;
                    Action<AttackResult> value2 = (Action<AttackResult>)Delegate.Combine(action2, value);
                    action = Interlocked.CompareExchange(ref _onAttacked, value2, action2);
                }
                while (action != action2);
            }
            [CompilerGenerated]
            remove
            {
                Action<AttackResult> action = _onAttacked;
                Action<AttackResult> action2;
                do
                {
                    action2 = action;
                    Action<AttackResult> value2 = (Action<AttackResult>)Delegate.Remove(action2, value);
                    action = Interlocked.CompareExchange(ref _onAttacked, value2, action2);
                }
                while (action != action2);
            }
        }

        [CompilerGenerated]
        private Action<AttackResult> _onAttacked;

        public event Action OnTriggered
        {
            [CompilerGenerated]
            add
            {
                Action action = _onTriggered;
                Action action2;
                do
                {
                    action2 = action;
                    Action value2 = (Action)Delegate.Combine(action2, value);
                    action = Interlocked.CompareExchange(ref _onTriggered, value2, action2);
                }
                while (action != action2);
            }
            [CompilerGenerated]
            remove
            {
                Action action = _onTriggered;
                Action action2;
                do
                {
                    action2 = action;
                    Action value2 = (Action)Delegate.Remove(action2, value);
                    action = Interlocked.CompareExchange(ref _onTriggered, value2, action2);
                }
                while (action != action2);
            }
        }

        [CompilerGenerated]
        private Action _onTriggered;

        private void ServerPerformAttack()
        {
            int num = Physics.OverlapSphereNonAlloc(OverlapSphereOrigin, _detectionRadius, DetectionsNonAlloc, DetectionMask);
            _syncAttack = AttackResult.None;
            // TODO(ATKDBG): временная диагностика melee-атак SCP — удалить после починки.
            UnityEngine.Debug.LogWarning($"[ATKDBG] {GetType().Name} SrvAttack: origin={OverlapSphereOrigin} r={_detectionRadius} overlaps={num} targets={TargettedPlayers.Count}");
            for (int i = 0; i < num; i++)
            {
                if (!DetectionsNonAlloc[i].TryGetComponent<IDestructible>(out var component))
                {
                    UnityEngine.Debug.LogWarning($"[ATKDBG]   skip '{DetectionsNonAlloc[i].name}': no IDestructible");
                    continue;
                }
                if (Physics.Linecast(PlyCam.position, component.CenterOfMass, BlockerMask))
                {
                    UnityEngine.Debug.LogWarning($"[ATKDBG]   skip '{DetectionsNonAlloc[i].name}': linecast blocked");
                    continue;
                }
                if (component is HitboxIdentity hitboxIdentity && !TargettedPlayers.Remove(hitboxIdentity.TargetHub))
                {
                    UnityEngine.Debug.LogWarning($"[ATKDBG]   skip '{DetectionsNonAlloc[i].name}': hub {hitboxIdentity.TargetHub?.PlayerId} not in TargettedPlayers");
                    continue;
                }
                if (!component.Damage(DamageAmount, DamageHandler, component.CenterOfMass))
                {
                    UnityEngine.Debug.LogWarning($"[ATKDBG]   skip '{DetectionsNonAlloc[i].name}': Damage() returned false");
                    continue;
                }
                UnityEngine.Debug.LogWarning($"[ATKDBG]   HIT '{DetectionsNonAlloc[i].name}' dmg={DamageAmount}");

                OnDestructibleDamaged(component);
                _syncAttack |= AttackResult.AttackedObject;

                if (component is HitboxIdentity hitboxIdentity2)
                {
                    _syncAttack |= AttackResult.AttackedHuman;
                    if (!(hitboxIdentity2.TargetHub.playerStats.GetModule<HealthStat>().CurValue > 0f))
                    {
                        _syncAttack |= AttackResult.KilledHuman;
                    }
                }
            }
            UnityEngine.Debug.LogWarning($"[ATKDBG] {GetType().Name} SrvAttack result: {_syncAttack}");
            ServerSendRpc(toAll: true);
        }

        protected virtual void OnDestructibleDamaged(IDestructible dest)
        {
        }

        public override void ClientWriteCmd(NetworkWriter writer)
        {
            base.ClientWriteCmd(writer);
            if (_attackTriggered)
            {
                RelativePositionSerialization.WriteRelativePosition(writer, default(RelativePosition));
                return;
            }
            
            Vector3 position = base.ScpRole.FpcModule.Position;
            float num = _detectionOffset + _detectionRadius;
            float num2 = num * num;
            
            RelativePosition selfRelative = new RelativePosition(position);
            RelativePositionSerialization.WriteRelativePosition(writer, selfRelative);
            writer.WriteLowPrecisionQuaternion(new LowPrecisionQuaternion(PlyCam.rotation));

            int writtenTargets = 0;
            foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
            {
                if (allHub.roleManager.CurrentRole is HumanRole humanRole)
                {
                    Vector3 position2 = humanRole.FpcModule.Position;
                    if (!((position2 - position).sqrMagnitude > num2))
                    {
                        ReferenceHubReaderWriter.WriteReferenceHub(writer, allHub);
                        RelativePositionSerialization.WriteRelativePosition(writer, new RelativePosition(position2));
                        writtenTargets++;
                    }
                }
            }
            // TODO(ATKDBG): временная диагностика melee-атак SCP — удалить после починки.
            UnityEngine.Debug.LogWarning($"[ATKDBG] {GetType().Name} CliCmd: pos={position} selfWp={selfRelative.WaypointId} wroteTargets={writtenTargets}");
        }

        public override void ServerProcessCmd(NetworkReader reader)
        {
            base.ServerProcessCmd(reader);
            RelativePosition relativePosition = RelativePositionSerialization.ReadRelativePosition(reader);
            
            if (relativePosition.WaypointId == 0)
            {
                // TODO(ATKDBG): временная диагностика melee-атак SCP — удалить после починки.
                UnityEngine.Debug.LogWarning($"[ATKDBG] {GetType().Name} SrvCmd: trigger packet (wp=0)");
                _attackTriggered = true;
                ServerSendRpc(toAll: true);
            }
            else
            {
                if (!_serverCooldown.TolerantIsReady && !base.Owner.isLocalPlayer)
                {
                    UnityEngine.Debug.LogWarning($"[ATKDBG] {GetType().Name} SrvCmd: rejected by server cooldown (remaining={_serverCooldown.Remaining:F2})");
                    return;
                }
                
                _attackTriggered = false;
                Vector3 position = relativePosition.Position;
                Quaternion value = reader.ReadLowPrecisionQuaternion().Value;
                
                BacktrackedPlayers.Add(new FpcBacktracker(base.Owner, position, value));
                
                while (reader.Position < reader.Capacity)
                {
                    ReferenceHub referenceHub = ReferenceHubReaderWriter.ReadReferenceHub(reader);
                    RelativePosition relativePosition2 = RelativePositionSerialization.ReadRelativePosition(reader);
                    
                    if (!(referenceHub == null) && referenceHub.roleManager.CurrentRole is HumanRole)
                    {
                        BacktrackedPlayers.Add(new FpcBacktracker(referenceHub, relativePosition2.Position));
                        TargettedPlayers.Add(referenceHub);
                    }
                }
                
                UnityEngine.Debug.LogWarning($"[ATKDBG] {GetType().Name} SrvCmd: attack packet, claimedPos={position}, parsedTargets={TargettedPlayers.Count}");

                // Listen-server only: the host player's own hitbox colliders are disabled
                // (CharacterModel.SpawnObject -> SetColliders(!isLocalPlayer)), so the overlap
                // sphere below can never detect the host. Briefly re-enable them, same pattern
                // as ExplosionGrenade.Explode / StandardHitregBase.ServerProcessShot.
                // Skipped when the host itself is the attacker: the victim is remote (colliders
                // already on) and enabling the host's own boxes would only pollute its attack.
                ReferenceHub.TryGetHostHub(out ReferenceHub hostHub);
                bool restoreHostHitboxes = !base.Owner.isLocalPlayer && HitboxIdentity.SetOwnHitboxes(hostHub, true);

                ServerPerformAttack();

                if (restoreHostHitboxes)
                {
                    HitboxIdentity.SetOwnHitboxes(hostHub, false);
                }

                Utils.NonAllocLINQ.HashsetExtensions.ForEach(BacktrackedPlayers, delegate(FpcBacktracker x)
                {
                    x.RestorePosition();
                });
                
                _serverCooldown.Trigger(BaseCooldown);
                BacktrackedPlayers.Clear();
                TargettedPlayers.Clear();
                ServerSendRpc(toAll: true);
            }
        }

        public override void ServerWriteRpc(NetworkWriter writer)
        {
            base.ServerWriteRpc(writer);
            if (!_attackTriggered)
            {
                writer.WriteByte((byte)_syncAttack);
            }
        }

        public override void ClientProcessRpc(NetworkReader reader)
        {
            base.ClientProcessRpc(reader);
            
            if (reader.Position >= reader.Capacity)
            {
                if (!base.Owner.isLocalPlayer)
                {
                    _clientCooldown.Trigger(BaseCooldown);
                    this._onTriggered?.Invoke();
                }
                return;
            }
            
            _syncAttack = (AttackResult)reader.ReadByte();
            // TODO(ATKDBG): временная диагностика melee-атак SCP — удалить после починки.
            UnityEngine.Debug.LogWarning($"[ATKDBG] {GetType().Name} CliRpc: result={_syncAttack}");
            this._onAttacked?.Invoke(_syncAttack);
            
            if (_syncAttack != AttackResult.None && (base.Owner.isLocalPlayer || SpectatorNetworking.IsLocallySpectated(base.Owner)))
            {
                Hitmarker.PlayHitmarker(1f);
            }
            
            if (HasFlagFast(AttackResult.KilledHuman) && _killSound != null)
            {
                AudioSourcePoolManager.PlaySound(_killSound, base.transform, SoundRange);
            }
            else if (HasFlagFast(AttackResult.AttackedHuman) && _hitClipsHuman.Length != 0)
            {
                AudioSourcePoolManager.PlaySound(_hitClipsHuman.RandomItem(), base.transform, SoundRange);
            }
            else if (HasFlagFast(AttackResult.AttackedObject) && _hitClipsObjects.Length != 0)
            {
                AudioSourcePoolManager.PlaySound(_hitClipsObjects.RandomItem(), base.transform, SoundRange);
            }
        }

        public override void ResetObject()
        {
            base.ResetObject();
            _attackTriggered = false;
            _delaySw.Reset();
            _clientCooldown.Clear();
            _serverCooldown.Clear();
            TargettedPlayers.Clear();
            BacktrackedPlayers.Clear();
        }

        protected override void Update()
        {
            base.Update();

            if (base.Owner != null && base.Owner.isLocalPlayer)
                OnClientUpdate();
        }

        protected virtual void OnClientUpdate()
        {
            if (_attackTriggered)
            {
                if (_delaySw.Elapsed.TotalSeconds >= AttackDelay)
                {
                    _attackTriggered = false;
                    ClientSendCmd();
                }
                return;
            }

            // ISIL: обе точки вызова передают константу true — замах шлётся всегда,
            // реальный удар уходит вторым пакетом после AttackDelay (для зомби — следующий кадр).
            if (SelfRepeating && IsKeyHeld && CanTriggerAbility)
                ClientPerformAttack();
        }

        protected override void OnKeyDown()
        {
            base.OnKeyDown();
            
            if (_attackTriggered)
                return;
            
            if (SelfRepeating)
                return;
            
            if (!CanTriggerAbility)
                return;
            
            ClientPerformAttack();
        }

        protected virtual void ClientPerformAttack(bool attackTriggered = true)
        {
            _attackTriggered = attackTriggered;
            _delaySw.Restart();
            _clientCooldown.Trigger(BaseCooldown);
            UnityEngine.Debug.LogWarning($"[CDBAR] ClientPerformAttack: {GetType().Name} cd#{_clientCooldown.GetHashCode()} baseCd={BaseCooldown} isReadyAfterTrigger={_clientCooldown.IsReady} remaining={_clientCooldown.Remaining:F2}");
            base.ClientSendCmd();
            this._onTriggered?.Invoke();
        }

        private bool HasFlagFast(AttackResult flag)
        {
            return (_syncAttack & flag) == flag;
        }

        private void OnDrawGizmosSelected()
        {
            if (base.Owner == null)
                return;
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(OverlapSphereOrigin, _detectionRadius);
        }
    }
}
