public static class NewInput
{
	public class ActionDefinition
	{
		public readonly ActionName Name;

		public readonly ActionCategory Category;

		public readonly global::UnityEngine.KeyCode DefaultKey;

		public readonly ActionName[] CompatibleActions;

		public ActionDefinition(ActionName actionName, global::UnityEngine.KeyCode k, ActionCategory c, params ActionName[] compatibleActions)
		{
			Name = actionName;
			Category = c;
			DefaultKey = k;
			CompatibleActions = compatibleActions;
		}
	}

	public static readonly global::System.Collections.Generic.Dictionary<ActionName, global::UnityEngine.KeyCode> UserKeybinds = new global::System.Collections.Generic.Dictionary<ActionName, global::UnityEngine.KeyCode>();

	private static readonly string SaveFilePath = global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder.ApplicationData) + "/SCP Secret Laboratory/keybinding.txt";

	private static readonly NewInput.ActionDefinition[] DefinedActions = new NewInput.ActionDefinition[38]
	{
		new NewInput.ActionDefinition(ActionName.Shoot, global::UnityEngine.KeyCode.Mouse0, ActionCategory.Weapons),
		new NewInput.ActionDefinition(ActionName.Zoom, global::UnityEngine.KeyCode.Mouse1, ActionCategory.Weapons),
		new NewInput.ActionDefinition(ActionName.Jump, global::UnityEngine.KeyCode.Space, ActionCategory.Movement),
		new NewInput.ActionDefinition(ActionName.Interact, global::UnityEngine.KeyCode.E, ActionCategory.Gameplay),
		new NewInput.ActionDefinition(ActionName.Inventory, global::UnityEngine.KeyCode.Tab, ActionCategory.Gameplay),
		new NewInput.ActionDefinition(ActionName.Reload, global::UnityEngine.KeyCode.R, ActionCategory.Weapons),
		new NewInput.ActionDefinition(ActionName.Run, global::UnityEngine.KeyCode.LeftShift, ActionCategory.Movement),
		new NewInput.ActionDefinition(ActionName.VoiceChat, global::UnityEngine.KeyCode.Q, ActionCategory.Communication),
		new NewInput.ActionDefinition(ActionName.Sneak, global::UnityEngine.KeyCode.C, ActionCategory.Movement),
		new NewInput.ActionDefinition(ActionName.MoveForward, global::UnityEngine.KeyCode.W, ActionCategory.Movement),
		new NewInput.ActionDefinition(ActionName.MoveBackward, global::UnityEngine.KeyCode.S, ActionCategory.Movement),
		new NewInput.ActionDefinition(ActionName.MoveLeft, global::UnityEngine.KeyCode.A, ActionCategory.Movement),
		new NewInput.ActionDefinition(ActionName.MoveRight, global::UnityEngine.KeyCode.D, ActionCategory.Movement),
		new NewInput.ActionDefinition(ActionName.PlayerList, global::UnityEngine.KeyCode.N, ActionCategory.Gameplay),
		new NewInput.ActionDefinition(ActionName.CharacterInfo, global::UnityEngine.KeyCode.F1, ActionCategory.Gameplay),
		new NewInput.ActionDefinition(ActionName.RemoteAdmin, global::UnityEngine.KeyCode.M, ActionCategory.System),
		new NewInput.ActionDefinition(ActionName.ToggleFlashlight, global::UnityEngine.KeyCode.F, ActionCategory.Weapons),
		new NewInput.ActionDefinition(ActionName.AltVoiceChat, global::UnityEngine.KeyCode.V, ActionCategory.Communication),
		new NewInput.ActionDefinition(ActionName.Noclip, global::UnityEngine.KeyCode.LeftAlt, ActionCategory.System),
		new NewInput.ActionDefinition(ActionName.NoClipFogToggle, global::UnityEngine.KeyCode.O, ActionCategory.System),
		new NewInput.ActionDefinition(ActionName.GameConsole, global::UnityEngine.KeyCode.BackQuote, ActionCategory.System),
		new NewInput.ActionDefinition(ActionName.HotkeyKeycard, global::UnityEngine.KeyCode.LeftControl, ActionCategory.Hotkey),
		new NewInput.ActionDefinition(ActionName.HotkeyPrimaryFirearm, global::UnityEngine.KeyCode.Alpha1, ActionCategory.Hotkey),
		new NewInput.ActionDefinition(ActionName.HotkeySecondaryFirearm, global::UnityEngine.KeyCode.Alpha2, ActionCategory.Hotkey),
		new NewInput.ActionDefinition(ActionName.InspectItem, global::UnityEngine.KeyCode.I, ActionCategory.Weapons),
		new NewInput.ActionDefinition(ActionName.RevolverCockHammer, global::UnityEngine.KeyCode.Mouse2, ActionCategory.Weapons),
		new NewInput.ActionDefinition(ActionName.ThrowItem, global::UnityEngine.KeyCode.T, ActionCategory.Gameplay),
		new NewInput.ActionDefinition(ActionName.HotkeyMedical, global::UnityEngine.KeyCode.X, ActionCategory.Hotkey),
		new NewInput.ActionDefinition(ActionName.HideGUI, global::UnityEngine.KeyCode.P, ActionCategory.System),
		new NewInput.ActionDefinition(ActionName.HotkeyGrenade, global::UnityEngine.KeyCode.G, ActionCategory.Hotkey),
		new NewInput.ActionDefinition(ActionName.PauseMenu, global::UnityEngine.KeyCode.Escape, ActionCategory.Unbindable),
		new NewInput.ActionDefinition(ActionName.DebugLogMenu, global::UnityEngine.KeyCode.F4, ActionCategory.Unbindable),
		new global::PlayerRoles.PlayableScps.Scp079.Scp079Keybinds.ActionDefinition(ActionName.Scp079FreeLook, global::UnityEngine.KeyCode.Space),
		new global::PlayerRoles.PlayableScps.Scp079.Scp079Keybinds.ActionDefinition(ActionName.Scp079LockDoor, global::UnityEngine.KeyCode.Mouse1),
		new global::PlayerRoles.PlayableScps.Scp079.Scp079Keybinds.ActionDefinition(ActionName.Scp079UnlockAll, global::UnityEngine.KeyCode.R),
		new global::PlayerRoles.PlayableScps.Scp079.Scp079Keybinds.ActionDefinition(ActionName.Scp079Blackout, global::UnityEngine.KeyCode.F),
		new global::PlayerRoles.PlayableScps.Scp079.Scp079Keybinds.ActionDefinition(ActionName.Scp079Lockdown, global::UnityEngine.KeyCode.G),
		new global::PlayerRoles.PlayableScps.Scp079.Scp079Keybinds.ActionDefinition(ActionName.Scp079PingLocation, global::UnityEngine.KeyCode.E)
	};

	private static bool _loaded;

	public static global::UnityEngine.KeyCode GetKey(ActionName actionName, global::UnityEngine.KeyCode fallbackKeycode = global::UnityEngine.KeyCode.None)
	{
		if (!_loaded)
		{
			Load();
		}
		if (!UserKeybinds.TryGetValue(actionName, out var value))
		{
			return fallbackKeycode;
		}
		return value;
	}

	public static void Save()
	{
		global::System.Text.StringBuilder stringBuilder = global::NorthwoodLib.Pools.StringBuilderPool.Shared.Rent();
		foreach (global::System.Collections.Generic.KeyValuePair<ActionName, global::UnityEngine.KeyCode> userKeybind in UserKeybinds)
		{
			stringBuilder.Append((int)userKeybind.Key);
			stringBuilder.Append(':');
			stringBuilder.Append((int)userKeybind.Value);
			stringBuilder.Append(';');
		}
		global::System.IO.File.WriteAllText(SaveFilePath, stringBuilder.ToString(0, stringBuilder.Length - 1));
		global::NorthwoodLib.Pools.StringBuilderPool.Shared.Return(stringBuilder);
	}

	public static void Load()
	{
		ResetToDefault();
		if (!global::System.IO.File.Exists(SaveFilePath))
		{
			Save();
		}
		string[] array = global::System.IO.File.ReadAllText(SaveFilePath).Split(';');
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(':');
			if (array2.Length == 2 && TryParseActionName(array2[0], out var actionName) && TryParseKeycode(array2[1], out var keyCode))
			{
				UserKeybinds[actionName] = keyCode;
			}
		}
		_loaded = true;
	}

	public static bool TryParseActionName(string s, out ActionName actionName)
	{
		if (int.TryParse(s, out var result) && global::System.Enum.IsDefined(typeof(ActionName), (ActionName)result))
		{
			actionName = (ActionName)result;
			return true;
		}
		global::UnityEngine.Debug.Log("Action name " + s + " is not defined");
		actionName = ActionName.Shoot;
		return false;
	}

	public static bool TryParseKeycode(string s, out global::UnityEngine.KeyCode keyCode)
	{
		if (int.TryParse(s, out var result) && global::System.Enum.IsDefined(typeof(global::UnityEngine.KeyCode), (global::UnityEngine.KeyCode)result))
		{
			keyCode = (global::UnityEngine.KeyCode)result;
			return true;
		}
		keyCode = global::UnityEngine.KeyCode.None;
		return false;
	}

	[global::UnityEngine.RuntimeInitializeOnLoadMethod]
	public static void ResetToDefault()
	{
		UserKeybinds.Clear();
		NewInput.ActionDefinition[] definedActions = DefinedActions;
		foreach (NewInput.ActionDefinition actionDefinition in definedActions)
		{
			global::UnityEngine.KeyCode defaultKey = actionDefinition.DefaultKey;
			UserKeybinds[actionDefinition.Name] = defaultKey;
		}
	}

	public static bool IsCompatible(this ActionName sourceAction, ActionName actionToCompare)
	{
		NewInput.ActionDefinition[] definedActions = DefinedActions;
		foreach (NewInput.ActionDefinition actionDefinition in definedActions)
		{
			if (actionDefinition.Name != sourceAction)
			{
				continue;
			}
			if (actionDefinition.CompatibleActions == null || actionDefinition.CompatibleActions.Length == 0)
			{
				return false;
			}
			ActionName[] compatibleActions = actionDefinition.CompatibleActions;
			for (int j = 0; j < compatibleActions.Length; j++)
			{
				if (compatibleActions[j] == actionToCompare)
				{
					return true;
				}
			}
			break;
		}
		return false;
	}

	public static bool TryGetCategory(this ActionName sourceAction, out ActionCategory cat)
	{
		NewInput.ActionDefinition[] definedActions = DefinedActions;
		foreach (NewInput.ActionDefinition actionDefinition in definedActions)
		{
			if (actionDefinition.Name == sourceAction)
			{
				cat = actionDefinition.Category;
				return true;
			}
		}
		cat = ActionCategory.Gameplay;
		return false;
	}
}
