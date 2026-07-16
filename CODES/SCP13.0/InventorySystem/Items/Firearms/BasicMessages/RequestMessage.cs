using Mirror;

namespace InventorySystem.Items.Firearms.BasicMessages
{
	public struct RequestMessage : NetworkMessage
	{
		public ushort Serial;

		public RequestType Request;

		public RequestMessage(ushort serial, RequestType request)
		{
			Serial = 0;
			Request = default(RequestType);
		}

		public void Deserialize(NetworkReader reader)
		{
		}

		public void Serialize(NetworkWriter writer)
		{
		}
	}
}
