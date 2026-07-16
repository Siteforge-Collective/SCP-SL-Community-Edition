using Interactables;
using Interactables.Verification;
using TMPro;
using UnityEngine;

namespace InventorySystem.Items.Firearms.Attachments
{
	public class AttachmentPresetSelector : MonoBehaviour, IClientInteractable, IInteractable
	{
		[SerializeField]
		private AttachmentSelectorBase _selectorRef;

		[SerializeField]
		private GameObject _rootObject;

		[SerializeField]
		private TextMeshProUGUI[] _saveButtons;

		[SerializeField]
		private TextMeshProUGUI[] _currentPresetIndicators;

		[SerializeField]
		private Color _normalColor;

		[SerializeField]
		private Color _currentColor;

		private const byte SaveOffset = 100;

		private const byte ResetAttachmentsCode = 254;

		private const byte SummaryToggleCode = 253;

		public IVerificationRule VerificationRule => null;

		private void Start()
		{
		}

		public void ProcessButton(int id)
		{
		}

		private void LateUpdate()
		{
		}

		public void ClientInteract(InteractableCollider collider)
		{
		}
	}
}
