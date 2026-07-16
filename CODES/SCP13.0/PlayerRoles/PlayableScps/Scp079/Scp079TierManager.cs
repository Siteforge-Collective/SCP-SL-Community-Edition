using System;
using System.Collections.Generic;
using Mirror;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079TierManager : StandardSubroutine<Scp079Role>
	{
		private readonly struct ExpQueuedNotification
		{
			public readonly int ExpAmount;

			public readonly Scp079HudTranslation Reason;

			public readonly RoleTypeId Subject;

			public void Write(NetworkWriter writer)
			{
			}

			public ExpQueuedNotification(NetworkReader reader)
			{
				ExpAmount = 0;
				Reason = default(Scp079HudTranslation);
				Subject = default(RoleTypeId);
			}

			public ExpQueuedNotification(int amount, Scp079HudTranslation reason, RoleTypeId subject)
			{
				ExpAmount = 0;
				Reason = default(Scp079HudTranslation);
				Subject = default(RoleTypeId);
			}
		}

		private readonly Queue<ExpQueuedNotification> _expGainQueue;

		private int _totalExp;

		private bool _valueDirty;

		private int _accessTier;

		private int _thresholdsCount;

		[SerializeField]
		private int[] _levelupThresholds;

		public Action OnLevelledUp;

		public Action OnTierChanged;

		public Action OnExpChanged;

		public int[] AbsoluteThresholds { get; private set; }

		public int TotalExp
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public int RelativeExp => 0;

		public int NextLevelThreshold => 0;

		public int AccessTierIndex
		{
			get
			{
				return 0;
			}
			private set
			{
			}
		}

		public int AccessTierLevel => 0;

		private void Update()
		{
		}

		protected override void Awake()
		{
		}

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		public void ServerGrantExperience(int amount, Scp079HudTranslation reason, RoleTypeId subject = RoleTypeId.None)
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
