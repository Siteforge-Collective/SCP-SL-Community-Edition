namespace MapGeneration.Distributors
{
	public class Locker : global::MapGeneration.Distributors.SpawnableStructure, global::Interactables.IServerInteractable, global::Interactables.IInteractable
	{
		public global::MapGeneration.Distributors.LockerLoot[] Loot;

		public global::MapGeneration.Distributors.LockerChamber[] Chambers;

		[global::Mirror.SyncVar]
		public ushort OpenedChambers;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _grantedBeep;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _deniedBeep;

		[global::UnityEngine.Header("Leave 0 to fill all chambers")]
		[global::UnityEngine.SerializeField]
		private int _minChambersToFill;

		[global::UnityEngine.SerializeField]
		private int _maxChambersToFill;

		private ushort _prevOpened;

		public global::Interactables.Verification.IVerificationRule VerificationRule => global::Interactables.Verification.StandardDistanceVerification.Default;

		public ushort NetworkOpenedChambers
		{
			get
			{
				return OpenedChambers;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref OpenedChambers))
				{
					ushort openedChambers = OpenedChambers;
					SetSyncVar(value, ref OpenedChambers, 1uL);
				}
			}
		}

		public void ServerInteract(ReferenceHub ply, byte colliderId)
		{
			if (colliderId >= Chambers.Length || !Chambers[colliderId].CanInteract)
			{
				return;
			}
			bool flag = !CheckPerms(Chambers[colliderId].RequiredPermissions, ply) && !ply.serverRoles.BypassMode;
			if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerInteractLocker, ply, this, Chambers[colliderId], !flag))
			{
				if (flag)
				{
					RpcPlayDenied(colliderId);
					return;
				}
				Chambers[colliderId].SetDoor(!Chambers[colliderId].IsOpen, _grantedBeep);
				RefreshOpenedSyncvar();
			}
		}

		protected virtual void Start()
		{
			if (!global::Mirror.NetworkServer.active)
			{
				return;
			}
			global::System.Collections.Generic.List<global::MapGeneration.Distributors.LockerChamber> list = new global::System.Collections.Generic.List<global::MapGeneration.Distributors.LockerChamber>(Chambers);
			if (_minChambersToFill != 0 && _maxChambersToFill >= _minChambersToFill)
			{
				int num = Chambers.Length - global::UnityEngine.Random.Range(_minChambersToFill, _maxChambersToFill + 1);
				for (int i = 0; i < num; i++)
				{
					list.RemoveAt(global::UnityEngine.Random.Range(0, list.Count));
				}
			}
			foreach (global::MapGeneration.Distributors.LockerChamber item in list)
			{
				FillChamber(item);
			}
		}

		protected virtual void Update()
		{
			if (_prevOpened != OpenedChambers)
			{
				int num = 1;
				global::MapGeneration.Distributors.LockerChamber[] chambers = Chambers;
				foreach (global::MapGeneration.Distributors.LockerChamber lockerChamber in chambers)
				{
					lockerChamber.SetDoor((OpenedChambers & num) == num || !lockerChamber.AnimatorSet, _grantedBeep);
					num *= 2;
				}
				_prevOpened = OpenedChambers;
			}
		}

		private void RefreshOpenedSyncvar()
		{
			int num = 1;
			int num2 = 0;
			global::MapGeneration.Distributors.LockerChamber[] chambers = Chambers;
			for (int i = 0; i < chambers.Length; i++)
			{
				if (chambers[i].IsOpen)
				{
					num2 += num;
				}
				num *= 2;
			}
			if (num2 != OpenedChambers)
			{
				NetworkOpenedChambers = (ushort)num2;
			}
		}

		private bool CheckPerms(global::Interactables.Interobjects.DoorUtils.KeycardPermissions perms, ReferenceHub ply)
		{
			if ((int)perms > 0)
			{
				if (ply.inventory.CurInstance == null)
				{
					return false;
				}
				if (!(ply.inventory.CurInstance is global::InventorySystem.Items.Keycards.KeycardItem keycardItem))
				{
					return false;
				}
				if (!global::Interactables.Interobjects.DoorUtils.DoorPermissionUtils.HasFlagFast(keycardItem.Permissions, perms))
				{
					return false;
				}
			}
			return true;
		}

		[global::Mirror.ClientRpc]
		private void RpcPlayDenied(byte chamberId)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.NetworkWriterExtensions.WriteByte(writer, chamberId);
			SendRPCInternal(typeof(global::MapGeneration.Distributors.Locker), "RpcPlayDenied", writer, 0, includeOwner: true);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		private void FillChamber(global::MapGeneration.Distributors.LockerChamber ch)
		{
			global::System.Collections.Generic.List<int> list = global::NorthwoodLib.Pools.ListPool<int>.Shared.Rent();
			for (int i = 0; i < Loot.Length; i++)
			{
				if (Loot[i].RemainingUses > 0 && (ch.AcceptableItems.Length == 0 || ch.AcceptableItems.Contains(Loot[i].TargetItem)))
				{
					for (int j = 0; j <= Loot[i].ProbabilityPoints; j++)
					{
						list.Add(i);
					}
				}
			}
			if (list.Count > 0)
			{
				int num = list[global::UnityEngine.Random.Range(0, list.Count)];
				ch.SpawnItem(Loot[num].TargetItem, global::UnityEngine.Random.Range(1, Loot[num].MaxPerChamber + 1));
				Loot[num].RemainingUses--;
			}
			global::NorthwoodLib.Pools.ListPool<int>.Shared.Return(list);
		}

		private void MirrorProcessed()
		{
		}

		private void UserCode_RpcPlayDenied(byte chamberId)
		{
			if (chamberId <= Chambers.Length)
			{
				Chambers[chamberId].PlayDenied(_deniedBeep);
			}
		}

		protected static void InvokeUserCode_RpcPlayDenied(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkClient.active)
			{
				global::UnityEngine.Debug.LogError("RPC RpcPlayDenied called on server.");
			}
			else
			{
				((global::MapGeneration.Distributors.Locker)obj).UserCode_RpcPlayDenied(global::Mirror.NetworkReaderExtensions.ReadByte(reader));
			}
		}

		static Locker()
		{
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(global::MapGeneration.Distributors.Locker), "RpcPlayDenied", InvokeUserCode_RpcPlayDenied);
		}

		public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
		{
			bool result = base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, OpenedChambers);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, OpenedChambers);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				ushort openedChambers = OpenedChambers;
				NetworkOpenedChambers = global::Mirror.NetworkReaderExtensions.ReadUInt16(reader);
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 1L) != 0L)
			{
				ushort openedChambers2 = OpenedChambers;
				NetworkOpenedChambers = global::Mirror.NetworkReaderExtensions.ReadUInt16(reader);
			}
		}
	}
}
