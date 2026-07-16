namespace PlayerRoles.PlayableScps.Scp106
{
	public static class Scp106PocketItemManager
	{
		private class PocketItem
		{
			public double TriggerTime;

			public bool Remove;

			public bool WarningSent;

			public global::RelativePositioning.RelativePosition DropPosition;
		}

		public struct WarningMessage : global::Mirror.NetworkMessage
		{
			public global::RelativePositioning.RelativePosition Position;
		}

		private const float WarningTime = 3f;

		private const float RaycastRange = 30f;

		private const float SoundRange = 12f;

		private const float SpawnOffset = 0.3f;

		private const float RandomEscapeVelocity = 0.2f;

		private const int MaxValidPositions = 64;

		private static readonly global::UnityEngine.Vector3[] ValidPositionsNonAlloc = new global::UnityEngine.Vector3[64];

		private static readonly global::System.Collections.Generic.HashSet<global::InventorySystem.Items.Pickups.ItemPickupBase> ToRemove = new global::System.Collections.Generic.HashSet<global::InventorySystem.Items.Pickups.ItemPickupBase>();

		private static readonly global::System.Collections.Generic.Dictionary<global::InventorySystem.Items.Pickups.ItemPickupBase, global::PlayerRoles.PlayableScps.Scp106.Scp106PocketItemManager.PocketItem> TrackedItems = new global::System.Collections.Generic.Dictionary<global::InventorySystem.Items.Pickups.ItemPickupBase, global::PlayerRoles.PlayableScps.Scp106.Scp106PocketItemManager.PocketItem>();

		private static readonly global::UnityEngine.Vector2 HeightLimit = new global::UnityEngine.Vector2(-1990f, -2002f);

		private static readonly global::UnityEngine.Vector2 TimerRage = new global::UnityEngine.Vector2(90f, 240f);

		private static readonly float[] RecycleChances = new float[3] { 0.5f, 0.7f, 1f };

		private static float RandomVel => global::UnityEngine.Random.Range(-0.2f, 0.2f);

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			global::InventorySystem.Items.Pickups.ItemPickupBase.OnPickupAdded += OnAdded;
			global::InventorySystem.Items.Pickups.ItemPickupBase.OnPickupDestroyed += OnRemoved;
			StaticUnityMethods.OnUpdate += Update;
			CustomNetworkManager.OnClientReady += delegate
			{
				TrackedItems.Clear();
				global::Mirror.NetworkClient.ReplaceHandler(delegate(global::PlayerRoles.PlayableScps.Scp106.Scp106PocketItemManager.WarningMessage x)
				{
					if (global::PlayerRoles.PlayerRoleLoader.TryGetRoleTemplate<global::PlayerRoles.PlayableScps.Scp106.Scp106Role>(global::PlayerRoles.RoleTypeId.Scp106, out var result))
					{
						global::AudioPooling.AudioSourcePoolManager.PlaySound(result.ItemSpawnSound, x.Position, 12f);
					}
				});
			};
		}

		private static void Update()
		{
			if (!global::Mirror.NetworkServer.active)
			{
				return;
			}
			bool flag = false;
			foreach (global::System.Collections.Generic.KeyValuePair<global::InventorySystem.Items.Pickups.ItemPickupBase, global::PlayerRoles.PlayableScps.Scp106.Scp106PocketItemManager.PocketItem> trackedItem in TrackedItems)
			{
				if (trackedItem.Key == null || !ValidateHeight(trackedItem.Key))
				{
					flag |= ToRemove.Add(trackedItem.Key);
					continue;
				}
				global::PlayerRoles.PlayableScps.Scp106.Scp106PocketItemManager.PocketItem value = trackedItem.Value;
				double num = value.TriggerTime - global::Mirror.NetworkTime.time;
				if (num > 3.0)
				{
					continue;
				}
				if (!value.Remove && !value.WarningSent)
				{
					global::Mirror.NetworkServer.SendToAll(new global::PlayerRoles.PlayableScps.Scp106.Scp106PocketItemManager.WarningMessage
					{
						Position = value.DropPosition
					});
					value.WarningSent = true;
				}
				if (!(num > 0.0))
				{
					global::InventorySystem.Items.Pickups.ItemPickupBase key = trackedItem.Key;
					global::UnityEngine.Rigidbody component;
					if (value.Remove)
					{
						key.DestroySelf();
					}
					else if (key.TryGetComponent<global::UnityEngine.Rigidbody>(out component))
					{
						component.velocity = new global::UnityEngine.Vector3(RandomVel, global::UnityEngine.Physics.gravity.y, RandomVel);
						key.transform.position = value.DropPosition.Position;
						key.RefreshPositionAndRotation();
					}
					flag |= ToRemove.Add(key);
				}
			}
			if (flag)
			{
				global::Utils.NonAllocLINQ.HashsetExtensions.ForEach(ToRemove, delegate(global::InventorySystem.Items.Pickups.ItemPickupBase x)
				{
					TrackedItems.Remove(x);
				});
			}
		}

		private static void OnAdded(global::InventorySystem.Items.Pickups.ItemPickupBase ipb)
		{
			if (global::Mirror.NetworkServer.active && ValidateHeight(ipb) && global::InventorySystem.InventoryItemLoader.TryGetItem<global::InventorySystem.Items.ItemBase>(ipb.Info.ItemId, out var result))
			{
				TrackedItems.Add(ipb, new global::PlayerRoles.PlayableScps.Scp106.Scp106PocketItemManager.PocketItem
				{
					Remove = (global::UnityEngine.Random.value > RecycleChances[GetRarity(result)]),
					TriggerTime = global::Mirror.NetworkTime.time + (double)global::UnityEngine.Random.Range(TimerRage.x, TimerRage.y),
					DropPosition = GetRandomValidSpawnPosition(),
					WarningSent = false
				});
			}
		}

		private static void OnRemoved(global::InventorySystem.Items.Pickups.ItemPickupBase ipb)
		{
			if (global::Mirror.NetworkServer.active)
			{
				TrackedItems.Remove(ipb);
			}
		}

		private static bool ValidateHeight(global::InventorySystem.Items.Pickups.ItemPickupBase ipb)
		{
			float y = ipb.transform.position.y;
			if (y >= HeightLimit.y)
			{
				return y <= HeightLimit.x;
			}
			return false;
		}

		private static int GetRarity(global::InventorySystem.Items.ItemBase ib)
		{
			int num = 0;
			if (HasFlagFast(ib, ItemTierFlags.Rare))
			{
				num++;
			}
			if (HasFlagFast(ib, ItemTierFlags.MilitaryGrade))
			{
				num++;
			}
			if (HasFlagFast(ib, ItemTierFlags.ExtraRare))
			{
				num += 2;
			}
			return global::UnityEngine.Mathf.Min(num, RecycleChances.Length - 1);
		}

		private static global::RelativePositioning.RelativePosition GetRandomValidSpawnPosition()
		{
			int num = 0;
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (!(allHub.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole))
				{
					continue;
				}
				global::UnityEngine.Vector3 position = fpcRole.FpcModule.Position;
				if (!(position.y < HeightLimit.x) && TryGetRoofPosition(position, out var result))
				{
					ValidPositionsNonAlloc[num] = result;
					if (++num > 64)
					{
						break;
					}
				}
			}
			if (num > 0)
			{
				return new global::RelativePositioning.RelativePosition(ValidPositionsNonAlloc[global::UnityEngine.Random.Range(0, num)]);
			}
			foreach (global::MapGeneration.RoomIdentifier allRoomIdentifier in global::MapGeneration.RoomIdentifier.AllRoomIdentifiers)
			{
				if ((allRoomIdentifier.Zone == global::MapGeneration.FacilityZone.HeavyContainment || allRoomIdentifier.Zone == global::MapGeneration.FacilityZone.Entrance) && TryGetRoofPosition(allRoomIdentifier.transform.position, out var result2))
				{
					ValidPositionsNonAlloc[num] = result2;
					if (++num > 64)
					{
						break;
					}
				}
			}
			if (num == 0)
			{
				throw new global::System.InvalidOperationException("GetRandomValidSpawnPosition found no valid spawn positions.");
			}
			int num2 = global::UnityEngine.Random.Range(0, num);
			return new global::RelativePositioning.RelativePosition(ValidPositionsNonAlloc[num2]);
		}

		private static bool TryGetRoofPosition(global::UnityEngine.Vector3 point, out global::UnityEngine.Vector3 result)
		{
			if (global::UnityEngine.Physics.Raycast(point, global::UnityEngine.Vector3.up, out var hitInfo, 30f, global::PlayerRoles.FirstPersonControl.FpcStateProcessor.Mask))
			{
				result = hitInfo.point + global::UnityEngine.Vector3.down * 0.3f;
				return true;
			}
			result = global::UnityEngine.Vector3.zero;
			return false;
		}

		private static bool HasFlagFast(global::InventorySystem.Items.ItemBase ib, ItemTierFlags flag)
		{
			return (ib.TierFlags & flag) == flag;
		}
	}
}
