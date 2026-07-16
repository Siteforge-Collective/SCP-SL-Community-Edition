using System.Collections.Generic;
using Mirror;

namespace Respawning
{
	public class RespawnEffectsController : NetworkBehaviour
	{
		public enum EffectType : byte
		{
			Selection = 0,
			UponRespawn = 1
		}

		private static readonly List<RespawnEffectsController> AllControllers;

		private static readonly int PlayKey;

		public RespawnEffect[] SelectionEffects;

		public RespawnEffect[] OnSpawnEffects;

		private void Awake()
		{
		}

		public static void ExecuteAllEffects(EffectType type, SpawnableTeamType team)
		{
		}

		[Server]
		private void ServerExecuteEffects(EffectType type, SpawnableTeamType team)
		{
		}

		[ClientRpc]
		private void RpcPlayEffects(byte[] effects)
		{
		}

		public static void PlayCassieAnnouncement(string words, bool makeHold, bool makeNoise, bool customAnnouncement = false)
		{
		}

		[Server]
		private void ServerPassCassie(string words, bool makeHold, bool makeNoise, bool customAnnouncement)
		{
		}

		[ClientRpc]
		private void RpcCassieAnnouncement(string words, bool makeHold, bool makeNoise, bool customAnnouncement)
		{
		}

		public static void ClearQueue()
		{
		}

		[Server]
		private void ServerPassClearQueue()
		{
		}

		[ClientRpc]
		public void RpcClearQueue()
		{
		}

		static RespawnEffectsController()
		{
		}

		protected void UserCode_RpcPlayEffects__Byte_005B_005D(byte[] effects)
		{
		}

		protected static void InvokeUserCode_RpcPlayEffects__Byte_005B_005D(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
		}

		protected void UserCode_RpcCassieAnnouncement__String__Boolean__Boolean__Boolean(string words, bool makeHold, bool makeNoise, bool customAnnouncement)
		{
		}

		protected static void InvokeUserCode_RpcCassieAnnouncement__String__Boolean__Boolean__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
		}

		protected void UserCode_RpcClearQueue()
		{
		}

		protected static void InvokeUserCode_RpcClearQueue(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
		}
	}
}
