using System;
using System.Runtime.CompilerServices;
using System.Text;
using GameObjectPools;
using Interactables.Interobjects.DoorUtils;
using Mirror;
using PlayerRoles.PlayableScps.Scp079.GUI;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079DoorLockChanger : Scp079DoorAbility, IPoolResettable, IScp079AuxRegenModifier, IScp079LevelUpNotifier
	{
		[SerializeField]
		private AudioClip _lockSound;

		[SerializeField]
		private AudioClip _unlockSound;

		[SerializeField]
		private float _costMaxAuxPercent;

		[SerializeField]
		private int _costRounding;

		[SerializeField]
		private float _cooldownBaseline;

		[SerializeField]
		private float _cooldownPerTimeLocked;

		[SerializeField]
		private float _lockCostPerSec;

		[SerializeField]
		private float _lockCostPow;

		[SerializeField]
		private AudioClip _whirSound;

		[SerializeField]
		private int _minTierIndex;

		private static string _lockText;

		private static string _unlockText;

		private static string _cooldownText;

		private static string _alreadyLockedText;

		private static string _abilityUnlockText;

		private readonly AbilityCooldown _cooldown;

		private DoorVariant _failedDoor;

		private double _lockTime;

		private const DoorLockReason LockReason = DoorLockReason.Regular079;

		public override ActionName ActivationKey => default(ActionName);

		public float AuxRegenMultiplier => 0f;

		public string AuxReductionMessage { get; private set; }

		public DoorVariant LockedDoor { get; private set; }

		public override string AbilityName => null;

		public override bool IsReady => false;

		public override bool IsVisible => false;

		public override string FailMessage => null;

		public int LockClosedDoorCost => 0;

		[field: SerializeField]
		public int LockOpenDoorCost { get; private set; }

		protected override DoorAction TargetAction => default(DoorAction);

		private bool IsHighlightLockedBy079 => false;

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

		protected override int GetCostForDoor(DoorAction action, DoorVariant door)
		{
			return 0;
		}

		protected virtual void OnDestroy()
		{
		}

		protected override void Start()
		{
		}

		protected override void Update()
		{
		}

		public void ServerUnlock()
		{
		}

		public override void OnFailMessageAssigned()
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

		public override void ResetObject()
		{
		}

		public bool WriteLevelUpNotification(StringBuilder sb, int newLevel)
		{
			return false;
		}
	}
}
