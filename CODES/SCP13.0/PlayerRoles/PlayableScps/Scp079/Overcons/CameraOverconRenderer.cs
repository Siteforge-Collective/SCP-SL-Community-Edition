using System.Collections.Generic;
using PlayerRoles.PlayableScps.Scp079.Cameras;

namespace PlayerRoles.PlayableScps.Scp079.Overcons
{
	public class CameraOverconRenderer : PooledOverconRenderer
	{
		private const float MaxSqrDistanceOtherRoom = 2165f;

		private const float ElevatorIconHeight = 3f;

		private const float ElevatorHeightMaxDiff = 50f;

		public static HashSet<CameraOvercon> VisibleOvercons;

		internal override void SpawnOvercons(Scp079Camera newCamera)
		{
		}

		private bool CheckVisibility(Scp079Camera cur, Scp079InteractableBase target)
		{
			return false;
		}
	}
}
