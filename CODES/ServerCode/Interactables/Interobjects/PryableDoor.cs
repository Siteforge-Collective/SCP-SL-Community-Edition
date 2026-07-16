namespace Interactables.Interobjects
{
	public class PryableDoor : global::Interactables.Interobjects.BasicDoor, global::Interactables.Interobjects.DoorUtils.IScp106PassableDoor
	{
		private static readonly int PryAnimHash;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _prySound;

		[global::UnityEngine.SerializeField]
		private global::Interactables.Interobjects.DoorUtils.DoorLockReason _blockPryingMask;

		[global::UnityEngine.SerializeField]
		private float _pryAnimDuration;

		public global::UnityEngine.Transform[] PryPositions;

		private float _remainingPryCooldown;

		public bool IsScp106Passable => true;

		[global::Mirror.Server]
		public bool TryPryGate(ReferenceHub player)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogWarning("[Server] function 'System.Boolean Interactables.Interobjects.PryableDoor::TryPryGate(ReferenceHub)' called when server was not active");
				return default(bool);
			}
			if (_blockPryingMask != global::Interactables.Interobjects.DoorUtils.DoorLockReason.None && global::Interactables.Interobjects.DoorUtils.DoorLockUtils.HasFlagFast((global::Interactables.Interobjects.DoorUtils.DoorLockReason)ActiveLocks, _blockPryingMask))
			{
				return false;
			}
			if (AllowInteracting(null, 0))
			{
				if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp096PryingGate, player, this))
				{
					return false;
				}
				RpcPryGate();
				_remainingPryCooldown = _pryAnimDuration;
				return true;
			}
			return false;
		}

		[global::Mirror.ClientRpc]
		private void RpcPryGate()
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			SendRPCInternal(typeof(global::Interactables.Interobjects.PryableDoor), "RpcPryGate", writer, 0, includeOwner: true);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		public override bool AllowInteracting(ReferenceHub ply, byte colliderId)
		{
			if (_remainingPryCooldown <= 0f)
			{
				return base.AllowInteracting(ply, colliderId);
			}
			return false;
		}

		protected override void Update()
		{
			base.Update();
			if (_remainingPryCooldown > 0f)
			{
				_remainingPryCooldown -= global::UnityEngine.Time.deltaTime;
				if (_remainingPryCooldown <= 0f)
				{
					MainAnimator.ResetTrigger(PryAnimHash);
				}
			}
		}

		static PryableDoor()
		{
			PryAnimHash = global::UnityEngine.Animator.StringToHash("PryGate");
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(global::Interactables.Interobjects.PryableDoor), "RpcPryGate", InvokeUserCode_RpcPryGate);
		}

		private void MirrorProcessed()
		{
		}

		private void UserCode_RpcPryGate()
		{
			MainAnimator.SetTrigger(PryAnimHash);
			MainSource.PlayOneShot(_prySound);
		}

		protected static void InvokeUserCode_RpcPryGate(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkClient.active)
			{
				global::UnityEngine.Debug.LogError("RPC RpcPryGate called on server.");
			}
			else
			{
				((global::Interactables.Interobjects.PryableDoor)obj).UserCode_RpcPryGate();
			}
		}
	}
}
