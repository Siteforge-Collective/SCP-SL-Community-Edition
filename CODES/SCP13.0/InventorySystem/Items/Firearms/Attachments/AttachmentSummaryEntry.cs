using TMPro;
using UnityEngine;

namespace InventorySystem.Items.Firearms.Attachments
{
	public class AttachmentSummaryEntry : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI _label;

		[SerializeField]
		private TextMeshProUGUI[] _valuesBank;

		[SerializeField]
		private Color _oddColor;

		private bool _firstSetup;

		public void Setup(string label, string[] values, bool isOdd)
		{
		}
	}
}
