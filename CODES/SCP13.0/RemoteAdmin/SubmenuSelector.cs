using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RemoteAdmin
{
	public class SubmenuSelector : MonoBehaviour
	{
		[Serializable]
		public class SubMenu
		{
			public string Title;

			public TMP_Text ButtonText;

			public GameObject Panel;

			public TMP_Text RaResponseBox;

			public void Reset()
			{
			}

			public void Select()
			{
			}

			public void SetResponse(bool isSuccess, string content)
			{
			}

			public void SetResponse(Color color, string content)
			{
			}
		}

		public Color c_selected;

		public Color c_deselected;

		public SubMenu[] menus;

		public TMP_Text WindowTitle;

		[Header("Toggled On Colors:")]
		public ColorBlock ToggleOn;

		[Header("Toggled Off Colors:")]
		public ColorBlock ToggleOff;

		private SubMenu _currentMenu;

		private int _lastMenuIndex;

		private int _currentMenuIndex;

		public static SubmenuSelector Singleton { get; private set; }

		public SubMenu SelectedMenu
		{
			get
			{
				return null;
			}
			private set
			{
			}
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		private void Awake()
		{
		}

		public void PlayerInfoQuery(string operation)
		{
		}

		public void AdminToolsConfirm(string operation)
		{
		}

		public void RunCommand(string command)
		{
		}

		internal static void ClearAll()
		{
		}

		public void ToggleMenu(GameObject panel)
		{
		}

		public void SelectMenu(int index)
		{
		}

		public void SelectMenu(GameObject panel)
		{
		}

		public void SelectMenu(SubMenu subMenu)
		{
		}

		private SubMenu FindMenu(GameObject panel)
		{
			return null;
		}
	}
}
