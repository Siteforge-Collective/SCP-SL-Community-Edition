using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RemoteAdmin.Menus
{
	public class RoleManagementMenu : RaCommandMenu
	{
		private const string DefaultName = "None";

		private const string SecretName = "Hubert Moszka";

		private const int SecretMaxTriggers = 173;

		[SerializeField]
		private RoleElement _roleElementPrefab;

		[SerializeField]
		private Transform _rootLayout;

		[SerializeField]
		private RawImage _avatar;

		[SerializeField]
		private Image _background;

		[SerializeField]
		private Image _border;

		[SerializeField]
		private TMP_Text _name;

		[SerializeField]
		private float _backgroundAlpha;

		[SerializeField]
		private Texture _defaultIcon;

		[SerializeField]
		private Texture _secretIcon;

		[SerializeField]
		private Color _secretColor;

		private int _triggers;

		public void UpdateGraphic(Color roleColor, Texture roleIcon, string roleName)
		{
		}

		public void ResetGraphic()
		{
		}

		public void ShowSecret()
		{
		}

		protected override string BuildCommand(string command, string format)
		{
			return null;
		}

		public override void SendCommand(string command, string format = "")
		{
		}

		protected override void OnStart()
		{
		}

		private Color GenerateBackground(Color originalColor)
		{
			return default(Color);
		}
	}
}
