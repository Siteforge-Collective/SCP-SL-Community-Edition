using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeycodeField : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	[SerializeField]
	private TMP_Text _keyText;

	[SerializeField]
	private KeyCode[] _cancelKeys;

	[SerializeField]
	private TextAlignmentOptions _readAlignment;

	[SerializeField]
	private TextAlignmentOptions _setAlignment;

	private const string ReadSymbol = "•  •  •";

	private const int InputCooldownFrames = 3;

	private bool _currentlyEditing;

	private int _inputCooldown;

	private KeyCode _requestedChange;

	private static KeycodeField _lastEntry;

	private static KeyCode[] _allKeyCodes;

	public KeyCode CurDisplayedKey { get; private set; }

	public event Action<KeyCode> OnKeySet
	{
		[CompilerGenerated]
		add
		{
		}
		[CompilerGenerated]
		remove
		{
		}
	}

	public void SetDisplayedKey(KeyCode key)
	{
	}

	public void OnPointerClick(PointerEventData eventData)
	{
	}

	public void ExitEditMode()
	{
	}

	protected virtual bool ValidatePressedKey(KeyCode key)
	{
		return false;
	}

	protected virtual void ApplyPressedKey(KeyCode key)
	{
	}

	protected virtual void OnDisable()
	{
	}

	protected virtual void Update()
	{
	}
}
