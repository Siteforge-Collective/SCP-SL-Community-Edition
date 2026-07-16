using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;
using MapGeneration;
using PlayerRoles.FirstPersonControl;
using UnityEngine;

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

		private static readonly RoomName[] BlacklistedRooms;

		private static readonly Dictionary<FacilityZone, DoorVariant[]> WhitelistedDoorsForZone;

		private static readonly DoorVariant[] DoorsNonAlloc;

		private static readonly Vector3[] PositionsCache;

		private static readonly bool[] PositionModifiers;

		private static readonly int Mask;

		private static readonly Vector3 GroundOffset;

		public static Vector3 GetBestExitPosition(IFpcRole role)
		{
			return default(Vector3);
		}

		private static Vector3 GetSafePositionForDoor(DoorVariant dv, float range, CharacterController ctrl)
		{
			return default(Vector3);
		}

		private static DoorVariant GetRandomDoor(DoorVariant[] doors)
		{
			return null;
		}

		private static DoorVariant[] GetWhitelistedDoorsForZone(FacilityZone zone)
		{
			return null;
		}

		private static bool ValidateDoor(DoorVariant dv)
		{
			return false;
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}
	}
}
