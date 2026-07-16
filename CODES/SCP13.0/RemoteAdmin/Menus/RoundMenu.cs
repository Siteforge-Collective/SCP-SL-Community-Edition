using RemoteAdmin.Communication;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RemoteAdmin.Menus
{
	public class RoundMenu : RaCommandMenu
	{
		public enum Panel
		{
			ServerEvents = 0,
			TicketSystem = 1
		}

		private const string DisabledCode = "\ud83d\udd12";

		private const string EnabledCode = "\ud83d\udd13";

		private const float BackgroundAlpha = 0.09f;

		[SerializeField]
		private Color _enabledStatus;

		[SerializeField]
		private Color _disabledStatus;

		[SerializeField]
		private Slider _teamDominationBar;

		[SerializeField]
		private TMP_Text _mtfTokens;

		[SerializeField]
		private TMP_Text _chaosTokens;

		[SerializeField]
		private TMP_Text _respawnTimer;

		[SerializeField]
		private TMP_Text _nextTeam;

		[SerializeField]
		private TMP_Text _roundLock;

		[SerializeField]
		private TMP_Text _lobbyLock;

		[SerializeField]
		private TMP_Text _warheadLock;

		[SerializeField]
		private TMP_Text _friendlyFire;

		[SerializeField]
		private TMP_Text _spawnProtect;

		[SerializeField]
		private GameObject _iconParent;

		[SerializeField]
		private Image _teamBorder;

		[SerializeField]
		private Image _teamBackground;

		[SerializeField]
		private Image _teamImage;

		[SerializeField]
		private Sprite _mtfSprite;

		[SerializeField]
		private Sprite _ciSprite;

		[SerializeField]
		private Color _ciColor;

		[SerializeField]
		private Color _mtfColor;

		private RaServerStatus _serverChannel;

		private RaTeamStatus _teamChannel;

		private Panel _selectedPanel;

		private float _updateTimer;

		public void SetPanel(int panel)
		{
		}

		public void RequestData()
		{
		}

		protected override void OnUpdate()
		{
		}

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void RefreshTeamWindow(string obj)
		{
		}

		private void RefreshServerWindow(string obj)
		{
		}

		private void RefreshTokens(string tokens, TMP_Text textComponent, bool updateDominationBar = true)
		{
		}

		private void UpdateIcon(TMP_Text textComponent, bool isEnabled, bool changeIcon = true, bool inversedColors = false)
		{
		}

		private Color GenerateBackground(Color originalColor)
		{
			return default(Color);
		}
	}
}
