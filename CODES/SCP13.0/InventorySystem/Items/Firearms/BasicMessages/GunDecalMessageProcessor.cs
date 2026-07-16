using UnityEngine;

namespace InventorySystem.Items.Firearms.BasicMessages
{
	public static class GunDecalMessageProcessor
	{
		private const float HoleRayLength = 1.1f;

		private const float BloodRayLength = 5f;

		private static BloodDrawer _bloodDrawer;

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void RegisterHandlers()
		{
		}

		private static bool TryGetBloodDrawer(out BloodDrawer bd)
		{
			bd = null;
			return false;
		}

		private static void ClientMessageReceived(GunDecalMessage msg)
		{
		}
	}
}
