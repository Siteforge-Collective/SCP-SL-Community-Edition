using System;
using System.Runtime.CompilerServices;
using System.Text;
using RemoteAdmin.Interfaces;

namespace RemoteAdmin.Communication
{
	public abstract class RaClientDataRequest : IServerCommunication, IClientCommunication
	{
		private readonly StringBuilder _stringBuilder;

		public abstract int DataId { get; }

		public event Action<string> OnClientReceiveData
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		public virtual void ReceiveData(string data, bool secure)
		{
		}

		public void Request()
		{
		}

		public virtual void ReceiveData(CommandSender sender, string data)
		{
		}

		protected abstract void GatherData();

		protected void AppendData(object data)
		{
		}

		protected int CastBool(bool value)
		{
			return 0;
		}
	}
}
