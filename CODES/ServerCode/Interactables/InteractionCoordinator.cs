namespace Interactables
{
	public class InteractionCoordinator : global::Mirror.NetworkBehaviour
	{
		private readonly global::System.Collections.Generic.HashSet<global::InventorySystem.Items.IInteractionBlocker> _blockers = new global::System.Collections.Generic.HashSet<global::InventorySystem.Items.IInteractionBlocker>();

		private ReferenceHub _hub;

		public void AddBlocker(global::InventorySystem.Items.IInteractionBlocker blocker)
		{
			_blockers.Add(blocker);
		}

		public bool AnyBlocker(global::InventorySystem.Items.BlockedInteraction interactions)
		{
			return AnyBlocker((global::InventorySystem.Items.IInteractionBlocker blocker) => (blocker.BlockedInteractions & interactions) == interactions);
		}

		public bool AnyBlocker(global::System.Func<global::InventorySystem.Items.IInteractionBlocker, bool> func)
		{
			_blockers.RemoveWhere((global::InventorySystem.Items.IInteractionBlocker x) => (x is global::UnityEngine.Object obj && obj == null) || (x?.CanBeCleared ?? true));
			return global::Utils.NonAllocLINQ.HashsetExtensions.Any(_blockers, func);
		}

		private void Start()
		{
			if (base.isLocalPlayer || global::Mirror.NetworkServer.active)
			{
				_hub = ReferenceHub.GetHub(base.gameObject);
			}
		}

		private static global::Interactables.Verification.IVerificationRule GetSafeRule(global::Interactables.IInteractable inter)
		{
			return inter.VerificationRule ?? global::Interactables.Verification.StandardDistanceVerification.Default;
		}

		[global::Mirror.Command(channel = 4)]
		private void CmdServerInteract(global::Mirror.NetworkIdentity targetInteractable, byte colId)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.NetworkWriterExtensions.WriteNetworkIdentity(writer, targetInteractable);
			global::Mirror.NetworkWriterExtensions.WriteByte(writer, colId);
			SendCommandInternal(typeof(global::Interactables.InteractionCoordinator), "CmdServerInteract", writer, 4);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		private void Update()
		{
		}

		private void MirrorProcessed()
		{
		}

		private void UserCode_CmdServerInteract(global::Mirror.NetworkIdentity targetInteractable, byte colId)
		{
			if (!(targetInteractable == null) && !(_hub == null) && global::PlayerRoles.PlayerRolesUtils.IsAlive(_hub) && targetInteractable.TryGetComponent<global::Interactables.IServerInteractable>(out var component) && global::Interactables.InteractableCollider.TryGetCollider(component, colId, out var res) && GetSafeRule(component).ServerCanInteract(_hub, res))
			{
				component.ServerInteract(_hub, colId);
			}
		}

		protected static void InvokeUserCode_CmdServerInteract(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogError("Command CmdServerInteract called on client.");
			}
			else
			{
				((global::Interactables.InteractionCoordinator)obj).UserCode_CmdServerInteract(global::Mirror.NetworkReaderExtensions.ReadNetworkIdentity(reader), global::Mirror.NetworkReaderExtensions.ReadByte(reader));
			}
		}

		static InteractionCoordinator()
		{
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(global::Interactables.InteractionCoordinator), "CmdServerInteract", InvokeUserCode_CmdServerInteract, requiresAuthority: true);
		}
	}
}
