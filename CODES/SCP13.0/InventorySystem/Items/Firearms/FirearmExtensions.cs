using System;

namespace InventorySystem.Items.Firearms
{
	public static class FirearmExtensions
	{
		public static Action<Firearm, byte, float> ServerSoundPlayed;

		public static void AnimForceUpdate(this Firearm fa)
		{
		}

		public static void AnimSetInt(this Firearm fa, int hash, int i)
		{
		}

		public static void AnimSetFloat(this Firearm fa, int hash, float f)
		{
		}

		public static void AnimSetBool(this Firearm fa, int hash, bool b)
		{
		}

		public static void AnimSetTrigger(this Firearm fa, int hash)
		{
		}

		public static void ServerSendAudioMessage(this Firearm firearm, byte clipId)
		{
		}
	}
}
