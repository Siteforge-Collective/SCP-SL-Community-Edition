namespace Interactables.Interobjects
{
	public class BasicDoor : global::Interactables.Interobjects.DoorUtils.DoorVariant
	{
		private static readonly int AnimHash;

		[global::UnityEngine.Header("General settings")]
		[global::UnityEngine.SerializeField]
		internal global::UnityEngine.Animator MainAnimator;

		[global::UnityEngine.SerializeField]
		internal global::UnityEngine.AudioSource MainSource;

		[global::UnityEngine.SerializeField]
		private float _cooldownDuration;

		[global::UnityEngine.SerializeField]
		private float _consideredOpenThreshold = 0.7f;

		[global::UnityEngine.SerializeField]
		private float _anticheatPassableThreshold = 0.2f;

		[global::UnityEngine.Header("These values are used to get the exact state")]
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Transform _stateMoveable;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Transform _stateStator;

		[global::UnityEngine.SerializeField]
		private float _stateMinDis;

		[global::UnityEngine.SerializeField]
		private float _stateMaxDis;

		[global::UnityEngine.HideInInspector]
		public bool UpdateAnimations;

		public global::System.Collections.Generic.List<global::UnityEngine.Collider> Scp106Colliders;

		private float _remainingAnimCooldown;

		public override bool AllowInteracting(ReferenceHub ply, byte colliderId)
		{
			return _remainingAnimCooldown <= 0f;
		}

		public override float GetExactState()
		{
			global::UnityEngine.Vector3 position = _stateMoveable.position;
			global::UnityEngine.Vector3 position2 = _stateStator.position;
			float value = global::UnityEngine.Mathf.Abs(position.x - position2.x) + global::UnityEngine.Mathf.Abs(position.y - position2.y) + global::UnityEngine.Mathf.Abs(position.z - position2.z);
			return global::UnityEngine.Mathf.Clamp01(global::UnityEngine.Mathf.InverseLerp(_stateMinDis, _stateMaxDis, value));
		}

		public override bool IsConsideredOpen()
		{
			return GetExactState() > _consideredOpenThreshold;
		}

		public override bool AnticheatPassageApproved()
		{
			if (!IsConsideredOpen())
			{
				if (!TargetState)
				{
					return GetExactState() > _anticheatPassableThreshold;
				}
				return false;
			}
			return true;
		}

		public override void LockBypassDenied(ReferenceHub ply, byte colliderId)
		{
			RpcPlayBeepSound(denied: false);
		}

		public override void PermissionsDenied(ReferenceHub ply, byte colliderId)
		{
			RpcPlayBeepSound(denied: true);
		}

		[global::Mirror.ClientRpc]
		private void RpcPlayBeepSound(bool denied)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, denied);
			SendRPCInternal(typeof(global::Interactables.Interobjects.BasicDoor), "RpcPlayBeepSound", writer, 0, includeOwner: true);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		protected override void Update()
		{
			base.Update();
			if (global::Mirror.NetworkServer.active && _remainingAnimCooldown > 0f)
			{
				_remainingAnimCooldown -= global::UnityEngine.Time.deltaTime;
			}
		}

		internal override void TargetStateChanged()
		{
			MainAnimator.SetBool(AnimHash, TargetState);
			if (global::Mirror.NetworkServer.active)
			{
				_remainingAnimCooldown = _cooldownDuration;
			}
		}

		protected override void LockChanged(ushort prevValue)
		{
			UpdateAnimations = true;
		}

		static BasicDoor()
		{
			AnimHash = global::UnityEngine.Animator.StringToHash("isOpen");
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(global::Interactables.Interobjects.BasicDoor), "RpcPlayBeepSound", InvokeUserCode_RpcPlayBeepSound);
		}

		private void MirrorProcessed()
		{
		}

		private void UserCode_RpcPlayBeepSound(bool denied)
		{
		}

		protected static void InvokeUserCode_RpcPlayBeepSound(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkClient.active)
			{
				global::UnityEngine.Debug.LogError("RPC RpcPlayBeepSound called on server.");
			}
			else
			{
				((global::Interactables.Interobjects.BasicDoor)obj).UserCode_RpcPlayBeepSound(global::Mirror.NetworkReaderExtensions.ReadBoolean(reader));
			}
		}
	}
}
