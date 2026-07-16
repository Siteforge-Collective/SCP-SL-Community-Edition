namespace Interactables.Interobjects
{
	public class Scp330Interobject : global::Mirror.NetworkBehaviour, global::Interactables.IServerInteractable, global::Interactables.IInteractable
	{
		private readonly global::System.Collections.Generic.List<global::Footprinting.Footprint> _takenCandies = new global::System.Collections.Generic.List<global::Footprinting.Footprint>();

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _takeSound;

		private const float TakeCooldown = 0.1f;

		private const int MaxAmountPerLife = 2;

		public global::Interactables.Verification.IVerificationRule VerificationRule => global::Interactables.Verification.StandardDistanceVerification.Default;

		public void ServerInteract(ReferenceHub ply, byte colliderId)
		{
			if (!global::PlayerRoles.PlayerRolesUtils.IsHuman(ply))
			{
				return;
			}
			global::Footprinting.Footprint footprint = new global::Footprinting.Footprint(ply);
			float num = 0.1f;
			int num2 = 0;
			foreach (global::Footprinting.Footprint takenCandy in _takenCandies)
			{
				if (takenCandy.SameLife(footprint))
				{
					num = global::UnityEngine.Mathf.Min(num, (float)takenCandy.Stopwatch.Elapsed.TotalSeconds);
					num2++;
				}
			}
			if (num < 0.1f)
			{
				return;
			}
			if (global::InventorySystem.Items.Usables.Scp330.Scp330Bag.ServerProcessPickup(ply, null, out var bag))
			{
				if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerInteractScp330, ply))
				{
					return;
				}
				RpcMakeSound();
				if (num2 >= 2)
				{
					ply.playerEffectsController.EnableEffect<global::CustomPlayerEffects.SeveredHands>();
					while (_takenCandies.Remove(footprint))
					{
					}
				}
				else
				{
					_takenCandies.Add(footprint);
				}
			}
			else
			{
				global::InventorySystem.Searching.Scp330SearchCompletor.ShowOverloadHint(ply, bag != null);
			}
		}

		[global::Mirror.ClientRpc]
		private void RpcMakeSound()
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			SendRPCInternal(typeof(global::Interactables.Interobjects.Scp330Interobject), "RpcMakeSound", writer, 0, includeOwner: true);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		private void MirrorProcessed()
		{
		}

		private void UserCode_RpcMakeSound()
		{
			global::AudioPooling.AudioSourcePoolManager.PlaySound(_takeSound, base.transform, 10f);
		}

		protected static void InvokeUserCode_RpcMakeSound(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkClient.active)
			{
				global::UnityEngine.Debug.LogError("RPC RpcMakeSound called on server.");
			}
			else
			{
				((global::Interactables.Interobjects.Scp330Interobject)obj).UserCode_RpcMakeSound();
			}
		}

		static Scp330Interobject()
		{
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(global::Interactables.Interobjects.Scp330Interobject), "RpcMakeSound", InvokeUserCode_RpcMakeSound);
		}
	}
}
