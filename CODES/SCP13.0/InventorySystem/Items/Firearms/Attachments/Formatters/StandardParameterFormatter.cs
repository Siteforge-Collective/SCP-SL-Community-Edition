namespace InventorySystem.Items.Firearms.Attachments.Formatters
{
	public class StandardParameterFormatter : IAttachmentsParameterFormatter
	{
		private readonly bool _isMultiplier;

		private readonly bool _moreIsBetter;

		private readonly bool _formatAsPrecent;

		private readonly string _suffix;

		public float DefaultValue => 0f;

		public StandardParameterFormatter(bool moreIsBetter, bool isMultiplier = true, bool formatAsPercent = true, string suffix = null)
		{
		}

		public bool FormatParameter(AttachmentParam param, Firearm firearm, int attId, float statsValue, out string formattedText, out bool isGood)
		{
			formattedText = null;
			isGood = default(bool);
			return false;
		}
	}
}
