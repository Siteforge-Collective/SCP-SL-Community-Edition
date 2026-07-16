public class ServerShutdown : global::UnityEngine.MonoBehaviour
{
	internal enum ServerShutdownState : byte
	{
		NotInitiated = 0,
		BroadcastingShutdown = 1,
		ShuttingDown = 2,
		Complete = 3
	}

	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Size = 1)]
	private struct ServerShutdownMessage : global::Mirror.NetworkMessage
	{
	}

	private static float _c;

	internal static ServerShutdown.ServerShutdownState ShutdownState { get; set; }

	[global::UnityEngine.RuntimeInitializeOnLoadMethod]
	private static void InitOnLoad()
	{
		CustomNetworkManager.OnClientReady += delegate
		{
			global::Mirror.NetworkClient.ReplaceHandler<ServerShutdown.ServerShutdownMessage>(HandleServerShutdown);
		};
	}

	private void Update()
	{
		switch ((byte)ShutdownState)
		{
		default:
			return;
		case 0:
		case 3:
			return;
		case 1:
			if (_c > 400f)
			{
				ServerConsole.AddLog("Shutting down the server...", global::System.ConsoleColor.DarkCyan);
				ShutdownState = ServerShutdown.ServerShutdownState.ShuttingDown;
				_c = 0f;
				global::Mirror.NetworkServer.Shutdown();
				return;
			}
			break;
		case 2:
			if (_c > 1000f)
			{
				ServerConsole.AddLog("Server shutdown completed.", global::System.ConsoleColor.DarkCyan);
				ShutdownState = ServerShutdown.ServerShutdownState.Complete;
				return;
			}
			break;
		}
		_c += global::UnityEngine.Time.unscaledDeltaTime;
	}

	internal static void Shutdown()
	{
		if (ShutdownState == ServerShutdown.ServerShutdownState.NotInitiated)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				ShutdownState = ServerShutdown.ServerShutdownState.Complete;
				return;
			}
			ServerConsole.AddLog("Server shutdown initiated.", global::System.ConsoleColor.DarkCyan);
			ShutdownState = ServerShutdown.ServerShutdownState.BroadcastingShutdown;
			IdleMode.SetIdleMode(state: false);
			CustomLiteNetLib4MirrorTransport.DelayConnections = true;
			ServerConsole.AddLog("Broadcasting server shutdown to all connected players...", global::System.ConsoleColor.DarkCyan);
			global::Mirror.NetworkServer.SendToAll(default(ServerShutdown.ServerShutdownMessage), 4, sendToReadyOnly: true);
		}
	}

	private static void HandleServerShutdown(ServerShutdown.ServerShutdownMessage ssm)
	{
	}
}
