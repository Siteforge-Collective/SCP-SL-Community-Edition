namespace PlayerRoles.PlayableScps.Scp106
{
	public static class Scp106PocketExitFinder
	{
		private const int RequiredTriggers = 2;

		private const int MaxArraySize = 64;

		private const int MaxDistanceSqr = 750;

		private const float ZombieSqrModifier = 0.3f;

		private const float RaycastRange = 11f;

		private const float SurfaceRaycastRange = 45f;

		private const float AngleVariation = 30f;

		private static readonly global::MapGeneration.RoomName[] BlacklistedRooms = new global::MapGeneration.RoomName[6]
		{
			global::MapGeneration.RoomName.Hcz079,
			global::MapGeneration.RoomName.LczCheckpointA,
			global::MapGeneration.RoomName.LczCheckpointB,
			global::MapGeneration.RoomName.HczCheckpointToEntranceZone,
			global::MapGeneration.RoomName.LczClassDSpawn,
			global::MapGeneration.RoomName.HczTesla
		};

		private static readonly global::System.Collections.Generic.Dictionary<global::MapGeneration.FacilityZone, global::Interactables.Interobjects.DoorUtils.DoorVariant[]> WhitelistedDoorsForZone = new global::System.Collections.Generic.Dictionary<global::MapGeneration.FacilityZone, global::Interactables.Interobjects.DoorUtils.DoorVariant[]>();

		private static readonly global::Interactables.Interobjects.DoorUtils.DoorVariant[] DoorsNonAlloc = new global::Interactables.Interobjects.DoorUtils.DoorVariant[64];

		private static readonly global::UnityEngine.Vector3[] PositionsCache = new global::UnityEngine.Vector3[64];

		private static readonly bool[] PositionModifiers = new bool[64];

		private static readonly int Mask = global::UnityEngine.LayerMask.GetMask("Default", "Glass");

		private static readonly global::UnityEngine.Vector3 GroundOffset = new global::UnityEngine.Vector3(0f, 0.25f, 0f);

		public static global::UnityEngine.Vector3 GetBestExitPosition(global::PlayerRoles.FirstPersonControl.IFpcRole role)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				throw new global::System.InvalidOperationException("Scp106PocketExitFinder.GetBestExitPosition is a server-side only method!");
			}
			if (!(role is global::PlayerRoles.PlayerRoleBase playerRoleBase) || !playerRoleBase.TryGetOwner(out var hub))
			{
				throw new global::System.InvalidOperationException("Scp106PocketExitFinder.GetBestExitPosition provided with non-compatible role!");
			}
			global::UnityEngine.Vector3 position = hub.playerEffectsController.GetEffect<global::CustomPlayerEffects.Corroding>().CapturePosition.Position;
			global::MapGeneration.RoomIdentifier roomIdentifier = global::MapGeneration.RoomIdUtils.RoomAtPositionRaycasts(position);
			if (roomIdentifier == null)
			{
				return position;
			}
			global::Interactables.Interobjects.DoorUtils.DoorVariant[] whitelistedDoorsForZone = GetWhitelistedDoorsForZone(roomIdentifier.Zone);
			if (whitelistedDoorsForZone.Length != 0)
			{
				global::Interactables.Interobjects.DoorUtils.DoorVariant randomDoor = GetRandomDoor(whitelistedDoorsForZone);
				float range = ((roomIdentifier.Zone == global::MapGeneration.FacilityZone.Surface) ? 45f : 11f);
				return GetSafePositionForDoor(randomDoor, range, role.FpcModule.CharController);
			}
			return position;
		}

		private static global::UnityEngine.Vector3 GetSafePositionForDoor(global::Interactables.Interobjects.DoorUtils.DoorVariant dv, float range, global::UnityEngine.CharacterController ctrl)
		{
			global::UnityEngine.Vector3 position = dv.transform.position;
			global::UnityEngine.Vector3 vector = dv.transform.forward;
			if (global::UnityEngine.Random.value > 0.5f)
			{
				vector = -vector;
			}
			vector = global::UnityEngine.Quaternion.Euler(0f, global::UnityEngine.Random.Range(-30f, 30f), 0f) * vector;
			float radius = ctrl.radius;
			float num = global::UnityEngine.Mathf.Lerp(radius, range, global::UnityEngine.Random.value);
			global::UnityEngine.Vector3 vector2 = global::UnityEngine.Vector3.up * ctrl.height / 2f;
			global::UnityEngine.Vector3 vector3 = position + ctrl.center + GroundOffset + global::UnityEngine.Vector3.up * radius;
			if (!global::UnityEngine.Physics.SphereCast(vector3, radius, vector, out var hitInfo, num + radius, global::PlayerRoles.FirstPersonControl.FpcStateProcessor.Mask))
			{
				return vector3 + vector * num + vector2;
			}
			return hitInfo.point + hitInfo.normal * radius + vector2;
		}

		private static global::Interactables.Interobjects.DoorUtils.DoorVariant GetRandomDoor(global::Interactables.Interobjects.DoorUtils.DoorVariant[] doors)
		{
			int num = 0;
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (allHub.roleManager.CurrentRole is global::PlayerRoles.PlayableScps.FpcStandardScp fpcStandardScp)
				{
					PositionsCache[num] = fpcStandardScp.FpcModule.Position;
					PositionModifiers[num] = fpcStandardScp.RoleTypeId == global::PlayerRoles.RoleTypeId.Scp0492;
					if (++num >= 64)
					{
						break;
					}
				}
			}
			if (num == 0)
			{
				return doors.RandomItem();
			}
			global::Interactables.Interobjects.DoorUtils.DoorVariant result = null;
			float num2 = float.MaxValue;
			int num3 = 0;
			foreach (global::Interactables.Interobjects.DoorUtils.DoorVariant doorVariant in doors)
			{
				float num4 = 0f;
				bool flag = true;
				for (int j = 0; j < num; j++)
				{
					float num5 = (doorVariant.transform.position - PositionsCache[j]).sqrMagnitude;
					if (PositionModifiers[j])
					{
						num5 *= 0.3f;
					}
					if (num5 < 750f)
					{
						flag = false;
					}
					num4 += num5;
				}
				if (flag)
				{
					DoorsNonAlloc[num3++] = doorVariant;
				}
				if (num4 < num2)
				{
					result = doorVariant;
					num2 = num4;
				}
			}
			if (num3 != 0)
			{
				return DoorsNonAlloc[global::UnityEngine.Random.Range(0, num3)];
			}
			return result;
		}

		private static global::Interactables.Interobjects.DoorUtils.DoorVariant[] GetWhitelistedDoorsForZone(global::MapGeneration.FacilityZone zone)
		{
			if (WhitelistedDoorsForZone.TryGetValue(zone, out var value))
			{
				return value;
			}
			int num = 0;
			foreach (global::Interactables.Interobjects.DoorUtils.DoorVariant allDoor in global::Interactables.Interobjects.DoorUtils.DoorVariant.AllDoors)
			{
				if (allDoor.Rooms.Length != 0 && allDoor.Rooms[0].Zone == zone && ValidateDoor(allDoor))
				{
					DoorsNonAlloc[num] = allDoor;
					if (++num >= 64)
					{
						break;
					}
				}
			}
			global::Interactables.Interobjects.DoorUtils.DoorVariant[] array = new global::Interactables.Interobjects.DoorUtils.DoorVariant[num];
			global::System.Array.Copy(DoorsNonAlloc, array, num);
			WhitelistedDoorsForZone[zone] = array;
			return array;
		}

		private static bool ValidateDoor(global::Interactables.Interobjects.DoorUtils.DoorVariant dv)
		{
			if (!(dv is global::Interactables.Interobjects.BasicDoor basicDoor))
			{
				return false;
			}
			if (dv.RequiredPermissions.RequiredPermissions != global::Interactables.Interobjects.DoorUtils.KeycardPermissions.None)
			{
				return false;
			}
			if (!global::Interactables.InteractableCollider.AllInstances.TryGetValue(basicDoor, out var value))
			{
				return false;
			}
			if (value.Count < 2)
			{
				return false;
			}
			global::MapGeneration.RoomIdentifier[] rooms = basicDoor.Rooms;
			foreach (global::MapGeneration.RoomIdentifier roomIdentifier in rooms)
			{
				if (BlacklistedRooms.Contains(roomIdentifier.Name))
				{
					return false;
				}
			}
			return true;
		}

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			global::MapGeneration.SeedSynchronizer.OnMapGenerated += WhitelistedDoorsForZone.Clear;
		}
	}
}
