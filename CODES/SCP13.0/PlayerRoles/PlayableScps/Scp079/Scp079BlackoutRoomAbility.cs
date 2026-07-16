using System.Collections.Generic;
using System.Text;
using Mirror;
using PlayerRoles.PlayableScps.Scp079.GUI;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079BlackoutRoomAbility : Scp079KeyAbilityBase, IScp079LevelUpNotifier
	{
		private enum ValidationError
		{
			None = 0,
			NotEnoughAux = 1,
			NoController = 26,
			MaxCapacityReached = 27,
			RoomOnCooldown = 28,
			AlreadyBlackedOut = 60
		}

		[SerializeField]
		private int[] _capacityPerTier;

		[SerializeField]
		private float _blackoutDuration;

		[SerializeField]
		private float _cooldown;

		[SerializeField]
		private int _cost;

		[SerializeField]
		private int _surfaceCost;

		private string _textUnlock;

		private string _textCapacityIncreased;

		private string _nameFormat;

		private string _failMessage;

		private bool _hasFailMessage;

		private bool _hasController;

		private RoomLightController _successfulController;

		private RoomLightController _roomController;

		private readonly Dictionary<uint, double> _blackoutCooldowns;

		private readonly HashSet<uint> _obsoleteCooldowns;

		public override ActionName ActivationKey => default(ActionName);

		public override bool IsReady => false;

		public override bool IsVisible => false;

		public override string AbilityName => null;

		public override string FailMessage => null;

		[field: SerializeField]
		public AudioClip ConfirmationSound { get; private set; }

		private int CurrentCapacity => 0;

		private int RoomsOnCooldown => 0;

		private float RemainingCooldown => 0f;

		private Scp079HudTranslation ErrorCode => default(Scp079HudTranslation);

		private bool IsOnSurface => false;

		private float AbilityCost => 0f;

		private void RefreshCurrentController()
		{
		}

		private int GetCapacityOfTier(int index)
		{
			return 0;
		}

		protected override void Start()
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

		public override void ResetObject()
		{
		}

		public void PlaySoundForController(RoomLightController flc)
		{
		}

		public override void OnFailMessageAssigned()
		{
		}

		public bool WriteLevelUpNotification(StringBuilder sb, int newLevel)
		{
			return false;
		}
	}
}
