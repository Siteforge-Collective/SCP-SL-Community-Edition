using Interactables;
using Interactables.Interobjects;
using Mirror;
using PlayerRoles.Subroutines;

namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096PrygateAbility : StandardSubroutine<Scp096Role>
	{
		private PryableDoor _syncDoor;

		private Scp096HitHandler _hitHandler;

		private Scp096AudioPlayer _audioPlayer;

		private const float DoorKillerHeight = 1.5f;

		private const float DoorKillerRadius = 0.2f;

		private const float MaxDisSqr = 8.12f;

		private const float HumanDamage = 200f;

		public void ClientTryPry(PryableDoor door)
		{
		}

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		public override void ClientWriteCmd(NetworkWriter writer)
		{
		}

		public override void ServerProcessCmd(NetworkReader reader)
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		protected override void Awake()
		{
		}

		private void Update()
		{
		}

		private void OnInteracted(InteractableCollider col)
		{
		}
	}
}
