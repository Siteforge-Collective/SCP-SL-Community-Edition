using PlayerRoles.PlayableScps.Scp079.Cameras;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079ScannerNotification : Scp079AccentedNotification
	{
		private static readonly CachedLayerMask Mask;

		private const string RedColorHex = "#ff1111";

		public Scp079ScannerNotification(HumanRole detectedHuman)
			: base(null, null, '\0')
		{
		}

		public Scp079ScannerNotification(int retryTime)
			: base(null, null, '\0')
		{
		}

		private static string HumanFoundText(HumanRole role)
		{
			return null;
		}

		private static string NoneFoundText(int retryTime)
		{
			return null;
		}

		private static Scp079Camera GetBestCamera(Vector3 pos)
		{
			return null;
		}
	}
}
