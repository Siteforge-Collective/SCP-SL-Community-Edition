namespace InventorySystem.Items.Firearms.Attachments.Formatters
{
	public class InaccuracyParameterFormatter : IAttachmentsParameterFormatter
	{
		public float DefaultValue => 0f;

		public bool FormatParameter(AttachmentParam param, Firearm firearm, int attId, float val, out string formattedText, out bool isGood)
		{
			formattedText = null;
			isGood = default(bool);
			return false;
		}
	}
}
