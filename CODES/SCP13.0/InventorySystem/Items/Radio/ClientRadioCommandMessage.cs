using Mirror;

namespace InventorySystem.Items.Radio
{
	public struct ClientRadioCommandMessage : NetworkMessage
	{
		public RadioMessages.RadioCommand Command;

		public ClientRadioCommandMessage(RadioMessages.RadioCommand cmd)
		{
			Command = default(RadioMessages.RadioCommand);
		}

		public void Serialize(NetworkWriter writer)
		{
		}

		public ClientRadioCommandMessage(NetworkReader reader)
		{
			Command = default(RadioMessages.RadioCommand);
		}
	}
}
