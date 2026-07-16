namespace PlayerRoles.PlayableScps.Scp079
{
	public abstract class Scp079AbilityBase : global::PlayerRoles.PlayableScps.Subroutines.ScpStandardSubroutine<global::PlayerRoles.PlayableScps.Scp079.Scp079Role>
	{
		protected global::PlayerRoles.PlayableScps.Scp079.Scp079TierManager TierManager { get; private set; }

		protected global::PlayerRoles.PlayableScps.Scp079.Scp079AuxManager AuxManager { get; private set; }

		protected global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync CurrentCamSync { get; private set; }

		protected global::PlayerRoles.PlayableScps.Scp079.Scp079LostSignalHandler LostSignalHandler { get; private set; }

		protected global::PlayerRoles.PlayableScps.Scp079.Rewards.Scp079RewardManager RewardManager { get; private set; }

		public virtual float AuxRegenMultiplier => 1f;

		protected override void Awake()
		{
			base.Awake();
			global::PlayerRoles.PlayableScps.Subroutines.ScpSubroutineBase[] allSubroutines = base.ScpRole.SubroutineModule.AllSubroutines;
			foreach (global::PlayerRoles.PlayableScps.Subroutines.ScpSubroutineBase scpSubroutineBase in allSubroutines)
			{
				if (scpSubroutineBase is global::PlayerRoles.PlayableScps.Scp079.Scp079TierManager tierManager)
				{
					TierManager = tierManager;
				}
				else if (scpSubroutineBase is global::PlayerRoles.PlayableScps.Scp079.Scp079AuxManager auxManager)
				{
					AuxManager = auxManager;
				}
				else if (scpSubroutineBase is global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync currentCamSync)
				{
					CurrentCamSync = currentCamSync;
				}
				else if (scpSubroutineBase is global::PlayerRoles.PlayableScps.Scp079.Scp079LostSignalHandler lostSignalHandler)
				{
					LostSignalHandler = lostSignalHandler;
				}
				else if (scpSubroutineBase is global::PlayerRoles.PlayableScps.Scp079.Rewards.Scp079RewardManager rewardManager)
				{
					RewardManager = rewardManager;
				}
			}
		}
	}
}
