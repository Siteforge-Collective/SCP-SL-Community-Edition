using System;
using System.Runtime.CompilerServices;
using Interactables.Interobjects.DoorUtils;
using PlayerRoles.PlayableScps.Scp079.Cameras;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079
{
	public abstract class Scp079DoorAbility : Scp079KeyAbilityBase
	{
		protected DoorVariant LastDoor;

		private static string _deniedText;

		private int _lastCost;

		private bool _lastActionValid;

		private int _failMessageAux;

		private bool _failMessageDenied;

		public override bool IsVisible => false;

		public override bool IsReady => false;

		public override string FailMessage => null;

		protected abstract DoorAction TargetAction { get; }

		public static event Action<Scp079Role, DoorVariant> OnServerAnyDoorInteraction
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

		protected abstract int GetCostForDoor(DoorAction action, DoorVariant door);

		protected override void Trigger()
		{
		}

		protected override void Start()
		{
		}

		public override void OnFailMessageAssigned()
		{
		}

		public void PlayConfirmationSound(AudioClip sound)
		{
		}

		public static bool ValidateAction(DoorAction action, DoorVariant door, Scp079Camera currentCamera)
		{
			return false;
		}

		public static bool CheckVisibility(DoorVariant door, Scp079Camera currentCamera)
		{
			return false;
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}
	}
}
