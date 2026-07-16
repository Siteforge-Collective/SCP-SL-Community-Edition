namespace PlayerRoles
{
	public interface IHealthbarRole
	{
		float MaxHealth { get; }

		global::PlayerStatsSystem.PlayerStats TargetStats { get; }
	}
}
