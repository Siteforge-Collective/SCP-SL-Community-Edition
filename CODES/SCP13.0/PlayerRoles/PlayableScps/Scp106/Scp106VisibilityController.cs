using PlayerRoles.FirstPersonControl;
using PlayerRoles.Visibility;

namespace PlayerRoles.PlayableScps.Scp106
{
	public class Scp106VisibilityController : FpcVisibilityController
	{
		private Scp106Role _role106;

		private Scp106StalkVisibilityController _visSubroutine;

		private bool CheckPlayer(ReferenceHub observer)
		{
			return false;
		}

		public override InvisibilityFlags GetActiveFlags(ReferenceHub observer)
		{
			return default(InvisibilityFlags);
		}

		public override void SpawnObject()
		{
		}
	}
}
