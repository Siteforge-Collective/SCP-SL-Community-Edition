using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotosensitivityWarning : MonoBehaviour
{
	private enum Photosensitivity
	{
		FirstViewing = 0
	}

	private const float FirstViewingTime = 5f;

	private const float ShortViewingTime = 0f;

	private float _currentViewingTime;

	private float _intendedViewingTime;

	[SerializeField]
	private TextMeshProUGUI _skipText;

	[SerializeField]
	private Image _loadingBar;

	[SerializeField]
	private Toggle _showToggle;

	[SerializeField]
	private float _lerpModifier;

	[SerializeField]
	private float _timeOffset;

	[SerializeField]
	private bool _reset;

	[SerializeField]
	private GameObject _parentObject;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Skip()
	{
	}

	public void Toggled()
	{
	}
}
