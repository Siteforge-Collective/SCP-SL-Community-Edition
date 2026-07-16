using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RemoteAdmin
{
	public class StaffChatNotification : MonoBehaviour
	{
		private const byte MaxUnreadNotifications = 9;

		private static readonly HashSet<StaffChatNotification> Notifications;

		private static byte _unreadMessagesCount;

		[SerializeField]
		private TextMeshProUGUI _notificationText;

		public static byte UnreadMessagesCount
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		private void Start()
		{
		}

		private void OnDestroy()
		{
		}

		private static void DisableNotifications()
		{
		}

		private static void RefreshUnreadCount(bool clamped)
		{
		}
	}
}
