using PlayerStatsSystem;

namespace PlayerRoles.Spectating
{
    public static class SpectatableExtensions
    {
        public static T GetModule<T>(this SpectatableModuleBase module) where T : StatBase
        {
            if (module.MainRole.TryGetOwner(out ReferenceHub hub))
            {
                return hub.playerStats.GetModule<T>();
            }
            return null;
        }
    }
}