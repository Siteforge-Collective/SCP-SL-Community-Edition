using PlayerRoles.Subroutines;

namespace PlayerRoles.PlayableScps.Scp106
{
	public abstract class Scp106VigorAbilityBase : KeySubroutine<Scp106Role>
	{
		private VigorStat _vigor;

		private bool _vigorSet;

		public virtual bool IsSubmerged => false;

		public virtual bool ForceHumanAnimations => false;

		protected float VigorAmount
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		private VigorStat Vigor => null;

		public override void ResetObject()
		{
		}
	}
}
