namespace RemoteAdmin.Communication
{
	public abstract class RaClientDataRequest : global::RemoteAdmin.Interfaces.IServerCommunication, global::RemoteAdmin.Interfaces.IClientCommunication
	{
		private readonly global::System.Text.StringBuilder _stringBuilder = new global::System.Text.StringBuilder();

		public abstract int DataId { get; }

		public virtual void ReceiveData(string data, bool secure)
		{
		}

		public virtual void ReceiveData(CommandSender sender, string data)
		{
			_stringBuilder.Clear();
			_stringBuilder.Append("$").Append(DataId).Append(" ");
			GatherData();
			sender.RaReply($"${DataId} {_stringBuilder}", success: true, logToConsole: false, string.Empty);
		}

		protected abstract void GatherData();

		protected void AppendData(object data)
		{
			_stringBuilder.Append(data).Append(",");
		}

		protected int CastBool(bool value)
		{
			if (!value)
			{
				return 0;
			}
			return 1;
		}
	}
}
