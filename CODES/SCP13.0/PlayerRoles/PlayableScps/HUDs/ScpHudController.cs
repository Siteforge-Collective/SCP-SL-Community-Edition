using UnityEngine;

namespace PlayerRoles.PlayableScps.HUDs
{
	public static class ScpHudController
	{
		public static ScpHudBase CurInstance { get; private set; }

		[RuntimeInitializeOnLoadMethod]
		private static void InitOnLoad()
		{
		}

		private static bool ValidatePlayer(ReferenceHub hub)
		{
			return false;
		}

		private static void RoleChanged(ReferenceHub hub, PlayerRoleBase prev, PlayerRoleBase cur)
		{
		}

		private static void TargetChanged()
		{
		}

		private static void DestroyOld()
		{
		}

		private static void SpawnNew(IHudScp hudScp, ReferenceHub owner)
		{
		}
	}
}
