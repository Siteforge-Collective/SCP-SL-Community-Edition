using System.Collections.Generic;

using UnityEngine;

public class MenuAnimator : MonoBehaviour
{
	[SerializeField]
	private GameObject sceneCamera;

	[SerializeField]
	private GameObject focusedPosition;

	[SerializeField]
	private GameObject unfocusedPosition;

	private Animator cameraSway;

	private MainMenuScript mms;

	[SerializeField]
	public static bool wasEverZoomed;

	public static bool retro;

	public bool retroSupported;

	public CameraFilterPack_TV_ARCADE_2 retroArcade;

	public CameraFilterPack_Colors_NewPosterize retroPosterize;

	private KeyCode[] konami = new KeyCode[11]
	{
		KeyCode.UpArrow,
		KeyCode.UpArrow,
		KeyCode.DownArrow,
		KeyCode.DownArrow,
		KeyCode.LeftArrow,
		KeyCode.RightArrow,
		KeyCode.LeftArrow,
		KeyCode.RightArrow,
		KeyCode.B,
		KeyCode.A,
		KeyCode.Return
	};

	private int konamiIndex;

	private void Start()
	{
		//throw new AnalysisFailedException("An action of type CallManagedFunctionAction at (0x1802E2B3D) is corrupt (Returned local is null but non-void) and cannot be created as IL.");
	}

	private void Update()
	{
		//throw new AnalysisFailedException("An action of type CallManagedFunctionAction at (0x1802E2C94) is corrupt (Returned local is null but non-void) and cannot be created as IL.");
	}

	private IEnumerator<float> _Animate()
	{
		throw new AnalysisFailedException("An action of type AllocateInstanceAction at (0x1802E32FF) is corrupt (Managed method being called is System.IDisposable.Dispose, not a ctor.) and cannot be created as IL.");
	}
}
