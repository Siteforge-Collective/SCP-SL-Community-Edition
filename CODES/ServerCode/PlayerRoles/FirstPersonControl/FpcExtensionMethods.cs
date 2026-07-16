namespace PlayerRoles.FirstPersonControl
{
	public static class FpcExtensionMethods
	{
		public static global::UnityEngine.Vector3 GetVelocity(this ReferenceHub hub)
		{
			if (!(hub.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole))
			{
				return global::UnityEngine.Vector3.zero;
			}
			return fpcRole.FpcModule.Motor.Velocity;
		}

		public static bool IsGrounded(this ReferenceHub hub)
		{
			if (hub.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole)
			{
				return fpcRole.FpcModule.IsGrounded;
			}
			return true;
		}

		public static global::UnityEngine.Bounds GenerateTracerBounds(this ReferenceHub hub, float time, bool ignoreTeleports)
		{
			if (!(hub.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole))
			{
				return new global::UnityEngine.Bounds(hub.transform.position, global::UnityEngine.Vector3.zero);
			}
			return fpcRole.FpcModule.Tracer.GenerateBounds(time, ignoreTeleports);
		}

		public static bool TryOverridePosition(this ReferenceHub hub, global::UnityEngine.Vector3 position, global::UnityEngine.Vector3 deltaRotation)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				return false;
			}
			if (!(hub.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole))
			{
				return false;
			}
			fpcRole.FpcModule.ServerOverridePosition(position, deltaRotation);
			return true;
		}
	}
}
