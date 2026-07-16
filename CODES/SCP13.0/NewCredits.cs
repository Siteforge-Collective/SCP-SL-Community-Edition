using UnityEngine;
using UnityEngine.UI;

public class NewCredits : MonoBehaviour
{
	public GameObject root;

	public GameObject canvas;

	public bool StartScroll;

	public float Speed;

	private const float OriginalSpeed = 1.45f;

	private const float FastSpeed = 6f;

	private bool _paused;

	public RectTransform Content;

	public RectTransform PrefCategory;

	public RectTransform PrefElementWithRole;

	public RectTransform PrefElement;

	public AudioSource CreditsMusic;

	private bool _loaded;

	private int _scrollDelay;

	private float _height;

	private RectTransform[] _contentChildren;

	private VerticalLayoutGroup layoutGroup;

	private MainMenuSoundtrackController menuSoundtrack;

	public void OnButtonClick()
	{
	}

	public void OnBackButtonClick()
	{
	}

	private void OnEnable()
	{
	}

	private void Update()
	{
	}

	private void FixedUpdate()
	{
	}

	private bool ScrollAndCanEnd()
	{
		return false;
	}

	private void Load()
	{
	}

	private void Initialize()
	{
	}
}
