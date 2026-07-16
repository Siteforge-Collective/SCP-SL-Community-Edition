using UnityEngine;
using UnityEngine.UI;

namespace RemoteAdmin.Presets
{
	[CreateAssetMenu(fileName = "MyColorPreset", menuName = "Northwood/UI/Color Preset (Toggle)")]
	public class ToggleColorPreset : ScriptableObject
	{
		[SerializeField]
		[Header("Selected:")]
		private ColorBlock _selected;

		[Space(5f)]
		[Header("Unselected:")]
		[SerializeField]
		private ColorBlock _unselected;

		public ColorBlock Selected => default(ColorBlock);

		public ColorBlock Unselected => default(ColorBlock);
	}
}
