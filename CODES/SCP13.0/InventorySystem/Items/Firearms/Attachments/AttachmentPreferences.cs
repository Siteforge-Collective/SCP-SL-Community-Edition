namespace InventorySystem.Items.Firearms.Attachments
{
	public static class AttachmentPreferences
	{
		private const string PrefKey = "_AttachmentsSetupPreference_";

		private const string PresetKey = "Preset_";

		private static string PreferencesPath(ItemType weaponId)
		{
			return null;
		}

		public static uint GetPreferenceCodeOfPreset(ItemType weapon, int preset)
		{
			return 0u;
		}

		public static int GetPreset(ItemType weapon)
		{
			return 0;
		}

		public static void SetPreset(ItemType weapon, int presetId)
		{
		}

		public static uint GetSavedPreferenceCode(ItemType weapon)
		{
			return 0u;
		}

		public static uint GetSavedPreferenceCode(this Firearm weapon)
		{
			return 0u;
		}

		public static void SavePreferenceCode(ItemType weapon, uint code)
		{
		}

		public static void SavePreferenceCode(this Firearm weapon)
		{
		}
	}
}
