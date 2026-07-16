using System;

namespace InventorySystem.Items.Firearms
{
	public readonly struct FirearmStatus : IEquatable<FirearmStatus>
	{
		public readonly byte Ammo;

		public readonly FirearmStatusFlags Flags;

		public readonly uint Attachments;

		public FirearmStatus(byte ammo, FirearmStatusFlags flags, uint attachments)
		{
			Ammo = 0;
			Flags = default(FirearmStatusFlags);
			Attachments = 0u;
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public static bool operator ==(FirearmStatus left, FirearmStatus right)
		{
			return false;
		}

		public static bool operator !=(FirearmStatus left, FirearmStatus right)
		{
			return false;
		}

		public bool Equals(FirearmStatus other)
		{
			return false;
		}

		public override bool Equals(object obj)
		{
			return false;
		}

		public override string ToString()
		{
			return null;
		}
	}
}
