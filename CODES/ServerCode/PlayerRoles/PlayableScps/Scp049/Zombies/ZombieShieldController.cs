namespace PlayerRoles.PlayableScps.Scp049.Zombies
{
	public class ZombieShieldController : global::PlayerRoles.PlayableScps.HumeShield.DynamicHumeShieldController
	{
		[global::UnityEngine.SerializeField]
		private float _maxShield;

		[global::UnityEngine.SerializeField]
		private float _maxActivateDistanceSqr;

		private static readonly global::System.Collections.Generic.HashSet<global::PlayerRoles.PlayableScps.Scp049.Scp049CallAbility> CallSubroutines = new global::System.Collections.Generic.HashSet<global::PlayerRoles.PlayableScps.Scp049.Scp049CallAbility>();

		private global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule _fpc;

		public override float HsMax => _maxShield;

		public override float HsRegeneration
		{
			get
			{
				if (!global::Utils.NonAllocLINQ.HashsetExtensions.Any(CallSubroutines, (global::PlayerRoles.PlayableScps.Scp049.Scp049CallAbility x) => x.IsMarkerShown && CheckDistanceTo(x.ScpRole)))
				{
					return 0f;
				}
				return base.HsRegeneration;
			}
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			_fpc = (base.Owner.roleManager.CurrentRole as global::PlayerRoles.FirstPersonControl.IFpcRole).FpcModule;
		}

		private bool CheckDistanceTo(global::PlayerRoles.PlayableScps.Scp049.Scp049Role role)
		{
			return (role.FpcModule.Position - _fpc.Position).sqrMagnitude <= _maxActivateDistanceSqr;
		}

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			CustomNetworkManager.OnClientReady += CallSubroutines.Clear;
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged += delegate(ReferenceHub hub, global::PlayerRoles.PlayerRoleBase oldRole, global::PlayerRoles.PlayerRoleBase newRole)
			{
				if (global::Mirror.NetworkServer.active)
				{
					if (TryGetCallSubroutine(oldRole, out var sr))
					{
						CallSubroutines.Remove(sr);
					}
					if (TryGetCallSubroutine(newRole, out var sr2))
					{
						CallSubroutines.Add(sr2);
					}
				}
			};
		}

		private static bool TryGetCallSubroutine(global::PlayerRoles.PlayerRoleBase prb, out global::PlayerRoles.PlayableScps.Scp049.Scp049CallAbility sr)
		{
			if (prb is global::PlayerRoles.PlayableScps.Scp049.Scp049Role scp049Role)
			{
				return scp049Role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp049.Scp049CallAbility>(out sr);
			}
			sr = null;
			return false;
		}
	}
}
