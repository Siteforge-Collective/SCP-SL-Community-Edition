namespace Interactables.Verification
{
	public interface IVerificationRule
	{
		bool ClientCanInteract(global::Interactables.InteractableCollider collider, global::UnityEngine.RaycastHit hit);

		bool ServerCanInteract(ReferenceHub hub, global::Interactables.InteractableCollider collider);
	}
}
