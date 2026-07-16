using System;
using GameCore;
using Mirror;

namespace Interactables.Interobjects.DoorUtils
{
    public class DoorEventOpenerExtension : DoorVariantExtension
    {
        public enum OpenerEventType
        {
            WarheadStart = 0,
            WarheadCancel = 1,
            DeconEvac = 2,
            DeconFinish = 3,
            DeconReset = 4
        }

        private static bool _isolateCheckpoints;
        private static bool _configLoaded;

        public static event Action<OpenerEventType> OnDoorsTriggerred;

        public static void TriggerAction(OpenerEventType eventType)
        {
            OnDoorsTriggerred?.Invoke(eventType);
        }

        private void Start()
        {
            OnDoorsTriggerred += Trigger;
            _configLoaded = false;
        }

        private void OnDestroy()
        {
            OnDoorsTriggerred -= Trigger;
        }

        private void Trigger(OpenerEventType eventType)
        {
            if (!NetworkServer.active) return;

            if (!_configLoaded)
            {
                _configLoaded = true;
                _isolateCheckpoints = ConfigFile.ServerConfig.GetBool("isolate_zones_on_countdown", true);
            }

            float yPos = base.transform.position.y;
            bool isValidHeight = yPos > -100f && yPos < 100f;

            switch (eventType)
            {
                case OpenerEventType.WarheadStart:
                    if (_isolateCheckpoints && isValidHeight)
                    {
                        if (TargetDoor is CheckpointDoor)
                        {
                            if (TargetDoor.TryGetComponent<DoorNametagExtension>(out var nametag) && nametag.GetName.Contains("GATE"))
                                return;

                            TargetDoor.TargetState = false;
                            TargetDoor.ServerChangeLock(DoorLockReason.Isolation, true);
                        }
                    }
                    break;

                case OpenerEventType.WarheadCancel:
                    if (_isolateCheckpoints && isValidHeight)
                    {
                        TargetDoor.ServerChangeLock(DoorLockReason.Isolation, false);
                    }
                    break;

                case OpenerEventType.DeconEvac:
                    if (isValidHeight)
                    {
                        TargetDoor.TargetState = true;
                        if (TargetDoor is CheckpointDoor)
                        {
                            TargetDoor.ServerChangeLock(DoorLockReason.DecontEvacuate, true);
                        }
                    }
                    break;

                case OpenerEventType.DeconFinish:
                    if (isValidHeight)
                    {
                        TargetDoor.TargetState = false;
                        TargetDoor.ServerChangeLock(DoorLockReason.DecontEvacuate, false);
                        TargetDoor.ServerChangeLock(DoorLockReason.DecontLockdown, true);
                    }
                    break;

                case OpenerEventType.DeconReset:
                    if (isValidHeight)
                    {
                        TargetDoor.ServerChangeLock(DoorLockReason.DecontEvacuate, false);
                        TargetDoor.ServerChangeLock(DoorLockReason.DecontLockdown, false);
                    }
                    break;
            }
        }

        static DoorEventOpenerExtension()
        {
            OnDoorsTriggerred = delegate { };
        }
    }
}