namespace Interactables
{
	public interface IServerInteractable : global::Interactables.IInteractable
	{
		[global::Mirror.Server]
		void ServerInteract(ReferenceHub ply, byte colliderId);
	}
}
