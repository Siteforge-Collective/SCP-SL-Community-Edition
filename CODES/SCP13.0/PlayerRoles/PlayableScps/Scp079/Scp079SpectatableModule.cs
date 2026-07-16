using PlayerRoles.Spectating;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079SpectatableModule : SpectatableModuleBase
	{
		private Scp079Role Scp079 => null;

		public override Vector3 CameraPosition => default(Vector3);

		public override Vector3 CameraRotation => default(Vector3);
	}
}
