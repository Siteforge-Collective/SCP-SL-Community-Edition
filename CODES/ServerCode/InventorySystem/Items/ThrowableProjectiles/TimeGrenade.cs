namespace InventorySystem.Items.ThrowableProjectiles
{
	public abstract class TimeGrenade : global::InventorySystem.Items.ThrowableProjectiles.ThrownProjectile
	{
		[global::UnityEngine.SerializeField]
		private float _fuseTime;

		private bool _alreadyDetonated;

		public float TargetTime { get; protected set; }

		[global::Mirror.ClientRpc]
		private void RpcSetTime(float time)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.NetworkWriterExtensions.WriteSingle(writer, time);
			SendRPCInternal(typeof(global::InventorySystem.Items.ThrowableProjectiles.TimeGrenade), "RpcSetTime", writer, 0, includeOwner: true);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		protected abstract void ServerFuseEnd();

		public override void ServerActivate()
		{
			RpcSetTime(_fuseTime);
		}

		protected override void Update()
		{
			base.Update();
			if (global::Mirror.NetworkServer.active && !_alreadyDetonated && TargetTime != 0f && !(global::UnityEngine.Time.timeSinceLevelLoad < TargetTime))
			{
				ServerFuseEnd();
				_alreadyDetonated = true;
			}
		}

		private void MirrorProcessed()
		{
		}

		private void UserCode_RpcSetTime(float time)
		{
			TargetTime = global::UnityEngine.Time.timeSinceLevelLoad + time;
		}

		protected static void InvokeUserCode_RpcSetTime(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkClient.active)
			{
				global::UnityEngine.Debug.LogError("RPC RpcSetTime called on server.");
			}
			else
			{
				((global::InventorySystem.Items.ThrowableProjectiles.TimeGrenade)obj).UserCode_RpcSetTime(global::Mirror.NetworkReaderExtensions.ReadSingle(reader));
			}
		}

		static TimeGrenade()
		{
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(global::InventorySystem.Items.ThrowableProjectiles.TimeGrenade), "RpcSetTime", InvokeUserCode_RpcSetTime);
		}
	}
}
