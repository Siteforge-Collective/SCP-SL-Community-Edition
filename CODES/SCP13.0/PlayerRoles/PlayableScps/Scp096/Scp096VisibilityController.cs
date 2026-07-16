using PlayerRoles.FirstPersonControl;
using PlayerRoles.Visibility;

namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096VisibilityController : FpcVisibilityController
	{
		private const float RageRangeBuffer = 10f;

		private Scp096Role _role;

		private Scp096TargetsTracker _targetsTracker;

		public override InvisibilityFlags IgnoredFlags => default(InvisibilityFlags);

		protected override int NormalMaxRangeSqr => 0;

		protected override int SurfaceMaxRangeSqr => 0;

		private bool HideNonTargets => false;

		private int EnsureVisiblityForState(int defaultRange)
		{
			return 0;
		}

		public override InvisibilityFlags GetActiveFlags(ReferenceHub observer)
		{
			return default(InvisibilityFlags);
		}

		public override bool ValidateVisibility(ReferenceHub target)
		{
			return false;
		}

		public override void SpawnObject()
		{
		}
	}
}
