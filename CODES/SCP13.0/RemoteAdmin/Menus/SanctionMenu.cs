using UnityEngine;
using UnityEngine.UI;

namespace RemoteAdmin.Menus
{
	public class SanctionMenu : RaCommandMenu
	{
		[SerializeField]
		private Transform _rootParent;

		[SerializeField]
		private GameObject _template;

		[SerializeField]
		private Button _predefinedReasonsButton;

		protected override void OnStart()
		{
		}
	}
}
