using System;
using CentralAuth;
using CustomPlayerEffects;
using GameObjectPools;
using Mirror;
using PlayerRoles.SpawnData;
using PlayerRoles.Voice;
using PlayerStatsSystem;
using RelativePositioning;
using UnityEngine;

namespace PlayerRoles.Spectating
{
    public class SpectatorRole : PlayerRoleBase, IPrivateSpawnDataWriter, IHealthbarRole, ISpawnDataReader, IAdvancedCameraController, ICameraController, IPoolSpawnable, IVoiceRole, IViewmodelRole, IAmbientLightRole
    {
        public SpectatorTargetTracker TrackerPrefab;
        public const float SpawnHeight = 6000f;

        private Transform _transformToRestore;
        private DamageHandlerBase _damageHandler;

        public override RoleTypeId RoleTypeId => RoleTypeId.Spectator;
        public override Team Team => Team.Dead;
        public override Color RoleColor => Color.white;
        public float VerticalRotation => SpectatorTargetTracker.CurrentOffset.rotation.x;
        public float HorizontalRotation => SpectatorTargetTracker.CurrentOffset.rotation.y;
        public float RollRotation => SpectatorTargetTracker.CurrentOffset.rotation.z;
        public Vector3 CameraPosition => SpectatorTargetTracker.CurrentOffset.position;

        public virtual bool ReadyToRespawn
        {
            get
            {
                if (TryGetOwner(out var hub))
                {
                    return hub.authManager.InstanceMode != ClientInstanceMode.DedicatedServer;
                }
                return false;
            }
        }

        [field: SerializeField]
        public VoiceModuleBase VoiceModule { get; private set; }

        public RelativePosition DeathPosition { get; private set; }
        public uint SyncedSpectatedNetId { get; internal set; }
        public float MaxHealth => 0f;

        public PlayerStats TargetStats
        {
            get
            {
                if (!SpectatorTargetTracker.TryGetTrackedPlayer(out var hub)) return null;
                return (hub.roleManager.CurrentRole is IHealthbarRole healthbarRole) ? healthbarRole.TargetStats : null;
            }
        }

        public float AmbientLight
        {
            get
            {
                if (!SpectatorTargetTracker.TryGetTrackedPlayer(out var hub) || !(hub.roleManager.CurrentRole is IAmbientLightRole ambientLightRole))
                {
                    return InsufficientLighting.DefaultIntensity;
                }
                return ambientLightRole.AmbientLight;
            }
        }

        public bool InsufficientLight => false;

        public override void DisableRole(RoleTypeId newRole)
        {
            base.DisableRole(newRole);
            _damageHandler = null;
            if (_transformToRestore != null)
            {
                _transformToRestore.position = DeathPosition.Position;
                _transformToRestore = null;
            }
        }

        public void SpawnObject()
        {
            if (!TryGetOwner(out var hub))
            {
                throw new InvalidOperationException("Spectator role failed to spawn - owner is null");
            }

            Transform transform = hub.transform;
            DeathPosition = new RelativePosition(transform.position);
            transform.position = Vector3.up * SpawnHeight;
            SyncedSpectatedNetId = 0u;

            if (NetworkServer.active || hub.isLocalPlayer)
            {
                _transformToRestore = transform;
            }
        }

        public void WritePrivateSpawnData(NetworkWriter writer)
        {
            if (_damageHandler != null)
            {
                _damageHandler.WriteDeathScreen(writer);
            }
            else
            {
                writer.WriteByte(0);
            }
            _damageHandler = null;
        }

        public void ReadSpawnData(NetworkReader reader)
        {
            if (!base.IsLocalPlayer) return;

            byte deathScreenType = reader.ReadByte();

            switch (deathScreenType)
            {
                case 1:
                    StartScreen.Show(this);
                    break;
                case 2:
                    string attackerName = reader.ReadString();
                    YouWereKilled.Singleton.PlayAttacker(attackerName, RoleTypeId.None);
                    break;
                case 3: 
                    DamageHandlerBase damageHandler = DamageHandlerReaderWriter.ReadDamageHandler(reader);
                    YouWereKilled.Singleton.PlayRegular(damageHandler);
                    break;
            }
        }

        public void ServerSetData(DamageHandlerBase dhb)
        {
            _damageHandler = dhb;
        }

        public bool TryGetViewmodelFov(out float fov)
        {
            fov = 0f;
            return SpectatorTargetTracker.CurrentTarget != null;
        }
    }
}