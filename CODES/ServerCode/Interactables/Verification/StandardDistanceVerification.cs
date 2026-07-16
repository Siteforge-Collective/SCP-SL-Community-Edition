namespace Interactables.Verification
{
	public class StandardDistanceVerification : global::Interactables.Verification.IVerificationRule
	{
		private const float InteractLagCompensation = 1.4f;

		public static global::Interactables.Verification.StandardDistanceVerification Default = new global::Interactables.Verification.StandardDistanceVerification();

		private readonly float _maxDistance;

		private readonly bool _allowHandcuffed;

		private readonly bool _cancel268;

		public StandardDistanceVerification(float maxDistance = 2.42f, bool allowHandcuffedInteraction = false, bool cancelScp268 = true)
		{
			_maxDistance = maxDistance;
			_allowHandcuffed = allowHandcuffedInteraction;
			_cancel268 = cancelScp268;
		}

		public bool ClientCanInteract(global::Interactables.InteractableCollider collider, global::UnityEngine.RaycastHit hit)
		{
			return hit.distance < _maxDistance;
		}

		public bool ServerCanInteract(ReferenceHub hub, global::Interactables.InteractableCollider collider)
		{
			if (!_allowHandcuffed && !PlayerInteract.CanDisarmedInteract && global::InventorySystem.Disarming.DisarmedPlayers.IsDisarmed(hub.inventory))
			{
				return false;
			}
			if (hub.interCoordinator.AnyBlocker(global::InventorySystem.Items.BlockedInteraction.GeneralInteractions))
			{
				return false;
			}
			if (!(hub.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole))
			{
				return false;
			}
			global::UnityEngine.Transform transform = collider.transform;
			if (global::UnityEngine.Vector3.Distance(fpcRole.FpcModule.Position, transform.position + transform.TransformDirection(collider.VerificationOffset)) > _maxDistance * 1.4f)
			{
				return false;
			}
			if (_cancel268)
			{
				hub.playerEffectsController.DisableEffect<global::CustomPlayerEffects.Invisible>();
			}
			return true;
		}
	}
}
