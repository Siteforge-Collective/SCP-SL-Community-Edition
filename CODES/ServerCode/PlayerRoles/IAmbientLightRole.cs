namespace PlayerRoles
{
	public interface IAmbientLightRole
	{
		global::UnityEngine.Color AmbientLight { get; }

		bool InsufficientLight { get; }
	}
}
