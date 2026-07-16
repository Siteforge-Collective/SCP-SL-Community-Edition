using TMPro;
using ToggleableMenus;
using UnityEngine;

public class SpectatorInterface : SimpleToggleableMenu
{
	public GameObject RootPanel;

	public static SpectatorInterface Singleton;

	public TextMeshProUGUI PlayerList;

	public TextMeshProUGUI PlayerInfo;

	[SerializeField]
	private TextMeshProUGUI _attachmentsEditorText;

	private Canvas _spectatorCanvas;

	public bool SpectatorCanvasActive { get; set; }

	public override bool CanToggle => false;

	protected override void Awake()
	{
	}

	private void OnEnable()
	{
	}

	private void Update()
	{
	}
}
