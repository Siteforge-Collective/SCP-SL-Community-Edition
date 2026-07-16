using System.Collections.Generic;
using GameObjectPools;
using MapGeneration;
using PlayerRoles.Subroutines;

namespace PlayerRoles.PlayableScps.Scp079.Rewards
{
	public class Scp079RewardManager : SubroutineBase, IPoolResettable
	{
		private const float MarkDuration = 12f;

		private readonly Dictionary<RoomIdentifier, double> _markedRooms;

		private static double CurTime => 0.0;

		public void MarkRoom(RoomIdentifier room)
		{
		}

		public void MarkRooms(RoomIdentifier[] rooms)
		{
		}

		public void ResetObject()
		{
		}

		public static bool CheckForRoomInteractions(ReferenceHub scp079Player, RoomIdentifier room)
		{
			return false;
		}

		public static bool CheckForRoomInteractions(RoomIdentifier room)
		{
			return false;
		}

		public static bool CheckForRoomInteractions(Scp079Role scp079, RoomIdentifier room)
		{
			return false;
		}

		public static void GrantExp(Scp079Role instance, int reward, Scp079HudTranslation gainReason, RoleTypeId subject = RoleTypeId.None)
		{
		}
	}
}
