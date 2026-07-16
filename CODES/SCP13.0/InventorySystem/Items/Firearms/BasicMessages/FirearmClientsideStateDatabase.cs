using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem.Items.Firearms.BasicMessages
{
	public static class FirearmClientsideStateDatabase
	{
		private static readonly Dictionary<ushort, FirearmStatus> PreReloadStatuses;

		private static readonly Dictionary<ushort, float> ReloadTimes;

		private static readonly Dictionary<ushort, RequestType> ReloadTracker;

		private static readonly Dictionary<ushort, bool> AdsTracker;

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void HandleMessage(RequestMessage msg)
		{
		}

		private static void HandleItemChange(ItemIdentifier newItem)
		{
		}

		public static RequestType GetReloadStateRaw(ushort serial)
		{
			return default(RequestType);
		}

		public static bool IsAdsing(ushort serial)
		{
			return false;
		}

		public static bool IsReloading(ushort serial)
		{
			return false;
		}

		public static bool IsUnloading(ushort serial)
		{
			return false;
		}

		public static bool IsIdle(ushort serial)
		{
			return false;
		}

		public static float ElapsedReloadState(ushort serial)
		{
			return 0f;
		}

		public static FirearmStatus GetPreReloadStatus(ushort serial)
		{
			return default(FirearmStatus);
		}
	}
}
