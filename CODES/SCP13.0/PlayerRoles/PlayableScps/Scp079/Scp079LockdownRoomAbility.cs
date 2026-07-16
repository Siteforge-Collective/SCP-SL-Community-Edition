using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Interactables.Interobjects.DoorUtils;
using MapGeneration;
using Mirror;
using PlayerRoles.PlayableScps.Scp079.GUI;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079LockdownRoomAbility : Scp079KeyAbilityBase, IScp079LevelUpNotifier, IScp079AuxRegenModifier
	{
		private enum ValidationError
		{
			None = 0,
			Unknown = 1,
			NotEnoughAux = 6,
			TierTooLow = 8,
			Cooldown = 31,
			NoDoors = 32
		}

		[SerializeField]
		private int _minimalTierIndex;

		[SerializeField]
		private float[] _regenerationPerTier;

		[SerializeField]
		private float _lockdownDuration;

		[SerializeField]
		private float _cooldown;

		[SerializeField]
		private int _cost;

		[SerializeField]
		private float _minStateToClose;

		[SerializeField]
		private AudioClip _lockdownStartSound;

		[SerializeField]
		private AudioClip _lockdownEndSound;

		private string _nameFormat;

		private string _failMessage;

		private string _unlockText;

		private double _nextUseTime;

		private bool _hasFailMessage;

		private bool _lockdownInEffect;

		private Scp079DoorLockChanger _doorLockChanger;

		private readonly HashSet<DoorVariant> _roomDoors;

		private readonly HashSet<DoorVariant> _doorsToLockDown;

		private readonly HashSet<DoorVariant> _alreadyLockedDown;

		private RoomIdentifier _lastLockedRoom;

		public override ActionName ActivationKey => default(ActionName);

		public override bool IsReady => false;

		public override bool IsVisible => false;

		public override string AbilityName => null;

		public override string FailMessage => null;

		public float AuxRegenMultiplier => 0f;

		public string AuxReductionMessage { get; private set; }

		private Scp079HudTranslation ErrorCode => default(Scp079HudTranslation);

		private float RemainingCooldown
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		private float RemainingLockdownDuration => 0f;

		public static event Action<Scp079Role, RoomIdentifier> OnServerLockdown
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

		public static event Action<Scp079Role, DoorVariant> OnServerDoorLocked
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

		private void ServerInitLockdown()
		{
		}

		private void ServerCancelLockdown()
		{
		}

		private bool ValidateDoor(DoorVariant dv)
		{
			return false;
		}

		protected override void Start()
		{
		}

		protected override void Update()
		{
		}

		protected override void Trigger()
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

		public override void OnFailMessageAssigned()
		{
		}

		public override void ResetObject()
		{
		}

		public static bool IsLockedDown(DoorVariant dv)
		{
			return false;
		}

		public bool WriteLevelUpNotification(StringBuilder sb, int newLevel)
		{
			return false;
		}
	}
}
