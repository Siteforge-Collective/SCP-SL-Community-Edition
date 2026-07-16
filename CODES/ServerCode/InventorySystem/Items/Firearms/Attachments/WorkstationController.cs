namespace InventorySystem.Items.Firearms.Attachments
{
	public class WorkstationController : global::Mirror.NetworkBehaviour, global::Interactables.IClientInteractable, global::Interactables.IInteractable, global::Interactables.IServerInteractable
	{
		private enum WorkstationStatus : byte
		{
			Offline = 0,
			PoweringUp = 1,
			PoweringDown = 2,
			Online = 3
		}

		public static readonly global::System.Collections.Generic.HashSet<global::InventorySystem.Items.Firearms.Attachments.WorkstationController> AllWorkstations = new global::System.Collections.Generic.HashSet<global::InventorySystem.Items.Firearms.Attachments.WorkstationController>();

		[global::UnityEngine.SerializeField]
		private global::Interactables.InteractableCollider _activateCollder;

		[global::Mirror.SyncVar]
		public byte Status;

		private const float StandbyDistance = 2.4f;

		private const float UpkeepTime = 30f;

		private const float CheckTime = 5f;

		private const float PowerupTime = 5.6f;

		private const float PowerdownTime = 2.5f;

		private byte _prevStatus;

		private ReferenceHub _knownUser;

		private readonly global::System.Diagnostics.Stopwatch _serverStopwatch = new global::System.Diagnostics.Stopwatch();

		public global::Interactables.Verification.IVerificationRule VerificationRule => global::Interactables.Verification.StandardDistanceVerification.Default;

		public byte NetworkStatus
		{
			get
			{
				return Status;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref Status))
				{
					byte status = Status;
					SetSyncVar(value, ref Status, 1uL);
				}
			}
		}

		public void ClientInteract(global::Interactables.InteractableCollider collider)
		{
		}

		public void ServerInteract(ReferenceHub ply, byte colliderId)
		{
			if (colliderId == _activateCollder.ColliderId && Status == 0)
			{
				NetworkStatus = 1;
				_serverStopwatch.Restart();
			}
		}

		private void Start()
		{
			AllWorkstations.Add(this);
		}

		private void OnDestroy()
		{
			AllWorkstations.Remove(this);
		}

		private void Update()
		{
			if (!global::Mirror.NetworkServer.active || Status == 0)
			{
				return;
			}
			switch ((global::InventorySystem.Items.Firearms.Attachments.WorkstationController.WorkstationStatus)Status)
			{
			case global::InventorySystem.Items.Firearms.Attachments.WorkstationController.WorkstationStatus.PoweringUp:
				if (_serverStopwatch.Elapsed.TotalSeconds > 5.599999904632568)
				{
					NetworkStatus = 3;
					_serverStopwatch.Restart();
				}
				break;
			case global::InventorySystem.Items.Firearms.Attachments.WorkstationController.WorkstationStatus.PoweringDown:
				if (_serverStopwatch.Elapsed.TotalSeconds > 2.5)
				{
					NetworkStatus = 0;
					_serverStopwatch.Stop();
				}
				break;
			case global::InventorySystem.Items.Firearms.Attachments.WorkstationController.WorkstationStatus.Online:
				if (_serverStopwatch.Elapsed.TotalSeconds < 30.0)
				{
					if (!(_serverStopwatch.Elapsed.TotalSeconds > 5.0))
					{
						break;
					}
					if (!IsInRange(_knownUser))
					{
						{
							foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
							{
								if (IsInRange(allHub))
								{
									_knownUser = allHub;
									_serverStopwatch.Restart();
									break;
								}
							}
							break;
						}
					}
					_serverStopwatch.Restart();
				}
				else
				{
					NetworkStatus = 2;
					_serverStopwatch.Restart();
				}
				break;
			}
		}

		public bool IsInRange(ReferenceHub hub)
		{
			if (hub != null && global::PlayerRoles.PlayerRolesUtils.IsAlive(hub) && global::UnityEngine.Mathf.Abs(hub.transform.position.y - base.transform.position.y) < 2.4f && global::UnityEngine.Mathf.Abs(hub.transform.position.x - base.transform.position.x) < 2.4f)
			{
				return global::UnityEngine.Mathf.Abs(hub.transform.position.z - base.transform.position.z) < 2.4f;
			}
			return false;
		}

		private void MirrorProcessed()
		{
		}

		public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
		{
			bool result = base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				global::Mirror.NetworkWriterExtensions.WriteByte(writer, Status);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteByte(writer, Status);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				byte status = Status;
				NetworkStatus = global::Mirror.NetworkReaderExtensions.ReadByte(reader);
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 1L) != 0L)
			{
				byte status2 = Status;
				NetworkStatus = global::Mirror.NetworkReaderExtensions.ReadByte(reader);
			}
		}
	}
}
