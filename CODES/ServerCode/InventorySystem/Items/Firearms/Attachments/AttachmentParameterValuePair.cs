namespace InventorySystem.Items.Firearms.Attachments
{
	[global::System.Serializable]
	public struct AttachmentParameterValuePair
	{
		public global::InventorySystem.Items.Firearms.Attachments.AttachmentParam Parameter;

		public float Value;

		public AttachmentParameterValuePair(global::InventorySystem.Items.Firearms.Attachments.AttachmentParam param, float val)
		{
			Parameter = param;
			Value = val;
		}
	}
}
