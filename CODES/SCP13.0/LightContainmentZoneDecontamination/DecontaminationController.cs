using CustomPlayerEffects;
using GameCore;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using MapGeneration;
using MEC;
using Mirror;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.PlayableScps.Scp079;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LightContainmentZoneDecontamination
{
    public class DecontaminationController : NetworkBehaviour
    {
        public enum DecontaminationStatus : byte
        {
            None = 0,
            Disabled = 1,
            Forced = 2,
            Finished = 3
        }

        [Serializable]
        public struct DecontaminationPhase
        {
            public enum PhaseFunction : byte
            {
                None = 0,
                GloballyAudible = 1,
                OpenCheckpoints = 2,
                Final = 3
            }

            public float TimeTrigger;
            public float GameTime;
            public AudioClip AnnouncementLine;
            public PhaseFunction Function;
        }

        public static DecontaminationController Singleton;

        public static bool AutoDeconBroadcastEnabled = true;
        public static string DeconBroadcastDeconMessage = "Light Containment Zone is locked.";
        public static ushort DeconBroadcastDeconMessageTime = 5;

        public float TimeOffset;
        public DecontaminationPhase[] DecontaminationPhases;
        public DecontaminationClientTimer ClientTimer;
        public AudioSource AnnouncementAudioSource;

        [SerializeField] private TMP_Text _checkpointHczA;
        [SerializeField] private TMP_Text _checkpointHczB;
        [SerializeField] private string _elevatorsLockedText = "ELEVATOR SYSTEM <color=#e00>DISABLED</color>";

        [SyncVar] public double RoundStartTime;
        [SyncVar(hook = nameof(OnChangeDisableDecontamination))] public DecontaminationStatus DecontaminationOverride;

        private DecontaminationPhase.PhaseFunction _curFunction;
        private int _nextPhase;
        private bool _stopUpdating;
        private bool _decontaminationBegun;
        private bool _elevatorsDirty;

        private const float LczUpperBound = 330f;
        private const float LczLowerBound = -500f;

        public static double GetServerTime()
        {
            if (Singleton == null) return 0.0;
            return NetworkTime.time - Singleton.RoundStartTime;
        }

        private bool IsAnnouncementHearable
        {
            get
            {
                if (!ReferenceHub.TryGetHostHub(out ReferenceHub host))
                    return false;

                if (_curFunction == DecontaminationPhase.PhaseFunction.Final ||
                    _curFunction == DecontaminationPhase.PhaseFunction.GloballyAudible)
                {
                    return true;
                }

                foreach (ReferenceHub hub in ReferenceHub.AllHubs)
                {
                    if (hub.isLocalPlayer)
                    {
                        if (hub.roleManager.CurrentRole is Scp079Role scp079)
                        {
                            return scp079.CurrentCamera != null &&
                                   scp079.CurrentCamera.Room.Zone == FacilityZone.LightContainment;
                        }

                        RoomIdentifier room = RoomIdUtils.RoomAtPositionRaycasts(hub.transform.position, true);
                        if (room != null && room.Zone == FacilityZone.LightContainment)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public bool IsDecontaminating => _decontaminationBegun;


        private void Awake()
        {
            Singleton = this;
        }

        private void Start()
        {
            if (ConfigFile.ServerConfig.GetBool("disable_decontamination", false))
            {
                DecontaminationOverride = DecontaminationStatus.Disabled;
            }

            if (NetworkServer.active)
            {
                RoundStartTime = NetworkTime.time;
                Timing.RunCoroutine(KillPlayers());
            }
        }

        private void Update()
        {
            if (_elevatorsDirty)
            {
                DisableElevators();
            }

            if (_stopUpdating) return;

            if (DecontaminationOverride == DecontaminationStatus.Disabled ||
                DecontaminationOverride == DecontaminationStatus.Finished)
            {
                return;
            }

            if (RoundStart.singleton.Timer == ushort.MaxValue) return;

            if (NetworkServer.active)
            {
                UpdateTime();
            }
            else
            {
                if (RoundStartTime <= 0) return;

                double clientTime = GetServerTime() + TimeOffset;
                ClientTimer.UpdateTimer((float)clientTime);
                UpdateSpeaker(false);
            }
        }


        private void UpdateTime()
        {
            double serverTime = GetServerTime() + TimeOffset;

            if (DecontaminationOverride == DecontaminationStatus.Forced)
            {
                FinishDecontamination();
                return;
            }

            if (_nextPhase >= DecontaminationPhases.Length)
            {
                _stopUpdating = true;
                return;
            }

            DecontaminationPhase phase = DecontaminationPhases[_nextPhase];

            if (serverTime < phase.TimeTrigger) return;

            _curFunction = phase.Function;

            if (AnnouncementAudioSource != null && phase.AnnouncementLine != null)
            {
                AnnouncementAudioSource.PlayOneShot(phase.AnnouncementLine);
            }

            if (phase.Function == DecontaminationPhase.PhaseFunction.Final)
            {
                FinishDecontamination();
            }
            else if (phase.Function == DecontaminationPhase.PhaseFunction.OpenCheckpoints)
            {
                DoorEventOpenerExtension.TriggerAction(DoorEventOpenerExtension.OpenerEventType.DeconEvac);
            }

            if (_nextPhase == DecontaminationPhases.Length - 1)
                _stopUpdating = true;
            else
                _nextPhase++;
        }

        private void FinishDecontamination()
        {
            DoorEventOpenerExtension.TriggerAction(DoorEventOpenerExtension.OpenerEventType.DeconFinish);

            DisableElevators();

            if (!_decontaminationBegun)
            {
                if (AutoDeconBroadcastEnabled)
                {
                    Broadcast.Singleton.RpcAddElement(
                        DeconBroadcastDeconMessage,
                        DeconBroadcastDeconMessageTime,
                        Broadcast.BroadcastFlags.Normal
                    );
                }
            }

            _decontaminationBegun = true;
            DecontaminationGas.TurnedOn = true;

            if (_checkpointHczA != null) _checkpointHczA.text = _elevatorsLockedText;
            if (_checkpointHczB != null) _checkpointHczB.text = _elevatorsLockedText;
        }

        private void DisableElevators()
        {
            foreach (DoorVariant door in DoorVariant.AllDoors)
            {
                if (door is ElevatorDoor)
                {
                    door.ServerChangeLock(DoorLockReason.DecontLockdown, true);
                }
            }
            _elevatorsDirty = false;
        }

        private IEnumerator<float> KillPlayers()
        {
            float timer = 1f;

            while (true)
            {
                yield return Timing.WaitForSeconds(timer);

                if (!_decontaminationBegun) continue;

                foreach (ReferenceHub hub in ReferenceHub.AllHubs)
                {
                    if (!hub.IsAlive()) continue;

                    Vector3 pos = hub.transform.position;
                    if (pos.y < LczUpperBound && pos.y > LczLowerBound) 
                    {
                        if (hub.playerEffectsController.TryGetEffect(out Decontaminating effect))
                        {
                            effect.Intensity = 1;
                        }
                    }
                }
            }
        }

        private void OnChangeDisableDecontamination(DecontaminationStatus oldValue, DecontaminationStatus newValue)
        {
            if (oldValue == newValue) return;

            if (newValue == DecontaminationStatus.Disabled)
            {
                DecontaminationGas.TurnedOn = false;

                if (NetworkServer.active)
                {
                    DoorEventOpenerExtension.TriggerAction(DoorEventOpenerExtension.OpenerEventType.DeconEvac);
                }
                else
                {
                    ClientTimer.enabled = false;
                    UpdateSpeaker(true);
                }
            }
            else if (newValue == DecontaminationStatus.Finished)
            {
                DecontaminationGas.TurnedOn = false;
            }
        }

        public static void ForceDecontamination()
        {
            if (Singleton != null)
            {
                Singleton.DecontaminationOverride = DecontaminationStatus.Forced;
                Singleton.FinishDecontamination();
            }
        }

        private void UpdateSpeaker(bool hard)
        {
            if (AnnouncementAudioSource == null) return;

            float target = IsAnnouncementHearable ? 1f : 0f;
            float t = hard ? 1f : Time.deltaTime * 4f;
            AnnouncementAudioSource.volume = Mathf.Lerp(AnnouncementAudioSource.volume, target, t);
        }
    }
}