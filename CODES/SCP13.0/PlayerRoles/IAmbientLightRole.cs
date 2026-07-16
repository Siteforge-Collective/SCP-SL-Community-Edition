namespace PlayerRoles
{
	public interface IAmbientLightRole
	{
		float AmbientLight { get; }

		bool InsufficientLight { get; }
	}
}
