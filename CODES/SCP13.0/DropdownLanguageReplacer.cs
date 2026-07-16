using TMPro;
using UnityEngine;

public class DropdownLanguageReplacer : MonoBehaviour
{
	[SerializeField]
	private TextLanguageReplacer[] _optionTranslations;

	[SerializeField]
	private bool _updateOnReload;

	private TMP_Dropdown _dropdown;

	private void Start()
	{
	}

	private void UpdateOption(int index)
	{
	}
}
