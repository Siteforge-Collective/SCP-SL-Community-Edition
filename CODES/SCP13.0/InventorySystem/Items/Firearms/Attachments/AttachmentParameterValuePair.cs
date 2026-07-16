using System;

namespace InventorySystem.Items.Firearms.Attachments
{
	[Serializable]
	public struct AttachmentParameterValuePair
	{
		public AttachmentParam Parameter;

		public float Value;

		public AttachmentParameterValuePair(AttachmentParam param, float val)
		{
			Parameter = default(AttachmentParam);
			Value = 0f;
		}
	}
}
