using System;
using System.Runtime.CompilerServices;
using Interactables.Interobjects.DoorUtils;
using Mirror;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079DoorStateChanger : Scp079DoorAbility
	{
		[Serializable]
		private struct DoorCost
		{
			public KeycardPermissions Perm;

			public int Cost;
		}

		[SerializeField]
		private int _defaultCost;

		[SerializeField]
		private DoorCost[] _doorCostsheet;

		[SerializeField]
		private AudioClip _confirmationSound;

		private Scp079DoorLockChanger _lockChanger;

		private static string _openText;

		private static string _closeText;

		public override ActionName ActivationKey => default(ActionName);

		public override string AbilityName => null;

		protected override DoorAction TargetAction => default(DoorAction);

		public static event Action<Scp079Role, DoorVariant> OnServerDoorToggled
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		protected override void Start()
		{
		}

		protected override int GetCostForDoor(DoorAction action, DoorVariant door)
		{
			return 0;
		}

		public override void ClientWriteCmd(NetworkWriter writer)
		{
		}

		public override void ServerProcessCmd(NetworkReader reader)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}
	}
}
