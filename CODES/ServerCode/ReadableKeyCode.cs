public readonly struct ReadableKeyCode
{
	private static readonly global::System.Collections.Generic.Dictionary<global::UnityEngine.KeyCode, ReadableKeyCode> AlreadyDefinedKeycodes = new global::System.Collections.Generic.Dictionary<global::UnityEngine.KeyCode, ReadableKeyCode>();

	private readonly int _normalVersionLength;

	public readonly string NormalVersion;

	public readonly string ShortVersion;

	public ReadableKeyCode(global::UnityEngine.KeyCode keycode)
	{
		if (AlreadyDefinedKeycodes.TryGetValue(keycode, out var value))
		{
			NormalVersion = value.NormalVersion;
			ShortVersion = value.ShortVersion;
			_normalVersionLength = value._normalVersionLength;
		}
		else
		{
			GetReadableForm(keycode, out NormalVersion, out ShortVersion);
			_normalVersionLength = NormalVersion.Length;
			AlreadyDefinedKeycodes[keycode] = this;
		}
	}

	public ReadableKeyCode(ActionName action)
		: this(NewInput.GetKey(action))
	{
	}

	public string GetBestVersion(int maxCharacters)
	{
		if (_normalVersionLength <= maxCharacters)
		{
			return NormalVersion;
		}
		return ShortVersion;
	}

	public override string ToString()
	{
		return NormalVersion;
	}

	private static void GetReadableForm(global::UnityEngine.KeyCode keycode, out string normalVer, out string shortVer)
	{
		switch (keycode)
		{
		case global::UnityEngine.KeyCode.Escape:
			normalVer = "Escape";
			shortVer = "Esc";
			break;
		case global::UnityEngine.KeyCode.BackQuote:
			normalVer = "~";
			shortVer = "~";
			break;
		case global::UnityEngine.KeyCode.Backspace:
			normalVer = "Backspace";
			shortVer = "BKSP";
			break;
		case global::UnityEngine.KeyCode.Insert:
			normalVer = "Insert";
			shortVer = "Ins";
			break;
		case global::UnityEngine.KeyCode.Delete:
			normalVer = "Delete";
			shortVer = "Del";
			break;
		case global::UnityEngine.KeyCode.PageUp:
			normalVer = "Page Up";
			shortVer = "PgUp";
			break;
		case global::UnityEngine.KeyCode.PageDown:
			normalVer = "Page Down";
			shortVer = "PgDn";
			break;
		case global::UnityEngine.KeyCode.UpArrow:
			normalVer = "Up Arrow";
			shortVer = "▲";
			break;
		case global::UnityEngine.KeyCode.DownArrow:
			normalVer = "Down Arrow";
			shortVer = "▼";
			break;
		case global::UnityEngine.KeyCode.LeftArrow:
			normalVer = "Left Arrow";
			shortVer = "◄";
			break;
		case global::UnityEngine.KeyCode.RightArrow:
			normalVer = "Right Arrow";
			shortVer = "►";
			break;
		case global::UnityEngine.KeyCode.LeftShift:
			normalVer = "Left Shift";
			shortVer = "Left ⇧";
			break;
		case global::UnityEngine.KeyCode.RightShift:
			normalVer = "Right Shift";
			shortVer = "Right ⇧";
			break;
		case global::UnityEngine.KeyCode.LeftControl:
			normalVer = "Left Control";
			shortVer = "L Ctrl";
			break;
		case global::UnityEngine.KeyCode.RightControl:
			normalVer = "Right Control";
			shortVer = "R Ctrl";
			break;
		case global::UnityEngine.KeyCode.LeftAlt:
			normalVer = "Left Alt";
			shortVer = "L Alt";
			break;
		case global::UnityEngine.KeyCode.RightAlt:
			normalVer = "Right Alt";
			shortVer = "R Alt";
			break;
		case global::UnityEngine.KeyCode.Mouse0:
			normalVer = "Left Mouse Button";
			shortVer = "LMB";
			break;
		case global::UnityEngine.KeyCode.Mouse1:
			normalVer = "Right Mouse Button";
			shortVer = "RMB";
			break;
		case global::UnityEngine.KeyCode.Mouse2:
			normalVer = "Middle Mouse Button";
			shortVer = "MMB";
			break;
		case global::UnityEngine.KeyCode.Mouse3:
		case global::UnityEngine.KeyCode.Mouse4:
		case global::UnityEngine.KeyCode.Mouse5:
		case global::UnityEngine.KeyCode.Mouse6:
			normalVer = keycode.ToString().Insert(5, " Button ");
			shortVer = "MB" + normalVer[normalVer.Length - 1];
			break;
		case global::UnityEngine.KeyCode.Alpha0:
		case global::UnityEngine.KeyCode.Alpha1:
		case global::UnityEngine.KeyCode.Alpha2:
		case global::UnityEngine.KeyCode.Alpha3:
		case global::UnityEngine.KeyCode.Alpha4:
		case global::UnityEngine.KeyCode.Alpha5:
		case global::UnityEngine.KeyCode.Alpha6:
		case global::UnityEngine.KeyCode.Alpha7:
		case global::UnityEngine.KeyCode.Alpha8:
		case global::UnityEngine.KeyCode.Alpha9:
			normalVer = ((int)(keycode - 48)).ToString();
			shortVer = normalVer;
			break;
		case global::UnityEngine.KeyCode.Keypad0:
		case global::UnityEngine.KeyCode.Keypad1:
		case global::UnityEngine.KeyCode.Keypad2:
		case global::UnityEngine.KeyCode.Keypad3:
		case global::UnityEngine.KeyCode.Keypad4:
		case global::UnityEngine.KeyCode.Keypad5:
		case global::UnityEngine.KeyCode.Keypad6:
		case global::UnityEngine.KeyCode.Keypad7:
		case global::UnityEngine.KeyCode.Keypad8:
		case global::UnityEngine.KeyCode.Keypad9:
		{
			int num = (int)(keycode - 256);
			normalVer = "Numpad " + num;
			shortVer = "Num " + num;
			break;
		}
		default:
			normalVer = keycode.ToString();
			shortVer = normalVer;
			break;
		}
	}
}
