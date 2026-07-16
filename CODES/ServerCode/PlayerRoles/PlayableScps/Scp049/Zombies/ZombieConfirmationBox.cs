namespace PlayerRoles.PlayableScps.Scp049.Zombies
{
	public class ZombieConfirmationBox : global::UnityEngine.MonoBehaviour
	{
		[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Size = 1)]
		public struct ScpReviveBlockMessage : global::Mirror.NetworkMessage
		{
		}

		private static void ServerReceiveMessage(global::Mirror.NetworkConnection conn)
		{
			if (global::Mirror.NetworkServer.active && ReferenceHub.TryGetHubNetID(conn.identity.netId, out var hub) && global::PlayerRoles.PlayableScps.Scp049.Scp049ResurrectAbility.GetResurrectionsNumber(hub) != 0)
			{
				global::PlayerRoles.PlayableScps.Scp049.Scp049ResurrectAbility.RegisterPlayerResurrection(hub, 4);
			}
		}

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			CustomNetworkManager.OnClientReady += delegate
			{
				global::Mirror.NetworkServer.ReplaceHandler(delegate(global::Mirror.NetworkConnection conn, global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieConfirmationBox.ScpReviveBlockMessage msg)
				{
					ServerReceiveMessage(conn);
				});
			};
		}
	}
}
