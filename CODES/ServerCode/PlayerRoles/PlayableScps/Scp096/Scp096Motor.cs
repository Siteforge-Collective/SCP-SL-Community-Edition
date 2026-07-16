namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096Motor : global::PlayerRoles.FirstPersonControl.FpcMotor
	{
		private readonly global::PlayerRoles.PlayableScps.Scp096.Scp096Role _role;

		private bool _hasOverride;

		private global::UnityEngine.Vector3 _overrideDir;

		protected override global::UnityEngine.Vector3 DesiredMove
		{
			get
			{
				if (!_role.IsLocalPlayer || !_hasOverride)
				{
					return base.DesiredMove;
				}
				_hasOverride = false;
				return _overrideDir;
			}
		}

		public void SetOverride(global::UnityEngine.Vector3 desiredMove)
		{
			_hasOverride = true;
			_overrideDir = desiredMove;
		}

		public Scp096Motor(ReferenceHub hub, global::PlayerRoles.PlayableScps.Scp096.Scp096Role role)
			: base(hub, role.FpcModule, enableFallDamage: false)
		{
			_role = role;
		}
	}
}
