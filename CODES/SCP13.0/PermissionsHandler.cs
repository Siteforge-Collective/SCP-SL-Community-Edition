using System.Collections.Generic;

public class PermissionsHandler
{
	internal readonly string OverridePassword;

	private readonly string _overrideRole;

	private readonly Dictionary<string, UserGroup> _groups;

	private readonly Dictionary<string, string> _members;

	private readonly Dictionary<string, ulong> _permissions;

	private readonly HashSet<ulong> _raPermissions;

	private readonly YamlConfig _config;

	private readonly YamlConfig _sharedGroups;

	private readonly YamlConfig _sharedGroupsMembers;

	private ulong _lastPerm;

	private readonly bool _managerAccess;

	private readonly bool _banTeamAccess;

	private readonly bool _banTeamSlots;

	private readonly bool _banTeamGeoBypass;

	public static readonly Dictionary<PlayerPermissions, string> PermissionCodes;

	public bool BanTeamSlots => false;

	public bool BanTeamBypassGeo => false;

	public UserGroup OverrideGroup => null;

	public bool OverrideEnabled => false;

	public ulong FullPerm { get; private set; }

	public bool StaffAccess { get; }

	public bool ManagersAccess => false;

	public bool BanningTeamAccess => false;

	public bool NorthwoodAccess { get; }

	public PermissionsHandler(ref YamlConfig configuration, ref YamlConfig sharedGroups, ref YamlConfig sharedGroupsMembers)
	{
	}

	public ulong RegisterPermission(string name, bool remoteAdmin, bool refresh = true)
	{
		return 0uL;
	}

	public void RefreshPermissions()
	{
	}

	public bool IsRaPermitted(ulong permissions)
	{
		return false;
	}

	public UserGroup GetGroup(string name)
	{
		return null;
	}

	public List<string> GetAllGroupsNames()
	{
		return null;
	}

	public Dictionary<string, UserGroup> GetAllGroups()
	{
		return null;
	}

	public string GetPermissionName(ulong value)
	{
		return null;
	}

	public ulong GetPermissionValue(string name)
	{
		return 0uL;
	}

	public List<string> GetAllPermissions()
	{
		return null;
	}

	public static bool IsPermitted(ulong permissions, PlayerPermissions check)
	{
		return false;
	}

	public static bool IsPermitted(ulong permissions, PlayerPermissions[] check)
	{
		return false;
	}

	public bool IsPermitted(ulong permissions, string check)
	{
		return false;
	}

	public bool IsPermitted(ulong permissions, string[] check)
	{
		return false;
	}

	public static bool IsPermitted(ulong permissions, ulong check)
	{
		return false;
	}

	public UserGroup GetUserGroup(string userId)
	{
		return null;
	}
}
