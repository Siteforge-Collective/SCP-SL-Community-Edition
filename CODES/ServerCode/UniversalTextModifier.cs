public abstract class UniversalTextModifier : global::UnityEngine.MonoBehaviour
{
	private global::TMPro.TMP_Text _tmpText;

	private global::UnityEngine.UI.Text _unityText;

	private bool _usesTmp;

	public string DisplayText
	{
		get
		{
			if (!_usesTmp)
			{
				return _unityText.text;
			}
			return _tmpText.text;
		}
		set
		{
			if (_usesTmp)
			{
				_tmpText.text = value;
			}
			else
			{
				_unityText.text = value;
			}
		}
	}

	protected virtual void Awake()
	{
		_usesTmp = TryGetComponent<global::TMPro.TMP_Text>(out _tmpText);
		if (!_usesTmp)
		{
			_unityText = GetComponent<global::UnityEngine.UI.Text>();
		}
	}
}
