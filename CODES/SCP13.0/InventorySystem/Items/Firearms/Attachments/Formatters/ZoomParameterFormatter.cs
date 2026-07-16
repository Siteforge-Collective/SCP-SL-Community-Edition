using InventorySystem.Items.Firearms.Attachments.Components;

namespace InventorySystem.Items.Firearms.Attachments.Formatters
{
	public class ZoomParameterFormatter : IAttachmentsParameterFormatter
	{
		public float DefaultValue => 0f;

		public bool FormatParameter(AttachmentParam param, Firearm firearm, int attId, float val, out string formattedText, out bool isGood)
		{
			formattedText = null;
			isGood = default(bool);
			return false;
		}

		private float GetMultiplier(Attachment attachment, AttachmentParam param)
		{
			return 0f;
		}
	}
}
