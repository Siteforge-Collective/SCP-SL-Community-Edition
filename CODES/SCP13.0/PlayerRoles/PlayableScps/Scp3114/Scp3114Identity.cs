using System;
using System.Runtime.CompilerServices;
using System.Text;
using Mirror;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.Ragdolls;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp3114
{
	public class Scp3114Identity : StandardSubroutine<Scp3114Role>, ICustomNicknameDisplayRole
	{
		public enum DisguiseStatus
		{
			None = 0,
			Equipping = 1,
			Active = 2
		}

		public class StolenIdentity
		{
			private DisguiseStatus _status;

			public BasicRagdoll Ragdoll;

			public byte UnitNameId;

			public DisguiseStatus Status
			{
				get
				{
					return default(DisguiseStatus);
				}
				set
				{
				}
			}

			public RoleTypeId StolenRole => default(RoleTypeId);

			public event Action OnStatusChanged
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

			public void Reset()
			{
			}
		}

		public readonly StolenIdentity CurIdentity;

		public readonly AbilityCooldown RemainingDuration;

		private bool _wasDisguised;

		[SerializeField]
		private AudioSource[] _revealEffectSources;

		[SerializeField]
		private AudioSource _revealWarningSource;

		[SerializeField]
		private float _warningTimeSeconds;

		[SerializeField]
		private float _disguiseDurationSeconds;

		private void OnRagdollRemoved(BasicRagdoll ragdoll)
		{
		}

		private void OnPlayerAdded(ReferenceHub player)
		{
		}

		private void Update()
		{
		}

		private void UpdateWarningAudio()
		{
		}

		private void OnIdentityStatusChanged()
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

		public void WriteNickname(ReferenceHub owner, StringBuilder sb, out Color texColor)
		{
			texColor = default(Color);
		}

		public void ServerResendIdentity()
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
