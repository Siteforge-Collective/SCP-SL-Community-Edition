using System;
using System.Text;
using GameCore;
using Mirror;
using UnityEngine;

namespace CommandSystem.Commands.Console
{
	[CommandHandler(typeof(GameConsoleCommandHandler))]
	public class BanCommand : ICommand
	{
		public string Command { get; } = "ban";

		public string[] Aliases { get; }

		public string Description { get; } = "Ban specified player from server.";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!ReferenceHub.TryGetHostHub(out var hub) || !hub.isLocalPlayer)
			{
				response = "You are not connected to a local server.";
				return false;
			}
			if (arguments.Count < 2)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Syntax: BAN [player name / ip] [minutes or time]");
				foreach (NetworkConnectionToClient value in NetworkServer.connections.Values)
				{
					string text = string.Empty;
					GameObject gameObject = GameCore.Console.FindConnectedRoot(value);
					if (gameObject != null)
					{
						text = gameObject.GetComponent<NicknameSync>().MyNick;
					}
					if (text == string.Empty)
					{
						stringBuilder.AppendLine("Player :: " + value.address);
					}
					else
					{
						stringBuilder.AppendLine("Player :: " + text + " :: " + value.address);
					}
				}
				response = stringBuilder.ToString();
				return true;
			}
			bool flag = false;
			long duration;
			try
			{
				duration = Misc.RelativeTimeToSeconds(arguments.At(1), 60);
			}
			catch
			{
				response = "Invalid time: " + arguments.At(1);
				return false;
			}
			foreach (NetworkConnectionToClient value2 in NetworkServer.connections.Values)
			{
				GameObject gameObject2 = GameCore.Console.FindConnectedRoot(value2);
				if (value2.address.Contains(arguments.At(0), StringComparison.OrdinalIgnoreCase) || (!(gameObject2 == null) && gameObject2.GetComponent<NicknameSync>().MyNick.Contains(arguments.At(0), StringComparison.OrdinalIgnoreCase)))
				{
					flag = true;
					BanPlayer.BanUser(ReferenceHub.GetHub(gameObject2), sender, string.Empty, duration);
				}
			}
			response = (flag ? "Player banned." : "Player not found.");
			return flag;
		}
	}
}
