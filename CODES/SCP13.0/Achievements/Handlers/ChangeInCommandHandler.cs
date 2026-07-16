using InventorySystem.Disarming;
using PlayerRoles;

namespace Achievements.Handlers
{
    public class ChangeInCommandHandler : AchievementHandlerBase
    {
        internal override void OnInitialize()
        {
            DisarmingHandlers.OnPlayerDisarmed += OnPlayerDisarmed;
        }

        private void OnPlayerDisarmed(ReferenceHub disarmerHub, ReferenceHub targetHub)
        {
            if (targetHub.GetTeam() == Team.FoundationForces && disarmerHub.GetRoleId() == RoleTypeId.ClassD)
            {
                AchievementHandlerBase.ServerAchieve(disarmerHub.networkIdentity.connectionToClient, AchievementName.ChangeInCommand);
            }
        }
    }
}
