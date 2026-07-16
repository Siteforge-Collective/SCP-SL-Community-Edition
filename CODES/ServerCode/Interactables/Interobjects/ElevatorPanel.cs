namespace Interactables.Interobjects
{
	public class ElevatorPanel : global::Interactables.InteractableCollider, global::Interactables.IClientInteractable, global::Interactables.IInteractable
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Material[] _levelMats;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Material _movingUpMat;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Material _movingDownMat;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Material _disabledMat;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Renderer _targetRenderer;

		public global::Interactables.Interobjects.ElevatorChamber AssignedChamber { get; private set; }

		public global::Interactables.Verification.IVerificationRule VerificationRule => global::Interactables.Verification.StandardDistanceVerification.Default;

		public void ClientInteract(global::Interactables.InteractableCollider _)
		{
			if (global::Interactables.Interobjects.ElevatorDoor.AllElevatorDoors.TryGetValue(AssignedChamber.AssignedGroup, out var value))
			{
				int num = AssignedChamber.CurrentLevel + 1;
				if (num >= value.Count)
				{
					num = 0;
				}
				global::Mirror.NetworkClient.Send(new global::Interactables.Interobjects.ElevatorManager.ElevatorSyncMsg(AssignedChamber.AssignedGroup, num));
			}
		}

		public void SetChamber(global::Interactables.Interobjects.ElevatorChamber chamber)
		{
			AssignedChamber = chamber;
		}

		public void SetLevel(int level)
		{
		}

		public void SetLocked()
		{
		}

		public void SetMoving(bool up)
		{
		}
	}
}
