using Mirror;

namespace InventorySystem.Items.Autosync
{
    public class AutosyncCmd : AutosyncWriterBase
    {
        public AutosyncCmd(AutosyncItem item, out NetworkWriter writer)
            : base(item, out writer)
        {
        }

        public AutosyncCmd(AutosyncItem item)
            : base(item, out var _)
        {
        }

        protected override void HandleSending(AutosyncMessage msg)
        {
            if (NetworkClient.ready && NetworkClient.active)
            {
                NetworkClient.Send(msg);
            }
        }
    }
}
