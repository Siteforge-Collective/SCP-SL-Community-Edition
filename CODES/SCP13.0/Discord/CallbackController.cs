using System.Runtime.InteropServices;
using AOT;
using Discord.Basic;

namespace Discord
{
	public static class CallbackController
	{
		public delegate void OnDisconnectedInfo(int errorCode, string message);

		public delegate void OnErrorInfo(int errorCode, string message);

		public delegate void OnJoinInfo(string secret);

		public delegate void OnReadyInfo(ref DiscordUser connectedUser);

		public delegate void OnRequestInfo(ref DiscordUser request);

		public delegate void OnSpectateInfo(string secret);

		internal static EventHandlers Callbacks;

		public static bool IsConnected { get; private set; }

		public static void Initialize(string applicationId, bool autoRegister, string optionalSteamId)
		{
		}

		public static void UpdatePresence(RichPresence presence)
		{
		}

		public static void RunCallbacks()
		{
		}

		public static void Shutdown()
		{
		}

		public static void Respond(string userId, Reply reply)
		{
		}

		[MonoPInvokeCallback(typeof(OnReadyInfo))]
		public static void ReadyCallback(ref DiscordUser connectedUser)
		{
		}

		[MonoPInvokeCallback(typeof(OnDisconnectedInfo))]
		public static void DisconnectedCallback(int errorCode, string message)
		{
		}

		[MonoPInvokeCallback(typeof(OnErrorInfo))]
		public static void ErrorCallback(int errorCode, string message)
		{
		}

		[MonoPInvokeCallback(typeof(OnJoinInfo))]
		public static void JoinCallback(string secret)
		{
		}

		[MonoPInvokeCallback(typeof(OnSpectateInfo))]
		public static void SpectateCallback(string secret)
		{
		}

		[MonoPInvokeCallback(typeof(OnRequestInfo))]
		public static void RequestCallback(ref DiscordUser request)
		{
		}

		[PreserveSig]
		private static extern void InitializeInternal(string applicationId, ref EventHandlers handlers, bool autoRegister, string optionalSteamId);

		[PreserveSig]
		private static extern void ShutdownInternal();

		[PreserveSig]
		private static extern void RunCallbacksInternal();

		[PreserveSig]
		private static extern void UpdatePresenceInternal(ref RichPresenceStruct presence);

		[PreserveSig]
		private static extern void ClearPresenceInternal();

		[PreserveSig]
		private static extern void RespondInternal(string userId, Reply reply);

		[PreserveSig]
		private static extern void UpdateHandlersInternal(ref EventHandlers handlers);
	}
}
