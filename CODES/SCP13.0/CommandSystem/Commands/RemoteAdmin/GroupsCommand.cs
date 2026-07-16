using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NorthwoodLib.Pools;

namespace CommandSystem.Commands.RemoteAdmin
{
	[CommandHandler(typeof(PermissionsManagementCommand))]
	public class GroupsCommand : ICommand
	{
		public string Command { get; } = "groups";

		public string[] Aliases { get; }

		public string Description { get; } = "Lists all defined permission groups.";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!sender.CheckPermission(PlayerPermissions.PermissionsManagement, out response))
			{
				return false;
			}
			StringBuilder stringBuilder = StringBuilderPool.Shared.Rent("Groups defined on this server:");
			Dictionary<string, UserGroup> allGroups = ServerStatic.PermissionsHandler.GetAllGroups();
			ServerRoles.NamedColor[] namedColors = ReferenceHub.LocalHub.serverRoles.NamedColors;
			foreach (KeyValuePair<string, UserGroup> permentry in allGroups)
			{
				try
				{
					stringBuilder.AppendFormat("\n{0} ({1}) - <color=#{2}>{3}</color> in color {4}", permentry.Key, permentry.Value.Permissions, namedColors.FirstOrDefault((ServerRoles.NamedColor x) => x.Name == permentry.Value.BadgeColor)?.ColorHex, permentry.Value.BadgeText, permentry.Value.BadgeColor);
				}
				catch
				{
					stringBuilder.AppendFormat("\n{0} ({1}) - {2} in color {3}", permentry.Key, permentry.Value.Permissions, permentry.Value.BadgeText, permentry.Value.BadgeColor);
				}
				foreach (KeyValuePair<PlayerPermissions, string> permissionCode in PermissionsHandler.PermissionCodes)
				{
					if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, permissionCode.Key))
					{
						stringBuilder.Append(" " + permissionCode.Value);
					}
				}
			}
			response = stringBuilder.ToString();
			StringBuilderPool.Shared.Return(stringBuilder);
			return true;
		}
	}
}
