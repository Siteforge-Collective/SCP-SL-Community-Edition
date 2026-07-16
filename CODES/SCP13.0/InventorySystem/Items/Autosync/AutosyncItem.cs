using System;
using Mirror;

namespace InventorySystem.Items.Autosync
{
	public abstract class AutosyncItem : ItemBase, IAcquisitionConfirmationTrigger, IAutosyncReceiver
	{
		public bool AcquisitionAlreadyReceived { get; set; }

		public virtual void ServerConfirmAcqusition(){ }
		public virtual void ServerProcessCmd(NetworkReader reader) { }
		public virtual void ClientProcessRpcTemplate(NetworkReader reader, ushort serial) { }
		public virtual void ClientProcessRpcLocally(NetworkReader reader) { }

        protected void ClientSendCmd(Action<NetworkWriter> extraData = null)
        {
            NetworkWriter writer;
            using (new AutosyncCmd(this, out writer))
            {
                extraData?.Invoke(writer);
            }
        }
        protected void ServerSendPublicRpc(Action<NetworkWriter> extraData = null)
        {
            NetworkWriter writer;
            using (new AutosyncRpc(this, toAll: true, out writer))
            {
                extraData?.Invoke(writer);
            }
        }
        protected void ServerSendPrivateRpc(Action<NetworkWriter> extraData = null)
        {
            NetworkWriter writer;
            using (new AutosyncRpc(this, toAll: false, out writer))
            {
                extraData?.Invoke(writer);
            }
        }
        protected void ServerSendConditionalRpc(Func<ReferenceHub, bool> receiveCondition, Action<NetworkWriter> extraData = null)
        {
            NetworkWriter writer;
            using (new AutosyncRpc(this, receiveCondition, out writer))
            {
                extraData?.Invoke(writer);
            }
        }
    }
}
