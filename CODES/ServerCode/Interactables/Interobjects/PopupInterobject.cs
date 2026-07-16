namespace Interactables.Interobjects
{
	public abstract class PopupInterobject : global::UnityEngine.MonoBehaviour, global::Interactables.IClientInteractable, global::Interactables.IInteractable
	{
		public global::Interactables.Verification.IVerificationRule VerificationRule => global::Interactables.Verification.StandardDistanceVerification.Default;

		private void Update()
		{
		}
	}
}
