using GameCore;
using InventorySystem.Items.Keycards;
using InventorySystem.Items.MicroHID;
using Mirror;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp096;
using PlayerStatsSystem;

namespace Achievements.Handlers
{
    public class GeneralKillsHandler : AchievementHandlerBase
    {
        private const int AnomalouslyEfficientTime = 60;

        internal override void OnInitialize()
        {
            PlayerStats.OnAnyPlayerDied += HandleDeath;
        }

        private static void HandleDeath(ReferenceHub deadPlayer, DamageHandlerBase handler)
        {
            if (!NetworkServer.active)
            {
                return;
            }
            AttackerDamageHandler attackerDamageHandler;
            if (!(handler is ExplosionDamageHandler explosionDamageHandler))
            {
                if (!(handler is ScpDamageHandler scpDamageHandler))
                {
                    if (!(handler is MicroHidDamageHandler microHidDamageHandler))
                    {
                        attackerDamageHandler = handler as AttackerDamageHandler;
                        if (attackerDamageHandler == null)
                        {
                            if (handler is UniversalDamageHandler universalDamageHandler && universalDamageHandler.TranslationId == DeathTranslations.Tesla.Id && deadPlayer.inventory.CurInstance is MicroHIDItem)
                            {
                                Send(deadPlayer, AchievementName.Overcurrent);
                            }
                            return;
                        }
                        goto IL_00f9;
                    }
                    if (deadPlayer.IsSCP())
                    {
                        Send(microHidDamageHandler.Attacker.Hub, AchievementName.MicrowaveMeal);
                    }
                    return;
                }
                if (scpDamageHandler.Attacker.Hub != null)
                {
                    if (RoundStart.RoundStartTimer.Elapsed.TotalSeconds < 60.0)
                    {
                        Send(scpDamageHandler.Attacker.Hub, AchievementName.AnomalouslyEfficient);
                    }
                    return;
                }
            }
            else if (explosionDamageHandler.Attacker.Hub != null)
            {
                if (explosionDamageHandler.Attacker.Hub != deadPlayer)
                {
                    Send(explosionDamageHandler.Attacker.Hub, AchievementName.FireInTheHole);
                }
                return;
            }
            attackerDamageHandler = (AttackerDamageHandler)handler;
            goto IL_00f9;
        IL_00f9:
            if (attackerDamageHandler.Attacker.Hub != null)
            {
                HandleAttackerKill(deadPlayer, attackerDamageHandler);
            }
        }

        private static void HandleAttackerKill(ReferenceHub deadPlayer, AttackerDamageHandler faDH)
        {
            ReferenceHub hub = faDH.Attacker.Hub;
            RoleTypeId roleId = deadPlayer.GetRoleId();
            switch (faDH.Attacker.Role.GetTeam())
            {
                case Team.Scientists:
                    if (roleId == RoleTypeId.ClassD)
                    {
                        Send(hub, AchievementName.JustResources);
                    }
                    else if (deadPlayer.IsSCP())
                    {
                        Send(hub, AchievementName.SomethingDoneRight);
                    }
                    break;
                case Team.ClassD:
                    if (roleId == RoleTypeId.Scientist && deadPlayer.inventory.CurInstance is KeycardItem)
                    {
                        Send(faDH.Attacker.Hub, AchievementName.AccessGranted);
                    }
                    break;
            }
            if (deadPlayer.GetRoleId() == RoleTypeId.Scp096 && (deadPlayer.roleManager.CurrentRole as Scp096Role).IsRageState(Scp096RageState.Distressed))
            {
                Send(faDH.Attacker.Hub, AchievementName.Pacified);
            }
        }

        private static void Send(ReferenceHub hub, AchievementName name)
        {
            if (hub != null)
            {
                AchievementHandlerBase.ServerAchieve(hub.networkIdentity.connectionToClient, name);
            }
        }
    }
}
