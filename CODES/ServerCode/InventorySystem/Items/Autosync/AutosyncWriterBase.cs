namespace InventorySystem.Items.Autosync
{
	public abstract class AutosyncWriterBase : global::System.IDisposable
	{
		private readonly global::Mirror.PooledNetworkWriter _writer;

		private readonly global::InventorySystem.Items.Autosync.AutosyncItem _targetItem;

		private bool _alreadySent;

		public AutosyncWriterBase(global::InventorySystem.Items.Autosync.AutosyncItem item, out global::Mirror.NetworkWriter writer)
		{
			_alreadySent = false;
			_writer = global::Mirror.NetworkWriterPool.GetWriter();
			_targetItem = item;
			writer = _writer;
		}

		public void Dispose()
		{
			Send();
		}

		public void Send()
		{
			if (!_alreadySent)
			{
				_alreadySent = true;
				HandleSending(new global::InventorySystem.Items.Autosync.AutosyncMessage(_writer, _targetItem));
				_writer.Dispose();
			}
		}

		protected abstract void HandleSending(global::InventorySystem.Items.Autosync.AutosyncMessage msg);

		public static implicit operator global::Mirror.NetworkWriter(global::InventorySystem.Items.Autosync.AutosyncWriterBase mask)
		{
			return mask._writer;
		}
	}
}
