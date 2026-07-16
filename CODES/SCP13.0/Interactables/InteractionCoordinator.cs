using System;
using System.Collections.Generic;
using Interactables.Verification;
using InventorySystem.Items;
using Mirror;
using PlayerRoles;
using UnityEngine;
using Utils.NonAllocLINQ;

namespace Interactables
{
    public class InteractionCoordinator : NetworkBehaviour
    {
        public static KeyCode InteractKey;
        public static RaycastHit LastRaycastHit;
        public static LayerMask InteractRaycastMask;

        public static event Action<InteractableCollider> OnClientInteracted;

        private readonly HashSet<IInteractionBlocker> _blockers = new HashSet<IInteractionBlocker>();
        private ReferenceHub _hub;

        public void AddBlocker(IInteractionBlocker blocker)
        {
            _blockers.Add(blocker);
        }

        public bool AnyBlocker(BlockedInteraction interactions)
        {
            return AnyBlocker(x => x.BlockedInteractions.HasFlagFast(interactions));
        }

        public bool AnyBlocker(Func<IInteractionBlocker, bool> func)
        {
            _blockers.RemoveWhere(x => (x as UnityEngine.Object) == null || (x?.CanBeCleared ?? true));
            return _blockers.Any(func);
        }

        private void Start()
        {
            if (isLocalPlayer || NetworkServer.active)
            {
                _hub = ReferenceHub.GetHub(gameObject);
            }

            if (isLocalPlayer)
            {
                CenterScreenRaycast.OnCenterRaycastHit += OnCenterScreenRaycast;

                var keyData = NewInput.GetKey((ActionName)3, KeyCode.E);
                InteractKey = keyData;
                NewInput.OnKeyModified += OnKeyModified;

                InteractRaycastMask = LayerMask.GetMask("Default", "Player", "InteractableNoPlayerCollision", "Hitbox", "Glass", "Door");
            }
        }

        private void OnDestroy()
        {
            if (isLocalPlayer)
            {
                NewInput.OnKeyModified -= OnKeyModified;
                CenterScreenRaycast.OnCenterRaycastHit -= OnCenterScreenRaycast;
            }
        }

        private void OnKeyModified(ActionName actionName, KeyCode keyCode)
        {
            if (actionName == (ActionName)3)
            {
                InteractKey = keyCode;
            }
        }

        private void OnCenterScreenRaycast(RaycastHit hit)
        {
            if (!isLocalPlayer) return;

            LastRaycastHit = hit;

            if (Input.GetKeyDown(InteractKey))
            {
                ClientInteract();
            }
        }

        private void ClientInteract()
        {
            if (!PlayerRolesUtils.IsAlive(_hub)) return;

            Collider collider = LastRaycastHit.collider;
            if (collider == null) return;

            if (!collider.TryGetComponent<InteractableCollider>(out var interactableCollider))
            {
                Transform parent = collider.transform.parent;
                if (parent == null || !parent.TryGetComponent<InteractableCollider>(out interactableCollider))
                {
                    return;
                }
            }

            if (interactableCollider.Target == null) return;
            if (interactableCollider.Target == _hub) return;

            IVerificationRule rule = GetSafeRule(interactableCollider.Target as IInteractable);
            if (!rule.ClientCanInteract(interactableCollider, LastRaycastHit))
            {
                return;
            }

            OnClientInteracted?.Invoke(interactableCollider);

            if (interactableCollider.TryGetComponent<NetworkIdentity>(out var ni))
            {
                CmdServerInteract(ni, interactableCollider.ColliderId);
            }
        }

        private static IVerificationRule GetSafeRule(IInteractable inter)
        {
            return inter?.VerificationRule ?? StandardDistanceVerification.Default;
        }

        [Command(channel = 4)]
        private void CmdServerInteract(NetworkIdentity targetInteractable, byte colId)
        {
            if (targetInteractable == null || _hub == null) return;

            if (!PlayerRolesUtils.IsAlive(_hub)) return;

            if (!targetInteractable.TryGetComponent<IServerInteractable>(out var serverInteractable)) return;

            if (!InteractableCollider.TryGetCollider(serverInteractable, colId, out var collider)) return;

            if (GetSafeRule(serverInteractable).ServerCanInteract(_hub, collider))
            {
                serverInteractable.ServerInteract(_hub, colId);
            }
        }
    }
}