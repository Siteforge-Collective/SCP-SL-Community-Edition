using System;
using GameCore;
using PluginAPI;
using UnityEngine;

namespace CommandSystem.Commands.Shared
{
	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	[CommandHandler(typeof(GameConsoleCommandHandler))]
	public class BuildInfoCommand : ICommand
	{
		public static string ModDescription;

		public string Command { get; } = "buildinfo";

		public string[] Aliases { get; } = new string[3] { "v", "ver", "version" };

		public string Description { get; } = "Displays information about the current build.";

		internal static string BuildInfoString => string.Format("Build info:\nGame version: {0}\nPreauth version: {1}.{2}.{3}\nBackward compatibility: {4}\nBuild timestamp: {5}\nBuild type: {6}\nAlways accept release builds: {7}\nBuild GUID: {8}\nUnity version: {9}\n\nPrivate beta: {10}\nPublic beta: {11}\nRelease candidate: {12}\nStreaming allowed: {13}\nHeadless: {14}\nModded: {15}", GameCore.Version.VersionString, GameCore.Version.Major, GameCore.Version.Minor, GameCore.Version.Revision, (!GameCore.Version.BackwardCompatibility || GameCore.Version.ExtendedVersionCheckNeeded) ? "False" : $"{GameCore.Version.Major}.{GameCore.Version.Minor}.{GameCore.Version.BackwardRevision} and newer", "2024-07-20 16:26:48Z", GameCore.Version.BuildType, GameCore.Version.AlwaysAcceptReleaseBuilds, PlatformInfo.singleton.BuildGuid, Application.unityVersion, GameCore.Version.PrivateBeta, GameCore.Version.PublicBeta, GameCore.Version.ReleaseCandidate, GameCore.Version.StreamingAllowed, PlatformInfo.singleton.IsHeadless, CustomNetworkManager.Modded ? $"{true}\nMod Description:\n{ModDescription}" : false.ToString()) + "\nNwPluginAPI version: " + PluginApiVersion.VersionStatic + "\nBuilt with NwPluginAPI version: 13.1.3";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			response = BuildInfoString;
			return true;
		}
	}
}
