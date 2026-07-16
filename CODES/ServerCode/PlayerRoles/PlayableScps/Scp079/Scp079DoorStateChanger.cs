namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079DoorStateChanger : global::PlayerRoles.PlayableScps.Scp079.Scp079DoorAbility
	{
		[global::System.Serializable]
		private struct DoorCost
		{
			public global::Interactables.Interobjects.DoorUtils.KeycardPermissions Perm;

			public int Cost;
		}

		[global::UnityEngine.SerializeField]
		private int _defaultCost;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp079.Scp079DoorStateChanger.DoorCost[] _doorCostsheet;

		private static string _openText;

		private static string _closeText;

		public override ActionName ActivationKey => ActionName.Shoot;

		public override string AbilityName => string.Format(LastDoor.TargetState ? _closeText : _openText, GetCostForDoor(TargetAction, LastDoor));

		protected override global::Interactables.Interobjects.DoorUtils.DoorAction TargetAction
		{
			get
			{
				if (!LastDoor.TargetState)
				{
					return global::Interactables.Interobjects.DoorUtils.DoorAction.Opened;
				}
				return global::Interactables.Interobjects.DoorUtils.DoorAction.Closed;
			}
		}

		public static event global::System.Action<global::PlayerRoles.PlayableScps.Scp079.Scp079Role, global::Interactables.Interobjects.DoorUtils.DoorVariant> OnServerDoorToggled;

		protected override void Start()
		{
			base.Start();
			_openText = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.OpenDoor);
			_closeText = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.CloseDoor);
		}

		protected override int GetCostForDoor(global::Interactables.Interobjects.DoorUtils.DoorAction action, global::Interactables.Interobjects.DoorUtils.DoorVariant door)
		{
			global::Interactables.Interobjects.DoorUtils.KeycardPermissions requiredPermissions = door.RequiredPermissions.RequiredPermissions;
			int num = _defaultCost;
			global::PlayerRoles.PlayableScps.Scp079.Scp079DoorStateChanger.DoorCost[] doorCostsheet = _doorCostsheet;
			for (int i = 0; i < doorCostsheet.Length; i++)
			{
				global::PlayerRoles.PlayableScps.Scp079.Scp079DoorStateChanger.DoorCost doorCost = doorCostsheet[i];
				if (global::Interactables.Interobjects.DoorUtils.DoorPermissionUtils.HasFlagFast(requiredPermissions, doorCost.Perm))
				{
					num = global::UnityEngine.Mathf.Max(num, doorCost.Cost);
				}
			}
			return num;
		}

		public override void ClientWriteCmd(global::Mirror.NetworkWriter writer)
		{
			base.ClientWriteCmd(writer);
			global::Mirror.NetworkWriterExtensions.WriteUInt32(writer, LastDoor.netId);
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			base.ServerProcessCmd(reader);
			if (global::Mirror.NetworkIdentity.spawned.TryGetValue(global::Mirror.NetworkReaderExtensions.ReadUInt32(reader), out var value) && value.TryGetComponent<global::Interactables.Interobjects.DoorUtils.DoorVariant>(out LastDoor) && IsReady && base.Role.TryGetOwner(out var hub) && !base.LostSignalHandler.Lost)
			{
				bool targetState = LastDoor.TargetState;
				LastDoor.ServerInteract(hub, 0);
				if (targetState != LastDoor.TargetState)
				{
					base.RewardManager.MarkRooms(LastDoor.Rooms);
					global::PlayerRoles.PlayableScps.Scp079.Scp079DoorStateChanger.OnServerDoorToggled?.Invoke(base.ScpRole, LastDoor);
					base.AuxManager.CurrentAux -= GetCostForDoor(TargetAction, LastDoor);
					ServerSendRpc(toAll: false);
				}
			}
		}
	}
}
