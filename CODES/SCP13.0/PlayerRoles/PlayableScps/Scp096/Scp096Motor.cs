using PlayerRoles.FirstPersonControl;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096Motor : FpcMotor
	{
		private readonly Scp096Role _role;

		private bool _hasOverride;

		private Vector3 _overrideDir;

		protected override Vector3 DesiredMove => default(Vector3);

		public void SetOverride(Vector3 desiredMove)
		{
		}

		public Scp096Motor(ReferenceHub hub, Scp096Role role)
			: base(null, null, enableFallDamage: false)
		{
		}
	}
}
