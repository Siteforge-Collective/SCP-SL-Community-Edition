using System.Collections.Generic;
using InventorySystem.Items.Firearms.Attachments;

namespace InventorySystem.Items.Firearms
{
	public static class FirearmAnimatorHashes
	{
		public static readonly Dictionary<AttachmentSlot, int> Slots;

		public static readonly int IsCocked;

		public static readonly int IsChambered;

		public static readonly int IsMagInserted;

		public static readonly int IsUnloaded;

		public static readonly int DrawSpeedMultiplier;

		public static readonly int ReloadSpeedMultiplier;

		public static readonly int FireRateMultiplier;

		public static readonly int Random;

		public static readonly int Inspect;

		public static readonly int Idle;

		public static readonly int Reload;

		public static readonly int Unload;

		public static readonly int Fire;

		public static readonly int DryFire;

		public static readonly int Ammo;

		public static readonly int RecoilPatternState;

		public static readonly int GripBlend;

		public static readonly int Shoot;
	}
}
