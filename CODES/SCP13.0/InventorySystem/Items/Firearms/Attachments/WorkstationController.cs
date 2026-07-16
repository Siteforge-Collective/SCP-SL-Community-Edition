using Interactables;
using Interactables.Verification;
using Mirror;
using PlayerRoles;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

namespace InventorySystem.Items.Firearms.Attachments
{
    public class WorkstationController : NetworkBehaviour, IClientInteractable, IInteractable, IServerInteractable
    {
        private enum WorkstationStatus : byte
        {
            Offline = 0,
            PoweringUp = 1,
            PoweringDown = 2,
            Online = 3
        }

        public static readonly HashSet<WorkstationController> AllWorkstations = new HashSet<WorkstationController>();

        [SerializeField]
        private WorkstationAttachmentSelector _selector;

        [SerializeField]
        private GameObject _idleScreen;

        [SerializeField]
        private GameObject _powerupScreen;

        [SerializeField]
        private GameObject _powerdownScreen;

        [SerializeField]
        private GameObject _selectorScreen;

        [SerializeField]
        private InteractableCollider _activateCollder;

        [SyncVar(hook = nameof(OnStatusChanged))]
        public byte Status;

        private const float StandbyDistance = 2.4f;
        private const float UpkeepTime = 30f;
        private const float CheckTime = 5f;
        private const float PowerupTime = 5.6f;
        private const float PowerdownTime = 5f;

        private Stopwatch _serverStopwatch = new Stopwatch();
        private ReferenceHub _knownUser;

        public IVerificationRule VerificationRule => StandardDistanceVerification.Default;

        private void Awake()
        {
            AllWorkstations.Add(this);
        }

        private void OnDestroy()
        {
            AllWorkstations.Remove(this);
        }

        private void Start()
        {
            OnStatusChanged(0, Status);
        }

        private void OnStatusChanged(byte oldStatus, byte newStatus)
        {
            _idleScreen.SetActive(newStatus == (byte)WorkstationStatus.Offline);
            _powerupScreen.SetActive(newStatus == (byte)WorkstationStatus.PoweringUp);
            _powerdownScreen.SetActive(newStatus == (byte)WorkstationStatus.PoweringDown);
            _selectorScreen.SetActive(newStatus == (byte)WorkstationStatus.Online);
        }

        public void ClientInteract(InteractableCollider collider)
        {
            if (collider != null && collider.ColliderId != _activateCollder.ColliderId)
            {
                _selector.ProcessCollider(collider.ColliderId);
            }
        }

        public void ServerInteract(ReferenceHub ply, byte colliderId)
        {
            if (colliderId == _activateCollder.ColliderId)
            {
                if (Status == (byte)WorkstationStatus.Offline)
                {
                    Status = (byte)WorkstationStatus.PoweringUp;
                    _knownUser = ply;
                    _serverStopwatch.Restart();
                }
            }
        }

        private void Update()
        {
            if (!NetworkServer.active)
                return;

            switch ((WorkstationStatus)Status)
            {
                case WorkstationStatus.PoweringUp:
                    if (_serverStopwatch.Elapsed.TotalSeconds >= PowerupTime)
                    {
                        Status = (byte)WorkstationStatus.Online;
                        _serverStopwatch.Restart();
                    }
                    break;

                case WorkstationStatus.PoweringDown:
                    if (_serverStopwatch.Elapsed.TotalSeconds >= PowerdownTime)
                    {
                        Status = (byte)WorkstationStatus.Offline;
                        _serverStopwatch.Restart();
                    }
                    break;

                case WorkstationStatus.Online:
                    if (_serverStopwatch.Elapsed.TotalSeconds >= UpkeepTime)
                    {
                        if (_knownUser == null || !IsInRange(_knownUser))
                        {
                            bool foundNewUser = false;
                            foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
                            {
                                if (IsInRange(allHub))
                                {
                                    _knownUser = allHub;
                                    _serverStopwatch.Restart();
                                    foundNewUser = true;
                                    break;
                                }
                            }

                            if (!foundNewUser)
                            {
                                Status = (byte)WorkstationStatus.PoweringDown;
                                _serverStopwatch.Restart();
                            }
                        }
                        else
                        {
                            _serverStopwatch.Restart();
                        }
                    }
                    break;
            }
        }

        public bool IsInRange(ReferenceHub hub)
        {
            if (hub == null || !hub.IsAlive())
                return false;

            Vector3 pos = hub.transform.position;
            Vector3 myPos = base.transform.position;

            return Mathf.Abs(pos.y - myPos.y) < StandbyDistance &&
                   Mathf.Abs(pos.x - myPos.x) < StandbyDistance &&
                   Mathf.Abs(pos.z - myPos.z) < StandbyDistance;
        }
    }
}