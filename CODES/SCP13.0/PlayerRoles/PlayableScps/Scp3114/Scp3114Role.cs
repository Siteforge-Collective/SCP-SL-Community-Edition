using System.Text;
using InventorySystem;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.PlayableScps.HUDs;
using PlayerRoles.PlayableScps.HumeShield;
using PlayerRoles.Subroutines;
using PlayerStatsSystem;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp3114
{
	public class Scp3114Role : FpcStandardScp, IHumeShieldedRole, IInventoryRole, ISubroutinedRole, IHudScp, ICustomNicknameDisplayRole, IDamageHandlerProcessingRole, IHitmarkerPreventer
	{
		private Scp3114Identity _identity;

		private Scp3114DamageProcessor _damageProcessor;

		[field: SerializeField]
		public HumeShieldModuleBase HumeShieldModule { get; private set; }

		[field: SerializeField]
		public SubroutineManagerModule SubroutineModule { get; private set; }

		[field: SerializeField]
		public ScpHudBase HudPrefab { get; private set; }

		public Scp3114Identity.StolenIdentity CurIdentity => null;

		public bool Disguised
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool SkeletonIdle => false;

		private void Awake()
		{
		}

		public void WriteNickname(ReferenceHub owner, StringBuilder sb, out Color texColor)
		{
			texColor = default(Color);
		}

		public DamageHandlerBase ProcessDamageHandler(DamageHandlerBase dhb)
		{
			return null;
		}

		public bool TryPreventHitmarker(AttackerDamageHandler adh)
		{
			return false;
		}

		public bool AllowDisarming(ReferenceHub detainer)
		{
			return false;
		}

		public bool AllowUndisarming(ReferenceHub releaser)
		{
			return false;
		}
	}
}
