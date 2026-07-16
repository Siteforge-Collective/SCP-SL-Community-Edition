using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem.Items.Firearms.Attachments.Components
{
	public class ReflexSightConfigWindow : AttachmentConfigWindow
	{
		[SerializeField]
		private RectTransform _shapeTemplate;

		[SerializeField]
		private RectTransform _colorTemplate;

		[SerializeField]
		private RectTransform _buttonReduce;

		[SerializeField]
		private RectTransform _buttonEnlarge;

		[SerializeField]
		private TextMeshProUGUI _textPercent;

		[SerializeField]
		private Color _selectedColor;

		[SerializeField]
		private Color _normalColor;

		private Image[] _shapeInstances;

		private Image[] _colorInstances;

		public override void Setup(AttachmentSelectorBase selector, Attachment attachment, RectTransform toFit)
		{
		}

		protected override void OnDestroy()
		{
		}

		private void UpdateValues()
		{
		}

		private Image[] GenerateOptions<T>(T[] array, RectTransform template, Action<int> action, Action<RectTransform, int> modifier)
		{
			return null;
		}
	}
}
