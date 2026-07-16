using System.Diagnostics;
using GameObjectPools;
using Mirror;
using PlayerRoles.PlayableScps.Subroutines;
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
            get => Mathf.Max(0f, _simulatedStareTime - (float)_simulatedStareSw.Elapsed.TotalSeconds);
            set
            {
                _simulatedStareTime = value;
                _simulatedStareSw.Restart();
            }
        }

        private void Update() => RefreshChaseState();

        public void RefreshChaseState()
        {
            if (!NetworkServer.active)
                return;

            if (!base.Role.TryGetOwner(out var hub))
                return;

            bool lookingAtTarget = SimulatedStare > 0f || AnyTargets(hub, hub.PlayerCameraReference);
            LookingAtTarget = lookingAtTarget;
            ServerSendRpc(toAll: true);
        }

        private bool AnyTargets(ReferenceHub owner, Transform camera)
        {
            foreach (ReferenceHub target in ReferenceHub.AllHubs)
            {
                if (!target.IsHuman())
                    continue;

                if (target.playerEffectsController.GetEffect<CustomPlayerEffects.Invisible>()?.IsEnabled == true)
                    continue;

                if (target.roleManager.CurrentRole is not FirstPersonControl.IFpcRole fpcRole)
                    continue;

                var vision = VisionInformation.GetVisionInformation(
                    owner, 
                    camera, 
                    fpcRole.FpcModule.Position, 
                    fpcRole.FpcModule.CharacterControllerSettings.Radius, 
                    _maxViewDistance
                );

                if (vision.IsLooking)
                    return true;
            }

            return false;
        }

        public override void ServerWriteRpc(NetworkWriter writer)
        {
            base.ClientWriteCmd(writer);
            NetworkWriterExtensions.WriteBool(writer, LookingAtTarget);
        }

        public override void ClientProcessRpc(NetworkReader reader)
        {
            base.ServerProcessCmd(reader);
            LookingAtTarget = NetworkReaderExtensions.ReadBool(reader);
        }
        public void ResetObject()
        {
            _simulatedStareTime = 0f;
        }
        public ZombieBloodlustAbility()
        {
            _simulatedStareSw = Stopwatch.StartNew();
        }
    }
}
