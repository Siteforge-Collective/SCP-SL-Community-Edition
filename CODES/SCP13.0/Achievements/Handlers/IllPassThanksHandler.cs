using InventorySystem.Items.MicroHID;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp106;
using PlayerStatsSystem;

namespace Achievements.Handlers
{
    public class IllPassThanksHandler : AchievementHandlerBase
    {
        internal override void OnInitialize()
        {
            PlayerStats.OnAnyPlayerDied += HandleDeath;
            Scp106Attack.OnPlayerTeleported += PlayerTeleported;
        }

        private void PlayerTeleported(ReferenceHub scp106, ReferenceHub hub)
        {
            if (hub.inventory.CurInstance is MicroHIDItem microHIDItem && (microHIDItem.State == HidState.PoweringUp || microHIDItem.State == HidState.Firing))
            {
                AchievementHandlerBase.ServerAchieve(scp106.connectionToClient, AchievementName.IllPassThanks);
            }
        }

        private void HandleDeath(ReferenceHub deadPlayer, DamageHandlerBase handler)
        {
            if (handler is AttackerDamageHandler attackerDamageHandler && !(attackerDamageHandler.Attacker.Hub == null))
            {
                ReferenceHub hub = attackerDamageHandler.Attacker.Hub;
                if (hub.IsSCP() && deadPlayer.inventory.CurInstance is MicroHIDItem microHIDItem && (microHIDItem.State == HidState.PoweringUp || microHIDItem.State == HidState.Firing))
                {
                    AchievementHandlerBase.ServerAchieve(hub.connectionToClient, AchievementName.IllPassThanks);
                }
            }
        }
    }
}
