using System;
using System.Text;
using MapGeneration;
using Mirror;
using PlayerRoles.PlayableScps.Scp079.GUI;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079BlackoutZoneAbility : Scp079KeyAbilityBase, IScp079LevelUpNotifier
	{
		private enum ValidationError
		{
			None = 0,
			NotEnoughAux = 1,
			Cooldown = 59,
			Unavailable = 61
		}

		public static Action<ReferenceHub, FacilityZone> OnClientZoneBlackout;

		[SerializeField]
		private int _cost;

		[SerializeField]
		private float _duration;

		[SerializeField]
		private float _cooldown;

		[SerializeField]
		private int _minTierIndex;

		[SerializeField]
		private FacilityZone[] _availableZones;

		private readonly AbilityCooldown _cooldownTimer;

		private string _nameFormat;

		private string _textUnlock;

		private string _failMessage;

		private bool _hasFailMessage;

		private FacilityZone _syncZone;

		private Scp079HudTranslation _failReason;

		public override ActionName ActivationKey => default(ActionName);

		public override bool IsReady => false;

		public override string AbilityName => null;

		public override bool IsVisible => false;

		public bool Unlocked => false;

		private Scp079HudTranslation ErrorCode => default(Scp079HudTranslation);

		public override string FailMessage => null;

		protected override void Start()
		{
		}

		protected override void Update()
		{
		}

		public override void OnFailMessageAssigned()
		{
		}

		public override void ResetObject()
		{
		}

		public bool WriteLevelUpNotification(StringBuilder sb, int newLevel)
		{
			return false;
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

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}
	}
}
