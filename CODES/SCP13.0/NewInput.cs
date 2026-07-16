public static class NewInput
{
    public static event global::System.Action OnAnyModified;

    public static event global::System.Action<global::ActionName, global::UnityEngine.KeyCode> OnKeyModified;

    public static global::UnityEngine.KeyCode GetKey(global::ActionName actionName, global::UnityEngine.KeyCode fallbackKeycode = global::UnityEngine.KeyCode.None)
    {
        if (!global::NewInput._loaded)
        {
            global::NewInput.Load();
        }
        global::UnityEngine.KeyCode result;
        if (!global::NewInput.UserKeybinds.TryGetValue(actionName, out result))
        {
            return fallbackKeycode;
        }
        return result;
    }

    public static void Save()
    {
        global::System.Text.StringBuilder stringBuilder = global::NorthwoodLib.Pools.StringBuilderPool.Shared.Rent();
        foreach (global::System.Collections.Generic.KeyValuePair<global::ActionName, global::UnityEngine.KeyCode> keyValuePair in global::NewInput.UserKeybinds)
        {
            stringBuilder.Append((int)keyValuePair.Key);
            stringBuilder.Append(':');
            stringBuilder.Append((int)keyValuePair.Value);
            stringBuilder.Append(';');
        }
        global::System.IO.File.WriteAllText(global::NewInput.SaveFilePath, stringBuilder.ToString(0, stringBuilder.Length - 1));
        global::NorthwoodLib.Pools.StringBuilderPool.Shared.Return(stringBuilder);
    }

    public static void Load()
    {
        global::NewInput.ResetToDefault();
        if (!global::System.IO.File.Exists(global::NewInput.SaveFilePath))
        {
            global::NewInput.Save();
        }
        string[] array = global::System.IO.File.ReadAllText(global::NewInput.SaveFilePath).Split(';', global::System.StringSplitOptions.None);
        for (int i = 0; i < array.Length; i++)
        {
            string[] array2 = array[i].Split(':', global::System.StringSplitOptions.None);
            global::ActionName key;
            global::UnityEngine.KeyCode value;
            if (array2.Length == 2 && global::NewInput.TryParseActionName(array2[0], out key) && global::NewInput.TryParseKeycode(array2[1], out value))
            {
                global::NewInput.UserKeybinds[key] = value;
            }
        }
        global::NewInput._loaded = true;
    }

    public static void ChangeKeybind(global::ActionName action, global::UnityEngine.KeyCode key)
    {
        if (key == global::UnityEngine.KeyCode.None && !global::NewInput.DefaultKeybinds.TryGetValue(action, out key))
        {
            return;
        }
        global::NewInput.UserKeybinds[action] = key;
        global::System.Action onAnyModified = global::NewInput.OnAnyModified;
        if (onAnyModified != null)
        {
            onAnyModified();
        }
        global::System.Action<global::ActionName, global::UnityEngine.KeyCode> onKeyModified = global::NewInput.OnKeyModified;
        if (onKeyModified != null)
        {
            onKeyModified(action, key);
        }
        global::NewInput.Save();
    }

    public static bool TryParseActionName(string s, out global::ActionName actionName)
    {
        int num;
        if (int.TryParse(s, out num) && global::System.Enum.IsDefined(typeof(global::ActionName), (global::ActionName)num))
        {
            actionName = (global::ActionName)num;
            return true;
        }
        global::UnityEngine.Debug.Log("Action name " + s + " is not defined");
        actionName = global::ActionName.Shoot;
        return false;
    }

    public static bool TryParseKeycode(string s, out global::UnityEngine.KeyCode keyCode)
    {
        int num;
        if (int.TryParse(s, out num) && global::System.Enum.IsDefined(typeof(global::UnityEngine.KeyCode), (global::UnityEngine.KeyCode)num))
        {
            keyCode = (global::UnityEngine.KeyCode)num;
            return true;
        }
        keyCode = global::UnityEngine.KeyCode.None;
        return false;
    }

    public static void ResetToDefault()
    {
        global::NewInput.UserKeybinds.Clear();
        foreach (global::NewInput.ActionDefinition actionDefinition in global::NewInput.DefinedActions)
        {
            global::UnityEngine.KeyCode defaultKey = actionDefinition.DefaultKey;
            global::NewInput.UserKeybinds[actionDefinition.Name] = defaultKey;
            global::NewInput.DefaultKeybinds[actionDefinition.Name] = defaultKey;
        }
    }

    public static bool TryGetCategory(this global::ActionName sourceAction, out global::ActionCategory cat)
    {
        foreach (global::NewInput.ActionDefinition actionDefinition in global::NewInput.DefinedActions)
        {
            if (actionDefinition.Name == sourceAction)
            {
                cat = actionDefinition.Category;
                return true;
            }
        }
        cat = global::ActionCategory.Gameplay;
        return false;
    }

    public static readonly global::System.Collections.Generic.Dictionary<global::ActionName, global::UnityEngine.KeyCode> DefaultKeybinds = new global::System.Collections.Generic.Dictionary<global::ActionName, global::UnityEngine.KeyCode>();

    private static readonly global::System.Collections.Generic.Dictionary<global::ActionName, global::UnityEngine.KeyCode> UserKeybinds = new global::System.Collections.Generic.Dictionary<global::ActionName, global::UnityEngine.KeyCode>();

    private static readonly string SaveFilePath = global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder.ApplicationData) + "/SCP Secret Laboratory/keybinding.txt";

    public static readonly global::NewInput.ActionDefinition[] DefinedActions = new global::NewInput.ActionDefinition[]
    {
        new global::NewInput.ActionDefinition(global::ActionName.Shoot, global::UnityEngine.KeyCode.Mouse0, global::ActionCategory.Weapons),
        new global::NewInput.ActionDefinition(global::ActionName.Zoom, global::UnityEngine.KeyCode.Mouse1, global::ActionCategory.Weapons),
        new global::NewInput.ActionDefinition(global::ActionName.Jump, global::UnityEngine.KeyCode.Space, global::ActionCategory.Movement),
        new global::NewInput.ActionDefinition(global::ActionName.Interact, global::UnityEngine.KeyCode.E, global::ActionCategory.Gameplay),
        new global::NewInput.ActionDefinition(global::ActionName.Inventory, global::UnityEngine.KeyCode.Tab, global::ActionCategory.Gameplay),
        new global::NewInput.ActionDefinition(global::ActionName.Reload, global::UnityEngine.KeyCode.R, global::ActionCategory.Weapons),
        new global::NewInput.ActionDefinition(global::ActionName.Run, global::UnityEngine.KeyCode.LeftShift, global::ActionCategory.Movement),
        new global::NewInput.ActionDefinition(global::ActionName.VoiceChat, global::UnityEngine.KeyCode.Q, global::ActionCategory.Communication),
        new global::NewInput.ActionDefinition(global::ActionName.Sneak, global::UnityEngine.KeyCode.C, global::ActionCategory.Movement),
        new global::NewInput.ActionDefinition(global::ActionName.MoveForward, global::UnityEngine.KeyCode.W, global::ActionCategory.Movement),
        new global::NewInput.ActionDefinition(global::ActionName.MoveBackward, global::UnityEngine.KeyCode.S, global::ActionCategory.Movement),
        new global::NewInput.ActionDefinition(global::ActionName.MoveLeft, global::UnityEngine.KeyCode.A, global::ActionCategory.Movement),
        new global::NewInput.ActionDefinition(global::ActionName.MoveRight, global::UnityEngine.KeyCode.D, global::ActionCategory.Movement),
        new global::NewInput.ActionDefinition(global::ActionName.PlayerList, global::UnityEngine.KeyCode.N, global::ActionCategory.Gameplay),
        new global::NewInput.ActionDefinition(global::ActionName.CharacterInfo, global::UnityEngine.KeyCode.F1, global::ActionCategory.Gameplay),
        new global::NewInput.ActionDefinition(global::ActionName.RemoteAdmin, global::UnityEngine.KeyCode.M, global::ActionCategory.System),
        new global::NewInput.ActionDefinition(global::ActionName.ToggleFlashlight, global::UnityEngine.KeyCode.F, global::ActionCategory.Weapons),
        new global::NewInput.ActionDefinition(global::ActionName.AltVoiceChat, global::UnityEngine.KeyCode.V, global::ActionCategory.Communication),
        new global::NewInput.ActionDefinition(global::ActionName.Noclip, global::UnityEngine.KeyCode.LeftAlt, global::ActionCategory.System),
        new global::NewInput.ActionDefinition(global::ActionName.NoClipFogToggle, global::UnityEngine.KeyCode.O, global::ActionCategory.System),
        new global::NewInput.ActionDefinition(global::ActionName.GameConsole, global::UnityEngine.KeyCode.BackQuote, global::ActionCategory.System),
        new global::NewInput.ActionDefinition(global::ActionName.InspectItem, global::UnityEngine.KeyCode.I, global::ActionCategory.Weapons),
        new global::NewInput.ActionDefinition(global::ActionName.ThrowItem, global::UnityEngine.KeyCode.T, global::ActionCategory.Gameplay),
        new global::NewInput.ActionDefinition(global::ActionName.HideGUI, global::UnityEngine.KeyCode.P, global::ActionCategory.System),
        new global::NewInput.ActionDefinition(global::ActionName.PauseMenu, global::UnityEngine.KeyCode.Escape, global::ActionCategory.Unbindable),
        new global::NewInput.ActionDefinition(global::ActionName.DebugLogMenu, global::UnityEngine.KeyCode.F4, global::ActionCategory.Unbindable),
        new global::NewInput.ActionDefinition(global::ActionName.Scp079FreeLook, global::UnityEngine.KeyCode.Space, global::ActionCategory.Scp079),
        new global::NewInput.ActionDefinition(global::ActionName.Scp079LockDoor, global::UnityEngine.KeyCode.Mouse1, global::ActionCategory.Scp079),
        new global::NewInput.ActionDefinition(global::ActionName.Scp079UnlockAll, global::UnityEngine.KeyCode.R, global::ActionCategory.Scp079),
        new global::NewInput.ActionDefinition(global::ActionName.Scp079Blackout, global::UnityEngine.KeyCode.F, global::ActionCategory.Scp079),
        new global::NewInput.ActionDefinition(global::ActionName.Scp079Lockdown, global::UnityEngine.KeyCode.G, global::ActionCategory.Scp079),
        new global::NewInput.ActionDefinition(global::ActionName.Scp079PingLocation, global::UnityEngine.KeyCode.E, global::ActionCategory.Scp079),
        new global::NewInput.ActionDefinition(global::ActionName.Scp079BreachScanner, global::UnityEngine.KeyCode.Space, global::ActionCategory.Scp079)
    };

    private static bool _loaded;

    public class ActionDefinition
    {
        public ActionDefinition(global::ActionName actionName, global::UnityEngine.KeyCode k, global::ActionCategory c)
        {
            this.Name = actionName;
            this.Category = c;
            this.DefaultKey = k;
        }

        public readonly global::ActionName Name;

        public readonly global::ActionCategory Category;

        public readonly global::UnityEngine.KeyCode DefaultKey;
    }
}
