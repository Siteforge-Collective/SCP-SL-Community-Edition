namespace CustomPlayerEffects
{
	public class InsufficientLighting : global::CustomPlayerEffects.StatusEffectBase, global::RemoteAdmin.Interfaces.ICustomRADisplay
	{
		private bool _prevTarget;

		private global::PlayerRoles.PlayerRoleBase CurRole => base.Hub.roleManager.CurrentRole;

		public string DisplayName { get; }

		public bool CanBeDisplayed { get; }

		public static global::UnityEngine.Color DefaultColor
		{
			get
			{
				if (!VeryHighPerformance.LightsOff)
				{
					return global::UnityEngine.Color.black;
				}
				return VeryHighPerformance.VHColor;
			}
		}

		internal override void OnRoleChanged(global::PlayerRoles.PlayerRoleBase previousRole, global::PlayerRoles.PlayerRoleBase newRole)
		{
			base.OnRoleChanged(previousRole, newRole);
			_prevTarget = false;
		}

		protected override void OnAwake()
		{
			base.OnAwake();
			StaticUnityMethods.OnUpdate += AlwaysUpdate;
		}

		private void OnDestroy()
		{
			StaticUnityMethods.OnUpdate -= AlwaysUpdate;
		}

		private void AlwaysUpdate()
		{
			if (global::Mirror.NetworkServer.active)
			{
				UpdateServer();
			}
		}

		private void UpdateServer()
		{
			bool flag = CurRole is global::PlayerRoles.IAmbientLightRole ambientLightRole && ambientLightRole.InsufficientLight;
			if (flag != _prevTarget)
			{
				base.Intensity = (byte)(flag ? 1u : 0u);
				_prevTarget = flag;
			}
		}
	}
}
