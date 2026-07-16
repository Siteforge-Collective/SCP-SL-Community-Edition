public class ImageBasedRealisticLoadingBar : global::UnityEngine.MonoBehaviour
{
	private RealisticLoadingBar _bar;

	[global::UnityEngine.SerializeField]
	private float _targetTime;

	[global::UnityEngine.SerializeField]
	private float _stepVar;

	[global::UnityEngine.SerializeField]
	private float _tickVar;

	[global::UnityEngine.SerializeField]
	private int _minNumOfSteps;

	[global::UnityEngine.SerializeField]
	private int _maxNumOfSteps;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.UI.Image _targetImage;

	[global::UnityEngine.SerializeField]
	private float _smoothing;

	private void OnEnable()
	{
		_targetImage.fillAmount = 0f;
		_bar = new RealisticLoadingBar(_targetTime, global::UnityEngine.Random.Range(_minNumOfSteps, _maxNumOfSteps + 1), _stepVar, _tickVar);
	}

	private void Update()
	{
		_targetImage.fillAmount = global::UnityEngine.Mathf.Lerp(_targetImage.fillAmount, _bar.Progress, global::UnityEngine.Time.deltaTime * _smoothing);
	}
}
