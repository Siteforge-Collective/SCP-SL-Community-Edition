using System.Collections.Generic;

namespace InventorySystem.Items.Firearms.Attachments
{
	public readonly struct AttachmentParameterDefinition
	{
		public static readonly Dictionary<AttachmentParam, AttachmentParameterDefinition> Definitions;

		public readonly ParameterMixingMode MixingMode;

		public readonly float MinValue;

		public readonly float MaxValue;

		public float DefaultValue => 0f;

		public AttachmentParameterDefinition(ParameterMixingMode mode, float min = float.MinValue, float max = float.MaxValue)
		{
			MixingMode = default(ParameterMixingMode);
			MinValue = 0f;
			MaxValue = 0f;
		}
	}
}
