using System;
using System.Runtime.CompilerServices;
using Interactables.Interobjects;
using Mirror;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079ElevatorStateChanger : Scp079KeyAbilityBase
	{
		[SerializeField]
		private AudioClip _confirmationSound;

		[SerializeField]
		private int _cost;

		private ElevatorDoor _lastElevator;

		private Scp079HudTranslation _failedReason;

		private string _abilityName;

		public override bool IsVisible => false;

		public override bool IsReady => false;

		public override string FailMessage => null;

		public override ActionName ActivationKey => default(ActionName);

		public override string AbilityName => null;

		private Scp079HudTranslation ErrorCode => default(Scp079HudTranslation);

		private bool ValidateLastElevator => false;

		public static event Action<Scp079Role, ElevatorDoor> OnServerElevatorDoorClosed
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

		protected override void Trigger()
		{
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

		public override void OnFailMessageAssigned()
		{
		}
	}
}
