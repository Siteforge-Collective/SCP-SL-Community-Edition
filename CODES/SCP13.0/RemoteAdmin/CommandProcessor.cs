using System;
using System.Collections.Generic;
using System.Linq;
using CentralAuth;
using CommandSystem;
using PlayerStatsSystem;
using PluginAPI.Events;
using RemoteAdmin.Communication;
using UnityEngine;

namespace RemoteAdmin
{
    public static class CommandProcessor
    {
        private const int MaxStaffChatMessageLength = 2000;

        public static readonly RemoteAdminCommandHandler RemoteAdminCommandHandler = RemoteAdminCommandHandler.Create();

        internal static void ProcessAdminChat(string q, CommandSender sender)
        {
            if (!CheckPermissions(sender, "Admin Chat", PlayerPermissions.AdminChat, string.Empty))
            {
                if (sender is PlayerCommandSender playerCommandSender)
                {
                    playerCommandSender.ReferenceHub.gameConsoleTransmission.SendToClient("You don't have permissions to access Admin Chat!", "red");
                    playerCommandSender.RaReply("You don't have permissions to access Admin Chat!", success: false, logToConsole: true, "");
                }
                return;
            }
            uint num = 0u;
            if (sender is PlayerCommandSender playerCommandSender2)
            {
                num = playerCommandSender2.ReferenceHub.netId;
            }
            q = Misc.SanitizeRichText(q.Replace("~", "-"), "[", "]");
            if (string.IsNullOrWhiteSpace(q.Replace("@", string.Empty)))
            {
                return;
            }
            if (q.Length > 2000)
            {
                q = q[..2000] + "...";
            }
            string content = num + "!" + q;
            if (ServerStatic.IsDedicated)
            {
                ServerConsole.AddLog("[AC " + sender.LogName + "] " + q, ConsoleColor.DarkYellow);
            }
            ServerLogs.AddLog(ServerLogs.Modules.Administrative, "[" + sender.LogName + "] " + q, ServerLogs.ServerLogType.AdminChat);
            foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
            {
                ClientInstanceMode mode = allHub.Mode;
                if (mode != ClientInstanceMode.Unverified && mode != ClientInstanceMode.DedicatedServer && allHub.serverRoles.AdminChatPerms)
                {
                    allHub.encryptedChannelManager.TrySendMessageToClient(content, EncryptedChannelManager.EncryptedChannel.AdminChat);
                }
            }
        }

        internal static string ProcessQuery(string q, CommandSender sender)
        {
            if (q.StartsWith("$", StringComparison.Ordinal))
            {
                string[] array = q.Remove(0, 1).Split(' ');
                if (array.Length <= 1)
                {
                    return null;
                }
                if (!int.TryParse(array[0], out var result))
                {
                    return null;
                }
                if (CommunicationProcessor.ServerCommunication.TryGetValue(result, out var value))
                {
                    value.ReceiveData(sender, string.Join(" ", array.Skip(1)));
                }
                return null;
            }
            string[] array2 = q.Trim().Split(QueryProcessor.SpaceArray, 512, StringSplitOptions.RemoveEmptyEntries);
            if (!EventManager.ExecuteEvent(new RemoteAdminCommandEvent(sender, array2[0], array2.Skip(1).ToArray())))
            {
                return null;
            }
            if (RemoteAdminCommandHandler.TryGetCommand(array2[0], out var command))
            {
                try
                {
                    string response;
                    bool flag = command.Execute(array2.Segment(1), sender, out response);
                    response = Misc.CloseAllRichTextTags(response);
                    if (!EventManager.ExecuteEvent(new RemoteAdminCommandExecutedEvent(sender, array2[0], array2.Skip(1).ToArray(), flag, response)))
                    {
                        return null;
                    }
                    if (!string.IsNullOrEmpty(response))
                    {
                        sender.RaReply(array2[0].ToUpperInvariant() + "#" + response, flag, logToConsole: true, "");
                    }
                    return response;
                }
                catch (Exception ex)
                {
                    string text = "Command execution failed! Error: " + Misc.RemoveStacktraceZeroes(ex.ToString());
                    if (!EventManager.ExecuteEvent(new RemoteAdminCommandExecutedEvent(sender, array2[0], array2.Skip(1).ToArray(), result: false, text)))
                    {
                        return null;
                    }
                    sender.RaReply(text, success: false, logToConsole: true, array2[0].ToUpperInvariant() + "#" + text);
                    return text;
                }
            }
            if (!EventManager.ExecuteEvent(new RemoteAdminCommandExecutedEvent(sender, array2[0], array2.Skip(1).ToArray(), result: false, "Unknown command!")))
            {
                return null;
            }
            sender.RaReply("SYSTEM#Unknown command!", success: false, logToConsole: true, string.Empty);
            return "Unknown command!";
        }

        internal static float GetRoundedStat<T>(ReferenceHub hub) where T : StatBase
        {
            return Mathf.Round(hub.playerStats.GetModule<T>().CurValue * 100f) / 100f;
        }

        internal static List<ICommand> GetAllCommands()
        {
            return RemoteAdminCommandHandler.AllCommands.ToList();
        }

        private static bool CheckPermissions(CommandSender sender, string queryZero, PlayerPermissions perm, string replyScreen = "", bool reply = true)
        {
            if (ServerStatic.IsDedicated && sender.FullPermissions)
            {
                return true;
            }
            if (PermissionsHandler.IsPermitted(sender.Permissions, perm))
            {
                return true;
            }
            if (reply)
            {
                sender.RaReply(queryZero + "#You don't have permissions to execute this command.\nRequired permission: " + perm, success: false, logToConsole: true, replyScreen);
            }
            return false;
        }

        internal static bool CheckPermissions(CommandSender sender, PlayerPermissions perm)
        {
            if (ServerStatic.IsDedicated && sender.FullPermissions)
            {
                return true;
            }
            if (PermissionsHandler.IsPermitted(sender.Permissions, perm))
            {
                return true;
            }
            return false;
        }
    }
}
