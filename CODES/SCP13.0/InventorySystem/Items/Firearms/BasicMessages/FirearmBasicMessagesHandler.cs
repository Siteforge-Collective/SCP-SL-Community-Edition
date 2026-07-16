using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Mirror;
using UnityEngine;

namespace InventorySystem.Items.Firearms.BasicMessages
{
	public static class FirearmBasicMessagesHandler
	{
		public static readonly Dictionary<ushort, FirearmStatus> ReceivedStatuses;

		private static readonly HashSet<int> AlreadyRequestedStatuses;

		public static event Action<StatusMessage> OnStatusMessageReceived
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		public static event Action<ReferenceHub> OnStatusRequested
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		public static event Action<RequestMessage> OnClientConfirmationReceived
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void RegisterHandlers()
		{
		}

		private static void ClientConfirmationReceived(RequestMessage msg)
		{
		}

		private static void ServerShotReceived(NetworkConnection conn, ShotMessage msg)
		{
		}

		private static void ServerRequestReceived(NetworkConnection conn, RequestMessage msg)
		{
		}

		private static void ClientStatusMessageReceived(StatusMessage msg)
		{
		}

		public static bool HasFlagFast(this FirearmStatusFlags flags, FirearmStatusFlags flag)
		{
			return false;
		}
	}
}
