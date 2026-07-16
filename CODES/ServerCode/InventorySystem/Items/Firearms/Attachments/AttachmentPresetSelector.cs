namespace InventorySystem.Items.Firearms.Attachments
{
	public class AttachmentPresetSelector : global::UnityEngine.MonoBehaviour, global::Interactables.IClientInteractable, global::Interactables.IInteractable
	{
		[global::UnityEngine.SerializeField]
		private global::InventorySystem.Items.Firearms.Attachments.AttachmentSelectorBase _selectorRef;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _rootObject;

		[global::UnityEngine.SerializeField]
		private global::TMPro.TextMeshProUGUI[] _saveButtons;

		[global::UnityEngine.SerializeField]
		private global::TMPro.TextMeshProUGUI[] _currentPresetIndicators;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Color _normalColor;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Color _currentColor;

		private const byte SaveOffset = 100;

		private const byte ResetAttachmentsCode = 254;

		private const byte SummaryToggleCode = 253;

		public global::Interactables.Verification.IVerificationRule VerificationRule => global::Interactables.Verification.StandardDistanceVerification.Default;

		private void Start()
		{
		}

		public void ProcessButton(int id)
		{
		}

		private void LateUpdate()
		{
		}

		public void ClientInteract(global::Interactables.InteractableCollider collider)
		{
			ProcessButton(collider.ColliderId);
		}
	}
}
