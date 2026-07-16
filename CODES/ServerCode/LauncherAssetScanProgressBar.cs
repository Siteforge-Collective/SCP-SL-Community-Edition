public class LauncherAssetScanProgressBar : global::UnityEngine.MonoBehaviour
{
	[global::UnityEngine.SerializeField]
	private global::UnityEngine.UI.Slider _slider;

	[global::UnityEngine.SerializeField]
	private SimpleMenu _menu;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.UI.Text _text;

	private void Update()
	{
		float num = global::UnityEngine.Mathf.Clamp(_menu.Progress, 0f, 100f);
		_slider.value = num;
		_text.text = $"Validating - {(global::UnityEngine.Mathf.RoundToInt(num))}%";
	}
}
