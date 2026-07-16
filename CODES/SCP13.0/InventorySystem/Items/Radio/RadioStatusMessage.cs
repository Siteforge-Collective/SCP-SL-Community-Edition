using Mirror;

namespace InventorySystem.Items.Radio
{
	public struct RadioStatusMessage : NetworkMessage
	{
		public readonly RadioMessages.RadioRangeLevel Range;

		public readonly byte Battery;

		public readonly uint Owner;

		public void Serialize(NetworkWriter writer)
		{
		}

		public RadioStatusMessage(NetworkReader reader)
		{
			Range = default(RadioMessages.RadioRangeLevel);
			Battery = 0;
			Owner = 0u;
		}

		public RadioStatusMessage(RadioItem radio)
		{
			Range = default(RadioMessages.RadioRangeLevel);
			Battery = 0;
			Owner = 0u;
		}
	}
}
