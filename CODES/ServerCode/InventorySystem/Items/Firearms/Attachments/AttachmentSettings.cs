namespace InventorySystem.Items.Firearms.Attachments
{
	[global::System.Serializable]
	public class AttachmentSettings
	{
		public float Weight;

		public float PhysicalLength;

		public global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterValuePair[] SerializedParameters;

		public global::InventorySystem.Items.Firearms.Attachments.AttachmentDescriptiveAdvantages AdditionalPros;

		public global::InventorySystem.Items.Firearms.Attachments.AttachmentDescriptiveDownsides AdditionalCons;
	}
}
