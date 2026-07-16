using System;
using Mirror;
using Utils.Networking;

namespace InventorySystem.Items.Autosync
{
	public class AutosyncRpc : AutosyncWriterBase
	{
		private enum Mode
		{
			Local = 0,
			AllClients = 1,
			Conditional = 2
		}

		private readonly Mode _mode;

		private readonly Func<ReferenceHub, bool> _predicate;

		private readonly NetworkConnection _ownerConnection;

        public AutosyncRpc(AutosyncItem item, bool toAll, out NetworkWriter writer)
            : base(item, out writer)
        {
            if (toAll)
            {
                _mode = Mode.AllClients;
                return;
            }
            _mode = Mode.Local;
            _ownerConnection = item.Owner.connectionToClient;
        }

        public AutosyncRpc(AutosyncItem item, Func<ReferenceHub, bool> predicate, out NetworkWriter writer)
            : base(item, out writer)
        {
            _mode = Mode.Conditional;
            _predicate = predicate;
        }

        public AutosyncRpc(AutosyncItem item, bool toAll)
            : this(item, toAll, out var _)
        {
        }

        public AutosyncRpc(AutosyncItem item, Func<ReferenceHub, bool> predicate)
            : this(item, predicate, out var _)
        {
        }

        protected override void HandleSending(AutosyncMessage msg)
        {
            if (NetworkServer.active)
            {
                switch (_mode)
                {
                    case Mode.Local:
                        _ownerConnection.Send(msg);
                        break;
                    case Mode.AllClients:
                        NetworkServer.SendToReady(msg);
                        break;
                    case Mode.Conditional:
                        msg.SendToHubsConditionally(_predicate);
                        break;
                }
            }
        }
    }
}
