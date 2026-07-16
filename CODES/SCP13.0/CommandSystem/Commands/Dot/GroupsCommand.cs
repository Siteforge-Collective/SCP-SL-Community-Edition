using System;
using System.Collections.Generic;
using System.Linq;
using RemoteAdmin;

namespace CommandSystem.Commands.Dot
{
	[CommandHandler(typeof(ClientCommandHandler))]
	public class GroupsCommand : ICommand
	{
		public string Command { get; } = "groups";

		public string[] Aliases { get; }

		public string Description { get; } = "Displays all the server's groups.";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (sender is PlayerCommandSender playerCommandSender && !playerCommandSender.ReferenceHub.serverRoles.RemoteAdmin && !playerCommandSender.ReferenceHub.authManager.BypassBansFlagSet && !playerCommandSender.ReferenceHub.isLocalPlayer)
			{
				response = "You don't have permissions to execute this command.";
				return false;
			}
			response = "Groups defined on this server:";
			Dictionary<string, UserGroup> allGroups = ServerStatic.PermissionsHandler.GetAllGroups();
			ServerRoles.NamedColor[] namedColors = ReferenceHub.LocalHub.serverRoles.NamedColors;
			foreach (KeyValuePair<string, UserGroup> permentry in allGroups)
			{
				try
				{
					if (namedColors != null)
					{
						response = response + "\n" + permentry.Key + " (" + permentry.Value.Permissions + ") - <color=#" + namedColors.FirstOrDefault((ServerRoles.NamedColor x) => x.Name == permentry.Value.BadgeColor)?.ColorHex + ">" + permentry.Value.BadgeText + "</color> in color " + permentry.Value.BadgeColor;
					}
				}
				catch
				{
					response = response + "\n" + permentry.Key + " (" + permentry.Value.Permissions + ") - " + permentry.Value.BadgeText + " in color " + permentry.Value.BadgeColor;
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.KickingAndShortTermBanning))
				{
					response += " BN1";
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.BanningUpToDay))
				{
					response += " BN2";
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.LongTermBanning))
				{
					response += " BN3";
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.ForceclassSelf))
				{
					response += " FSE";
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.ForceclassToSpectator))
				{
					response += " FSP";
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.ForceclassWithoutRestrictions))
				{
					response += " FWR";
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.GivingItems))
				{
					response += " GIV";
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.WarheadEvents))
				{
					response += " EWA";
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.RespawnEvents))
				{
					response += " ERE";
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.RoundEvents))
				{
					response += " ERO";
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.SetGroup))
				{
					response += " SGR";
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.GameplayData))
				{
					response += " GMD";
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.Overwatch))
				{
					response += " OVR";
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.FacilityManagement))
				{
					response += " FCM";
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.PlayersManagement))
				{
					response += " PLM";
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.PermissionsManagement))
				{
					response += " PRM";
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.ServerConsoleCommands))
				{
					response += " SCC";
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.ViewHiddenBadges))
				{
					response += " VHB";
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.ServerConfigs))
				{
					response += " CFG";
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.Broadcasting))
				{
					response += " BRC";
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.PlayerSensitiveDataAccess))
				{
					response += " CDA";
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.Noclip))
				{
					response += " NCP";
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.AFKImmunity))
				{
					response += " AFK";
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.AdminChat))
				{
					response += " ATC";
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.ViewHiddenGlobalBadges))
				{
					response += " GHB";
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.Announcer))
				{
					response += " ANN";
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.Effects))
				{
					response += " EFF";
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.FriendlyFireDetectorImmunity))
				{
					response += " FFI";
				}
				if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.FriendlyFireDetectorTempDisable))
				{
					response += " FFT";
				}
			}
			return true;
		}
	}
}
