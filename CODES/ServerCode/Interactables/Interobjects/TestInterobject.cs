namespace Interactables.Interobjects
{
	public class TestInterobject : global::Mirror.NetworkBehaviour, global::Interactables.IClientInteractable, global::Interactables.IInteractable, global::Interactables.IServerInteractable
	{
		public global::TMPro.TextMeshProUGUI ClientText;

		public global::TMPro.TextMeshProUGUI GlobalText;

		public global::Interactables.Verification.IVerificationRule VerificationRule => global::Interactables.Verification.StandardDistanceVerification.Default;

		[global::Mirror.Server]
		public void ServerInteract(ReferenceHub ply, byte colliderId)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void Interactables.Interobjects.TestInterobject::ServerInteract(ReferenceHub,System.Byte)' called when server was not active");
				return;
			}
			RpcSendText("Player " + ply.LoggedNameFromRefHub() + " interacted using collider " + colliderId);
		}

		[global::Mirror.ClientRpc]
		private void RpcSendText(string s)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.NetworkWriterExtensions.WriteString(writer, s);
			SendRPCInternal(typeof(global::Interactables.Interobjects.TestInterobject), "RpcSendText", writer, 0, includeOwner: true);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		private void MirrorProcessed()
		{
		}

		private void UserCode_RpcSendText(string s)
		{
			GlobalText.text = s;
		}

		protected static void InvokeUserCode_RpcSendText(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkClient.active)
			{
				global::UnityEngine.Debug.LogError("RPC RpcSendText called on server.");
			}
			else
			{
				((global::Interactables.Interobjects.TestInterobject)obj).UserCode_RpcSendText(global::Mirror.NetworkReaderExtensions.ReadString(reader));
			}
		}

		static TestInterobject()
		{
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(global::Interactables.Interobjects.TestInterobject), "RpcSendText", InvokeUserCode_RpcSendText);
		}
	}
}
