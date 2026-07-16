using System;
using Mirror;
using UnityEngine;
using Utils.Networking;

namespace PlayerRoles.Subroutines
{
    public abstract class SubroutineBase : MonoBehaviour
    {
        private byte _syncIndex;

        public PlayerRoleBase Role { get; private set; }

        public byte SyncIndex
        {
            get
            {
                if (_syncIndex != 0)
                {
                    return _syncIndex;
                }
                SubroutineBase[] allSubroutines = ((Role as ISubroutinedRole) ?? throw new InvalidOperationException("Could not generate a SyncIndex of '" + base.name + "' subroutine. The role does not derive from ISubroutinedRole!")).SubroutineModule.AllSubroutines;
                for (int i = 0; i < allSubroutines.Length; i++)
                {
                    if (!(allSubroutines[i] != this))
                    {
                        _syncIndex = (byte)(i + 1);
                        return _syncIndex;
                    }
                }
                throw new InvalidOperationException("Could not generate a SyncIndex of '" + base.name + "' subroutine. It's not on the list of registered subroutines!");
            }
        }

        protected virtual void Awake()
        {
            Role = GetComponentInParent<PlayerRoleBase>();
        }

        protected virtual void OnValidate()
        {
            SubroutineManagerModule componentInParent = GetComponentInParent<SubroutineManagerModule>();
            if (!(componentInParent == null))
            {
                componentInParent.AllSubroutines = componentInParent.GetComponentsInChildren<SubroutineBase>();
            }
        }

        protected void ClientSendCmd()
        {
            if (!Role.Pooled)
            {
                if (!Role.IsLocalPlayer)
                {
                    throw new InvalidOperationException("ClientSendCmd can only be called on local player!");
                }
                NetworkClient.Send(new SubroutineMessage(this, isConfirmation: false));
            }
        }

        protected void ServerSendRpc(bool toAll)
        {
            if (NetworkServer.active && !Role.Pooled)
            {
                ReferenceHub hub;
                if (toAll)
                {
                    NetworkServer.SendToReady(new SubroutineMessage(this, isConfirmation: true));
                }
                else if (Role.TryGetOwner(out hub))
                {
                    ServerSendRpc(hub);
                }
            }
        }

        protected void ServerSendRpc(ReferenceHub target)
        {
            if (NetworkServer.active && !Role.Pooled)
            {
                target.connectionToClient.Send(new SubroutineMessage(this, isConfirmation: true));
            }
        }

        protected void ServerSendRpc(Func<ReferenceHub, bool> condition)
        {
            if (NetworkServer.active && !Role.Pooled)
            {
                new SubroutineMessage(this, isConfirmation: true).SendToHubsConditionally(condition);
            }
        }

        public virtual void ClientWriteCmd(NetworkWriter writer)
        {
        }

        public virtual void ServerProcessCmd(NetworkReader reader)
        {
        }

        public virtual void ServerWriteRpc(NetworkWriter writer)
        {
        }

        public virtual void ClientProcessRpc(NetworkReader reader)
        {
        }
    }
}
