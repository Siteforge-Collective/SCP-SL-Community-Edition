namespace Respawning
{
	public class RespawnEffectsController : global::Mirror.NetworkBehaviour
	{
		public enum EffectType : byte
		{
			Selection = 0,
			UponRespawn = 1
		}

		private static readonly global::System.Collections.Generic.List<global::Respawning.RespawnEffectsController> AllControllers;

		private static readonly int PlayKey;

		public global::Respawning.RespawnEffect[] SelectionEffects;

		public global::Respawning.RespawnEffect[] OnSpawnEffects;

		private void Awake()
		{
			while (AllControllers.Contains(null))
			{
				AllControllers.Remove(null);
			}
			AllControllers.Add(this);
		}

		public static void ExecuteAllEffects(global::Respawning.RespawnEffectsController.EffectType type, global::Respawning.SpawnableTeamType team)
		{
			foreach (global::Respawning.RespawnEffectsController allController in AllControllers)
			{
				if (allController != null)
				{
					allController.ServerExecuteEffects(type, team);
				}
			}
		}

		[global::Mirror.Server]
		private void ServerExecuteEffects(global::Respawning.RespawnEffectsController.EffectType type, global::Respawning.SpawnableTeamType team)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void Respawning.RespawnEffectsController::ServerExecuteEffects(Respawning.RespawnEffectsController/EffectType,Respawning.SpawnableTeamType)' called when server was not active");
				return;
			}
			global::Respawning.RespawnEffect[] array = ((type == global::Respawning.RespawnEffectsController.EffectType.Selection) ? SelectionEffects : OnSpawnEffects);
			global::System.Collections.Generic.List<byte> list = global::NorthwoodLib.Pools.ListPool<byte>.Shared.Rent();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].TargetTeam == team)
				{
					int num = i + ((type == global::Respawning.RespawnEffectsController.EffectType.Selection) ? 128 : 0);
					list.Add((byte)num);
				}
			}
			if (list.Count > 0)
			{
				RpcPlayEffects(list.ToArray());
			}
			global::NorthwoodLib.Pools.ListPool<byte>.Shared.Return(list);
		}

		[global::Mirror.ClientRpc]
		private void RpcPlayEffects(byte[] effects)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.NetworkWriterExtensions.WriteBytesAndSize(writer, effects);
			SendRPCInternal(typeof(global::Respawning.RespawnEffectsController), "RpcPlayEffects", writer, 0, includeOwner: true);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		public static void PlayCassieAnnouncement(string words, bool makeHold, bool makeNoise, bool customAnnouncement = false)
		{
			foreach (global::Respawning.RespawnEffectsController allController in AllControllers)
			{
				if (allController != null)
				{
					allController.ServerPassCassie(words, makeHold, makeNoise, customAnnouncement);
					break;
				}
			}
		}

		[global::Mirror.Server]
		private void ServerPassCassie(string words, bool makeHold, bool makeNoise, bool customAnnouncement)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void Respawning.RespawnEffectsController::ServerPassCassie(System.String,System.Boolean,System.Boolean,System.Boolean)' called when server was not active");
			}
			else
			{
				RpcCassieAnnouncement(words, makeHold, makeNoise, customAnnouncement);
			}
		}

		[global::Mirror.ClientRpc]
		private void RpcCassieAnnouncement(string words, bool makeHold, bool makeNoise, bool customAnnouncement)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.NetworkWriterExtensions.WriteString(writer, words);
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, makeHold);
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, makeNoise);
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, customAnnouncement);
			SendRPCInternal(typeof(global::Respawning.RespawnEffectsController), "RpcCassieAnnouncement", writer, 0, includeOwner: true);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		public static void ClearQueue()
		{
			foreach (global::Respawning.RespawnEffectsController allController in AllControllers)
			{
				if (allController != null)
				{
					allController.ServerPassClearQueue();
					break;
				}
			}
		}

		[global::Mirror.Server]
		private void ServerPassClearQueue()
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void Respawning.RespawnEffectsController::ServerPassClearQueue()' called when server was not active");
			}
			else
			{
				RpcClearQueue();
			}
		}

		[global::Mirror.ClientRpc]
		public void RpcClearQueue()
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			SendRPCInternal(typeof(global::Respawning.RespawnEffectsController), "RpcClearQueue", writer, 0, includeOwner: true);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		static RespawnEffectsController()
		{
			AllControllers = new global::System.Collections.Generic.List<global::Respawning.RespawnEffectsController>();
			PlayKey = global::UnityEngine.Animator.StringToHash("Play");
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(global::Respawning.RespawnEffectsController), "RpcPlayEffects", InvokeUserCode_RpcPlayEffects);
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(global::Respawning.RespawnEffectsController), "RpcCassieAnnouncement", InvokeUserCode_RpcCassieAnnouncement);
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(global::Respawning.RespawnEffectsController), "RpcClearQueue", InvokeUserCode_RpcClearQueue);
		}

		private void MirrorProcessed()
		{
		}

		private void UserCode_RpcPlayEffects(byte[] effects)
		{
			foreach (byte b in effects)
			{
				global::Respawning.RespawnEffect respawnEffect = ((b < 128) ? OnSpawnEffects[b] : SelectionEffects[b - 128]);
				if (!respawnEffect.WhitelistEnabled || respawnEffect.WhitelistedRoles.Contains(global::PlayerRoles.PlayerRolesUtils.GetRoleId(ReferenceHub.LocalHub)))
				{
					if (respawnEffect.AnimatorEffects != null)
					{
						respawnEffect.AnimatorEffects.SetTrigger(PlayKey);
					}
					if (respawnEffect.AudioAnnouncement != null)
					{
						respawnEffect.AudioAnnouncement.Play();
					}
				}
			}
		}

		protected static void InvokeUserCode_RpcPlayEffects(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkClient.active)
			{
				global::UnityEngine.Debug.LogError("RPC RpcPlayEffects called on server.");
			}
			else
			{
				((global::Respawning.RespawnEffectsController)obj).UserCode_RpcPlayEffects(global::Mirror.NetworkReaderExtensions.ReadBytesAndSize(reader));
			}
		}

		private void UserCode_RpcCassieAnnouncement(string words, bool makeHold, bool makeNoise, bool customAnnouncement)
		{
			if (!string.IsNullOrEmpty(words))
			{
				NineTailedFoxAnnouncer.singleton.AddPhraseToQueue(words, makeNoise, rawNumber: false, makeHold, customAnnouncement);
			}
		}

		protected static void InvokeUserCode_RpcCassieAnnouncement(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkClient.active)
			{
				global::UnityEngine.Debug.LogError("RPC RpcCassieAnnouncement called on server.");
			}
			else
			{
				((global::Respawning.RespawnEffectsController)obj).UserCode_RpcCassieAnnouncement(global::Mirror.NetworkReaderExtensions.ReadString(reader), global::Mirror.NetworkReaderExtensions.ReadBoolean(reader), global::Mirror.NetworkReaderExtensions.ReadBoolean(reader), global::Mirror.NetworkReaderExtensions.ReadBoolean(reader));
			}
		}

		public void UserCode_RpcClearQueue()
		{
		}

		protected static void InvokeUserCode_RpcClearQueue(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkClient.active)
			{
				global::UnityEngine.Debug.LogError("RPC RpcClearQueue called on server.");
			}
			else
			{
				((global::Respawning.RespawnEffectsController)obj).UserCode_RpcClearQueue();
			}
		}
	}
}
