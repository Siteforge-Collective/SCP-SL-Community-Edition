namespace Interactables.Interobjects
{
	public class CheckpointDoor : global::Interactables.Interobjects.DoorUtils.DoorVariant, global::Interactables.Interobjects.DoorUtils.IDamageableDoor
	{
		private enum CheckpointSequenceStage
		{
			Idle = 0,
			Granted = 1,
			Open = 2,
			Closing = 3
		}

		private enum CheckpointErrorType : byte
		{
			Denied = 0,
			LockedDown = 1,
			Destroyed = 2
		}

		[global::UnityEngine.SerializeField]
		private global::Interactables.Interobjects.DoorUtils.DoorVariant[] _subDoors;

		[global::UnityEngine.SerializeField]
		private float _openingTime;

		[global::UnityEngine.SerializeField]
		private float _waitTime;

		[global::UnityEngine.SerializeField]
		private float _warningTime;

		private float _mainTimer;

		private bool _permanentDestroyment;

		private global::Interactables.Interobjects.CheckpointDoor.CheckpointSequenceStage _currentSequence;

		private string _warningText;

		public global::Interactables.Interobjects.DoorUtils.DoorVariant[] SubDoors => _subDoors;

		public bool IsDestroyed
		{
			get
			{
				return GetHealthPercent() == 0f;
			}
			set
			{
			}
		}

		public override bool AllowInteracting(ReferenceHub ply, byte colliderId)
		{
			int num = 0;
			global::Interactables.Interobjects.DoorUtils.DoorVariant[] subDoors = _subDoors;
			foreach (global::Interactables.Interobjects.DoorUtils.DoorVariant doorVariant in subDoors)
			{
				if (doorVariant is global::Interactables.Interobjects.DoorUtils.IDamageableDoor damageableDoor && damageableDoor.IsDestroyed)
				{
					num++;
				}
				else if (!doorVariant.AllowInteracting(null, colliderId))
				{
					return false;
				}
			}
			if (num >= _subDoors.Length)
			{
				RpcPlayBeepSound(2);
				return false;
			}
			return _currentSequence == global::Interactables.Interobjects.CheckpointDoor.CheckpointSequenceStage.Idle;
		}

		public override float GetExactState()
		{
			if (_subDoors.Length == 0)
			{
				return 0f;
			}
			float num = 0f;
			global::Interactables.Interobjects.DoorUtils.DoorVariant[] subDoors = _subDoors;
			for (int i = 0; i < subDoors.Length; i++)
			{
				float exactState = subDoors[i].GetExactState();
				if (num < exactState)
				{
					num = exactState;
				}
			}
			return num;
		}

		public override bool IsConsideredOpen()
		{
			global::Interactables.Interobjects.DoorUtils.DoorVariant[] subDoors = _subDoors;
			for (int i = 0; i < subDoors.Length; i++)
			{
				if (subDoors[i].IsConsideredOpen())
				{
					return true;
				}
			}
			return false;
		}

		public override void LockBypassDenied(ReferenceHub ply, byte colliderId)
		{
			RpcPlayBeepSound(1);
		}

		public override void PermissionsDenied(ReferenceHub ply, byte colliderId)
		{
			RpcPlayBeepSound(0);
		}

		public override bool AnticheatPassageApproved()
		{
			global::Interactables.Interobjects.DoorUtils.DoorVariant[] subDoors = _subDoors;
			for (int i = 0; i < subDoors.Length; i++)
			{
				if (subDoors[i].AnticheatPassageApproved())
				{
					return true;
				}
			}
			return false;
		}

		protected override void Start()
		{
			base.Start();
		}

		protected override void LockChanged(ushort prevValue)
		{
		}

		protected override void Update()
		{
			base.Update();
			UpdateSequence();
		}

		private void UpdateSequence()
		{
			bool flag = global::Interactables.Interobjects.DoorUtils.DoorLockUtils.HasFlagFast((global::Interactables.Interobjects.DoorUtils.DoorLockReason)ActiveLocks, global::Interactables.Interobjects.DoorUtils.DoorLockReason.DecontLockdown);
			bool flag2 = global::Interactables.Interobjects.DoorUtils.DoorLockUtils.HasFlagFast((global::Interactables.Interobjects.DoorUtils.DoorLockReason)ActiveLocks, global::Interactables.Interobjects.DoorUtils.DoorLockReason.DecontEvacuate) || global::Interactables.Interobjects.DoorUtils.DoorLockUtils.HasFlagFast((global::Interactables.Interobjects.DoorUtils.DoorLockReason)ActiveLocks, global::Interactables.Interobjects.DoorUtils.DoorLockReason.Warhead);
			if (TargetState && _currentSequence == global::Interactables.Interobjects.CheckpointDoor.CheckpointSequenceStage.Idle)
			{
				if (global::Mirror.NetworkServer.active)
				{
					ToggleAllDoors(newState: true);
				}
				_currentSequence = global::Interactables.Interobjects.CheckpointDoor.CheckpointSequenceStage.Granted;
				_mainTimer = 0f;
				return;
			}
			switch (_currentSequence)
			{
			case global::Interactables.Interobjects.CheckpointDoor.CheckpointSequenceStage.Granted:
				_mainTimer += global::UnityEngine.Time.deltaTime;
				if (_mainTimer > _openingTime)
				{
					_currentSequence = global::Interactables.Interobjects.CheckpointDoor.CheckpointSequenceStage.Open;
					_mainTimer = 0f;
				}
				break;
			case global::Interactables.Interobjects.CheckpointDoor.CheckpointSequenceStage.Open:
				if (global::Mirror.NetworkServer.active)
				{
					if (!flag2)
					{
						_mainTimer += global::UnityEngine.Time.deltaTime;
					}
					if (_mainTimer > _waitTime || flag)
					{
						_mainTimer = 0f;
						base.NetworkTargetState = false;
					}
				}
				if (!TargetState)
				{
					_currentSequence = global::Interactables.Interobjects.CheckpointDoor.CheckpointSequenceStage.Closing;
				}
				break;
			case global::Interactables.Interobjects.CheckpointDoor.CheckpointSequenceStage.Closing:
			{
				if (global::Mirror.NetworkServer.active)
				{
					_mainTimer += global::UnityEngine.Time.deltaTime;
					if (_mainTimer > _warningTime || flag)
					{
						_currentSequence = global::Interactables.Interobjects.CheckpointDoor.CheckpointSequenceStage.Idle;
						ToggleAllDoors(newState: false);
						if (!global::Interactables.Interobjects.DoorUtils.DoorLockUtils.HasFlagFast(global::Interactables.Interobjects.DoorUtils.DoorLockUtils.GetMode((global::Interactables.Interobjects.DoorUtils.DoorLockReason)ActiveLocks), global::Interactables.Interobjects.DoorUtils.DoorLockMode.CanClose) && global::Interactables.Interobjects.DoorUtils.DoorLockUtils.HasFlagFast(global::Interactables.Interobjects.DoorUtils.DoorLockUtils.GetMode((global::Interactables.Interobjects.DoorUtils.DoorLockReason)ActiveLocks), global::Interactables.Interobjects.DoorUtils.DoorLockMode.CanOpen))
						{
							base.NetworkTargetState = true;
						}
					}
					break;
				}
				global::Interactables.Interobjects.DoorUtils.DoorVariant[] subDoors = _subDoors;
				foreach (global::Interactables.Interobjects.DoorUtils.DoorVariant doorVariant in subDoors)
				{
					if ((!(doorVariant is global::Interactables.Interobjects.DoorUtils.IDamageableDoor damageableDoor) || !damageableDoor.IsDestroyed) && doorVariant.GetExactState() >= 1f)
					{
						return;
					}
				}
				_currentSequence = global::Interactables.Interobjects.CheckpointDoor.CheckpointSequenceStage.Idle;
				break;
			}
			}
		}

		private void ToggleAllDoors(bool newState)
		{
			global::Interactables.Interobjects.DoorUtils.DoorVariant[] subDoors = _subDoors;
			foreach (global::Interactables.Interobjects.DoorUtils.DoorVariant doorVariant in subDoors)
			{
				if (!(doorVariant is global::Interactables.Interobjects.DoorUtils.IDamageableDoor damageableDoor) || !damageableDoor.IsDestroyed)
				{
					doorVariant.NetworkTargetState = newState;
				}
			}
		}

		[global::Mirror.ClientRpc]
		private void RpcPlayBeepSound(byte deniedType)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.NetworkWriterExtensions.WriteByte(writer, deniedType);
			SendRPCInternal(typeof(global::Interactables.Interobjects.CheckpointDoor), "RpcPlayBeepSound", writer, 0, includeOwner: true);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		public bool ServerDamage(float hp, global::Interactables.Interobjects.DoorUtils.DoorDamageType type)
		{
			bool flag = false;
			global::Interactables.Interobjects.DoorUtils.DoorVariant[] subDoors = _subDoors;
			for (int i = 0; i < subDoors.Length; i++)
			{
				if (subDoors[i] is global::Interactables.Interobjects.DoorUtils.IDamageableDoor damageableDoor)
				{
					flag |= damageableDoor.ServerDamage(hp, type);
				}
			}
			return flag;
		}

		public void ClientDestroyEffects()
		{
		}

		public float GetHealthPercent()
		{
			float num = 1f;
			global::Interactables.Interobjects.DoorUtils.DoorVariant[] subDoors = _subDoors;
			for (int i = 0; i < subDoors.Length; i++)
			{
				if (subDoors[i] is global::Interactables.Interobjects.DoorUtils.IDamageableDoor damageableDoor)
				{
					num *= damageableDoor.GetHealthPercent();
				}
			}
			return num;
		}

		private void MirrorProcessed()
		{
		}

		private void UserCode_RpcPlayBeepSound(byte deniedType)
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
				((global::Interactables.Interobjects.CheckpointDoor)obj).UserCode_RpcPlayBeepSound(global::Mirror.NetworkReaderExtensions.ReadByte(reader));
			}
		}

		static CheckpointDoor()
		{
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(global::Interactables.Interobjects.CheckpointDoor), "RpcPlayBeepSound", InvokeUserCode_RpcPlayBeepSound);
		}
	}
}
