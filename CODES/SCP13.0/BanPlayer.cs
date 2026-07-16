using CommandSystem;
using Footprinting;

public static class BanPlayer
{
	private const string _globalBanReason = "You have been Globally Banned.";

	public static void GlobalBanUser(ReferenceHub target, ICommandSender issuer)
	{
	}

	public static bool KickUser(ReferenceHub target, ICommandSender issuer, string reason)
	{
		return false;
	}

	public static bool KickUser(ReferenceHub target, ReferenceHub issuer, string reason)
	{
		return false;
	}

	public static bool KickUser(ReferenceHub target, string reason)
	{
		return false;
	}

	public static bool BanUser(ReferenceHub target, string reason, long duration)
	{
		return false;
	}

	public static bool BanUser(Footprint target, string reason, long duration)
	{
		return false;
	}

	public static bool BanUser(ReferenceHub target, ReferenceHub issuer, string reason, long duration)
	{
		return false;
	}

	public static bool BanUser(ReferenceHub target, ICommandSender issuer, string reason, long duration)
	{
		return false;
	}

	public static bool BanUser(Footprint target, ICommandSender issuer, string reason, long duration)
	{
		return false;
	}

	public static void ApplyIpBan(ReferenceHub target, ICommandSender issuer, string reason, long duration)
	{
	}

	public static void ApplyIpBan(Footprint target, ICommandSender issuer, string reason, long duration)
	{
	}

	public static string ValidateNick(string username)
	{
		return null;
	}
}
