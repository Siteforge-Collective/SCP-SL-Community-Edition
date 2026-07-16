using TMPro;
using UnityEngine;

namespace Tooltips
{
	public class Tooltip : MonoBehaviour
	{
		[SerializeField]
		private Canvas _canvas;

		[SerializeField]
		private RectTransform _rectTransform;

		[SerializeField]
		private RectTransform _background;

		[SerializeField]
		private TMP_Text _textComponent;

		[SerializeField]
		private Vector3 _offset;

		[SerializeField]
		private float _padding;

		public void SetText(string text)
		{
		}

		public void SetPosition(Vector3 position)
		{
		}
	}
}
