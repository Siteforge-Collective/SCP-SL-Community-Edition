namespace Interactables.Interobjects
{
	public class ElevatorManager : global::UnityEngine.MonoBehaviour
	{
		public enum ElevatorGroup
		{
			GateA = 0,
			GateB = 1,
			LczA01 = 2,
			LczA02 = 3,
			LczB01 = 4,
			LczB02 = 5,
			Nuke = 6,
			Scp049 = 7
		}

		[global::System.Serializable]
		private struct ChamberTypePair
		{
			[global::UnityEngine.SerializeField]
			private global::Interactables.Interobjects.ElevatorManager.ElevatorGroup _group;

			[global::UnityEngine.SerializeField]
			private global::Interactables.Interobjects.ElevatorChamber _prefab;

			public bool TryGet(global::Interactables.Interobjects.ElevatorManager.ElevatorGroup group, out global::Interactables.Interobjects.ElevatorChamber chamber)
			{
				chamber = _prefab;
				return _group == group;
			}
		}

		public struct ElevatorSyncMsg : global::Mirror.NetworkMessage
		{
			public byte Data;

			public ElevatorSyncMsg(global::Interactables.Interobjects.ElevatorManager.ElevatorGroup group, int targetLvl)
			{
				Misc.ByteToBools((byte)group, out var @bool, out var bool2, out var bool3, out var bool4, out var bool5, out var bool6, out var bool7, out var bool8);
				Misc.ByteToBools((byte)targetLvl, out var bool9, out var bool10, out var bool11, out bool8, out bool7, out bool6, out var _, out var _);
				Data = Misc.BoolsToByte(@bool, bool2, bool3, bool4, bool5, bool9, bool10, bool11);
			}

			public void Unpack(out global::Interactables.Interobjects.ElevatorManager.ElevatorGroup group, out int targetLvl)
			{
				Misc.ByteToBools(Data, out var @bool, out var bool2, out var bool3, out var bool4, out var bool5, out var bool6, out var bool7, out var bool8);
				group = (global::Interactables.Interobjects.ElevatorManager.ElevatorGroup)Misc.BoolsToByte(@bool, bool2, bool3, bool4, bool5);
				targetLvl = Misc.BoolsToByte(bool6, bool7, bool8);
			}
		}

		[global::UnityEngine.SerializeField]
		private global::Interactables.Interobjects.ElevatorManager.ChamberTypePair[] _customChambers;

		[global::UnityEngine.SerializeField]
		private global::Interactables.Interobjects.ElevatorChamber _defaultChamber;

		private static bool _refreshNextFrame;

		internal static readonly global::System.Collections.Generic.Dictionary<global::Interactables.Interobjects.ElevatorManager.ElevatorGroup, global::Interactables.Interobjects.ElevatorChamber> SpawnedChambers = new global::System.Collections.Generic.Dictionary<global::Interactables.Interobjects.ElevatorManager.ElevatorGroup, global::Interactables.Interobjects.ElevatorChamber>();

		private static readonly global::System.Collections.Generic.Dictionary<global::Interactables.Interobjects.ElevatorManager.ElevatorGroup, int> SyncedDestinations = new global::System.Collections.Generic.Dictionary<global::Interactables.Interobjects.ElevatorManager.ElevatorGroup, int>();

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			CustomNetworkManager.OnClientReady += delegate
			{
				global::Mirror.NetworkClient.ReplaceHandler<global::Interactables.Interobjects.ElevatorManager.ElevatorSyncMsg>(ClientReceiveMessage);
				global::Mirror.NetworkServer.ReplaceHandler<global::Interactables.Interobjects.ElevatorManager.ElevatorSyncMsg>(ServerReceiveMessage);
			};
			global::Interactables.Interobjects.ElevatorDoor.OnPairsChanged = (global::System.Action<global::Interactables.Interobjects.ElevatorManager.ElevatorGroup, global::Interactables.Interobjects.ElevatorDoor>)global::System.Delegate.Combine(global::Interactables.Interobjects.ElevatorDoor.OnPairsChanged, (global::System.Action<global::Interactables.Interobjects.ElevatorManager.ElevatorGroup, global::Interactables.Interobjects.ElevatorDoor>)delegate
			{
				_refreshNextFrame = true;
			});
			ReferenceHub.OnPlayerAdded = (global::System.Action<ReferenceHub>)global::System.Delegate.Combine(ReferenceHub.OnPlayerAdded, new global::System.Action<ReferenceHub>(ServerSendAllToPlayer));
		}

		private void Update()
		{
			if (_refreshNextFrame)
			{
				RefreshChambers();
				_refreshNextFrame = false;
			}
		}

		private static void ServerSendAllToPlayer(ReferenceHub ply)
		{
			if (!global::Mirror.NetworkServer.active || ply.isLocalPlayer)
			{
				return;
			}
			foreach (global::System.Collections.Generic.KeyValuePair<global::Interactables.Interobjects.ElevatorManager.ElevatorGroup, int> syncedDestination in SyncedDestinations)
			{
				ply.connectionToClient.Send(new global::Interactables.Interobjects.ElevatorManager.ElevatorSyncMsg(syncedDestination.Key, syncedDestination.Value));
			}
		}

		private global::Interactables.Interobjects.ElevatorChamber GetChamberForGroup(global::Interactables.Interobjects.ElevatorManager.ElevatorGroup group)
		{
			global::Interactables.Interobjects.ElevatorManager.ChamberTypePair[] customChambers = _customChambers;
			foreach (global::Interactables.Interobjects.ElevatorManager.ChamberTypePair chamberTypePair in customChambers)
			{
				if (chamberTypePair.TryGet(group, out var chamber))
				{
					return chamber;
				}
			}
			return _defaultChamber;
		}

		public static bool TrySetDestination(global::Interactables.Interobjects.ElevatorManager.ElevatorGroup group, int lvl, bool force = false)
		{
			if (!SpawnedChambers.TryGetValue(group, out var value) || value == null)
			{
				return false;
			}
			if (!value.TrySetDestination(lvl, force))
			{
				return false;
			}
			if (global::Mirror.NetworkServer.active)
			{
				global::Mirror.NetworkServer.SendToReady(new global::Interactables.Interobjects.ElevatorManager.ElevatorSyncMsg(group, lvl));
				SyncedDestinations[group] = lvl;
			}
			return true;
		}

		private static void ClientReceiveMessage(global::Interactables.Interobjects.ElevatorManager.ElevatorSyncMsg msg)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				msg.Unpack(out var group, out var targetLvl);
				SyncedDestinations[group] = targetLvl;
				TrySetDestination(group, targetLvl, force: true);
			}
		}

		private static void ServerReceiveMessage(global::Mirror.NetworkConnection conn, global::Interactables.Interobjects.ElevatorManager.ElevatorSyncMsg msg)
		{
			if (!ReferenceHub.TryGetHubNetID(conn.identity.netId, out var hub) || !global::PlayerRoles.PlayerRolesUtils.IsAlive(hub))
			{
				return;
			}
			msg.Unpack(out var group, out var targetLvl);
			if (!SpawnedChambers.TryGetValue(group, out var value) || value == null || !value.IsReady)
			{
				return;
			}
			foreach (global::Interactables.Interobjects.ElevatorPanel allPanel in value.AllPanels)
			{
				if (allPanel.AssignedChamber.AssignedGroup == group && (allPanel.AssignedChamber.ActiveLocks == global::Interactables.Interobjects.DoorUtils.DoorLockReason.None || hub.serverRoles.BypassMode) && allPanel.VerificationRule.ServerCanInteract(hub, allPanel))
				{
					if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerInteractElevator, hub, value))
					{
						TrySetDestination(group, targetLvl);
					}
					break;
				}
			}
		}

		private void RefreshChambers()
		{
			foreach (global::System.Collections.Generic.KeyValuePair<global::Interactables.Interobjects.ElevatorManager.ElevatorGroup, global::System.Collections.Generic.List<global::Interactables.Interobjects.ElevatorDoor>> allElevatorDoor in global::Interactables.Interobjects.ElevatorDoor.AllElevatorDoors)
			{
				global::Interactables.Interobjects.ElevatorManager.ElevatorGroup key = allElevatorDoor.Key;
				global::System.Collections.Generic.List<global::Interactables.Interobjects.ElevatorDoor> value = allElevatorDoor.Value;
				if (value == null || value.Count == 0 || (SpawnedChambers.TryGetValue(key, out var value2) && value2 != null))
				{
					continue;
				}
				global::Interactables.Interobjects.ElevatorChamber elevatorChamber = global::UnityEngine.Object.Instantiate(GetChamberForGroup(key));
				elevatorChamber.AssignedGroup = key;
				SpawnedChambers[key] = elevatorChamber;
				global::UnityEngine.Transform obj = elevatorChamber.transform;
				obj.position = value[0].TargetPosition;
				obj.SetParent(value[0].transform.parent);
				if (!SyncedDestinations.TryGetValue(key, out var value3))
				{
					if (!global::Mirror.NetworkServer.active)
					{
						continue;
					}
					value3 = global::UnityEngine.Random.Range(0, value.Count);
				}
				TrySetDestination(key, value3, force: true);
			}
		}
	}
}
