namespace InventorySystem.Items.Firearms.Attachments
{
	public readonly struct AttachmentParameterDefinition
	{
		public static readonly global::System.Collections.Generic.Dictionary<global::InventorySystem.Items.Firearms.Attachments.AttachmentParam, global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition> Definitions = new global::System.Collections.Generic.Dictionary<global::InventorySystem.Items.Firearms.Attachments.AttachmentParam, global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition>
		{
			[global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.AdsZoomMultiplier] = new global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition(global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Percent),
			[global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.AdsMouseSensitivityMultiplier] = new global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition(global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Percent),
			[global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.DamageMultiplier] = new global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition(global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Percent),
			[global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.PenetrationMultiplier] = new global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition(global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Percent),
			[global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.FireRateMultiplier] = new global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition(global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Percent),
			[global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.OverallRecoilMultiplier] = new global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition(global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Percent, 0.2f),
			[global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.AdsRecoilMultiplier] = new global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition(global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Percent, 0.2f),
			[global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.BulletInaccuracyMultiplier] = new global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition(global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Percent),
			[global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.HipInaccuracyMultiplier] = new global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition(global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Percent, 0.2f),
			[global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.AdsInaccuracyMultiplier] = new global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition(global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Percent, 0.1f),
			[global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.DrawSpeedMultiplier] = new global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition(global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Percent),
			[global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.GunshotLoudnessMultiplier] = new global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition(global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Percent),
			[global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.MagazineCapacityModifier] = new global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition(global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Additive),
			[global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.DrawTimeModifier] = new global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition(global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Additive),
			[global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.ReloadTimeModifier] = new global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition(global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Additive),
			[global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.ShotClipIdOverride] = new global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition(global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Override),
			[global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.AdsSpeedMultiplier] = new global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition(global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Percent),
			[global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.SpreadMultiplier] = new global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition(global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Percent),
			[global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.SpreadPredictability] = new global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition(global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Percent),
			[global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.AmmoConsumptionMultiplier] = new global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition(global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Percent),
			[global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.ReloadSpeedMultiplier] = new global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition(global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Percent),
			[global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.PreventReload] = new global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition(global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Additive)
		};

		public readonly global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode MixingMode;

		public readonly float MinValue;

		public readonly float MaxValue;

		public float DefaultValue => (MixingMode == global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Percent) ? 1 : 0;

		public AttachmentParameterDefinition(global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode mode, float min = float.MinValue, float max = float.MaxValue)
		{
			MixingMode = mode;
			MinValue = min;
			MaxValue = max;
		}
	}
}
