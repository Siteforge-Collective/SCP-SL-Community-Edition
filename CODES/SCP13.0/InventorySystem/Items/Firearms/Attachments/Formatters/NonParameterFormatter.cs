using System;

namespace InventorySystem.Items.Firearms.Attachments.Formatters
{
	public static class NonParameterFormatter
	{
		private const int FlagsIteration = 8;

		private static string WeightString => null;

		private static string LengthString => null;

		public static void Format(Firearm fa, int attachmentId, out string pros, out string cons)
		{
			pros = null;
			cons = null;
		}

		private static string FormatPercent(float percent)
		{
			return null;
		}

		private static string FormatFlags(int value, Type enumType, string translationKey)
		{
			return null;
		}
	}
}
